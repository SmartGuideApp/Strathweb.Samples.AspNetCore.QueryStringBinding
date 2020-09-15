using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
                else
                {
                    // inspired by https://stackoverflow.com/a/55854405/379279 - but it goes only to the first level of nested models
                    
                    // evaluate nested models
                    var props = parameter.ParameterInfo.ParameterType.GetProperties();
                    if (props.Length > 0)
                    {
                        // start the recursive call
                        EvaluateProperties(parameter, attribute, props);
                    }
                }
            }
        }

        private void EvaluateProperties(ParameterModel parameter, SeparatedQueryStringAttribute attribute, PropertyInfo[] properties)
        {
            foreach (var prop in properties)
            {
                if (prop.GetCustomAttributes(true).OfType<CommaSeparatedAttribute>().Any())
                {
                    if (attribute == null)
                    {
                        attribute = new SeparatedQueryStringAttribute(_separator, _isCommaSeparatedCollectionSwaggerText);
                        parameter.Action.Filters.Add(attribute);
                    }

                    // get the binding attribute that implements the model name provider
                    var nameProvider = prop.GetCustomAttributes(true).OfType<IModelNameProvider>().FirstOrDefault(a => !string.IsNullOrWhiteSpace(a.Name));
                    attribute.AddKey(nameProvider?.Name ?? prop.Name);
                }
            }
        }
    }
}
