using Microsoft.AspNetCore.Html;
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
    // <div class="form-group row">
    //     <div class="col">
    //         <label asp-for="Username" class="col-form-label"></label>
    //         <input asp-for="Username" class="form-control" placeholder="Username" />
    //         <span asp-validation-for="Username" class=""></span>
    //     </div>
    // </div>

    [HtmlTargetElement("form-entry")]
    public class FormEntryTagHelper : TagHelper
    {
        private const string _forAttributeName = "asp-for";
        private const string _defaultWraperDivClass = "form-group row";
        private const string _defaultRowDivClass = "row";
        private const string _defaultLabelClass = "col-form-label";
        private const string _defaultInputClass = "form-control";
        private const string _defaultInnerDivClass = "col";
        private const string _defaultValidationMessageClass = "";

        public FormEntryTagHelper(IHtmlGenerator generator)
        {
            this.Generator = generator;
        }

        [HtmlAttributeName(_forAttributeName)]
        public ModelExpression For { get; set; }

        public IHtmlGenerator Generator { get; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Replace this parent tag helper with div tags wrapping the entire form block
            output.TagName = "div";
            output.Attributes.SetAttribute("class", _defaultWraperDivClass);

            // Manually new-up each child asp form tag helper element
            TagHelperOutput labelElement = await CreateLabelElement(context);
            TagHelperOutput inputElement = await CreateInputElement(context);
            TagHelperOutput validationMessageElement = await CreateValidationMessageElement(context);

            // Wrap all elements with a row div
            IHtmlContent rowDiv = WrapElementsWithDiv(
                    new List<IHtmlContent>()
                    {
                        labelElement,
                        inputElement,
                        validationMessageElement
                    },
                    _defaultInnerDivClass
                );

            // Put everything into the innerHtml of this tag helper
            output.Content.SetHtmlContent(rowDiv);
        }

        private async Task<TagHelperOutput> CreateLabelElement(TagHelperContext context)
        {
            LabelTagHelper labelTagHelper =
                new LabelTagHelper(Generator)
                {
                    For = this.For,
                    ViewContext = this.ViewContext
                };

            TagHelperOutput labelOutput = CreateTagHelperOutput("label");

            await labelTagHelper.ProcessAsync(context, labelOutput);

            labelOutput.Attributes.Add(
                new TagHelperAttribute("class", _defaultLabelClass));

            return labelOutput;
        }

        private async Task<TagHelperOutput> CreateInputElement(TagHelperContext context)
        {
            InputTagHelper inputTagHelper =
                new InputTagHelper(Generator)
                {
                    For = this.For,
                    ViewContext = this.ViewContext
                };

            TagHelperOutput inputOutput = CreateTagHelperOutput("input");

            await inputTagHelper.ProcessAsync(context, inputOutput);

            inputOutput.Attributes.Add(
                new TagHelperAttribute("class", _defaultInputClass));

            return inputOutput;
        }

        private async Task<TagHelperOutput> CreateValidationMessageElement(TagHelperContext context)
        {
            ValidationMessageTagHelper validationMessageTagHelper =
                new ValidationMessageTagHelper(Generator)
                {
                    For = this.For,
                    ViewContext = this.ViewContext
                };

            TagHelperOutput validationMessageOutput = CreateTagHelperOutput("span");

            await validationMessageTagHelper.ProcessAsync(context, validationMessageOutput);

            return validationMessageOutput;
        }

        private IHtmlContent WrapElementsWithDiv(List<IHtmlContent> elements, string classValue)
        {
            TagBuilder div = new TagBuilder("div");
            div.AddCssClass(classValue);
            foreach (IHtmlContent element in elements)
            {
                div.InnerHtml.AppendHtml(element);
            }

            return div;
        }

        private TagHelperOutput CreateTagHelperOutput(string tagName)
        {
            return new TagHelperOutput(
                tagName: tagName,
                attributes: new TagHelperAttributeList(),
                getChildContentAsync: (s, t) =>
                {
                    return Task.Factory.StartNew<TagHelperContent>(
                            () => new DefaultTagHelperContent());
                }
            );
        }
    }
}
