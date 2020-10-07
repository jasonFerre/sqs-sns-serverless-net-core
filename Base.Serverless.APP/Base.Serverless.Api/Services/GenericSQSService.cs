using Amazon.Lambda.SQSEvents;
using Base.Serverless.Api.Interfaces;
using Base.Serverless.Domain.Entities;
using Base.Serverless.Infrastructure.AWS.SQS;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Serverless.Api.Services
{
    public class GenericSQSService : IGenericSQSService
    {
        private readonly ISQSHelper _sQSHelper;

        public GenericSQSService(ISQSHelper sQSHelper)
        {
            _sQSHelper = sQSHelper;
        }

        public void Post(SQSEvent request, string urlQueue)
        {
            try
            {
                var count = Convert.ToInt32(request.Records.Select(s => s.Body).FirstOrDefault());

                var listBody = new List<GenericMessage>();
                for (int i = 1; i <= count; i++)
                {
                    var body = new GenericMessage() { 
                        Identify = i.ToString(),
                        Test = $"Test{i}"
                    };
                    listBody.Add(body);
                } 
                
                if (listBody.Any())
                    _sQSHelper.Post(JsonConvert.SerializeObject(listBody), urlQueue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(JsonConvert.SerializeObject(ex));
            }
        }

        public void Get(SQSEvent request)
        {
            try
            {
                var messageList = new List<GenericMessage>();
                //passing body messages
                HandlerRecords(request.Records.Select(s => s.Body), messageList);

                int counter = 0;
                messageList.ForEach(message => {
                    message.ProcesseAt = DateTime.Now;
                    Console.WriteLine($"Working with message {++counter} record body \n {JsonConvert.SerializeObject(message)} \n Message processed");
                });                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void HandlerRecords<T>(IEnumerable<string> bodyMessages, List<T> messageList) where T : class
        {
            //getting every body for records and parsing to T
            var bodyList = bodyMessages.Select(s => JsonConvert.DeserializeObject<List<T>>(s)).ToList();
            //adding a body parsed for T list
            bodyList.ForEach(body => { messageList.AddRange(body); });
            Console.WriteLine($"After descompact records, the total numbers of messages are: {messageList?.Count}");
        }
    }
}
