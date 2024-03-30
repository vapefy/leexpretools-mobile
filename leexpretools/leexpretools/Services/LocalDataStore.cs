using System.Threading.Tasks;
using Xamarin.Essentials;

namespace leexpretools.Services {
    
    public class LocalDataStore {
        
        public async Task SaveData(string key, string value) {
            await SecureStorage.SetAsync(key, value);
        }
        
        public async Task<string> GetData(string key) {
            var username = await SecureStorage.GetAsync(key);
            return username;
        }
    }
}