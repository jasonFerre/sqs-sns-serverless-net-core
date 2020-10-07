namespace Base.Serverless.Infrastructure.AWS.SQS
{
    public interface ISQSHelper
    {
        void Post(string message, string queueUrl);
    }
}
