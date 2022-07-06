using System.Collections.Generic;
using DL.HelperInterfaces;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace DL.EntityTypeBuilders
{
    public static class TotalBuilder
    {
        public static PropertyBuilder ApplyTotalToJson<T>(this EntityTypeBuilder<T> entity)
            where T : class, ITotal
        {
            return entity.Property(b => b.Totals)
                .HasConversion(
                    totals => JsonConvert.SerializeObject(totals, new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }),
                    totals => JsonConvert.DeserializeObject<Dictionary<string, int>>(totals)
                );
        }
    }
}