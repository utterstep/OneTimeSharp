using System;

namespace VStepanov.OneTimeSharp.DataStructures
{
    /// <summary>
    /// Represents the parameters to use while the creation of a new secret.
    /// </summary>
    public struct SharingParams
    {
        internal string Password { get; private set; }

        internal string Recipient { get; private set; }
        
        internal string Ttl { get; private set; }

        /// <summary>
        /// Create new <see cref="SharingParams"/> object, using the given parameters.
        /// </summary>
        /// <param name="password">Desired password. See <see cref="SetPassword"/>.</param>
        /// <param name="recipient">Valid e-mail adress. See <see cref="SetRecipient"/>.</param>
        /// <param name="ttl">TTL (seconds). See <see cref="SetTtl"/>.</param>
        public SharingParams(string password, string recipient=null, string ttl=null)
            : this()
        {
            SetPassword(password);
            SetRecipient(recipient);
            SetTtl(ttl);
        }

        #region Setters
        /// <summary>
        /// Set password which recipient must know in order to view the secret.
        /// This value is also used to encrypt the secret and is BCrypt'ed before being stored.
        /// </summary>
        /// <param name="password">Desired password.</param>
        public void SetPassword(string password)
        {
            Password = password;
        }

        /// <summary>
        /// Set the recipient of a secret.
        /// He will get the e-mail, containing secret link (not a secret itself).
        /// </summary>
        /// <param name="recipient">Valid e-mail adress.</param>
        public void SetRecipient(string recipient)
        {
            Recipient = recipient;
        }

        /// <summary>
        /// Set the maximum amount of time, in seconds, that the secret should survive (i.e. time-to-live).
        /// Once this time expires, the secret will be deleted and not recoverable.
        /// </summary>
        /// <param name="ttl">TTL (seconds).</param>
        public void SetTtl(string ttl)
        {
            Ttl = ttl;
        }

        /// <summary>
        /// Set the maximum amount of time, in seconds, that the secret should survive (i.e. time-to-live).
        /// Once this time expires, the secret will be deleted and not recoverable.
        /// </summary>
        /// <param name="ttl">TTL (seconds).</param>
        public void SetTtl(int ttl)
        {
            Ttl = ttl.ToString();
        }

        /// <summary>
        /// Set the maximum amount of time, in seconds, that the secret should survive (i.e. time-to-live).
        /// Once this time expires, the secret will be deleted and not recoverable.
        /// </summary>
        /// <param name="ttl">TTL as .</param>
        public void SetTtl(TimeSpan ttl)
        {
            Ttl = ttl.TotalSeconds.ToString();
        } 
        #endregion

        #region Resetters
        /// <summary>
        /// Remove the password.
        /// </summary>
        public void RemovePassword()
        {
            Password = null;
        }

        /// <summary>
        /// Remove the recipient.
        /// </summary>
        public void RemoveRecipient()
        {
            Recipient = null;
        }

        /// <summary>
        /// Remove the TTL.
        /// </summary>
        public void RemoveTtl()
        {
            Ttl = null;
        } 
        #endregion
    }
}
