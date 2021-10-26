using Effectory.Questions.Core;
using Effectory.Questions.Core.Interfaces;

namespace Effectory.Questions.Adapters
{

    public class EntityFrameworkQuestionRepository : BaseEntityFrameworkRepository<QuestionEntity, QuestionsDbContext>, IQuestionRepository
    {
        private readonly ISendMessages _messageSender;

        public EntityFrameworkQuestionRepository(QuestionsDbContext context, ISendMessages messageSender) : base(context)
        {
            _messageSender = messageSender;
        }


        public IAsyncEnumerable<Question> Get(int take, int skip)
        {
            return base.GetAsync(take: take, skip: skip)
                .Select(qe => MapQuestion(qe));
        }

        private static Question MapQuestion(QuestionEntity qe) => new Question(
                            new QuestionId(qe.Id),
                            new SubjectId(qe.SubjectId),
                            Enum.Parse<AnswerCategoryType>(qe.AnswerCategoryType),
                            qe.Texts,
                            new Answers(new[] { // todo(riaan): this should of course come from the query entity, time saver
                                new Answer(
                                    new AnswerId(17969124),
                                    new QuestionId(qe.Id),
                                    AnswerType.Selection,
                                    new Dictionary<string, string> { { "en-US", "Strongly Disagree" } },
                                    0)
                                })
                            );

        public async Task<Question> GetAsync(QuestionId questionId)
        {
            QuestionEntity? questionEntity = await base.GetByIdAsync(questionId.GetValue());

            if(questionEntity is null)
            {
                // usually would throw some exception here, get should always return something, I generally would use the getOrCreate pattern otherwise
            }

            return MapQuestion(questionEntity!);
        }

        public async Task UpdateAsync(Question question)
        {

            QuestionEntity? questionEntity = await base.GetByIdAsync(question.Id);

            // todo(riaan): map incoming domain to db model.


            await base.CommitAsync();

            await _messageSender.PublishMessages(question.GetUncommittedEvents());

            question.ClearEvents();

        }
    }

}