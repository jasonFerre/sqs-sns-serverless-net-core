using System;

namespace Base.Serverless.Domain.Entities
{
    public class GenericMessage
    {
        public string Identify { get; set; }
        public string Test { get; set; }
        public DateTime? ProcesseAt { get; set; }
    }
}
