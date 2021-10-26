namespace Effectory.Questions.Contract
{
    public enum AnswerTypeDto
    {
        Selection,
        Open
    }
    public class QuestionAnsweredEvent : IEvent
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public AnswerTypeDto AnswerType { get; set; }

        public int? Value { get; set; }
        public string? FreeText { get; set; }
        public string? Department { get; set; }
    }
}
