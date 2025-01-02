using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace libtwee
{
    public partial class Babel
    {
        [GeneratedRegex("^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")]
        private static partial Regex TwineBabelRegex();

        public static string GenerateTwineIFID()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }

        public static bool IsValidTwineIFID(string ifid)
        {
            // An IFID is a sequence of between 8 and 63 characters, 
            // each of which shall be a digit, a capital letter or a
            // hyphen that uniquely identify a story (see Treaty of Babel).
            return TwineBabelRegex().IsMatch(ifid);
        }
    }
}