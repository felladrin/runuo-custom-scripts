// Country v1.0.0
// Author: Felladrin
// Started: 2016-01-16
// Updated: 2016-01-16

using System.Collections.Generic;

namespace Felladrin.Utilities
{
    public static class Country
    {
        /// <summary>
        /// Gets the country name from a ISO 639 language code.
        /// </summary>
        /// <returns>The country name if it was found, null otherwise.</returns>
        /// <param name="code">The language code.</param>
        public static string GetNameFromCode(string code)
        {
            if (code == null || code.Length != 3)
                return null;

            foreach (KeyValuePair<string, string> country in Countries)
                if (code == country.Key)
                    return country.Value;

            return null;
        }

        /// <summary>
        /// Gets the ISO 639 language code from a country name.
        /// </summary>
        /// <returns>The language code if it was found, null otherwise.</returns>
        /// <param name="name">The country name.</param>
        public static string GetCodeFromName(string name)
        {
            if (name == null)
                return null;

            foreach (KeyValuePair<string, string> country in Countries)
                if (name == country.Value)
                    return country.Key;

            return null;
        }

        /// <summary>
        /// Dictionary of ISO 639 language codes and their respective countries.
        /// </summary>
        public static readonly Dictionary<string, string> Countries = new Dictionary<string, string> 
        {
            {"AFK", "South Africa"},
            {"ARA", "Saudi Arabia"},
            {"ARB", "Lebanon"},
            {"ARE", "Egypt"},
            {"ARG", "Algeria"},
            {"ARH", "Bahrain"},
            {"ARI", "Iraq"},
            {"ARJ", "Jordan"},
            {"ARK", "Kuwait"},
            {"ARL", "Libya"},
            {"ARM", "Morocco"},
            {"ARO", "Oman"},
            {"ARQ", "Qatar"},
            {"ARS", "Syria"},
            {"ART", "Tunisia"},
            {"ARU", "U.A.E."},
            {"ARY", "Yemen"},
            {"ASM", "India"},
            {"AZE", "Azerbaijan"},
            {"BEL", "Belarus"},
            {"BEN", "India"},
            {"BGR", "Bulgaria"},
            {"CAT", "Spain"},
            {"CHS", "PRC"},
            {"CHT", "Taiwan"},
            {"CSY", "Czech Republic"},
            {"DAN", "Denmark"},
            {"DEA", "Austria"},
            {"DEC", "Liechtenstein"},
            {"DEL", "Luxembourg"},
            {"DES", "Switzerland"},
            {"DEU", "Germany"},
            {"ELL", "Greece"},
            {"ENA", "Australia"},
            {"ENB", "Caribbean"},
            {"ENC", "Canada"},
            {"ENG", "United Kingdom"},
            {"ENI", "Ireland"},
            {"ENJ", "Jamaica"},
            {"ENL", "Belize"},
            {"ENP", "Philippines"},
            {"ENS", "South Africa"},
            {"ENT", "Trinidad"},
            {"ENU", "United States"},
            {"ENW", "Zimbabwe"},
            {"ENZ", "New Zealand"},
            {"ESA", "Panama"},
            {"ESB", "Bolivia"},
            {"ESC", "Costa Rica"},
            {"ESD", "Dominican Republic"},
            {"ESE", "El Salvador"},
            {"ESF", "Ecuador"},
            {"ESG", "Guatemala"},
            {"ESH", "Honduras"},
            {"ESI", "Nicaragua"},
            {"ESL", "Chile"},
            {"ESM", "Mexico"},
            {"ESN", "Spain"},
            {"ESO", "Colombia"},
            {"ESP", "Spain"},
            {"ESR", "Peru"},
            {"ESS", "Argentina"},
            {"ESU", "Puerto Rico"},
            {"ESV", "Venezuela"},
            {"ESY", "Uruguay"},
            {"ESZ", "Paraguay"},
            {"ETI", "Estonia"},
            {"EUQ", "Spain"},
            {"FAR", "Iran"},
            {"FIN", "Finland"},
            {"FOS", "Faeroe Islands"},
            {"FRA", "France"},
            {"FRB", "Belgium"},
            {"FRC", "Canada"},
            {"FRL", "Luxembourg"},
            {"FRM", "Monaco"},
            {"FRS", "Switzerland"},
            {"GUJ", "India"},
            {"HEB", "Israel"},
            {"HIN", "India"},
            {"HRV", "Croatia"},
            {"HUN", "Hungary"},
            {"HYE", "Armenia"},
            {"IND", "Indonesia"},
            {"ISL", "Iceland"},
            {"ITA", "Italy"},
            {"ITS", "Switzerland"},
            {"JPN", "Japan"},
            {"KAN", "India"},
            {"KAT", "Georgia"},
            {"KAZ", "Kazakstan"},
            {"KOK", "India"},
            {"KOR", "Korea"},
            {"LTC", "Lithuania"},
            {"LTH", "Lithuania"},
            {"LVI", "Latvia"},
            {"MAL", "India"},
            {"MAR", "India"},
            {"MKI", "Macedonia"},
            {"MSB", "Brunei Darussalam"},
            {"MSL", "Malaysia"},
            {"NLB", "Belgium"},
            {"NLD", "Netherlands"},
            {"NON", "Norway"},
            {"NOR", "Norway"},
            {"ORI", "India"},
            {"PAN", "India"},
            {"PLK", "Poland"},
            {"PTB", "Brazil"},
            {"PTG", "Portugal"},
            {"ROM", "Romania"},
            {"RUS", "Russia"},
            {"SAN", "India"},
            {"SKY", "Slovakia"},
            {"SLV", "Slovenia"},
            {"SQI", "Albania"},
            {"SRB", "Serbia"},
            {"SRL", "Serbia"},
            {"SVE", "Sweden"},
            {"SVF", "Finland"},
            {"SWK", "Kenya"},
            {"TAM", "India"},
            {"TAT", "Tatarstan"},
            {"TEL", "India"},
            {"THA", "Thailand"},
            {"TRK", "Turkey"},
            {"UKR", "Ukraine"},
            {"URP", "Pakistan"},
            {"UZB", "Uzbekistan"},
            {"VIT", "Viet Nam"},
            {"ZHH", "Hong Kong"},
            {"ZHI", "Singapore"},
            {"ZHM", "Macau"}
        };
    }
}

