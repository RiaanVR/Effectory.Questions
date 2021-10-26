using System.Reflection;
using System.Text.Json;
using Effectory.Questions.CrossCutting;

namespace Effectory.Questions.Adapters
{
    public class SeedDatabaseFromJsonFile : IStartupTask
    {
        private readonly QuestionsDbContext _dbContext;

        public SeedDatabaseFromJsonFile(QuestionsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Execute()
        {

            string? binDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            await using FileStream fileStream = File.OpenRead($"{binDirectory}/questionnaire.json");
            Rootobject? rootObject = await JsonSerializer.DeserializeAsync<Rootobject>(fileStream);

            IEnumerable<Question>? allQuestions = rootObject.questionnaireItems.SelectMany(s => s.questionnaireItems);

            _dbContext.Questions.AddRange(allQuestions.Select(q => new QuestionEntity
            {
                Id = q.questionId,
                SubjectId = q.subjectId,
                AnswerCategoryType = ((Core.AnswerCategoryType)q.answerCategoryType).ToString(),
                Texts = q.texts
            }));

            await _dbContext.SaveChangesAsync();

        }

        private class Rootobject
        {
            public int questionnaireId { get; set; }
            public Subject[] questionnaireItems { get; set; }
        }

        private class Subject
        {
            public int subjectId { get; set; }
            public int orderNumber { get; set; }
            public Dictionary<string, string> texts { get; set; }
            public int itemType { get; set; }
            public Question[] questionnaireItems { get; set; }
        }

        private class Question
        {
            public int questionId { get; set; }
            public int subjectId { get; set; }
            public int answerCategoryType { get; set; }
            public int orderNumber { get; set; }
            public Dictionary<string, string> texts { get; set; }
            public int itemType { get; set; }
            public Answer[] questionnaireItems { get; set; }
        }


        private class Answer
        {
            public int? answerId { get; set; }
            public int questionId { get; set; }
            public int answerType { get; set; }
            public int orderNumber { get; set; }
            public Dictionary<string, string> texts { get; set; }
            public int itemType { get; set; }
        }
    }

}