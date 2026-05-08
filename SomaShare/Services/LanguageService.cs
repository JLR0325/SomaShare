namespace SomaShare.Services
{
    public class LanguageService
    {
        public string CurrentLanguage { get; set; } = "en";
        public void SetLanguage(string lang) => CurrentLanguage = lang;
        public string T(string english, string zulu) => CurrentLanguage == "zu" ? zulu : english;
    }
}