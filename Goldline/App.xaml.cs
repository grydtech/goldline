using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace Goldline
{
    /// <summary>
    ///     Interaction logic for App
    /// </summary>
    public partial class App : Application
    {
        public static IEnumerable<string> GetAllCountries()
        {
            var cultureList = new List<string>();

            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var culture in cultures)
            {
                var region = new RegionInfo(culture.LCID);

                if (!cultureList.Contains(region.EnglishName))
                    cultureList.Add(region.EnglishName);
            }
            cultureList.Sort();
            return cultureList;
        }
    }
}