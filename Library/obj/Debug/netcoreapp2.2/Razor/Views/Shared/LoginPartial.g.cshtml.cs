#pragma checksum "C:\Users\Dimitar\Desktop\New folder\LibraryApp\Library\Views\Shared\LoginPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "098db04961e883d7c40ca8005ef328ceffd4ff3f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_LoginPartial), @"mvc.1.0.view", @"/Views/Shared/LoginPartial.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/LoginPartial.cshtml", typeof(AspNetCore.Views_Shared_LoginPartial))]
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
#line 1 "C:\Users\Dimitar\Desktop\New folder\LibraryApp\Library\Views\_ViewImports.cshtml"
using Library;

#line default
#line hidden
#line 2 "C:\Users\Dimitar\Desktop\New folder\LibraryApp\Library\Views\_ViewImports.cshtml"
using Library.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"098db04961e883d7c40ca8005ef328ceffd4ff3f", @"/Views/Shared/LoginPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"dadb7a731bfbb305c411bc5eb7a307dbd6008a89", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_LoginPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 2 "C:\Users\Dimitar\Desktop\New folder\LibraryApp\Library\Views\Shared\LoginPartial.cshtml"
 if (ViewBag.UserId != null)
{
    using (Html.BeginForm("LogOff", "Home", FormMethod.Post, new { id = "logoutForm", @class = "navbar-right" }))
    {
        

#line default
#line hidden
            BeginContext(166, 23, false);
#line 6 "C:\Users\Dimitar\Desktop\New folder\LibraryApp\Library\Views\Shared\LoginPartial.cshtml"
   Write(Html.AntiForgeryToken());

#line default
#line hidden
            EndContext();
            BeginContext(215, 90, true);
            WriteLiteral("        <ul class=\"nav navbar-nav navbar-right\">\r\n            <li>\r\n                Hello ");
            EndContext();
            BeginContext(306, 16, false);
#line 11 "C:\Users\Dimitar\Desktop\New folder\LibraryApp\Library\Views\Shared\LoginPartial.cshtml"
                 Write(ViewBag.UserName);

#line default
#line hidden
            EndContext();
            BeginContext(322, 165, true);
            WriteLiteral("!\r\n            </li>\r\n            <li style=\"padding-left:30px\"><a href=\"javascript:document.getElementById(\'logoutForm\').submit()\">Log off</a></li>\r\n        </ul>\r\n");
            EndContext();
#line 15 "C:\Users\Dimitar\Desktop\New folder\LibraryApp\Library\Views\Shared\LoginPartial.cshtml"
    }
}
else
{

#line default
#line hidden
            BeginContext(506, 46, true);
            WriteLiteral("    <ul class=\"nav navbar-nav navbar-right\">\r\n");
            EndContext();
            BeginContext(690, 12, true);
            WriteLiteral("        <li>");
            EndContext();
            BeginContext(703, 42, false);
#line 21 "C:\Users\Dimitar\Desktop\New folder\LibraryApp\Library\Views\Shared\LoginPartial.cshtml"
       Write(Html.ActionLink("Log in", "Index", "Home"));

#line default
#line hidden
            EndContext();
            BeginContext(745, 18, true);
            WriteLiteral("</li>\r\n    </ul>\r\n");
            EndContext();
#line 23 "C:\Users\Dimitar\Desktop\New folder\LibraryApp\Library\Views\Shared\LoginPartial.cshtml"
}

#line default
#line hidden
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
