using Gebhardt.StoreWare.WcsWms.Constants;
using MESClient.MessageType;
using Newtonsoft.Json;
using System.Text;
using Gebhardt.Shared;


namespace MESClient
{
	public class StayEnd : MESClient
	{
		public StayEnd(IConfiguration configuration) : base(configuration)
		{
			header = new Header();
			header.InterfaceCode = "StayEnd";
		}

		public async Task<StayEndMessageResult> MakeRequest(StayEndMessage requestData)
		{
			using (var client = new HttpClient()) // Use HttpClient and dispose it
			{
				string data = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

				try
				{
					var content = new StringContent(data, Encoding.UTF8, "application/json");
					var response = await client.PostAsync(stayEndUrl, content);
					if (response.StatusCode != System.Net.HttpStatusCode.OK)
					{
						throw new HttpRequestException("Mes Server Response fail - Status Code: " + response.StatusCode);
					}
					// Handle successful response (e.g., deserialize response content)
					string responseContent = await response.Content.ReadAsStringAsync();
					// ... process responseContent

					MesResponseMsg mesResponseMsg = new MesResponseMsg();
					mesResponseMsg = JsonConvert.DeserializeObject<MesResponseMsg>(responseContent);

					return JsonConvert.DeserializeObject<StayEndMessageResult>(mesResponseMsg.Output.ToString());
				}
				catch (HttpRequestException ex)
				{
					// Handle HTTP errors
					Log.WriteException(ex);
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
