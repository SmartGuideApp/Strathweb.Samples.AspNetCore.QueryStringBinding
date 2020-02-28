# Strathweb.Samples.AspNetCore.QueryStringBinding

Code for blog post: https://www.strathweb.com/2017/07/customizing-query-string-parameter-binding-in-asp-net-core-mvc/

---
This forked version adds support for:
- comma-separated collection passed to API as a form data parameter (`[FromForm]` parameter attribute)
- parsing comma-separated quoted/escaped string collection passed from [Swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)