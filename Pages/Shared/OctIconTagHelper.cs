

using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("icon", Attributes = "src")]
public class OctIconTagHelper : TagHelper
{


    public override void Process(TagHelperContext context, TagHelperOutput output)
    {

        var src = context.AllAttributes["src"];
        if ( src != null)
        {
            Console.WriteLine(context.AllAttributes.ToString());
            var srcValue = src.Value.Equals(null) ? "" : src.Value.ToString();
            var iconSrc = $"/lib/octicons/svg/{srcValue}.svg";



            output.Attributes.Remove(src);
            output.Attributes.Add("src", iconSrc);
            output.TagName = "img";

        }

    }



}