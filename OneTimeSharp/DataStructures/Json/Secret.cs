using System.Runtime.Serialization;

namespace VStepanov.OneTimeSharp.DataStructures.Json
{
    /// <summary>
    /// Contains the information about retrieved secret.
    /// </summary>
    [DataContract]
    public class Secret
    {
        [DataMember]
        internal string secret_key;

        [DataMember]
        internal string value;

        /// <summary>
        /// The unique key to access the secret you created. 
        /// 
        /// This is key that you can share.
        /// </summary>
        public string SecretKey { get { return secret_key; } }
        
        /// <summary>
        /// The secret itself. 
        /// 
        /// Be accurate with this value, you are not able to retrieve this secret ever again.
        /// </summary>
        public string Value { get { return value; } }
    }
}
