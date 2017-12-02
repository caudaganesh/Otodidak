using HtmlAgilityPack;
using Otodidak.ViewModels;
using System.Linq;
using System.Text;

namespace Otodidak.Helpers
{
    class WebHelper
    {

        public class WebContentHelper:BaseViewModel
        {
            public static string HtmlHeader() //adapt parametres
            {
                var head = new StringBuilder();

                var R = "0";
                var G = "125";
                var B = "88";
                head.Append("<head>");

                head.Append("<meta name=\"viewport\" content=\"initial-scale=1, maximum-scale=1, user-scalable=0\"/>");
                head.Append("<script type=\"text/javascript\">" +
                    //"document.documentElement.style.msScrollTranslation = 'vertical-to-horizontal';" +
                    "</script>"); //horizontal scrolling
                                  //head.Append("<meta name=\"viewport\" content=\"width=720px\">");
                head.Append("<style type=\"text/css\">");
                head.Append("html { }");
                head.Append(string.Format("a {{color:rgb(" + R + "," + G + "," + B + "); font-size:19px; display:inline;overflow-x:hidden; word-wrap: break-word;}}"));

                head.Append(string.Format("h1{{font-size: 21px;color:rgb(" + R + "," + G + "," + B + ");}} " +
                "body {{background-color:rgb(230,230,230);color:black;font-family:'Arial';font-size:19px;font-weight:lighter;margin:0;padding:5px 5px 5px 5px;display:block;" +
                //"height: 100%;" +
                "overflow-y: scroll; overflow-x:hidden;" +
                "position: relative;" +
                "width: auto;" +
                "z-index: 0;}}" +
                "article{{column-fill: auto;column-gap: 0px; column-height:100%;" +
                "}}" +
                "img,p.object,iframe,video {{ max-width:100%; width:100% ; height:auto;}}"));
                head.Append(string.Format("p {{text-align:left;}}"));
                head.Append(string.Format("strong {{color:rgb(" + R + "," + G + "," + B + "); font-size:60px;padding: 5px 5px 5px 5px;display:block;}}"));
                head.Append(string.Format("figure {{width:auto; display: inline;align:center;}}"));
                head.Append(string.Format("span {{font-size:20pt; display:inline;}}"));
                head.Append(string.Format("table {{width:auto;border-width:thin; border-style:solid;border-color:rgb(0,0,0);}}"));
                head.Append(string.Format("table td {{width:auto;border-width:thin; border-style:solid;border-color:rgb(0,0,0); font-size:5px;}}"));
                head.Append(string.Format("table tr {{width:auto;border-width:thin; border-style:solid;border-color:rgb(0,0,0);}}"));
                head.Append(string.Format("div {{height:auto; display:block; overflow-x:hidden;}}"));
                head.Append(string.Format("td {{font-size:5pt;}}"));
                head.Append(string.Format("blockquote {{width:auto; display: inline;margin-top: 1em;margin-bottom: 1em; color:gray;font-style:italic;font-weight:normal;font-size:15px}}"));
                head.Append(string.Format("li {{ background-color:white; margin-top:5px; padding:5px 5px 5px 5px;}}"));
                head.Append(string.Format("ol {{list-style-type:none; padding-left: 0 ;}}"));
                head.Append(string.Format("ul {{list-style-type:none; padding-left: 0 ;}}"));
                head.Append("</style>");

                // head.Append(NotifyScript);
                head.Append("</head>");
                return head.ToString();
            }
            public static string WrapHtml(string htmlSubString)
            {
                var html = new StringBuilder();
                html.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"id\" lang=\"id\" dir=\"ltr\" class=\"client-js\"");
                html.Append(" <script src=\"https://maps.googleapis.com/maps/api/js?sensor=false\"></script>");
                html.Append(HtmlHeader());
                html.Append("<body><article class=\"content\">");
                html.Append(htmlSubString);
                //html.Append("<script src=\"https://cdn.jsdelivr.net/holder/2.9.0/holder.min.js\"></script>");
                html.Append("</article></body>");
                html.Append("</html>");
                var str = html.ToString();
                return str;
            }

            public string WrapContent(string htmlPage)
            {
                htmlDocument.LoadHtml(htmlPage);

                var innerText = htmlDocument.DocumentNode
                    .Descendants("div")
                    .Where(o => o.GetAttributeValue("id", "") == "bodycontents").FirstOrDefault();


                string htmlDiv = "";


                htmlDiv = "<html>" + innerText.OuterHtml + "</html>";

                innerHtmlDocument.LoadHtml(htmlDiv);
                var innerdiv = innerHtmlDocument.DocumentNode
                    .Descendants("div")
                    .Where(o => o.GetAttributeValue("class", "").Contains("adunit")).ToList();

                var titlea = innerHtmlDocument.DocumentNode
                    .Descendants("a").FirstOrDefault();
                if (titlea != null) htmlDiv = htmlDiv.Replace(titlea.OuterHtml, titlea.InnerHtml);

                if (innerdiv.Count != 0)
                {
                    foreach (var item in innerdiv)
                    {
                        htmlDiv = htmlDiv.Replace(item.OuterHtml, "");
                    }

                }

                var innerspan = innerHtmlDocument.DocumentNode
                        .Descendants("span")
                        .Where(o => o.GetAttributeValue("class", "").Contains("ad_label")).ToList();

                if (innerspan.Count != 0)
                {
                    foreach (var item in innerspan)
                    {
                        htmlDiv = htmlDiv.Replace(item.OuterHtml, "");
                    }

                }

                innerHtmlDocument.LoadHtml(htmlDiv);

                var innerp = innerHtmlDocument.DocumentNode
                        .Descendants("p")
                        .Where(o => o.GetAttributeValue("class", "") == "sp_method_toc").FirstOrDefault();


                if (innerp != null)
                {
                    innerHtmlDocument.LoadHtml(innerp.OuterHtml);
                    var innera = innerHtmlDocument.DocumentNode
                        .Descendants("a").ToList();

                    if (innera.Count != 0)
                    {

                        foreach (var a in innera)
                        {
                            htmlDiv = htmlDiv.Replace(a.OuterHtml, "<br/><br/>" + a.OuterHtml);
                        }
                    }
                }


                //var stepdiv = htmlDocument.DocumentNode
                //        .Descendants("div")
                //        .Where(o => o.GetAttributeValue("class", "") == ("info")).FirstOrDefault();
                //    var innereff = htmlDocument.DocumentNode
                //   .Descendants("sup").Where(o => o.GetAttributeValue("class", "") == ("reference")).ToList();

                //if (innereff.Count != 0)
                //{

                //    foreach (var item in innereff)
                //    {
                //        htmlDiv = htmlDiv.Replace(item.OuterHtml, "");
                //    }

                //}


                innerHtmlDocument.LoadHtml(htmlDiv);
                var innerdiv2 = innerHtmlDocument.DocumentNode
                    .Descendants("div")
                    .Where(o => o.GetAttributeValue("id", "") == "bodycontents").FirstOrDefault();
                if (innerdiv2 != null)
                {
                    innerHtmlDocument.LoadHtml(innerdiv2.OuterHtml);
                    var innerb = innerHtmlDocument.DocumentNode
                    .Descendants("b").ToList();

                    if (innerb.Count != 0)
                    {

                        //foreach (var a in innerb)
                        //{
                        //    htmlDiv = htmlDiv.Replace(a.OuterHtml, "<b>" + a.InnerHtml + "</b>" + "<br/><br/>");
                        //}
                    }

                }

                innerHtmlDocument.LoadHtml(htmlDiv);
                var innerstepnum = innerHtmlDocument.DocumentNode
                    .Descendants("div")
                    .Where(o => o.GetAttributeValue("class", "").Contains("step_num")).ToList();

                if (innerstepnum.Count != 0)
                {
                    foreach (var item in innerstepnum)
                    {
                        htmlDiv = htmlDiv.Replace(item.OuterHtml, "<strong><b>" + item.InnerHtml + "</b></strong>");
                    }
                }

                var innerscript = htmlDocument.DocumentNode
                   .Descendants("script").ToList();

                if (innerscript.Count != 0)
                {
                    foreach (var item in innerscript)
                    {
                        htmlDiv = htmlDiv.Replace(item.OuterHtml, "");
                    }
                }

                innerHtmlDocument.LoadHtml(htmlDiv);

                var imagelink = innerHtmlDocument.DocumentNode.Descendants("a").ToList();
                foreach (var nodes in imagelink)
                {
                    if (nodes.OuterHtml.Contains("#/Berkas:"))
                        htmlDiv = htmlDiv.Replace(nodes.OuterHtml, nodes.InnerHtml);

                }

                var sourcelink = innerHtmlDocument.DocumentNode
                .Descendants("a")
                .Where(o => o.GetAttributeValue("class", "") == "showsources").FirstOrDefault();

                if (sourcelink != null)
                    htmlDiv = htmlDiv.Replace(sourcelink.OuterHtml, "");

                var mlink = innerHtmlDocument.DocumentNode.Descendants("a")
                                  .Select(p => p.GetAttributeValue("href", "not found"))
                                  .ToList();

                if (mlink.Count != 0)
                {

                    foreach (string href in mlink)
                    {
                        if (href.Contains("id.wikihow") && !href.Contains("http"))
                            htmlDiv = htmlDiv.Replace(href, "http:" + href);
                        else if (!href.Contains("http") && !href.Contains("#") && !href.Contains("not found"))
                            htmlDiv = htmlDiv.Replace(href, "http://id.wikihow.com" + href);


                    }
                }

                var noscript = innerHtmlDocument.DocumentNode.Descendants("noscript").ToList();

                if (noscript.Count != 0)
                {
                    foreach (var item in noscript)
                    {
                        htmlDiv = htmlDiv.Replace(item.OuterHtml, item.InnerHtml);
                    }
                }

                var imgs = innerHtmlDocument.DocumentNode.Descendants("div").Where(o => o.GetAttributeValue("class", "") == "mwimg  largeimage  floatcenter ").ToList();

                if (imgs.Count != 0)
                {
                    foreach (var item in imgs)
                    {
                        var doc = new HtmlDocument();
                        doc.LoadHtml(item.OuterHtml);
                        var img = doc.DocumentNode.Descendants("img").ToList();
                        if (img.Count > 1)
                            htmlDiv = htmlDiv.Replace(img[0].OuterHtml, "");
                    }
                }

                htmlDiv = WebHelper.WebContentHelper.WrapHtml(htmlDiv);

                return htmlDiv;
            }

        }
    }
}