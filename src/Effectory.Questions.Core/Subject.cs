using System.Collections.ObjectModel;

namespace Effectory.Questions.Core
{
    public class Subject : Entity<SubjectId>
    {
        public Subject(SubjectId id, Dictionary<string, string> texts, Questions questions) : base(id)
        {
            Texts = new ReadOnlyDictionary<string, string>(texts);
            Questions = new ReadOnlyCollection<Question>(questions);
        }

        public IReadOnlyDictionary<string, string> Texts { get; }
        public IReadOnlyCollection<Question> Questions { get; }
        public int OrderNumber { get; }
    }

    
}
