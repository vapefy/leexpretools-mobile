namespace leexpretools.Services {
    public class Language {

        private string _lang;
        public string Lang {
            get { return _lang; }
            set { _lang = value; }
        }
        
        public string ErrorTitle {
            get {
                if (Lang.Equals("DE")) {
                    return "Fehler";
                }
                return "Error";
            }
        }

        public string InvalidCredentials {
            get {
                if (Lang.Equals("DE")) {
                    return "Der Benutzername oder das Passwort ist nicht korrekt.";
                }
                return "The entered credentials are not correct.";
            }
        }
        
        public string InvalidMarketId {
            get {
                if (Lang.Equals("DE")) {
                    return "Bitte gebe eine gültige Markt ID ein.";
                }
                return "Please enter a valid market ID.";
            }
        }
        
        
        
    }
}