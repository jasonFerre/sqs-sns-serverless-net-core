using Amazon.Lambda.SQSEvents;

namespace Base.Serverless.Api.Interfaces
{
    public interface IGenericSQSService
    {
        void Get(SQSEvent request);
        void Post(SQSEvent request, string urlQueue);
    }
}
