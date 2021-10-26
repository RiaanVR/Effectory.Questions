using System.Threading.Tasks;
using Effectory.Questions.Core;

namespace Effectory.Questions.Core.Interfaces
{

    public interface IQuestionRepository
    {
        public IAsyncEnumerable<Question> Get(int take, int skip);
        Task<Question> GetAsync(QuestionId questionId);
        Task UpdateAsync(Question question);
    }
}
