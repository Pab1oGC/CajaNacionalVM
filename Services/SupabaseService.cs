using Supabase;
using Supabase.Realtime.Converters;

namespace CNSVM.Services
{
	public class SupabaseService
	{
		private readonly Client _client;
		public SupabaseService(Client client)
		{
			_client = client;
		}
		public async Task<string> GetPublicImageUrl(int id)
		{
			string bucketName = "Photos";
			try
			{
				var response = _client.Storage.From(bucketName).GetPublicUrl($"{id}.jpeg");
				using (var httpClient = new HttpClient())
				{
					var headResponse = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Head, response));

					if (headResponse.IsSuccessStatusCode)
					{
						return response;
					}
					else
					{
						return "~/images/default.png";
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}
	}
}