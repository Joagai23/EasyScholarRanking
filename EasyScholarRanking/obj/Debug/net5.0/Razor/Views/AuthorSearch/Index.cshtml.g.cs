#pragma checksum "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\AuthorSearch\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4a963592e9b87aa053d874ce942cd241c633ba4c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_AuthorSearch_Index), @"mvc.1.0.view", @"/Views/AuthorSearch/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\_ViewImports.cshtml"
using EasyScholarRanking;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\_ViewImports.cshtml"
using EasyScholarRanking.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\AuthorSearch\Index.cshtml"
using EasyScholarRanking.Views.AuthorSearch;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4a963592e9b87aa053d874ce942cd241c633ba4c", @"/Views/AuthorSearch/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d80e76235dd78045c3ece98c4a021842ee3c2ec6", @"/Views/_ViewImports.cshtml")]
    public class Views_AuthorSearch_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\AuthorSearch\Index.cshtml"
  
    ViewData["Title"] = "Author Search";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"text-center\">\r\n    <h2 class=\"display-4\">Author Search</h2>\r\n    <hr>\r\n</div>\r\n\r\n");
#nullable restore
#line 11 "C:\Jorge\Universidad\5\EasyScholarRanking\Projects\EasyScholarRanking\EasyScholarRanking\Views\AuthorSearch\Index.cshtml"
Write(await Html.RenderComponentAsync<AuthorSearch>(RenderMode.ServerPrerendered));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<script src=\"_framework/blazor.server.js\"></script>\r\n\r\n");
            WriteLiteral(@"<script src=""_content/BlazorAnimate/blazorAnimateInterop.js""></script>
<script src=""https://cdn.jsdelivr.net/npm/chart.js@2.9.4/dist/Chart.min.js""></script>

<!-- This is the glue between Blazor and Chart.js -->
<script src=""_content/ChartJs.Blazor.Fork/ChartJsBlazorInterop.js""></script>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
