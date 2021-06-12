using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KudanARDemo.Models
{
    public class AppSyncService
    {
        public static AppSyncService Instance { get; private set; } = new AppSyncService();
        private GraphQLHttpClient GraphQLHttpClient { get; set; }

        public static readonly string ApiName_GetKudanARApiKey = "GetKudanARApiKey";

        public AppSyncService()
        {
            var options = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(ApiKey.AppSyncApiUrl),
            };
            this.GraphQLHttpClient = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer());
            this.GraphQLHttpClient.HttpClient.DefaultRequestHeaders.Add("x-api-key", ApiKey.AppSyncApiKey);
        }

        /// <summary>
        /// Queryを実行する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="apiName"></param>
        /// <param name="variables"></param>
        /// <returns></returns>
        public async Task<T> ExecQueryAsync<T>(string apiName, object variables = null)
        {
            // GraphQL取得
            var resourceId = $"KudanARDemo.GraphQLs.{apiName}.gql";
            var query = await GetQueryAsync(resourceId);

            // リクエスト作成
            var request = new GraphQLRequest
            {
                Query = query,
                OperationName = apiName,
                Variables = variables,
            };

            // Query実行
            var response = await this.GraphQLHttpClient.SendQueryAsync<T>(request);
            return response.Data;
        }

        /// <summary>
        /// GraphQLを取得
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        private async Task<string> GetQueryAsync(string resourceId)
        {
            var query = string.Empty;
            var assembly = typeof(AppSyncService).GetTypeInfo().Assembly;

            using (var stream = assembly.GetManifestResourceStream(resourceId))
            using (var reader = new StreamReader(stream))
            {
                query = await reader.ReadToEndAsync();
            }

            return query;
        }
    }
}
