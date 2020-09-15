using System;

namespace Strathweb.Samples.AspNetCore.QueryStringBinding
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CommaSeparatedAttribute : Attribute
    {
    }
}
