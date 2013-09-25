using System.Runtime.Serialization;

namespace VStepanov.OneTimeSharp.DataStructures.Json
{
    /// <summary>
    /// Abstract class representing the metadata of a secret.
    /// </summary>
    [DataContract]
    public abstract class Metadata
    {
        [DataMember]
        internal string custid;

        [DataMember]
        internal string metadata_key;

        [DataMember]
        internal string secret_key;

        [DataMember]
        internal int ttl;

        [DataMember]
        internal int metadata_ttl;

        [DataMember]
        internal int secret_ttl;

        [DataMember]
        internal string recepient;

        [DataMember]
        internal int created;

        [DataMember]
        internal int updated;

        [DataMember]
        internal bool passphrase_required;

        /// <summary>
        /// Your ID in OTS system.
        /// </summary>
        public string CustomerId { get { return custid; } }

        /// <summary>
        /// The unique key to access the metadata. 
        /// 
        /// DO NOT share this.
        /// </summary>
        public string MetadataKey { get { return metadata_key; } }

        /// <summary>
        /// The unique key to access the secret you created. 
        /// 
        /// This is key that you can share.
        /// </summary>
        public string SecretKey { get { return secret_key; } }

        /// <summary>
        /// The time-to-live that was specified (i.e. not the time remaining).
        /// </summary>
        public int Ttl { get { return ttl; } }

        /// <summary>
        /// The remaining time (in seconds) that the metadata has left to live.
        /// </summary>
        public int MetadataTtl { get { return metadata_ttl; } }

        /// <summary>
        /// The remaining time (in seconds) that the secret has left to live.
        /// </summary>
        public int SecretTtl { get { return secret_ttl; } }

        /// <summary>
        /// If a recipient was specified, this is an obfuscated version of the email address.
        /// </summary>
        public string Recepient { get { return recepient; } }

        /// <summary>
        /// Time the secret was created in UNIX time (UTC).
        /// </summary>
        public int Created { get { return created; } }

        /// <summary>
        /// TODO: Ask Delano for the meaning of this field.
        /// </summary>
        public int Updated { get { return updated; } }

        /// <summary>
        /// Indicates, was password provided while creating a secret.
        /// </summary>
        public bool PassphraseRequired { get { return passphrase_required; } }
    }
}
