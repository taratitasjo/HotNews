using HotNews.Core.Objects;
using System.Collections.Generic;

namespace HotNews.Core
{
    public interface IBlogRepository
    {
        IList<Post> Posts(int pageNo, int pageSize);

        int TotalPosts();

        IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize);

        int TotalPostsForCategory(string categorySlug);

        Category Category(string categorySlug);


        IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize);
        int TotalPostsForTag(string tagSlug);
        Tag Tag(string tagSlug);

        IList<Post> PostsForSearch(string search, int pageNo, int pageSize);
        int TotalPostsForSearch(string search);

        Post Post(int year, int month, string titleSlug);
        IList<Category> Categories();
    }
}