using System;
using System.Collections.Generic;
using System.Linq;

namespace Strathweb.Samples.AspNetCore.QueryStringBinding
{
    public static class SwaggerCommaSeparatedCollectionSplitter
    {
        public static IEnumerable<string> Split(string text)
        {
            return text
                .Substring(1, text.Length - 2)
                .Split(new[] {@""","""}, StringSplitOptions.None)
                .Select(x => x.Replace("\\\\", "\\").Replace("\\\"", "\""));
        }
    }
}