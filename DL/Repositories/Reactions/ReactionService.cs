using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Reactions;
using DL.EntitiesV1.Reactions;
using DL.Enums;
using DL.Repositories.Helpers;
using DL.ResultModels;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Reactions
{
    public class ReactionService
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public ReactionService(
            AppDBContext DbContext,
            ICurrentUserService currentUserService)
        {
            _dbContext = DbContext;
            _currentUserService = currentUserService;
        }

        #region Main Method

        public async Task<PayloadServiceResult<IDictionary<string, int>>> PostReactionAsync(
            UpdateReactionDto UpdateReactionDto)
        {
            var result = new PayloadServiceResult<IDictionary<string, int>>();
            if (!IsAllowedReaction(UpdateReactionDto))
            {
                result.Errors.Add("invalid reaction type.");
                return result;
            }

            if (await IsAlreadyReactedAsync(UpdateReactionDto))
            {
                result.Errors.Add("You already reacted on this entity.");
                return result;
            }

            if (!await _dbContext.IsEntityExistsAsync(UpdateReactionDto.EntityId, UpdateReactionDto.EntityType))
            {
                result.Errors.Add(NonLocalizedErrorMessages.NoEntityWithThisId);
                return result;
            }
            
            var reaction = new Reaction()
            {
                Created = DateTime.UtcNow,
                EntityId = UpdateReactionDto.EntityId,
                EntityType = UpdateReactionDto.EntityType,
                ReactionType = UpdateReactionDto.ReactionType,
                UserId = _currentUserService.UserId
            };

            var totals = await UpdateAndGetTotalsAsync(UpdateReactionDto);
            
            await _dbContext.Reactions.AddAsync(reaction);
            await _dbContext.SaveChangesAsync();
            result.Data = totals;
            return result;
        }


        public async Task<PayloadServiceResult<IDictionary<string, int>>> DeleteReactionAsync(
            UpdateReactionDto UpdateReactionDto)
        {
            var result = new PayloadServiceResult<IDictionary<string, int>>();

            var entityWithTotal = await _dbContext
                .GetEntityWithTotalAsync(UpdateReactionDto);
            
            if (entityWithTotal == null)
            {
                result.Errors.Add(NonLocalizedErrorMessages.NoEntityWithThisId);
                return result;
            }
            
            var isDeleted = await TryDeleteReactionAsync(UpdateReactionDto.EntityId, UpdateReactionDto.ReactionType);
            if (!isDeleted)
            {
                result.Errors.Add("There is no reaction on this entity.");
                return result;
            }

            entityWithTotal.Totals[UpdateReactionDto.ReactionType.MapToTotalKey()]--;
            await _dbContext.SaveChangesAsync();
            result.Data = entityWithTotal.Totals;
            return result;
        }

        #endregion

        #region Validation

        private async Task<bool> IsAlreadyReactedAsync(UpdateReactionDto UpdateReactionDto)
        {
            return await _dbContext.Reactions.AnyAsync(BuildGetReactionPostReactionDto(UpdateReactionDto));
        }

        private bool IsAllowedReaction(UpdateReactionDto UpdateReactionDto)
        {
            switch (UpdateReactionDto.EntityType)
            {
                case EntityType.Article:
                    return true;
                case EntityType.Comment:
                    return UpdateReactionDto.ReactionType == ReactionType.Like;
                default:
                    return false;
            }
        }

        #endregion
        
        private async Task<bool> TryDeleteReactionAsync(long EntityId, ReactionType ReactionType)
        {
            var reaction = await _dbContext.Reactions.FirstOrDefaultAsync(
                r =>
                    r.UserId == _currentUserService.UserId &&
                    r.EntityId == EntityId &&
                    r.ReactionType == ReactionType
            );

            if (reaction == null) return false;
            _dbContext.Reactions.Remove(reaction);
            return true;
        }


        private Expression<Func<Reaction, bool>> BuildGetReactionPostReactionDto(UpdateReactionDto UpdateReactionDto)
        {
            return r =>
                r.UserId == _currentUserService.UserId &&
                r.EntityId == UpdateReactionDto.EntityId &&
                r.EntityType == UpdateReactionDto.EntityType &&
                r.ReactionType == UpdateReactionDto.ReactionType;
        }

        private async Task<IDictionary<string, int>> UpdateAndGetTotalsAsync(UpdateReactionDto UpdateReactionDto)
        {
            var entityWithTotals = await _dbContext.GetEntityWithTotalAsync(UpdateReactionDto);

            var keyToIncrease = UpdateReactionDto.ReactionType.MapToTotalKey();
            entityWithTotals.Totals[keyToIncrease]++;
            return entityWithTotals.Totals;
        }
    }
}