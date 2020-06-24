using System;

namespace PFSoftware.Extensions.DataTypeHelpers
{
    /// <summary>Extension class to more easily parse TimeSpans.</summary>
    public static class TimeSpanHelper
    {
        /// <summary>Utilizes TimeSpan.TryParse to easily parse a TimeSpan.</summary>
        /// <param name="value">Object to be parsed</param>
        /// <returns>Parsed TimeSpan</returns>
        public static TimeSpan Parse(object value) => value != null ? Parse(value.ToString()) : TimeSpan.Zero;

        /// <summary>Utilizes TimeSpan.TryParse to easily parse a TimeSpan.</summary>
        /// <param name="text">Text to be parsed.</param>
        /// <returns>Parsed TimeSpan</returns>
        public static TimeSpan Parse(string text) => string.IsNullOrWhiteSpace(text) ? TimeSpan.Zero : TimeSpan.TryParse(text, out TimeSpan temp) ? temp : TimeSpan.Zero;
    }
}