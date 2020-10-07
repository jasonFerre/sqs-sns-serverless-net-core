using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;
using System;

namespace Base.Serverless.Infrastructure.AWS.SQS
{
    public class SQSHelper : ISQSHelper
    {
        private readonly IAmazonSQS _amazonSQS;

        public SQSHelper(IAmazonSQS amazonSQS)
        {
            _amazonSQS = amazonSQS;
        }

        public void Post(string message, string queueUrl)
        {
            try
            {
                Console.WriteLine($"Posting at Queue: {queueUrl}");

                var request = new SendMessageRequest(queueUrl, message);
                var result = _amazonSQS.SendMessageAsync(request).Result;

                Console.WriteLine($"Post sqs succefuly: {result.HttpStatusCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
                throw ex;
            }
        }
    }
}
