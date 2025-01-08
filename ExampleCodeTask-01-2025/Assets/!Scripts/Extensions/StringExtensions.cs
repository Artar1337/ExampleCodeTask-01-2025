using UnityEngine;
using UnityEngine.Localization.Settings;

public static class StringExtensions
{
    public static string Localize(this string str, string localizationTable = "Main")
    {
        if (LocalizationSettings.StringDatabase.GetTable(localizationTable) == null)
        {
            Debug.LogError($"StringExtensions | Localization table {localizationTable} does not exists! Check adressables.");
        }

        return LocalizationSettings.StringDatabase.GetLocalizedString(localizationTable, str);
    }
}
