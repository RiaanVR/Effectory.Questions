using System.Threading.Tasks;
using Effectory.Questions.Contract;
using Effectory.Questions.Core.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Effectory.Questions.Worker
{
    public class ServiceBusTriggerAggregateAnswers
    {
        private readonly IMaterializedAggregatedAnswerRepository _materializedAggregatedAnswerRepository;

        public ServiceBusTriggerAggregateAnswers(IMaterializedAggregatedAnswerRepository materializedAggregatedAnswerRepository)
        {
            _materializedAggregatedAnswerRepository = materializedAggregatedAnswerRepository;
        }

        [FunctionName("ServiceBusTriggerAggregateAnswers")]
        public async Task Run([ServiceBusTrigger("sbt-questions", "sbts-questions-aggregate-answer", Connection = "ServiceBusConnection")] 
            QuestionAnsweredEvent questionAnsweredEvent, ILogger log)
        {
            // 1. setup some startup to make use of DI
            // 2. get the aggregate entity
            // 3. aggregate the result from the event
            // 4. persist the aggregate entity  ( gotta apply a row-version, and apply optimistic concurrency )
            
      
        }
    }
}
