using BL.Helpers;
using BL.Infrastructure;
using DL.DBContext;
using DL.DTOs.VideoDTO;
using DL.DTOs.SharedDTO;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace BL.Repositories
{
    public interface IVideoRepository
    {
        PagedList<MVideo> GetAllVideo(VideoQueryPramter VideoParameters);

    }

    public class VideoRepository : Repository<MVideo>, IVideoRepository
    {
        public VideoRepository(AppDBContext ctx) : base(ctx)
        { }


        public PagedList<MVideo> GetAllVideo(VideoQueryPramter VideoParameters)
        {

            IQueryable<MVideo> Video = GetAll();
            //searhing
            SearchByPramter(ref Video, VideoParameters.fromDate, VideoParameters.toDate, VideoParameters.Title, VideoParameters.BlogTypeId);

            return PagedList<MVideo>.ToPagedList(Video,
                VideoParameters.PageNumber,
                VideoParameters.PageSize);

        }

        private void SearchByPramter(ref IQueryable<MVideo> Video, DateTime? fromdate, DateTime? todate, string title, int blogTypeId)
        {
            if (Video.Any() && fromdate != null)
            {
                Video = Video.Where(o => o.CreatedTime >= fromdate);
            }
            if (Video.Any() && todate != null)
            {
                Video = Video.Where(o => o.CreatedTime <= todate);
            }
            if (!Video.Any() && !string.IsNullOrWhiteSpace(title))
            {
                Video = Video.Where(o => o.Title.ToLower().Contains(title.Trim().ToLower()));
            }
            if (Video.Any() && blogTypeId > 0)
            {
                Video = Video.Where(o => o.BlogTypeId == blogTypeId);
            }
            return;
        }
    }
}
