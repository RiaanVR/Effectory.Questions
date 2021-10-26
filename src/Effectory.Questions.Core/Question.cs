using System.Collections.ObjectModel;
using Ardalis.GuardClauses;
using Effectory.Questions.Contract;

namespace Effectory.Questions.Core
{
    public class Question : Entity<QuestionId>
    {
        private readonly Answers _answers;

        public Question(QuestionId questionId,
                        SubjectId subjectId,
                        AnswerCategoryType answerCategoryType,
                        Dictionary<string, string> texts,
                        Answers answers) : base(questionId)
        {
            SubjectId = subjectId;
            AnswerCategoryType = answerCategoryType;
            Texts = new ReadOnlyDictionary<string, string>(texts);
            _answers = answers;
        }


        public SubjectId SubjectId { get; }
        public AnswerCategoryType AnswerCategoryType { get; }
        public IReadOnlyDictionary<string, string> Texts { get; }


        public int OrderNumber { get; }
        public IReadOnlyCollection<Answer> Answers => new ReadOnlyCollection<Answer>(_answers);

        public bool HasBeenAnswered => Answered is not null;
        public Answer? Answered { get; private set; }

        public void Answer(AnswerId answerId, string department, string? response = null)
        {
            if (HasBeenAnswered)
            {
                // consider that this might be a retry
                // if not retry (same answerId), business should decide if we allow users to change answers, I guess yes.

            }

            if (!_answers.TryGetValue(answerId, out var answered))
            {
                // hmm malicous attempt to answer with an unassociated answer?
                // what should we do..?
                return;
            }

            if (answered.AnswerType == AnswerType.Open)
            {
                Guard.Against.Null(response, nameof(response));

            }

            Answered = answered;
            RaiseEvent(new QuestionAnsweredEvent
            {
                QuestionId = Id,
                AnswerId = answerId,
                FreeText = response,
                Department = department,
                Value = answered.OrderNumber,
                AnswerType = answered.AnswerType switch
                {
                    AnswerType.Selection => AnswerTypeDto.Selection,
                    AnswerType.Open => AnswerTypeDto.Open,
                    _ => throw new ArgumentOutOfRangeException()
                }
            });
        }

        
    }

    
}
