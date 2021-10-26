using System.Collections.ObjectModel;

namespace Effectory.Questions.Core
{
    public class Answer : Entity<AnswerId>
    {
        public Answer(AnswerId id, QuestionId questionId, AnswerType answerType, Dictionary<string, string> texts, int orderNumber) : base(id)
        {
            QuestionId = questionId;
            AnswerType = answerType;
            OrderNumber = orderNumber;
            Texts = new ReadOnlyDictionary<string, string>(texts);
        }

        public QuestionId QuestionId { get; }
        public AnswerType AnswerType { get; }
        public int OrderNumber { get; }
        public IReadOnlyDictionary<string, string> Texts { get; }
    }

    
}
