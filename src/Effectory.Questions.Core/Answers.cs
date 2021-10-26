using System.Collections.ObjectModel;
using Ardalis.GuardClauses;

namespace Effectory.Questions.Core
{
    public class Answers : KeyedCollection<AnswerId, Answer>
    {

        public Answers(IEnumerable<Answer> answers)
        {
            Guard.Against.NullOrEmpty(answers, nameof(answers));
            foreach (var question in answers.OrderBy(q => q.OrderNumber))
            {
                Add(question);
            }
        }
        protected override AnswerId GetKeyForItem(Answer item) => item.Id;
    }

    
}
