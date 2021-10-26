using Ardalis.ApiEndpoints;
using Effectory.Questions.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Effectory.Questions.Web.Questions
{
    public class QuestionResultsRequest
    {
        public int QuestionId { get; set; }
    }
    public class QuestionResultsResponse
    {
        public DepartmentResult[] Results { get; set; } 
        public class DepartmentResult
        {
            public string Department { get; set; }
            public int Minimum { get; set; }
            public int Maximum { get; set; }
            public decimal Average { get; set; }
        }
    }
    public class Results : BaseAsyncEndpoint.WithRequest<QuestionResultsRequest>.WithResponse<QuestionResultsResponse>
    {
        private readonly IMaterializedAggregatedAnswerRepository _aggregatedAnswerRepository;

        public Results(IMaterializedAggregatedAnswerRepository aggregatedAnswerRepository)
        {
            _aggregatedAnswerRepository = aggregatedAnswerRepository;
        }

        [HttpGet("/question/{questionId:int}/results")]
        [SwaggerOperation(
           Summary = "Returns an aggregation of the answers per department for a given Question",
           OperationId = "Question.Results",
           Tags = new[] { "QuestionEndpoints" })]
        [SwaggerResponse(StatusCodes.Status200OK, "Successfully retrieved the aggregate", typeof(QuestionResultsResponse))]
        public override Task<ActionResult<QuestionResultsResponse>> HandleAsync([FromRoute]QuestionResultsRequest request,CancellationToken cancellationToken = default)
        {
            // todo(riaan): 1. implement a backing store for the repository
            // 2. Implement a new method to return the aggregated results for a given question id
            // 3. Call new method returning the result.
            throw new NotImplementedException();
        }
    }
}
