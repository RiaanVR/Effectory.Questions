using Ardalis.ApiEndpoints;
using Effectory.Questions.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Effectory.Questions.Web.Questions
{
    public class AnswerQuestionRequest
    {
        [FromRoute] public int QuestionId { get; set; }
        [FromRoute] public int AnswerId { get; set; }
        public int UserId { get; set; }
        public Departments Department { get; set; }

        public string? Response { get; set; }

        public enum Departments
        {
            Marketing,
            Sales,
            Development,
            Reception
        }
    }

    public class AnswerQuestionResponse
    {

    }
    public class Answer : BaseAsyncEndpoint.WithRequest<AnswerQuestionRequest>.WithResponse<AnswerQuestionResponse>
    {
        private readonly IQuestionRepository _questionRepository;

        public Answer(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        [HttpPost("/question/{questionId:int}/answer/{answerId:int}")]
        [SwaggerOperation(
           Summary = "Answer a previously recieved question",
           OperationId = "Question.Answer",
           Tags = new[] { "QuestionEndpoints" })]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully processed the answer", typeof(AnswerQuestionResponse))]
        public override async Task<ActionResult<AnswerQuestionResponse>> HandleAsync(AnswerQuestionRequest request, CancellationToken cancellationToken = default)
        {
            var question = await _questionRepository.GetAsync(new Core.QuestionId(request.QuestionId));

            question.Answer(new Core.AnswerId(request.AnswerId), request.Department.ToString(), request.Response);

            await _questionRepository.UpdateAsync(question);

            return Ok(new AnswerQuestionResponse
            {

            });
        }
    }

}
