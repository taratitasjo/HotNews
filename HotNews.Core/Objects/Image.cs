using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace HotNews.Core.Objects
{
    public class Image
    {
        public virtual int Id
        { get; set; }


        public HttpPostedFileBase ImagePath
        { get; set; }

        public virtual string ImageAltName
        { get; set; }

        [JsonIgnore]
        public virtual IList<Post> Posts
        { get; set; }

    }
}
