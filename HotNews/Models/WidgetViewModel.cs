using System;
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
        }

        public IList<Category> Categories { get; private set; }
    }
}
