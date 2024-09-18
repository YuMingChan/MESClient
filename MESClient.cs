using MESClient.MessageType;

namespace MESClient
{
	
	public class MESClient
	{
		protected string transportRequestUrl = "";
		protected string rackEntryRequestUrl = "";
		protected string stayEndUrl = "";
		protected string locationUpdateUrl = "";
		protected Header header;

		public IConfiguration Configuration { get; }

		public MESClient(IConfiguration configuration)
		{
			Configuration = configuration;

			transportRequestUrl = Configuration["TransportRequestURL"]; // Access the value from environment variable
			rackEntryRequestUrl = Configuration["RackEntryRequestURL"]; // Access the value from environment variable
			stayEndUrl = Configuration["StayEndURL"]; // Access the value from environment variable
			locationUpdateUrl = Configuration["LocationUpdateURL"]; // Access the value from environment variable


		}

	}
}
