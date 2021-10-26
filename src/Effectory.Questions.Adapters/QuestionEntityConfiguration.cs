using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Effectory.Questions.Adapters
{
    public class QuestionEntityConfiguration : IEntityTypeConfiguration<QuestionEntity>
    {
        private static readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
        };
        public void Configure(EntityTypeBuilder<QuestionEntity> builder)
        {
            builder.HasKey(q => q.Id);


            builder.Property(q => q.SubjectId).IsRequired();
            builder.Property(q => q.AnswerCategoryType).HasMaxLength(64).IsRequired();

            builder.Property(q => q.Texts)
                .HasConversion(
                    item => JsonSerializer.Serialize(item, typeof(Dictionary<string, string>), jsonSerializerOptions),
                    s => JsonSerializer.Deserialize<Dictionary<string, string>>(s, jsonSerializerOptions));
        }
    }

}