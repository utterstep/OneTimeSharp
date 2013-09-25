using System.Runtime.Serialization;

namespace VStepanov.OneTimeSharp.DataStructures.Json
{
    /// <summary>
    /// Contains the metadata of generated secret.
    /// </summary>
    [DataContract]
    public class GeneratedSecret : Metadata
    {
        [DataMember]
        internal string value;

        /// <summary>
        /// The randomly generated secret.
        /// </summary>
        public string Value { get { return value; } }
    }
}
