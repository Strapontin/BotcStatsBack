using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BotcRoles.Models
{
    /// <summary>
    /// Datas about a UpdateHistory
    /// </summary>
    public class UpdateHistory
    {
        public UpdateHistory() { }

        public UpdateHistory(string updateDetails)
        {
            UpdateDetails = updateDetails;
        }

        public long UpdateHistoryId { get; set; }
        public string UpdateDetails { get; set; }
        public DateTime Date { get; set; }

        public long? PlayerId { get; set; }
        public long? RoleId { get; set; }
        public long? EditionId { get; set; }
        public long? GameId { get; set; }
    }



    public class UpdateHistoryEntityTypeConfiguration : IEntityTypeConfiguration<UpdateHistory>
    {
        public void Configure(EntityTypeBuilder<UpdateHistory> builder)
        {
            builder
                .HasKey(b => b.UpdateHistoryId);

            builder
                .Property(b => b.Date)
                .HasConversion(Helper.Converter.GetConverterDateTimeToString());
        }
    }
}
