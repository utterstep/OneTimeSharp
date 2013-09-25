using System.Runtime.Serialization;

namespace VStepanov.OneTimeSharp.DataStructures.Json
{
    [DataContract]
    internal class ErrorMessage
    {
        [DataMember]
        internal string message;

        public string Message { get { return message;} }
    }
}
