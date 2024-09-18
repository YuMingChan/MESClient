using MESClient.MessageType;
using Newtonsoft.Json;
using System.Text;
using Gebhardt.Shared;


namespace MESClient
{
	public class LocationUpdate : MESClient
	{
		public LocationUpdate(IConfiguration configuration) : base(configuration)
		{
			header = new Header();
			header.InterfaceCode = "LocationUpdate";
		}

		public async Task<LocationUpdateMessageResult> MakeRequest(LocationUpdateMessage requestData)
		{
			using (var client = new HttpClient()) // Use HttpClient and dispose it
			{
				string data = Newtonsoft.Json.JsonConvert.SerializeObject(requestData);

				try
				{
					var content = new StringContent(data, Encoding.UTF8, "application/json");
					var response = await client.PostAsync(locationUpdateUrl, content);
					if (response.StatusCode != System.Net.HttpStatusCode.OK)
					{
						throw new HttpRequestException("Mes Server Response fail - Status Code:" + response.StatusCode);
					}
					// Handle successful response (e.g., deserialize response content)
					string responseContent = await response.Content.ReadAsStringAsync();
					// ... process responseContent
					MesResponseMsg mesResponseMsg = new MesResponseMsg();
					mesResponseMsg = JsonConvert.DeserializeObject<MesResponseMsg>(responseContent);

					return JsonConvert.DeserializeObject<LocationUpdateMessageResult>(mesResponseMsg.Output.ToString());
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
