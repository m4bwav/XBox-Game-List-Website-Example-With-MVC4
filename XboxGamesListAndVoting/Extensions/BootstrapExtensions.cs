using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace XboxGamesListAndVoting.Extensions
{
    public static class BootstrapExtensions
    {
        private static TagBuilder CreateGlyphSpan(string glyph)
        {
            var spanBuilder = new TagBuilder("span");

            spanBuilder.AddCssClass("glyphicon");
            spanBuilder.AddCssClass(glyph);

            return spanBuilder;
        }

        public static MvcHtmlString GlyphActionLink(this HtmlHelper htmlHelper,
            string linkText,
            string glyph,
            string actionName,
            string controllerName,
            object routeValues = null,
            object htmlAttributes = null)
        {
            var routeValuesDictionary = new RouteValueDictionary(routeValues);
            var htmlAttributesDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            var urlHelper = DependencyResolver.Current.GetService<UrlHelper>();
            var linkUrl = urlHelper.Action(actionName, controllerName, routeValuesDictionary);

            var spanBuilder = CreateGlyphSpan(glyph).ToString(TagRenderMode.Normal);
            var glyphText = String.IsNullOrEmpty(linkText) ? spanBuilder : spanBuilder + " " + linkText;
            var tagBuilder = new TagBuilder("a")
            {
                InnerHtml = glyphText
            };

            tagBuilder.MergeAttributes(htmlAttributesDictionary);
            tagBuilder.MergeAttribute("href", linkUrl);

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
        }
    }
}