using CodingChallenge.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingChallenge.Extensions
{
    public static class ThemeExtensions
    {
        public static ThemeMode ToThemeMode(this string themeModeString)
        {
            foreach (ThemeMode mode in Enum.GetValues(typeof(ThemeMode)))
            {
                if (mode.ToString() == themeModeString)
                {
                    return mode;
                }
            }

            return ThemeMode.Auto;
        }
    }
}
