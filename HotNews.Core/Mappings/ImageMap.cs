using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using HotNews.Core.Objects;

namespace HotNews.Core.Mappings
{
    public class ImageMap: ClassMap<Image>
    {
        public ImageMap()
        {
            Id(x => x.Id);

            Map(x => x.ImagePath)
                .Length(100)
                .Not.Nullable();

            Map(x => x.ImageAltName)
                .Length(100)
                .Not.Nullable();

            HasMany(x => x.Posts)
                .Inverse()
                .Cascade.All()
                .KeyColumn("Image");
        }




    }
}
