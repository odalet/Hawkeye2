using System;
using System.Collections.Generic;
using System.Text;

namespace Hawkeye
{
    internal static class StringExtensions
    {
        public static string GetLabel(this Clr clr)
        {
            switch (clr)
            {
                case Clr.None: return string.Empty;
                case Clr.Undefined: return "?";
                case Clr.Unsupported: return "??";
                case Clr.Net2: return ".net 2";
                case Clr.Net4: return ".net 4";
            }

            return "???";
        }
        
        /////// <summary>
        /////// Use this method to remove sensitive data (password) from a connection string.
        /////// </summary>
        /////// <param name="connstr">The connection string.</param>
        /////// <param name="showStars">
        /////// if set to <c>true</c> then the passwords are replaces with stars; 
        /////// otherwise, the passwords are completly removed (key and value).</param>
        /////// <returns>The connection string without any password.</returns>
        ////public static string ToStringNoPassword(this string connstr, bool showStars)
        ////{
        ////    if (string.IsNullOrEmpty(connstr)) return connstr;

        ////    // parse the connection string
        ////    string[] parts = connstr.Split(';');
        ////    var pairs = new Dictionary<string, string>();

        ////    foreach (string part in parts)
        ////    {
        ////        if (part.Contains("="))
        ////        {
        ////            int index = part.IndexOf('=');
        ////            var left = part.Substring(0, index).Trim();
        ////            var right = part.Substring(index + 1).Trim();
        ////            pairs.Add(left, right);
        ////        }
        ////        else pairs.Add(part, string.Empty);
        ////    }

        ////    // Rebuild the connection string.
        ////    var builder = new List<string>();

        ////    foreach (string key in pairs.Keys)
        ////    {
        ////        string value = pairs[key];
        ////        if ((string.Compare(key, "password", StringComparison.InvariantCultureIgnoreCase) == 0) ||
        ////            (string.Compare(key, "pwd", StringComparison.InvariantCultureIgnoreCase) == 0))
        ////        {
        ////            // depending on the option showStars, we don't echo the key nor its value
        ////            // or we replace its value with stars.
        ////            if (showStars)
        ////            {
        ////                string temp = key;
        ////                if (!string.IsNullOrEmpty(value))
        ////                    temp += string.Format("={0}", new string('*', value.Length));
        ////                builder.Add(temp);
        ////            }
        ////        }
        ////        else
        ////        {
        ////            string temp = key;
        ////            if (!string.IsNullOrEmpty(value))
        ////                temp += string.Format("={0}", value);
        ////            builder.Add(temp);
        ////        }
        ////    }

        ////    return string.Join(";", builder.ToArray());
        ////}
    }
}
