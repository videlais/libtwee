using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace libtwee
{
    /// <summary>
    /// Class <c>Babel</a> handles Interactive Fiction Identification (IFID) generation and validation.
    /// The class is used to generate and validate IFIDs (Interactive Fiction IDs) for Twine stories.
    /// Twine stories follow an uppercase UUID format based on The Treaty of Babel (https://babel.ifarchive.org/babel.html)
    /// </summary>
    public partial class Babel
    {
        [GeneratedRegex("^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")]
        private static partial Regex TwineBabelRegex();

        /// <summary>
        /// <c>GenerateTwineIFID</c> generates a new Twine IFID (Interactive Fiction ID) in uppercase UUID format.
        /// </summary>
        /// <returns>A new string Twine IFID in uppercase UUID format.</returns>
        public static string GenerateTwineIFID()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }

        /// <summary>
        /// <c>IsValidTwineIFID</c> checks if a given string is a valid Twine IFID.
        /// </summary>
        /// <param name="ifid">The string to check.</param>
        /// <returns><c>true</c> if the string is a valid Twine IFID, <c>false</c> otherwise.</returns>
        public static bool IsValidTwineIFID(string ifid)
        {
            // An IFID is a sequence of between 8 and 63 characters, 
            // each of which shall be a digit, a capital letter or a
            // hyphen that uniquely identify a story (see Treaty of Babel).
            return TwineBabelRegex().IsMatch(ifid);
        }
    }
}