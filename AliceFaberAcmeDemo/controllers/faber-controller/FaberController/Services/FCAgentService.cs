using System;
using System.Net.Http;
using System.Threading.Tasks;
using FaberController.Enums;
using Newtonsoft.Json.Linq;

namespace FaberController.Services
{
    public class FCAgentService
    {
        public FCAgentService(HttpClient Http)
        {
            _http = Http;
        }

        private HttpClient _http { get; }

        public async Task<AgentStatus> GetStatus()
        {
            try
            {
                var response = await _http.GetAsync("http://192.168.1.8:8021/status");
                response.EnsureSuccessStatusCode();
                return AgentStatus.Up;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return AgentStatus.Down;
            }
        }

        public async Task<JArray> GetConnections()
        {
            try
            {
                var response = await _http.GetAsync("http://192.168.1.8:8021/connections");
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseString);
                return jsonResponse.Value<JArray>("results");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new JArray();
            }
        }

        public async Task<JObject> RemoveConnection(string connectionId)
        {
            if (connectionId == null || connectionId == "")
            {
                Console.Error.WriteLine("Must provide a connection ID");
                return new JObject();
            }
            try
            {
                using var content = new StringContent("");
                var response = await _http.PostAsync(string.Format("http://192.168.1.8:8021/connections/{0}/remove", connectionId), content);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseString);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new JObject();
            }
        }

        public async Task<JObject> CreateInvitation()
        {
            try
            {
                using var content = new StringContent("");
                var response = await _http.PostAsync("http://192.168.1.8:8021/connections/create-invitation", content);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseString);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new JObject();
            }
        }

        public async Task<JObject> ReceiveInvitation(string invitation)
        {
            try
            {

                using var content = new StringContent(invitation);
                var response = await _http.PostAsync("http://192.168.1.8:8021/connections/receive-invitation", content);

                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseString);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new JObject();
            }
        }

        public async Task<JArray> GetSchemas()
        {
            try
            {
                var response = await _http.GetAsync("http://192.168.1.8:8021/schemas/created");
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseString);
                return jsonResponse.Value<JArray>("schema_ids");

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new JArray();
            }
        }

        public async Task<JObject> GetSchema(string schemaId)
        {
            try
            {
                var response = await _http.GetAsync(string.Format("http://192.168.1.8:8021/schemas/{0}", schemaId));
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseString);
                return jsonResponse.Value<JObject>("schema_json");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new JObject();
            }
        }

        public async Task<JArray> GetCredentialDefinitions()
        {
            try
            {
                var response = await _http.GetAsync("http://192.168.1.8:8021/credential-definitions/created");
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseString);
                return jsonResponse.Value<JArray>("credential_definition_ids");

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new JArray();
            }
        }

        public async Task<JObject> GetCredentialDefinition(string credentialDefinitionId)
        {
            try
            {
                var response = await _http.GetAsync(string.Format("http://192.168.1.8:8021/credential-definitions/{0}", credentialDefinitionId));
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseString);
                return jsonResponse.Value<JObject>("credential_definition");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new JObject();
            }
        }

        public async Task<JObject> SendCredential(string credential)
        {
            try
            {

                using var content = new StringContent(credential);
                var response = await _http.PostAsync("http://192.168.1.8:8021/issue-credential/send", content);

                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                return JObject.Parse(responseString);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return new JObject();
            }
        }
    }
}
