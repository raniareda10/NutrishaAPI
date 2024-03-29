﻿using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Comments;
using DL.EntitiesV1.Blogs;
using DL.Enums;
using DL.Extensions;
using DL.Repositories.Helpers;
using DL.ResultModels;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Comments
{
    public class CommentService
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public CommentService(
            AppDBContext DbContext,
            ICurrentUserService currentUserService)
        {
            _dbContext = DbContext;
            _currentUserService = currentUserService;
        }

        public async Task<PayloadServiceResult<CommentDto>> PostCommentAsync(PostCommentDto postCommentDto)
        {
            var result = new PayloadServiceResult<CommentDto>();
            if (!await _dbContext.IsEntityExistsAsync(postCommentDto.EntityId, postCommentDto.EntityType))
            {
                result.Errors.Add(NonLocalizedErrorMessages.InvalidParameters);
                return result;
            }

            await UpdateParentTotalsAsync(
                postCommentDto.EntityId,
                postCommentDto.EntityType,
                TotalKeys.Comments,
                true);

            await UpdateParentTotalsAsync(
                _currentUserService.UserId,
                EntityType.User,
                TotalKeys.Comments,
                true);
            
            var comment = postCommentDto.ToCommentEntity(_currentUserService.UserId);
            _dbContext.Add(comment);
            await _dbContext.SaveChangesAsync();

            result.Data = CommentDto.FromCommentEntity(comment);
            return result;
        }

        public async Task<BaseServiceResult> DeleteCommentAsync(long commentId, bool isAdmin = false)
        {
            var result = new BaseServiceResult();

            var query = _dbContext.Comments
                .Where(c => c.Id == commentId);

            if (!isAdmin)
            {
                query = query.Where(c => c.UserId == _currentUserService.UserId);
            }

            var comment = await query.FirstOrDefaultAsync();
            if (comment == null)
            {
                result.Errors.Add(NonLocalizedErrorMessages.InvalidId);
                return result;
            }

            _dbContext.Comments.Remove(comment);
            await UpdateParentTotalsAsync(
                comment.EntityId,
                comment.EntityType,
                TotalKeys.Comments,
                false);

            await UpdateParentTotalsAsync(
                comment.UserId,
                EntityType.User,
                TotalKeys.Comments,
                false);

            await _dbContext.SaveChangesAsync();

            return result;
        }

        public async Task<PagedResult<CommentDto>> GetPagedListAsync(GetCommentsModel model)
        {
            var comments = await _dbContext.Comments
                .Where(c => c.EntityId == model.EntityId)
                .OrderByDescending(c => c.Created)
                .Include(c => c.User)
                .Select(c => CommentDto.FromCommentEntity(c))
                .ToPagedListAsync(model);

            var commentIds = comments.Data.Select(c => c.Id).ToList();

            var reactions =
                await _dbContext.Reactions.GetReactionsOnEntitiesAsync(commentIds, _currentUserService.UserId);

            foreach (var commentDto in comments.Data)
            {
                if (reactions.TryGetValue(commentDto.Id, out var reactionType))
                {
                    commentDto.ReactionType = reactionType;
                }
            }

            return comments;
        }


        private async Task UpdateParentTotalsAsync(
            long entityId,
            EntityType entityType,
            string key,
            bool increase)
        {
            var entityWithTotal = await _dbContext
                .GetEntityWithTotalAsync(entityId, entityType);

            if (increase)
            {
                entityWithTotal.Totals[key]++;
            }
            else
            {
                entityWithTotal.Totals[key]--;
            }

            _dbContext.Update(entityWithTotal);
        }
    }
}