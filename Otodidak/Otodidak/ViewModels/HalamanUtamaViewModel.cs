using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Net;
using HtmlAgilityPack;
using Otodidak.Models;
using Otodidak.Helpers;
using Xamarin.Forms;
using System.Diagnostics;

namespace Otodidak.ViewModels
{
    public class HalamanUtamaViewModel:BaseViewModel
    {
        public ObservableRangeCollection<Post> BodyPosts { get; set; }
        public ObservableRangeCollection<Post> HeaderPosts { get; set; } 

        public Command LoadHeaderCommand { get; set; }
        public Command LoadBodyCommand { get; set; }

        public HalamanUtamaViewModel()
        {
            HeaderPosts = new ObservableRangeCollection<Post>();
            BodyPosts = new ObservableRangeCollection<Post>();

            LoadHeaderCommand = new Command(async () => await ExecuteHeaderCommand());
            LoadBodyCommand = new Command(async () => await ExecuteBodyCommand());
        }

        public async Task ExecuteHeaderCommand()
        {
            if (IsHeaderBusy)
                return;

            IsHeaderBusy = true;

            try
            {
                HeaderPosts.Clear();
                await getHeaderArticleData().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsHeaderBusy = false;
            }
        }

        public async Task ExecuteBodyCommand()
        {
            if (IsBodyBusy)
                return;

            IsBodyBusy = true;

            try
            {
                BodyPosts.Clear();
                await getBodyArticleData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBodyBusy = false;
            }
        }

        public async Task getHeaderArticle()
        {
            await getHeaderArticleData();
            
        }

        public async Task getHeaderArticleData()
        {

            if (HeaderPosts.Count != 0)
                return;

            string htmlPage;

            try
            {
                using (var client = new HttpClient())
                {
                    htmlPage = await client.GetStringAsync("http://id.wikihow.com/Halaman-Utama").ConfigureAwait(false);
                }


                htmlDocument.LoadHtml(htmlPage);
                var innerText = htmlDocument.DocumentNode
                    .Descendants("div")
                    .Where(o => o.GetAttributeValue("class", "") == "hp_top").ToList();


                foreach (var div in innerText)
                {
                    Post newPosts = new Post();
                    string imageUrl = "";
                    string title = "";
                    string url = "";

                    string htmlDiv = "<html>" + div.OuterHtml + "</html>";



                    imageUrl = Regex.Match(htmlDiv, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    title = Regex.Match(htmlDiv, "<div.+?title=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    url = Regex.Match(htmlDiv, "<a.+?href=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    title = WebUtility.HtmlDecode(title);
                    //HtmlDecode
                    //"http://id.wikihow.com" +
                    newPosts.Title ="CARA "+ title;
                    newPosts.ImageUrl = imageUrl;
                    newPosts.PostUrl = "https://id.wikihow.com" + url;

                    HeaderPosts.Add(newPosts);

                }
            }
            catch (Exception)
            {

            }

        }

        public async Task getBodyArticle()
        {
            await getBodyArticleData();
        }

        public async Task getBodyArticleData()
        {
            if (BodyPosts.Count != 0)
                return;

            string htmlPage;

            try
            {
                using (var client = new HttpClient())
                {
                    htmlPage = await client.GetStringAsync("http://id.wikihow.com/Halaman-Utama").ConfigureAwait(false);
                }


                htmlDocument.LoadHtml(htmlPage);
                var innerText = htmlDocument.DocumentNode
                    .Descendants("div")
                    .Where(o => o.GetAttributeValue("id", "") == "fa_container").FirstOrDefault();

                innerHtmlDocument.LoadHtml(innerText.OuterHtml);

                var td = innerHtmlDocument.DocumentNode
                    .Descendants("td").ToList();

                PostHelper ph = new PostHelper();
                var bodyposts = ph.GetPostFromTd(td);
                BodyPosts.ReplaceRange(bodyposts);
                #region unused
                //foreach (var div in td)
                //{
                //    Post newPosts = new Post();
                //    string imageUrl = "";
                //    string title = "";
                //    string url = "";


                //    imageUrl = Regex.Match(div.OuterHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                //    title = div.InnerText;
                //    url = Regex.Match(div.OuterHtml, "<a.+?href=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

                //    newPosts.Title = title;
                //    newPosts.ImageUrl = imageUrl;
                //    if (!url.Contains("http:")) url = "http:" + url;
                //    newPosts.Url = url;

                //    BodyPosts.Add(newPosts);

                //} 
                #endregion
            }
            catch (Exception)
            {

            }


        }
    }

}
