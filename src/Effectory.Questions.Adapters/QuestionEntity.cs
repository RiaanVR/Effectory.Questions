namespace Effectory.Questions.Adapters
{
    public class QuestionEntity
    {
        public int Id { get; set; }
        public Dictionary<string, string> Texts { get; set; }
        public int SubjectId { get; set; }
        public string AnswerCategoryType { get; set; }
    }

}