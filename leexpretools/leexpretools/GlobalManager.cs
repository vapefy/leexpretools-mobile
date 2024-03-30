using leexpretools.Models;
using leexpretools.Services;

namespace leexpretools {
    public class GlobalManager {
        private static GlobalManager _instance;

        public static GlobalManager Instance => _instance ?? (_instance = new GlobalManager());

        public Market Market { get; set; }
        public string User { get; set; }
        public Language AppLanguage { get; set; }

        public void SetLanguage(string lang) {
            AppLanguage.Lang = lang;
        }
        
        public LocalDataStore LocalDataStore { get; set; }
        public DataStore DataStore { get; set; }
    }
}