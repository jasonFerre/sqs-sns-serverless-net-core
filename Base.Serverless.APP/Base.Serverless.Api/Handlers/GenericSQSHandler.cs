using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Base.Serverless.Api.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Base.Serverless.Api.Handlers
{
    public class GenericSQSHandler : Functions
    {
        private readonly IGenericSQSService _postSQSService;
        public GenericSQSHandler() : base()
        {
            _postSQSService = _serviceProvider.GetService<IGenericSQSService>();
        }

        public void Get(SQSEvent request, ILambdaContext lambdaContext) 
        {
            lambdaContext.Logger.LogLine($"Request for pos sqs incoming, number records: {request?.Records?.Count}");
            
            _postSQSService.Get(request);
        }

        public void Post(SQSEvent request, ILambdaContext lambdaContext)
        {
            lambdaContext.Logger.LogLine($"Request for pos sqs incoming, number records: {request?.Records?.Count}");

            _postSQSService.Post(request, _configuration["QueueArn"]);
        }
    }
}
