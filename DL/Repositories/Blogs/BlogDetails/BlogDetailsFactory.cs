using System;
using DL.Enums;
using DL.Repositories.Blogs.Articles;
using Microsoft.Extensions.DependencyInjection;

namespace DL.Repositories.Blogs.BlogDetails
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