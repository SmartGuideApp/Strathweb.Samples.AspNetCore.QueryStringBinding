using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
#if (NETCOREAPP2_2 || NETCOREAPP2_1 || NETCOREAPP2_0 || NETCOREAPP1_1 || NETCOREAPP1_0)
using Microsoft.AspNetCore.Http.Internal;
#endif
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Strathweb.Samples.AspNetCore.QueryStringBinding
{
    public class SeparatedQueryStringValueProviderFactory : IValueProviderFactory
    {
        private readonly string _separator;
        private HashSet<string> _keys;
        private readonly bool _isCommaSeparatedCollectionSwaggerText;

        public SeparatedQueryStringValueProviderFactory(string separator, bool isCommaSeparatedCollectionSwaggerText) : this((IEnumerable<string>)null, separator, isCommaSeparatedCollectionSwaggerText)
        { }

        public SeparatedQueryStringValueProviderFactory(string key, string separator, bool isCommaSeparatedCollectionSwaggerText) : this(new List<string> { key }, separator, isCommaSeparatedCollectionSwaggerText)
        {
        }

        public SeparatedQueryStringValueProviderFactory(IEnumerable<string> keys, string separator, bool isCommaSeparatedCollectionSwaggerText)
        {
            _isCommaSeparatedCollectionSwaggerText = isCommaSeparatedCollectionSwaggerText;
            _keys = keys != null ? new HashSet<string>(keys) : null;
            _separator = separator;
        }

        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            var queryCollection = context.ActionContext.HttpContext.Request.HasFormContentType
                ? _GetQueryCollectionFromFormCollection(context.ActionContext.HttpContext.Request.Form)
                : context.ActionContext.HttpContext.Request.Query;

            var bindingSource = context.ActionContext.HttpContext.Request.HasFormContentType
                ? BindingSource.Form
                : BindingSource.Query;

            context.ValueProviders.Insert(0,
                new SeparatedQueryStringValueProvider(bindingSource, _keys, queryCollection,
                    _separator, isCommaSeparatedCollectionSwaggerText: _isCommaSeparatedCollectionSwaggerText));
            return Task.CompletedTask;
        }

        private IQueryCollection _GetQueryCollectionFromFormCollection(IFormCollection formCollection)
        {
            return new QueryCollection(formCollection.Select(x => x).ToDictionary(x => x.Key, x => x.Value));
        }

        public void AddKey(string key)
        {
            if (_keys == null)
            {
                _keys = new HashSet<string>();
            }

            _keys.Add(key);
        }
    }
}
