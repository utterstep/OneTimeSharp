using System;
using VStepanov.OneTimeSharp.DataStructures.Json;

namespace VStepanov.OneTimeSharp
{
    /// <summary>
    /// The exception that is thrown when an error occurs while working with the OneTimeSecret API.
    /// </summary>
    [Serializable]
    public class OtsApiException : Exception
    {
        public OtsApiException() { }
        public OtsApiException(string message) : base(message) { }
        public OtsApiException(string message, Exception inner) : base(message, inner) { }
        internal OtsApiException(ErrorMessage message) : base(message.Message) { }
        internal OtsApiException(ErrorMessage message, Exception inner) : base(message.Message, inner) { }
        protected OtsApiException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
