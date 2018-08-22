namespace Elements.Web.TagHelpers
{
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("user-badge")]
    public class UserBadgeTagHelper : TagHelper
    {
        public string User { get; set; }

        public string Role { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "span";

            if (User != null)
            {
                output.Content.Append(User);
                output.Attributes.Add("class", "color-" + Role.ToLower());
            }
        }
    }
}
