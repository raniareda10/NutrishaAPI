using System;
using DL.DtosV1.Blogs.Details;
using DL.Enums;
using DL.Services.Blogs.Articles;
using Microsoft.Extensions.DependencyInjection;

namespace DL.Services.Blogs.BlogDetails
{
    public class BlogDetailsFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public BlogDetailsFactory(IServiceProvider ServiceProvider)
        {
            _serviceProvider = ServiceProvider;
        }
        public IBlogDetailsService GetBlogDetailsService(EntityType entityType)
        {
            return entityType switch
            {
                EntityType.Article => _serviceProvider.GetRequiredService<ArticleService>(),
                _ => throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null)
            };
        }
    }
}