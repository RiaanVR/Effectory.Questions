using System.Collections.ObjectModel;
using Ardalis.GuardClauses;

namespace Effectory.Questions.Core
{
    public class Questions : KeyedCollection<QuestionId, Question>
    {
        public Questions(IEnumerable<Question> questions)
        {
            Guard.Against.NullOrEmpty(questions, nameof(questions));
            foreach (var question in questions.OrderBy(q => q.OrderNumber))
            {
                Add(question);
            }
        }

        protected override QuestionId GetKeyForItem(Question item) => item.Id;
    }

    
}
