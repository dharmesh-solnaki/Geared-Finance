using Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Entities.Extentions
{
    public static class ModelBuilderExtensions
    {

        public static void ConfigureKeylessEntities(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DisplayLeadDTO>(x =>
            {
                x.HasNoKey();
                x.ToView(null);
            });
        }
    }
}
