using Ardalis.ApiEndpoints;
using Effectory.Questions.Core;
using Effectory.Questions.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Effectory.Questions.Web.Controllers
{

    public class ListQuestionRequest
    {
        [FromQuery] public int? Take { get; set; }
        [FromQuery] public int? Skip { get; set; }
    }

    public class ListQuestionResponse
    {
        public Question[] Results { get; set; } = Array.Empty<Question>();
    }

    public class List : BaseAsyncEndpoint
        .WithRequest<ListQuestionRequest>
        .WithResponse<ListQuestionResponse>
    {

        private readonly ILogger<List> _logger;
        private readonly IQuestionRepository _questionnaireRepository;

        public List(ILogger<List> logger, IQuestionRepository questionnaireRepository)
        {
            _logger = logger;
            _questionnaireRepository = questionnaireRepository;
        }


        [HttpGet("/question")]
        [SwaggerOperation(
           Summary = "Gets the latest list of questions paged",
           OperationId = "Question.List",
           Tags = new[] { "QuestionEndpoints" })]
        [SwaggerResponse(StatusCodes.Status200OK, "There are some questions", typeof(ListQuestionResponse))]
        public override async Task<ActionResult<ListQuestionResponse>> HandleAsync([FromQuery]ListQuestionRequest request, CancellationToken cancellationToken = default)
        {
            return Ok(new ListQuestionResponse
            {
                Results = await _questionnaireRepository.Get(request.Take ?? 5, request.Skip ?? 0).ToArrayAsync(cancellationToken)
            });
        }
    }

}