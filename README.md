# Strathweb.Samples.AspNetCore.QueryStringBinding

Code for blog post: https://www.strathweb.com/2017/07/customizing-query-string-parameter-binding-in-asp-net-core-mvc/

---
This forked version adds support for comma-separated collection passed to API as a form data parameter ([FromForm] attribute for a parameter), and it supports removing enclosing quotes from collection values which is useful for comma-separated quoted string collection passed from Swagger/Swashbuckle.AspNetCore