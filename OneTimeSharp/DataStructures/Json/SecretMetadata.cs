using System.Runtime.Serialization;

namespace VStepanov.OneTimeSharp.DataStructures.Json
{
    /// <summary>
    /// Contains the retrieved metadata of a secret.
    /// </summary>
    [DataContract]
    public class SecretMetadata : Metadata
    {
        [DataMember]
        internal int recieved;

        [DataMember]
        internal string state;

        /// <summary>
        /// Time the secret was recieved in UNIX time (UTC).
        /// </summary>
        public int Recieved { get { return recieved; } }

        /// <summary>
        /// State of a secret. 
        /// 
        /// One of: "new", "recieved".
        /// </summary>
        public string State { get { return state; } }
    }
}
