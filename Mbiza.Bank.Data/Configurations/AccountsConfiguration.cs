using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mbiza.Bank
{
    public class AccountsConfiguration : IEntityTypeConfiguration<Accounts>
    {
        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Accounts> builder)
        {
            builder.HasKey("Id").Metadata.IsPrimaryKey();
            builder.Property(x => x.Amount);
            builder.Property(x => x.Balance);
            builder.Property(x => x.CreatedDateTime);
            builder.Property(x => x.CreatedBy);
            builder.Property(x => x.UpdatedDateTime);
            builder.Property(x => x.UpdatedBy);
        }
    }
}
