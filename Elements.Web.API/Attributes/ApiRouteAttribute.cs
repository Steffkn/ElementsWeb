using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Elements.Web.API.Attributes
{
    public class ApiRouteAttribute : ApiControllerAttribute, IRouteTemplateProvider
    {
        public string Template => "api/[controller]";

        public int? Order { get; set; }

        public string Name { get; set; }
    }
}
