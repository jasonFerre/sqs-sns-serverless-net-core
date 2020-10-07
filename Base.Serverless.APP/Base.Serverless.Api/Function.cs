using Amazon.Lambda.Core;
using Amazon.SQS;
using Base.Serverless.Api.Interfaces;
using Base.Serverless.Api.Services;
using Base.Serverless.Infrastructure.AWS.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SlackWebHook;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Base.Serverless.Api
{
    public class Functions
    {
        private IServiceCollection _serviceCollection;
        protected IServiceProvider _serviceProvider;
        public IConfiguration _configuration;

        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
            Configure();
            Services();
        }

        private void Configure()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddEnvironmentVariables()
                            .AddJsonFile("appsettings.json");

            _configuration = builder.Build();
        }

        private void Services()
        {
            if (_serviceCollection == null)
                _serviceCollection = new ServiceCollection();
            else
                return;

            //aws services 
            _serviceCollection.AddDefaultAWSOptions(_configuration.GetAWSOptions());

            //example how to inject aws service
            _serviceCollection.AddAWSService<IAmazonSQS>();

            //infra services
            _serviceCollection.AddTransient<ISQSHelper, SQSHelper>();

            //api services
            _serviceCollection.AddTransient<IGenericSQSService, GenericSQSService>();

            //Log.Logger = new LoggerConfiguration()
            //.MinimumLevel.Information()
            //.WriteTo.Slack(
            //    slackWebHookUrl: "https://hooks.slack.com/services/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",
            //    slackChannel: "#serverless-test-notifications",
            //    slackEmojiIcon: ":ghost:",
            //    periodicBatchingSinkOptionsBatchSizeLimit: 1,
            //    periodicBatchingSinkOptionsPeriod: TimeSpan.FromMilliseconds(100),
            //    periodicBatchingSinkOptionsQueueLimit: 10000,
            //    sinkRestrictedToMinimumLevel: LogEventLevel.Information,
            //    slackAddExceptionAttachment: true)
            //.CreateLogger();

            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }
    }
}
