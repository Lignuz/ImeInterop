using System.Runtime.InteropServices;

namespace ImeInterop
{
    public static class ImeUtilities
    {
        // https://learn.microsoft.com/ko-kr/windows-hardware/manufacture/desktop/available-language-packs-for-windows
        private static readonly Dictionary<ushort, (string LanguageName, string CultureTag)> LanguageMap = new()
        {
            { 0x0436, ("Afrikaans", "af-ZA") },
            { 0x041c, ("Albanian", "sq-AL") },
            { 0x0484, ("Alsatian", "gsw-FR") },
            { 0x045e, ("Amharic", "am-ET") },
            { 0x0401, ("Arabic (Saudi Arabia)", "ar-SA") },
            { 0x0801, ("Arabic (Iraq)", "ar-IQ") },
            { 0x0c01, ("Arabic (Egypt)", "ar-EG") },
            { 0x1001, ("Arabic (Libya)", "ar-LY") },
            { 0x1401, ("Arabic (Algeria)", "ar-DZ") },
            { 0x1801, ("Arabic (Morocco)", "ar-MA") },
            { 0x1c01, ("Arabic (Tunisia)", "ar-TN") },
            { 0x2001, ("Arabic (Oman)", "ar-OM") },
            { 0x2401, ("Arabic (Yemen)", "ar-YE") },
            { 0x2801, ("Arabic (Syria)", "ar-SY") },
            { 0x2c01, ("Arabic (Jordan)", "ar-JO") },
            { 0x3001, ("Arabic (Lebanon)", "ar-LB") },
            { 0x3401, ("Arabic (Kuwait)", "ar-KW") },
            { 0x3801, ("Arabic (UAE)", "ar-AE") },
            { 0x3c01, ("Arabic (Bahrain)", "ar-BH") },
            { 0x4001, ("Arabic (Qatar)", "ar-QA") },
            { 0x042b, ("Armenian", "hy-AM") },
            { 0x044d, ("Assamese", "as-IN") },
            { 0x082c, ("Azerbaijani (Cyrillic)", "az-Cyrl-AZ") },
            { 0x042c, ("Azerbaijani (Latin)", "az-Latn-AZ") },
            { 0x046d, ("Bashkir", "ba-RU") },
            { 0x042d, ("Basque", "eu-ES") },
            { 0x0423, ("Belarusian", "be-BY") },
            { 0x0445, ("Bengali (India)", "bn-IN") },
            { 0x0845, ("Bengali (Bangladesh)", "bn-BD") },
            { 0x141a, ("Bosnian (Latin)", "bs-Latn-BA") },
            { 0x201a, ("Bosnian (Cyrillic)", "bs-Cyrl-BA") },
            { 0x047e, ("Breton", "br-FR") },
            { 0x0402, ("Bulgarian", "bg-BG") },
            { 0x0403, ("Catalan", "ca-ES") },
            { 0x0c04, ("Chinese (Hong Kong SAR)", "zh-HK") },
            { 0x1404, ("Chinese (Macao SAR)", "zh-MO") },
            { 0x0804, ("Chinese (Simplified)", "zh-CN") },
            { 0x1004, ("Chinese (Singapore)", "zh-SG") },
            { 0x0404, ("Chinese (Traditional)", "zh-TW") },
            { 0x0483, ("Corsican", "co-FR") },
            { 0x041a, ("Croatian", "hr-HR") },
            { 0x101a, ("Croatian (Latin, Bosnia and Herzegovina)", "hr-BA") },
            { 0x0405, ("Czech", "cs-CZ") },
            { 0x0406, ("Danish", "da-DK") },
            { 0x048c, ("Dari", "prs-AF") },
            { 0x0465, ("Divehi", "dv-MV") },
            { 0x0413, ("Dutch (Netherlands)", "nl-NL") },
            { 0x0813, ("Dutch (Belgium)", "nl-BE") },
            { 0x0409, ("English (United States)", "en-US") },
            { 0x0809, ("English (United Kingdom)", "en-GB") },
            { 0x0c09, ("English (Australia)", "en-AU") },
            { 0x1009, ("English (Canada)", "en-CA") },
            { 0x1409, ("English (New Zealand)", "en-NZ") },
            { 0x1809, ("English (Ireland)", "en-IE") },
            { 0x1c09, ("English (South Africa)", "en-ZA") },
            { 0x2009, ("English (Jamaica)", "en-JM") },
            { 0x2409, ("English (Caribbean)", "en-029") },
            { 0x2809, ("English (Belize)", "en-BZ") },
            { 0x2c09, ("English (Trinidad)", "en-TT") },
            { 0x3009, ("English (Zimbabwe)", "en-ZW") },
            { 0x3409, ("English (Philippines)", "en-PH") },
            { 0x0425, ("Estonian", "et-EE") },
            { 0x0438, ("Faroese", "fo-FO") },
            { 0x0464, ("Filipino", "fil-PH") },
            { 0x040b, ("Finnish", "fi-FI") },
            { 0x040c, ("French (France)", "fr-FR") },
            { 0x080c, ("French (Belgium)", "fr-BE") },
            { 0x0c0c, ("French (Canada)", "fr-CA") },
            { 0x100c, ("French (Switzerland)", "fr-CH") },
            { 0x140c, ("French (Luxembourg)", "fr-LU") },
            { 0x180c, ("French (Monaco)", "fr-MC") },
            { 0x0462, ("Frisian", "fy-NL") },
            { 0x0456, ("Galician", "gl-ES") },
            { 0x0437, ("Georgian", "ka-GE") },
            { 0x0407, ("German (Germany)", "de-DE") },
            { 0x0807, ("German (Switzerland)", "de-CH") },
            { 0x0c07, ("German (Austria)", "de-AT") },
            { 0x1007, ("German (Luxembourg)", "de-LU") },
            { 0x1407, ("German (Liechtenstein)", "de-LI") },
            { 0x0408, ("Greek", "el-GR") },
            { 0x0447, ("Gujarati", "gu-IN") },
            { 0x0468, ("Hausa", "ha-Latn-NG") },
            { 0x0475, ("Hawaiian", "haw-US") },
            { 0x040d, ("Hebrew", "he-IL") },
            { 0x0439, ("Hindi", "hi-IN") },
            { 0x040e, ("Hungarian", "hu-HU") },
            { 0x040f, ("Icelandic", "is-IS") },
            { 0x0470, ("Igbo", "ig-NG") },
            { 0x0421, ("Indonesian", "id-ID") },
            { 0x045d, ("Inuktitut (Syllabics)", "iu-CA") },
            { 0x085d, ("Inuktitut (Latin)", "iu-Latn-CA") },
            { 0x083c, ("Irish", "ga-IE") },
            { 0x0410, ("Italian (Italy)", "it-IT") },
            { 0x0810, ("Italian (Switzerland)", "it-CH") },
            { 0x0411, ("Japanese", "ja-JP") },
            { 0x044b, ("Kannada", "kn-IN") },
            { 0x0471, ("Kashmiri", "ks-IN") },
            { 0x043f, ("Kazakh", "kk-KZ") },
            { 0x0457, ("Konkani", "kok-IN") },
            { 0x0412, ("Korean", "ko-KR") },
            { 0x0440, ("Kyrgyz", "ky-KG") },
            { 0x0426, ("Latvian", "lv-LV") },
            { 0x0427, ("Lithuanian", "lt-LT") },
            { 0x082e, ("Lower Sorbian", "dsb-DE") },
            { 0x046e, ("Luxembourgish", "lb-LU") },
            { 0x042f, ("Macedonian", "mk-MK") },
            { 0x083e, ("Malay (Brunei)", "ms-BN") },
            { 0x043e, ("Malay (Malaysia)", "ms-MY") },
            { 0x044c, ("Malayalam", "ml-IN") },
            { 0x043a, ("Maltese", "mt-MT") },
            { 0x0481, ("Maori", "mi-NZ") },
            { 0x047a, ("Mapudungun", "arn-CL") },
            { 0x044e, ("Marathi", "mr-IN") },
            { 0x047c, ("Mohawk", "moh-CA") },
            { 0x0450, ("Mongolian (Cyrillic)", "mn-MN") },
            { 0x0850, ("Mongolian (Traditional)", "mn-Mong-CN") },
            { 0x0461, ("Nepali (Nepal)", "ne-NP") },
            { 0x0414, ("Norwegian Bokmål", "nb-NO") },
            { 0x0814, ("Norwegian Nynorsk", "nn-NO") },
            { 0x0448, ("Oriya", "or-IN") },
            { 0x0463, ("Pashto", "ps-AF") },
            { 0x0429, ("Persian", "fa-IR") },
            { 0x0415, ("Polish", "pl-PL") },
            { 0x0416, ("Portuguese (Brazil)", "pt-BR") },
            { 0x0816, ("Portuguese (Portugal)", "pt-PT") },
            { 0x0446, ("Punjabi", "pa-IN") },
            { 0x046b, ("Quechua (Bolivia)", "quz-BO") },
            { 0x086b, ("Quechua (Ecuador)", "quz-EC") },
            { 0x0c6b, ("Quechua (Peru)", "quz-PE") },
            { 0x0418, ("Romanian", "ro-RO") },
            { 0x0419, ("Russian", "ru-RU") },
            { 0x243b, ("Sami (Inari)", "smn-FI") },
            { 0x103b, ("Sami (Lule)", "smj-NO") },
            { 0x143b, ("Sami (Lule, Sweden)", "smj-SE") },
            { 0x0c3b, ("Sami (Northern)", "se-NO") },
            { 0x043b, ("Sami (Northern, Sweden)", "se-SE") },
            { 0x083b, ("Sami (Northern, Finland)", "se-FI") },
            { 0x203b, ("Sami (Skolt)", "sms-FI") },
            { 0x183b, ("Sami (Southern)", "sma-NO") },
            { 0x1c3b, ("Sami (Southern, Sweden)", "sma-SE") },
            { 0x044f, ("Sindhi", "sd-IN") },
            { 0x045b, ("Sinhala", "si-LK") },
            { 0x041b, ("Slovak", "sk-SK") },
            { 0x0424, ("Slovenian", "sl-SI") },
            { 0x040a, ("Spanish (Spain, Traditional)", "es-ES") },
            { 0x080a, ("Spanish (Mexico)", "es-MX") },
            { 0x0c0a, ("Spanish (Spain)", "es-ES_tradnl") },
            { 0x100a, ("Spanish (Guatemala)", "es-GT") },
            { 0x140a, ("Spanish (Costa Rica)", "es-CR") },
            { 0x180a, ("Spanish (Panama)", "es-PA") },
            { 0x1c0a, ("Spanish (Dominican Republic)", "es-DO") },
            { 0x200a, ("Spanish (Venezuela)", "es-VE") },
            { 0x240a, ("Spanish (Colombia)", "es-CO") },
            { 0x280a, ("Spanish (Peru)", "es-PE") },
            { 0x2c0a, ("Spanish (Argentina)", "es-AR") },
            { 0x300a, ("Spanish (Ecuador)", "es-EC") },
            { 0x340a, ("Spanish (Chile)", "es-CL") },
            { 0x380a, ("Spanish (Uruguay)", "es-UY") },
            { 0x3c0a, ("Spanish (Paraguay)", "es-PY") },
            { 0x400a, ("Spanish (Bolivia)", "es-BO") },
            { 0x440a, ("Spanish (El Salvador)", "es-SV") },
            { 0x480a, ("Spanish (Honduras)", "es-HN") },
            { 0x4c0a, ("Spanish (Nicaragua)", "es-NI") },
            { 0x500a, ("Spanish (Puerto Rico)", "es-PR") },
            { 0x0441, ("Swahili", "sw-KE") },
            { 0x041d, ("Swedish (Sweden)", "sv-SE") },
            { 0x081d, ("Swedish (Finland)", "sv-FI") },
            { 0x0449, ("Tamil", "ta-IN") },
            { 0x0444, ("Tatar", "tt-RU") },
            { 0x044a, ("Telugu", "te-IN") },
            { 0x041e, ("Thai", "th-TH") },
            { 0x0451, ("Tibetan", "bo-CN") },
            { 0x0873, ("Tigrinya (Eritrea)", "ti-ER") },
            { 0x0473, ("Tigrinya (Ethiopia)", "ti-ET") },
            { 0x041f, ("Turkish", "tr-TR") },
            { 0x0442, ("Turkmen", "tk-TM") },
            { 0x0480, ("Uyghur", "ug-CN") },
            { 0x0422, ("Ukrainian", "uk-UA") },
            { 0x0420, ("Urdu (Pakistan)", "ur-PK") },
            { 0x0820, ("Urdu (India)", "ur-IN") },
            { 0x0443, ("Uzbek (Latin)", "uz-Latn-UZ") },
            { 0x0843, ("Uzbek (Cyrillic)", "uz-Cyrl-UZ") },
            { 0x042a, ("Vietnamese", "vi-VN") },
            { 0x0452, ("Welsh", "cy-GB") },
            { 0x0488, ("Wolof", "wo-SN") },
            { 0x046a, ("Yoruba", "yo-NG") },
        };

        public static (string LanguageName, string CultureTag) GetLanguageInfo(ushort langId)
        {
            if (LanguageMap.TryGetValue(langId, out var info))
                return info;
            else
                return ("Unknown", "Unknown");
        }

        public static (int LangId, string LanguageName, string CultureTag) GetKeyboardLanguageInfo()
        {
            ushort langId = GetKeyboardLangId();
            (string LanguageName, string CultureTag) = GetLanguageInfo(langId);
            return (langId, LanguageName, CultureTag);
        }

        public static ushort GetKeyboardLangId()
        {
            IntPtr hKL = GetKeyboardLayout(GetCurrentThreadId());
            ushort langId = (ushort)(hKL.ToInt64() & 0xFFFF);
            return langId;
        }

        public static string GetInputLanguageEncoding()
        {
            int langId = GetKeyboardLangId();

            return langId switch
            {
                0x0412 => "ks_c_5601-1987", // Korean
                0x0411 => "shift_jis",      // Japanese
                0x0804 => "gb2312",         // Chinese (Simplified)
                0x0404 => "big5",           // Chinese (Traditional)
                _ => "utf-8",
            };
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();
    }
}
