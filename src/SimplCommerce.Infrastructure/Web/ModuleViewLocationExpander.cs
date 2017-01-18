﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SimplCommerce.Infrastructure.Web
{
    public class ModuleViewLocationExpander : IViewLocationExpander
    {
        private const string MODULE_KEY = "module";
        private const string THEME_KEY = "theme";

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context.Values.ContainsKey(THEME_KEY) && !string.IsNullOrWhiteSpace(context.Values[THEME_KEY]))
            {
                var theme = context.Values[THEME_KEY];

                if (context.Values.ContainsKey(MODULE_KEY) && !string.IsNullOrWhiteSpace(context.Values[MODULE_KEY]))
                {
                    var module = context.Values[MODULE_KEY];
                    var moduleViewLocations = new string[]
                    {
                        $"/Themes/{theme}/Modules/SimplCommerce.Module.{module}/Views/{{1}}/{{0}}.cshtml",
                        $"/Themes/{theme}/Modules/SimplCommerce.Module.{module}/Views/Shared/{{0}}.cshtml",
                        $"/Modules/SimplCommerce.Module.{module}/Views/{{1}}/{{0}}.cshtml",
                        $"/Modules/SimplCommerce.Module.{module}/Views/Shared/{{0}}.cshtml",
                        $"/Themes/{theme}/Views/Shared/{{0}}.cshtml"
                    };

                    viewLocations = moduleViewLocations.Concat(viewLocations);
                }
            }
            else
            {
                if (context.Values.ContainsKey(MODULE_KEY) && !string.IsNullOrWhiteSpace(context.Values[MODULE_KEY]))
                {
                    var module = context.Values[MODULE_KEY];
                    var moduleViewLocations = new string[]
                    {
                        $"/Modules/SimplCommerce.Module.{module}/Views/{{1}}/{{0}}.cshtml",
                        $"/Modules/SimplCommerce.Module.{module}/Views/Shared/{{0}}.cshtml"
                    };

                    viewLocations = moduleViewLocations.Concat(viewLocations);
                }
            }

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var controller = context.ActionContext.ActionDescriptor.DisplayName;
            var moduleName = controller.Split('.')[2];
            context.Values[MODULE_KEY] = moduleName;

            var config = context.ActionContext.HttpContext.RequestServices.GetService<IConfiguration>();
            context.Values[THEME_KEY] = config["theme"];
        }
    }
}
