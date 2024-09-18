using MESClient.MessageType;
using Newtonsoft.Json;
using System.Text;
using Gebhardt.Shared;


namespace MESClient
{
	public class TransportRequest : MESClient
	{
		public TransportRequest(IConfiguration configuration) : base(configuration)
		{
			header = new Header();
			header.InterfaceCode = "TransportRequest";
		}

		public async Task<TransportRequestResultMessage> MakeRequest(TransportRequestMessage requestData)
		{
			//const string sURL = "http://cpec1qmes002v/Apriso/WebServices/Public/TransportRequest";
			using (var client = new HttpClient()) // Use HttpClient and dispose it
			{
				MesRequestMsg msg = new MesRequestMsg();
				msg.Header = header;
				msg.RequestJSON = requestData;

				string data = Newtonsoft.Json.JsonConvert.SerializeObject(msg);
			
				try
				{
					var content = new StringContent(data, Encoding.UTF8, "application/json");
					var response = await client.PostAsync(transportRequestUrl, content);
					if (response.StatusCode != System.Net.HttpStatusCode.OK)
					{
						throw new HttpRequestException("Mes Server Response fail - Status Code:" + response.StatusCode);
					}         
					
					// Handle successful response (e.g., deserialize response content)
					string responseContent = await response.Content.ReadAsStringAsync();
					// ... process responseContent
					MesResponseMsg mesResponseMsg = new MesResponseMsg();
					mesResponseMsg = JsonConvert.DeserializeObject<MesResponseMsg>(responseContent);

					return JsonConvert.DeserializeObject<TransportRequestResultMessage>(mesResponseMsg.Output.ToString());

				}
				catch (HttpRequestException ex)
				{
					// Handle HTTP errors
					Log.WriteException (ex);
					//Console.WriteLine($"Error during request: {ex.Message}");
					throw; // Re-throw or handle the exception differently
				}
				catch (Exception ex)
				{
					Log.WriteException(ex);
					throw;
				}
			}
		}


	}
}
