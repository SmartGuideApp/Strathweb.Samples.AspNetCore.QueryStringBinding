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
        private readonly bool _isCommaSeparatedCollectionSwaggerText;

        public SeparatedQueryStringValueProvider(IQueryCollection values, CultureInfo culture)
            : base(null, values, culture)
        {
        }

        public SeparatedQueryStringValueProvider(BindingSource bindingSource, string key, IQueryCollection values, string separator, bool isCommaSeparatedCollectionSwaggerText)
            : this(bindingSource, new List<string> { key }, values, separator, isCommaSeparatedCollectionSwaggerText)
        {
        }

        public SeparatedQueryStringValueProvider(BindingSource bindingSource, IEnumerable<string> keys, IQueryCollection values, string separator, 
            bool isCommaSeparatedCollectionSwaggerText)
            : base(bindingSource, values, CultureInfo.InvariantCulture)
        {
            _isCommaSeparatedCollectionSwaggerText = isCommaSeparatedCollectionSwaggerText;
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

            if (result != ValueProviderResult.None)
            {
                var splitValues = _isCommaSeparatedCollectionSwaggerText
                    ? new StringValues(result.Values
                        .SelectMany(SwaggerCommaSeparatedCollectionSplitter.Split).ToArray())
                    : new StringValues(result.Values
                        .SelectMany(x => x.Split(new[] {_separator}, StringSplitOptions.None)).ToArray());

                return new ValueProviderResult(splitValues, result.Culture);
            }

            return result;
        }

    }
}
