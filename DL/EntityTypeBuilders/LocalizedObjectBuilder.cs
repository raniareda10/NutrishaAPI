using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DL.DtosV1.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace DL.EntityTypeBuilders
{
    public static class LocalizedObjectBuilder
    {
        public static PropertyBuilder HasLocalizedObject<T, TProp>(
            this EntityTypeBuilder<T> entity, Expression<Func<T, TProp>> selector)
            where T : class
        {
            return entity.Property(selector)
                .HasConversion(
                    totals => JsonConvert.SerializeObject(totals),
                    totals => JsonConvert.DeserializeObject<TProp>(totals)
                );
        }
    }
}