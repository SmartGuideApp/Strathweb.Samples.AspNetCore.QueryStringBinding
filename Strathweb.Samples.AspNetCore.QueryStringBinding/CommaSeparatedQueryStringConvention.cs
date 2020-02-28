using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Strathweb.Samples.AspNetCore.QueryStringBinding
{
    public class CommaSeparatedQueryStringConvention : IActionModelConvention
    {
        private readonly bool _removeEnclosingQuotes;

        public CommaSeparatedQueryStringConvention(bool removeEnclosingQuotes = false)
        {
            _removeEnclosingQuotes = removeEnclosingQuotes;
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
                        attribute = new SeparatedQueryStringAttribute(",", _removeEnclosingQuotes);
                        parameter.Action.Filters.Add(attribute);
                    }

                    attribute.AddKey(parameter.ParameterName);
                }
            }
        }
    }
}
