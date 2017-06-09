using System;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Net;
using Otodidak.Models;
using Otodidak.Helpers;
using Xamarin.Forms;
using Otodidak.Views;

namespace Otodidak.ViewModels
{
    public class DetailPostViewModel:BaseViewModel
    {
        public DetailPost DetailPost { get; set; }
        public Command LoadDetailCommand { get; set; }

        public DetailPostViewModel()
        {
            DetailPost = new DetailPost();
            LoadDetailCommand = new Command(async () => await ExecuteDetailCommand(null));
        }

        public async Task ExecuteDetailCommand(Post post)
        {
            if (post!=null)
            await GetDetailPost(post);
        }

        public async Task GetDetailPost(Post post)
        {
            await GetDetail(post.PostUrl, post.ImageUrl);
        }


        public async Task GetDetail(string posturl, string postimageurl)
        {
            DetailPost dp = new DetailPost();
            string htmlPage = "";
            string category = "";
            try
            {

                using (var client = new HttpClient())
                {
                    if (!posturl.Contains("http:")) posturl = "http:" + posturl;
                    htmlPage = await client.GetStringAsync(posturl).ConfigureAwait(false);
                }

                HtmlDocument htmlDocument = new HtmlDocument();
                HtmlDocument innerHtmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(htmlPage);

                var innerText = htmlDocument.DocumentNode
                    .Descendants("div")
                    .Where(o => o.GetAttributeValue("id", "") == "bodycontents").FirstOrDefault();

               
                string htmlDiv = "";
                htmlDiv = "<html>" + innerText.OuterHtml + "</html>";
                innerHtmlDocument.LoadHtml(htmlDiv);
                
                // Get Category
                var innerinfo = htmlDocument.DocumentNode
                    .Descendants("p")
                    .Where(o => o.GetAttributeValue("class", "") == ("info")).FirstOrDefault();
                if(innerinfo!=null)
                { 
                    foreach (Match m in Regex.Matches(innerinfo.OuterHtml, "<a[^>]*>([^<]*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                    {
                        category = m.Groups[1].Value;

                        // add src to some array
                    }
                }
                
                // Get Title
                var title = htmlDocument.DocumentNode
                .Descendants("h1")
                .Where(o => o.GetAttributeValue("class", "") == "firstHeading").FirstOrDefault() ;

                var match = Regex.Match(title.InnerHtml, @"(<a.*?>.*?</a>)", RegexOptions.Singleline);
                string titles = Regex.Replace(match.Groups[1].Value, @"\s*<.*?>\s*", "");

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(title.OuterHtml);

                var link = doc.DocumentNode.Descendants("a")
                                  .Select(p => p.GetAttributeValue("href", "not found"))
                                  .FirstOrDefault();

                var img = innerHtmlDocument.DocumentNode
                .Descendants("img").ToList();
                var imageUrl = "";
                if (img.Count != 0)
                {
                     imageUrl= Regex.Match(img[img.Count - 1].OuterHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

                }
                else
                {
                    imageUrl = "http://pad3.whstatic.com/images/thumb/a/a4/Default_wikihow_green_intl.png/-crop-163-119-141px-Default_wikihow_green_intl.png";
                }
                

                const string FMT = "O";
                DateTime now1 = DateTime.Now;
                string strDate = now1.ToString(FMT);

                titles = WebUtility.HtmlDecode(titles);
                //htmlDiv = WebUtility.HtmlDecode(htmlDiv);

                dp.Title = titles;

                if (postimageurl != "") dp.ImageUrl = postimageurl;
                else
                    dp.ImageUrl = imageUrl;

                if (!link.Contains("http:")) link = "http:" + link;
                dp.Url = link;
                dp.SavedTime = strDate;
                dp.Content = htmlPage;
                if (category != "")
                    dp.Category = category;
                else dp.Category = "Tidak Terkategori";
            }
            catch
            {

            }
            DetailPost = dp;
        }
    }

   
}