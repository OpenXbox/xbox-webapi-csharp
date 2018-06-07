using System;

namespace XboxWebApi.Services
{
    public class XblLocale
    {
        public string Name { get; internal set; }
        public string ShortId { get; internal set; }
        public string Identifier { get; internal set; }
        public string Locale { get; internal set; }
        public XblLocale(string name, string shortId, string identifier, string locale)
        {
            Name = name;
            ShortId = shortId;
            Identifier = identifier;
            Locale = locale;
        }
    }

    public static class XblLanguage
    {
        public static XblLocale Argentina = new XblLocale("Argentina", "AR", "es_AR", "es-AR");
        public static XblLocale Australia = new XblLocale("Australia", "AU", "en_AU", "en-AU");
        public static XblLocale Austria = new XblLocale("Austria", "AT", "de_AT", "de-AT");
        public static XblLocale Belgium = new XblLocale("Belgium", "BE", "fr_BE", "fr-BE");
        public static XblLocale Belgium_NL = new XblLocale("Belgium (NL);", "NL", "nl_BE", "nl-BE");
        public static XblLocale Brazil = new XblLocale("Brazil", "BR", "pt_BR", "pt-BR");
        public static XblLocale Canada = new XblLocale("Canada", "CA", "en_CA", "en-CA");
        public static XblLocale Canada_FR = new XblLocale("Canada (FR);", "CA", "fr_CA", "fr-CA");
        public static XblLocale Czech_Republic = new XblLocale("Czech Republic", "CZ", "en_CZ", "en-CZ");
        public static XblLocale Denmark = new XblLocale("Denmark", "DK", "da_DK", "da-DK");
        public static XblLocale Finland = new XblLocale("Finland", "FI", "fi_FI", "fi-FI");
        public static XblLocale France = new XblLocale("France", "FR", "fr_FR", "fr-FR");
        public static XblLocale Germany = new XblLocale("Germany", "DE", "de_DE", "de-DE");
        public static XblLocale Greece = new XblLocale("Greece", "GR", "en_GR", "en-GR");
        public static XblLocale Hong_Kong = new XblLocale("Hong Kong", "HK", "en_HK", "en-HK");
        public static XblLocale Hungary = new XblLocale("Hungary", "HU", "en_HU", "en-HU");
        public static XblLocale India = new XblLocale("India", "IN", "en_IN", "en-IN");
        public static XblLocale Great_Britain = new XblLocale("Great Britain", "GB", "en_GB", "en-GB");
        public static XblLocale Israel = new XblLocale("Israel", "IL", "en_IL", "en-IL");
        public static XblLocale Italy = new XblLocale("Italy", "IT", "it_IT", "it-IT");
        public static XblLocale Japan = new XblLocale("Japan", "JP", "ja_JP", "ja-JP");
        public static XblLocale Mexico = new XblLocale("Mexico", "MX", "es_MX", "es-MX");
        public static XblLocale Chile = new XblLocale("Chile", "CL", "es_CL", "es-CL");
        public static XblLocale Colombia = new XblLocale("Colombia", "CO", "es_CO", "es-CO");
        public static XblLocale Netherlands = new XblLocale("Netherlands", "NL", "nl_NL", "nl-NL");
        public static XblLocale New_Zealand = new XblLocale("New Zealand", "NZ", "en_NZ", "en-NZ");
        public static XblLocale Norway = new XblLocale("Norway", "NO", "nb_NO", "nb-NO");
        public static XblLocale Poland = new XblLocale("Poland", "PL", "pl_PL", "pl-PL");
        public static XblLocale Portugal = new XblLocale("Portugal", "PT", "pt_PT", "pt-PT");
        public static XblLocale Russia = new XblLocale("Russia", "RU", "ru_RU", "ru-RU");
        public static XblLocale Saudi_Arabia = new XblLocale("Saudi Arabia", "SA", "en_SA", "en-SA");
        public static XblLocale Singapore = new XblLocale("Singapore", "SG", "en_SG", "en-SG");
        public static XblLocale Slovakia = new XblLocale("Slovakia", "SK", "en_SK", "en-SK");
        public static XblLocale South_Africa = new XblLocale("South Afrida", "ZA", "en_ZA", "en-ZA");
        public static XblLocale Korea = new XblLocale("Korea", "KR", "ko_KR", "ko-KR");
        public static XblLocale Spain = new XblLocale("Spain", "ES", "es_ES", "es-ES");
        public static XblLocale Switzerland = new XblLocale("Switzerland", "CH", "de_CH", "de-CH");
        public static XblLocale Switzerland_FR = new XblLocale("Switzerland (FR);", "CH", "fr_CH", "fr-CH");
        public static XblLocale United_Arab_Emirates = new XblLocale("United Arab Emirates", "AE", "en_AE", "en-AE");
        public static XblLocale United_States = new XblLocale("United States", "US", "en_US", "en-US");
        public static XblLocale Ireland = new XblLocale("Ireland", "IE", "en_IE", "en-IE");
    }
}