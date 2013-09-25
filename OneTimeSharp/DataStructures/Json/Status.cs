using System;
using System.Runtime.Serialization;

namespace VStepanov.OneTimeSharp.DataStructures.Json
{
    /// <summary>
    /// Represents status of the OTS server.
    /// </summary>
    [DataContract]
    public class Status
    {
        /// <summary>
        /// OneTimeSecret server status.
        /// </summary>
        public enum SystemStatus
        {
            /// <summary>
            /// Server is fully functional.
            /// </summary>
            Nominal,

            /// <summary>
            /// Server is offline.
            /// </summary>
            Offline,

            /// <summary>
            /// The state is unknown. See <see cref="StatusAnswer"/>.
            /// </summary>
            Unknown
        }

        [DataMember]
        internal string status;

        /// <summary>
        /// The raw status string which the server has retuned.
        /// </summary>
        public string StatusAnswer { get { return status; } }

        /// <summary>
        /// Current status of the OTS server.
        /// </summary>
        public SystemStatus OtsStatus
        {
            get
            {
                if (status == "nominal")
                {
                    return SystemStatus.Nominal;
                }
                if (status == "offline")
                {
                    return SystemStatus.Offline;
                }
                return SystemStatus.Unknown;
            }
        }
    }
}
