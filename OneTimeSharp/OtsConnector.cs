using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;
using VStepanov.OneTimeSharp.DataStructures;
using VStepanov.OneTimeSharp.DataStructures.Json;


namespace VStepanov.OneTimeSharp
{
    /// <summary>
    /// The connector to the OneTimeSecret server.
    /// </summary>
    public class OneTimeSecret : IDisposable
    {
        #region Constants
        private const string DefaultApiUrl = "https://onetimesecret.com/api/v1";
        #endregion

        #region Properties
        /// <summary>
        /// The OTS API URL used by the connector.
        /// </summary>
        public string ApiUrl { get; private set; }

        /// <summary>
        /// The OTS username.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// The OTS API key.
        /// </summary>
        public string ApiKey { get; private set; } 
        #endregion

        #region Fields (private)
        private readonly WebClient _webClient; 
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes new OTS connector instance using passed username, API key and API url, if needed.
        /// </summary>
        /// <param name="username">Username for OTS API.</param>
        /// <param name="apiKey">User-specific key for OTS API.</param>
        /// <param name="apiUrl">URL of the OTS API server (optional, default is "https://onetimesecret.com/api/v1").</param>
        public OneTimeSecret(string username, string apiKey, string apiUrl=DefaultApiUrl)
            : this(apiUrl)
        {
            Username = username;
            ApiKey = apiKey;

            var authToken = Convert.ToBase64String(Encoding.Default.GetBytes(Username + ":" + ApiKey));
            _webClient.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authToken);
        }

        /// <summary>
        /// Initializes new anonymous OTS connector, using given API url, if needed so. 
        /// </summary>
        /// <param name="apiUrl">URL of the OTS API server (optional, default is "https://onetimesecret.com/api/v1").</param>
        public OneTimeSecret(string apiUrl=DefaultApiUrl)
        {
            ApiUrl = apiUrl;
            Username = "anon";

            _webClient = new WebClient();
        } 

        #endregion

        #region API methods

        #region Creating a secret

        #region Sharing methods
        /// <summary>
        /// Shares secret message.
        /// </summary>
        /// <param name="secret">Secret message to share.</param>
        /// <returns>Metadata object related to shared secret.</returns>
        public SharedSecret ShareSecret(string secret)
        {
            const string urlPartShare = "/share";
            var shareUrl = ApiUrl + urlPartShare;

            var postData = new Dictionary<string, string> { { "secret", secret } };

            return ApiPostRequest<SharedSecret>(shareUrl, postData);
        }

        /// <summary>
        /// Shares secret message, using given sharing parameters.
        /// </summary>
        /// <param name="secret">Secret message to share.</param>
        /// <param name="sharingParams">Sharing parameters. See <see cref="VStepanov.OneTimeSharp.DataStructures.SharingParams"/>.</param>
        /// <returns>Metadata object related to shared secret.</returns>
        public SharedSecret ShareSecret(string secret, SharingParams sharingParams)
        {
            const string urlPartShare = "/share";
            var shareUrl = ApiUrl + urlPartShare;

            var postData = SharingParamsToPost(sharingParams);
            postData.Add("secret", secret);

            return ApiPostRequest<SharedSecret>(shareUrl, postData);
        }

        /// <summary>
        /// Shares secret message, securing it with given <paramref name="password"/>.
        /// </summary>
        /// <param name="secret">Secret message to share.</param>
        /// <param name="password">A string that the recipient must know to view the secret.
        /// This value is also used to encrypt the secret and is bcrypted before being stored so OTS server only have this value in transit.</param>
        /// <returns>Metadata object related to shared secret.</returns>
        public SharedSecret ShareSecret(string secret, string password)
        {
            return ShareSecret(secret, new SharingParams(password));
        }
        #endregion

        #region Generating methods
        /// <summary>
        /// Generate a short, unique secret. This is useful for temporary passwords, one-time pads, salts, etc.
        /// </summary>
        /// <returns>Metadata object related to generated secret.</returns>
        public GeneratedSecret GenerateSecret()
        {
            const string urlPartGenerate = "/generate";
            var generateUrl = ApiUrl + urlPartGenerate;

            return ApiPostRequest<GeneratedSecret>(generateUrl);
        }

        /// <summary>
        /// Generate a short, unique secret, using given sharing parameters.
        /// 
        /// This is useful for temporary passwords, one-time pads, salts, etc.
        /// </summary>
        /// <param name="sharingParams">Sharing parameters. See <see cref="SharingParams"/>.</param>
        /// <returns>Metadata object related to generated secret.</returns>
        public GeneratedSecret GenerateSecret(SharingParams sharingParams)
        {
            const string urlPartGenerate = "/generate";
            var generateUrl = ApiUrl + urlPartGenerate;

            var postData = SharingParamsToPost(sharingParams);

            return ApiPostRequest<GeneratedSecret>(generateUrl, postData);
        }

        /// <summary>
        /// Generate short, unique secret, secruring it using given password.
        /// 
        /// </summary>
        /// <param name="password">A string that the recipient must know to view the secret.
        /// This value is also used to encrypt the secret and is bcrypted before being stored so we only have this value in transit.</param>
        /// <returns>Metadata object related to generated secret.</returns>
        public GeneratedSecret GenerateSecret(string password)
        {
            return GenerateSecret(new SharingParams(password));
        }
        #endregion

        #endregion

        #region Gathering information

        #region Retrieving secret
        /// <summary>
        /// Retrieves secret using its secret key.
        /// </summary>
        /// <param name="secretKey">The unique key for this secret.</param>
        /// <returns>Retrieved secret.</returns>
        public Secret RetrieveSecret(string secretKey)
        {
            const string urlPartSecret = "/secret/";
            var secretUrl = ApiUrl + urlPartSecret + secretKey;

            return ApiPostRequest<Secret>(secretUrl);
        }

        /// <summary>
        /// Retrieves secret using its <paramref name="metadata"/>.
        /// </summary>
        /// <param name="metadata">The unique metadata object, containing info about the secret.</param>
        /// <returns>Retrieved secret.</returns>
        public Secret RetrieveSecret(Metadata metadata)
        {
            if (metadata.PassphraseRequired)
            {
                throw new Exception("Password required");
            }

            return RetrieveSecret(metadata.SecretKey);
        }

        /// <summary>
        /// Retrieves secret, secured with user <paramref name="password"/>, using its secret key.
        /// </summary>
        /// <param name="secretKey">The unique key for this secret.</param>
        /// <param name="password">The passphrase secret was created with.</param>
        /// <returns>Retrieved secret.</returns>
        public Secret RetrieveSecret(string secretKey, string password)
        {
            const string urlPartSecret = "/secret/";
            var secretUrl = ApiUrl + urlPartSecret + secretKey;

            var postData = new Dictionary<string, string> { { "passphrase", password } };

            return ApiPostRequest<Secret>(secretUrl, postData);
        }

        /// <summary>
        /// Retrieves secret, secured with user <paramref name="password"/>, using its secret key.
        /// </summary>
        /// <param name="metadata">The unique metadata object, containing info about the secret.</param>
        /// <param name="password">The passphrase secret was created with.</param>
        /// <returns>Retrieved secret.</returns>
        public Secret RetrieveSecret(Metadata metadata, string password)
        {
            return RetrieveSecret(metadata.SecretKey, password);
        }
        #endregion

        #region Retrieving metadata
        /// <summary>
        /// Retrieves basic information about the secret itself (e.g. if or when it was viewed) using the the metadata key.
        /// </summary>
        /// <param name="metadataKey">The unique key for this metadata.</param>
        /// <returns>Retrieved metadata.</returns>
        public SecretMetadata RetrieveMetadata(string metadataKey)
        {
            const string urlPartMetadata = "/private/";
            var metadataUrl = ApiUrl + urlPartMetadata + metadataKey;

            return ApiPostRequest<SecretMetadata>(metadataUrl);
        }
        #endregion

        #endregion

        #region Service methods

        /// <summary>
        /// Gets current status of the system.
        /// </summary>
        /// <returns>The current status of the OTS server.</returns>
        public Status GetStatus()
        {
            const string urlPartStatus = "/status";
            var statusUrl = ApiUrl + urlPartStatus;

            return ApiGetRequest<Status>(statusUrl);
        }

        /// <summary>
        /// Registers a new user on the OTS server.
        /// 
        /// Will be availible as soon as API changes.
        /// </summary>
        /// <param name="username">Desired username.</param>
        /// <param name="password">Desired password.</param>
        public static void RegisterUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        #endregion 

        #endregion

        #region Helper methods

        /// <summary>
        /// Creates URL, which can be used to view given secret.
        /// 
        /// Assumes URL is %base_url%/secret/%SECRET_KEY%.
        /// </summary>
        /// <param name="metadata">Metadata object linked to the secret, needed to be accessed.</param>
        /// <returns>URL of the secret.</returns>
        public string GetSecretLink(Metadata metadata)
        {
            var url = new Uri(ApiUrl);
            var rootUrl = url.GetLeftPart(UriPartial.Authority);

            return rootUrl + "/secret/" + metadata.SecretKey;
        }

        /// <summary>
        /// Creates URL, which can be used to view given secret information.
        /// 
        /// Assumes URL is %base_url%/private/%METADATA_KEY%.
        /// </summary>
        /// <param name="metadata">Metadata object linked to the secret, needed to be accessed.</param>
        /// <returns>URL of the information about the secret.</returns>
        public string GetMetadataLink(Metadata metadata)
        {
            var url = new Uri(ApiUrl);
            var rootUrl = url.GetLeftPart(UriPartial.Authority);

            return rootUrl + "/private/" + metadata.MetadataKey;
        }

        private T ApiPostRequest<T>(string url, Dictionary<string, string> data=null)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var jsonAnswerStream = GetPostResult(url, data);
            
            return (T)serializer.ReadObject(jsonAnswerStream);    
        }

        private T ApiGetRequest<T>(string url)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var jsonAnswerStream = GetGetResult(url);

            return (T)serializer.ReadObject(jsonAnswerStream);
        }

        private MemoryStream GetPostResult(string url, Dictionary<string, string> data=null)
        {
            var postBytes = PostDataToBytes(data);

            try
            {
                var bytes = _webClient.UploadData(url, "POST", postBytes);

                return new MemoryStream(bytes, false);
            }
            catch (WebException ex)
            {
                var serializer = new DataContractJsonSerializer(typeof(ErrorMessage));
                ErrorMessage message;

                try { message = (ErrorMessage)serializer.ReadObject(ex.Response.GetResponseStream()); }
                catch (Exception) { throw ex; }

                throw new OtsApiException(message);
            }
        }

        private MemoryStream GetGetResult(string url)
        {
            var bytes = _webClient.DownloadData(url);

            return new MemoryStream(bytes, false);
        }

        private static Dictionary<string, string> SharingParamsToPost(SharingParams sharingParams)
        {
            var postData = new Dictionary<string, string>();

            if (sharingParams.Password != null)
            {
                postData.Add("passphrase", sharingParams.Password);
            }
            if (sharingParams.Ttl != null)
            {
                postData.Add("ttl", sharingParams.Ttl);
            }
            if (sharingParams.Recipient != null)
            {
                postData.Add("recipient", sharingParams.Recipient);
            }

            return postData;
        }

        private static byte[] PostDataToBytes(Dictionary<string, string> data=null)
        {
            if (data == null)
            {
                data = new Dictionary<string, string>();
            }

            string postData = "";

            foreach (var pair in data)
            {
                postData +=
                    HttpUtility.HtmlEncode(pair.Key) + "=" +
                    HttpUtility.HtmlEncode(pair.Value) + "&";
            }

            return Encoding.UTF8.GetBytes(postData);
        }

        #endregion

        public void Dispose()
        {
            _webClient.Dispose();
        }
    }
}
