using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elements.Web.TagHelpers
{
    /*
            <div class="form-group">
                <label asp-for="RegisterNewUserModel.Username"></label>
                <input asp-for="RegisterNewUserModel.Username" class="form-control" placeholder="Username" />
                <span asp-validation-for="RegisterNewUserModel.Username" class="text-dange"></span>
            </div>
         */
    [HtmlTargetElement("form-entry")]
    public class FormEntryTagHelper : TagHelper
    {
        private IHtmlGenerator htmlGenerator;
        public FormEntryTagHelper(IHtmlGenerator generator)
        {
            this.htmlGenerator = generator;
        }

        // An expression to be evaluated against the current model.
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.SelfClosing;
            var label = new LabelTagHelper(htmlGenerator) { For = For, ViewContext = ViewContext };
            var input = new InputTagHelper(htmlGenerator) { For = For, ViewContext = ViewContext };
            var validation = new ValidationMessageTagHelper(htmlGenerator) { For = For, ViewContext = ViewContext };

            label.Init(context);
            input.Init(context);
            validation.Init(context);

            await label.ProcessAsync(context, output);
            await input.ProcessAsync(context, output);
            await validation.ProcessAsync(context, output);

            output.TagName = "div";
            output.Content.AppendHtml(label.ToString());
            output.Content.AppendHtml(input.ToString());
            output.Content.AppendHtml(validation.ToString());
        }
    }
}
