﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotNews.Core;
using HotNews.Core.Objects;


namespace HotNews.Models
{
    public class WidgetViewModel
    {
        public WidgetViewModel(IBlogRepository blogRepository)
        {
            Categories = blogRepository.Categories();
            Tags = blogRepository.Tags();
            LatestPosts = blogRepository.Posts(0, 10);

        }

        public IList<Category> Categories { get; private set; }
        public IList<Tag> Tags { get; private set; }

        public IList<Post> LatestPosts
        { get; private set; }
    }
}
