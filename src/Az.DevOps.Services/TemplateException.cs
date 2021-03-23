using System;
using System.Runtime.Serialization;

namespace Az.DevOps.Services
{
    [Serializable]
    public class TemplateException : Exception
    {
        public TemplateException(string message)
            : base(message)
        {
        }

        public TemplateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected TemplateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}