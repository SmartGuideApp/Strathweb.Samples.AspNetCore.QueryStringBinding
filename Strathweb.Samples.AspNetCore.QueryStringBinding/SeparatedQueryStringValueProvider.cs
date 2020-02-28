using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Microsoft.Extensions.Primitives;

namespace Strathweb.Samples.AspNetCore.QueryStringBinding
{
    public class SeparatedQueryStringValueProvider : QueryStringValueProvider
    {
        
        private readonly HashSet<string> _keys;
        private readonly string _separator;
        private readonly IQueryCollection _values;
        private readonly bool _removeEnclosingQuotes;

        public SeparatedQueryStringValueProvider(IQueryCollection values, CultureInfo culture)
            : base(null, values, culture)
        {
        }

        public SeparatedQueryStringValueProvider(BindingSource bindingSource, string key, IQueryCollection values, string separator, bool removeEnclosingQuotes)
            : this(bindingSource, new List<string> { key }, values, separator, removeEnclosingQuotes)
        {
        }

        public SeparatedQueryStringValueProvider(BindingSource bindingSource, IEnumerable<string> keys, IQueryCollection values, string separator, 
            bool removeEnclosingQuotes)
            : base(bindingSource, values, CultureInfo.InvariantCulture)
        {
            _removeEnclosingQuotes = removeEnclosingQuotes;
            _keys = new HashSet<string>(keys);
            _values = values;
            _separator = separator;
        }

        public override ValueProviderResult GetValue(string key)
        {
            var result = base.GetValue(key);

            if (_keys != null && !_keys.Contains(key))
            {
                return result;
            }

            if (result != ValueProviderResult.None &&
                result.Values.Any(x => x.IndexOf(_separator, StringComparison.OrdinalIgnoreCase) > 0))
            {
                var splitValues = new StringValues(result.Values
                    .SelectMany(x =>
                    {
                        var values = x.Split(new[] {_separator}, StringSplitOptions.None);
                        return _removeEnclosingQuotes
                            ? values.Select(_RemoveEnclosingQuotes)
                            : values;
                    }).ToArray());

                return new ValueProviderResult(splitValues, result.Culture);
            }

            return result;
        }

        public string _RemoveEnclosingQuotes(string text)
        {
            return text.Substring(1, text.Length - 2);
        }
    }
}
