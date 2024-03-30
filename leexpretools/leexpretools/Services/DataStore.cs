using leexpretools.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace leexpretools.Services
{
    public class DataStore {

		private readonly HttpClient _client = new HttpClient();
		private const string ApiBase = "http://172.104.227.188/api";
		
	    public async Task<bool> AddItemAsync(Item item) {
			var dictionary = new Dictionary<string, object> {
				{ "id", item.Id },
				{ "name", item.Name },
				{ "expires", item.Expires.ToString("dd.MM.yyyy") },
				{ "ean",  item.Ean},
				{ "description", item.Description },
				{ "area_id", item.Area.Id },
				{ "group_id", item.ItemGroup.Id },
				{ "expired", item.Expired },
				{ "price", item.Price },
				{ "zone", item.Zone }
			};

			var data = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
			var content = new StringContent(data, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync(ApiBase + "/items", content);
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseContent);

			return await Task.FromResult(true);
		}

		public async Task<bool> UpdateItemAsync(Item item) {
			var dictionary = new Dictionary<string, object> {
				{ "id", item.Id },
				{ "name", item.Name },
				{ "expires", item.Expires.ToString("dd.MM.yyyy") },
				{ "ean",  item.Ean},
				{ "description", item.Description },
				{ "area_id", item.Area.Id },
				{ "group_id", item.ItemGroup.Id },
				{ "expired", item.Expired },
				{ "price", item.Price },
				{ "zone", item.Zone }

			};

			var data = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
			var content = new StringContent(data, Encoding.UTF8, "application/json");
			var response = await _client.PutAsync(ApiBase + "/items", content);
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseContent);

			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteItemAsync(int id) {
			var response = await _client.DeleteAsync(ApiBase + "/items/" + id.ToString());
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseContent);

			return await Task.FromResult(true);
		}

		public async Task<Item> GetItemAsync(int id) {
			var response = await _client.GetAsync(ApiBase + "/items/" + id.ToString());
			var responseContent = await response.Content.ReadAsStringAsync();
			var jsonObject = JObject.Parse(responseContent);
			var itemJson = jsonObject["item"];
			var areaJson = itemJson["area"];
			var groupJson = itemJson["group"];

			var group = new Item() {
				Id = (int)itemJson["id"],
				Name = (string)itemJson["name"],
				Expires = DateTime.ParseExact((string)itemJson["expires"], "MM.dd.yyyy", CultureInfo.InvariantCulture),
				Ean = (string)itemJson["ean"],
				Description = (string)itemJson["description"],
				Area = new Area() {Id = (int)areaJson["id"], Name = (string)areaJson["name"], Description = (string)areaJson["description"], 
					MarketId = (int)areaJson["market"]["id"]},
				ItemGroup = new ItemGroup() {Id = (int)groupJson["id"], Name = (string)groupJson["name"], Offset = (int)groupJson["offset"], 
					YellowFlag = (int)groupJson["yellow_flag"], RedFlag = (int)groupJson["red_flag"], MarketId = (int)groupJson["market_id"]},
				Price = (float)itemJson["price"],
				Zone = (string)itemJson["zone"],
				Expired = (bool)itemJson["expired"]


			};

			return await Task.FromResult(group);
		}

		public async Task<IEnumerable<Item>> GetItemsAsync() {
			var response = await _client.GetAsync(ApiBase + "/items");
			var responseContent = await response.Content.ReadAsStringAsync();
			var jsonObject = JObject.Parse(responseContent);
			var itemsListJson = jsonObject["item"].ToList();
			Console.WriteLine(itemsListJson.Count);
			var itemList = new List<Item>();

			foreach (var a in itemsListJson) {
				Color flagColor;
				bool expired = false;
				var expires = DateTime.ParseExact((string)a["expires"], "dd.MM.yyyy",
					CultureInfo.CreateSpecificCulture("de-DE"), DateTimeStyles.None);
				int redFlag = (int)a["group"]["red_flag"];
				int yellowFlag = (int)a["group"]["yellow_flag"];
				int daysDiff = (expires - DateTime.Now).Days;
				

				if (daysDiff <= 0) {
					flagColor = Color.Default;
					expired = true;
				} else if (daysDiff <= redFlag) {
					flagColor = Color.IndianRed;
				} else if (daysDiff <= yellowFlag) {
					flagColor = Color.DarkGoldenrod;
				} else {
					flagColor = Color.DarkGreen;
				}
				itemList.Add(new Item() {
					Id = (int)a["id"],
					Name = (string)a["name"],
					Expires = expires,
					Ean = (string)a["ean"],
					Description = (string)a["description"],
					Area = new Area() {Id = (int)a["area"]["id"], Name = (string)a["area"]["name"], Description = (string)a["area"]["description"], 
						MarketId = (int)a["area"]["market"]["id"]},
					ItemGroup = new ItemGroup() {Id = (int)a["group"]["id"], Name = (string)a["group"]["name"], Offset = (int)a["group"]["offset"], 
						YellowFlag = (int)a["group"]["yellow_flag"], RedFlag = (int)a["group"]["red_flag"], MarketId = (int)a["group"]["market_id"]},
					Price = (float)a["price"],
					Zone = (string)a["zone"],
					Expired = expired,
					FlagColor = flagColor
				});
			}

			IEnumerable<Item> items = itemList;
			return await Task.FromResult(items);
		}

		public async Task<IEnumerable<Item>> GetYellowItems() {
			var allItems = await GetItemsAsync();
			var yellowItems = new List<Item>();

			foreach (var item in allItems) {
				if (item.FlagColor == Color.DarkGoldenrod || item.FlagColor == Color.IndianRed) {
					yellowItems.Add(item);
				}
			}
			IEnumerable<Item> items = yellowItems;
			return await Task.FromResult(items);
		}
		
	    public async Task<bool> AddItemGroupAsync(ItemGroup group) {
			var dictionary = new Dictionary<string, object> {
				{ "id", group.Id },
				{ "name", group.Name },
				{ "offset", group.Offset },
				{ "yellow_flag", group.YellowFlag },
				{ "red_flag", group.RedFlag }
			};

			var data = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
			var content = new StringContent(data, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync(ApiBase + "/item_groups", content);
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseContent);

			return await Task.FromResult(true);
		}

		public async Task<bool> UpdateItemGroupAsync(ItemGroup group) {
			var dictionary = new Dictionary<string, object> {
				{ "id", group.Id },
				{ "name", group.Name },
				{ "offset", group.Offset },
				{ "yellow_flag", group.YellowFlag },
				{ "red_flag", group.RedFlag }
			};

			var data = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
			var content = new StringContent(data, Encoding.UTF8, "application/json");
			var response = await _client.PutAsync(ApiBase + "/item_groups", content);
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseContent);

			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteItemGroupAsync(int id) {
			var response = await _client.DeleteAsync(ApiBase + "/item_groups/" + id.ToString());
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseContent);

			return await Task.FromResult(true);
		}

		public async Task<ItemGroup> GetItemGroupAsync(int id) {
			var response = await _client.GetAsync(ApiBase + "/item_groups/" + id.ToString());
			var responseContent = await response.Content.ReadAsStringAsync();
			var jsonObject = JObject.Parse(responseContent);
			var groupJson = jsonObject["item group"];
			var group = new ItemGroup() {
				Id = (int)groupJson["id"],
				Name = (string)groupJson["name"],
				Offset = (int)groupJson["offset"],
				YellowFlag = (int)groupJson["yellow_flag"],
				RedFlag = (int)groupJson["red_flag"],
				MarketId = (int)groupJson["market_id"]

			};

			return await Task.FromResult(group);
		}

		public async Task<IEnumerable<ItemGroup>> GetItemGroupsAsync() {
			var response = await _client.GetAsync(ApiBase + "/item_groups");
			var responseContent = await response.Content.ReadAsStringAsync();
			var jsonObject = JObject.Parse(responseContent);
			var groupListJson = jsonObject["item group"].ToList();
			Console.WriteLine(responseContent);
			var groupList = groupListJson.Select(a => new ItemGroup() {
				Id = (int)a["id"],
				Name = (string)a["name"],
				Offset = (int)a["offset"],
				YellowFlag = (int)a["yellow_flag"],
				RedFlag = (int)a["red_flag"],
				MarketId = (int)a["market_id"]

			}).ToList();
			
			foreach(var group in groupList) {
				Console.WriteLine(group.Name);
			}

			IEnumerable<ItemGroup> groups = groupList;
			return await Task.FromResult(groups);
		}
		
		public async Task<bool> AddAreaAsync(Area area) {
			var dictionary = new Dictionary<string, object> {
				{ "id", area.Id },
				{ "name", area.Name },
				{ "description", area.Description },
				{ "market_id", area.MarketId }
			};

			var data = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
			var content = new StringContent(data, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync(ApiBase + "/areas", content);
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseContent);

			return await Task.FromResult(true);
		}

		public async Task<bool> UpdateAreaAsync(Area area) {
			var json = JsonConvert.SerializeObject(area);
			var data = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _client.PutAsync(ApiBase + "/areas", data);
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseContent);

			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteAreaAsync(int id) {
			var response = await _client.DeleteAsync(ApiBase + "/areas/" + id.ToString());
			var responseContent = await response.Content.ReadAsStringAsync();
			Console.WriteLine(responseContent);

			return await Task.FromResult(true);
		}

		public async Task<Area> GetAreaAsync(int id) {
			var response = await _client.GetAsync(ApiBase + "/areas/" + id.ToString());
			var responseContent = await response.Content.ReadAsStringAsync();
			var jsonObject = JObject.Parse(responseContent);
			var areaJson = jsonObject["area"];
			var area = new Area() {
				Id = (int)areaJson["id"],
				Name = (string)areaJson["name"],
				Description = (string)areaJson["description"],
				MarketId = (int)areaJson["market_id"]
			};

			return await Task.FromResult(area);
		}

		public async Task<IEnumerable<Area>> GetAreasAsync() {
			var response = await _client.GetAsync(ApiBase + "/areas");
			var responseContent = await response.Content.ReadAsStringAsync();
			var jsonObject = JObject.Parse(responseContent);
			var areaListJson = jsonObject["area"].ToList();

			var areasList = areaListJson.Select(a => new Area {
				Id = (int)a["id"],
				Name = (string)a["name"],
				Description = (string)a["description"],
				MarketId = (int)a["market"]["id"]

			}).ToList();

			IEnumerable<Area> areas = areasList;
			return await Task.FromResult(areas);
		}
		
		public async Task<Market> GetMarketAsync(int id) {
			var response = await _client.GetAsync(ApiBase + "/markets/" + id.ToString());
			var responseContent = await response.Content.ReadAsStringAsync();
			var jsonObject = JObject.Parse(responseContent);
			var areaJson = jsonObject["market"];
			var locationJson = areaJson["location"];

			var market = new Market() {
				Id = (int)areaJson["id"],
				Name = (string)areaJson["name"],
				Description = (string)areaJson["description"],
				Location = new Location() {
					Id = "",
					Street = (string)locationJson["street"],
					StreetNo = (string)locationJson["street_number"],
					City = (string)locationJson["city"],
					Zip = (string)locationJson["zip"],
					Country = (string)locationJson["country"]
				}
			};

			return await Task.FromResult(market);
		}

		public async Task<string> CheckLoginCredentials(int marketId, string username, string password) {
			var dictionary = new Dictionary<string, object> {
				{ "email", username },
				{ "password", password },
				{ "market_id", marketId }
			};

			var data = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
			var content = new StringContent(data, Encoding.UTF8, "application/json");
			var response = await _client.PostAsync(ApiBase + "/login", content);
			var jsonString = await response.Content.ReadAsStringAsync();
			var responseData = new Dictionary<string, string>();
			try {
				responseData = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);
			} catch (JsonException e) {
				Console.WriteLine("Error parsing JSON response: " + e.Message);
			}
			
			return await Task.FromResult(responseData["message"]);
		}
	}
}