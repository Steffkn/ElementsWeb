/*
 *  <div>
            <span class="badge-item1">Item1</span>
            <span class="badge-item1">Item2</span>
        </div>
 */
namespace Elements.Web.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using System.Collections.Generic;

    [HtmlTargetElement("badge")]
    public class BadgeTagHelper : TagHelper
    {
        public IEnumerable<string> Items { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";

            foreach (var badgeItem in Items)
            {
                output.Content.AppendHtml(string.Format("<{0} class=\"{2}\">{1}</{0}>", "div", badgeItem, "badge-" + badgeItem.ToLower()));
            }

        }
    }
}
