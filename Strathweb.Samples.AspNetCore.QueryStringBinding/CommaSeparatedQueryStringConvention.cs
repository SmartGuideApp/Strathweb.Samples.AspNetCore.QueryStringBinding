using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Strathweb.Samples.AspNetCore.QueryStringBinding
{
    public class CommaSeparatedQueryStringConvention : IActionModelConvention
    {
        private readonly bool _isCommaSeparatedCollectionSwaggerText;
        private readonly string _separator;

        public CommaSeparatedQueryStringConvention(
            string separator = ",",
            bool isCommaSeparatedCollectionSwaggerText = false)
        {
            _separator = separator;
            _isCommaSeparatedCollectionSwaggerText = isCommaSeparatedCollectionSwaggerText;
        }

        public void Apply(ActionModel action)
        {
            SeparatedQueryStringAttribute attribute = null;
            foreach (var parameter in action.Parameters)
            {
                if (parameter.Attributes.OfType<CommaSeparatedAttribute>().Any())
                {
                    if (attribute == null)
                    {
                        attribute = new SeparatedQueryStringAttribute(_separator, _isCommaSeparatedCollectionSwaggerText);
                        parameter.Action.Filters.Add(attribute);
                    }

                    attribute.AddKey(parameter.ParameterName);
                }
            }
        }
    }
}
