using NUnit.Framework;
using Shouldly;

namespace Strathweb.Samples.AspNetCore.QueryStringBinding.Tests.SwaggerCommaSeparatedCollectionSplitters
{
    [TestFixture]
    public class when_splitting_swagger_comma_separated_collection_text
    {
        [TestCase(@"""hello"",""hi""", new[] { "hello", "hi" }, TestName = "1")]
        [TestCase(@"""ahoy"",""hello\\\"",\\\""hi""", new[] { "ahoy", @"hello\"",\""hi" }, TestName = "2")]
        [TestCase(@"""ahoy"",""hello\"",\""hi""", new[] { "ahoy", @"hello"",""hi" }, TestName = "3")]
        public void swagger_comma_separated_collection_text_is_split(string text, string[] expectedResult)
        {
            SwaggerCommaSeparatedCollectionSplitter.Split(text).ShouldBe(expectedResult);
        }
    }
}