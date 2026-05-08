namespace SomaShare.Services
{
    public class LanguageService
    {
        private string _currentLanguage = "en"; // default English

        public string CurrentLanguage => _currentLanguage;

        public void ToggleLanguage()
        {
            _currentLanguage = _currentLanguage == "en" ? "zu" : "en";
        }

        public string T(string english, string zulu)
        {
            return _currentLanguage == "en" ? english : zulu;
        }
    }
}