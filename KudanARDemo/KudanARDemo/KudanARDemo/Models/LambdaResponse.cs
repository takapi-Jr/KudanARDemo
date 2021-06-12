using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace KudanARDemo.Models
{
    #region Common
    [DataContract]
    public class LambdaResult
    {
        [DataMember(Name = "status_code")]
        public int StatusCode { get; set; }
    }
    #endregion



    #region GetKudanARApiKey
    [DataContract]
    public class GetKudanARApiKey
    {
        [DataMember(Name = "GetKudanARApiKey")]
        public GetKudanARApiKeyResponse Response { get; set; }
    }

    [DataContract]
    public class GetKudanARApiKeyResponse
    {
        [DataMember(Name = "result")]
        public LambdaResult Result { get; set; }

        [DataMember(Name = "data")]
        public GetKudanARApiKeyData Data { get; set; }
    }

    [DataContract]
    public class GetKudanARApiKeyData
    {
        [DataMember(Name = "api_key")]
        public string ApiKey { get; set; }
    }
    #endregion
}
