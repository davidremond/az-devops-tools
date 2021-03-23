using System;
using System.Runtime.Serialization;

namespace Az.DevOps.Tools.CLI
{
    [Serializable]
    public class AzDevOpsToolsException : Exception
    {
        public AzDevOpsToolsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        internal AzDevOpsToolsException(string message)
            : base(message)
        {
        }

        protected AzDevOpsToolsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}