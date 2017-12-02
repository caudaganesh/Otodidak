using HtmlAgilityPack;
using Otodidak.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Otodidak.Helpers
{
    public class PostHelper
    {
        public ObservableRangeCollection<Post> GetPostFromTd(List<HtmlNode> nodes)
        {
            ObservableRangeCollection<Post> Posts = new ObservableRangeCollection<Post>();
            foreach (var div in nodes)
            {
                Post newPosts = new Post();
                string imageUrl = "";
                string title = "";
                string urls = "";

                imageUrl = Regex.Match(div.OuterHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                //title = div.InnerText;
                title = Regex.Match(div.InnerHtml, "<a.+?href=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                urls = Regex.Match(div.OuterHtml, "<a.+?href=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

                title = WebUtility.HtmlDecode(title);
                title = title.Replace("//id.wikihow.com/", "");
                title = title.Replace("-", " ");

                newPosts.Title = title;
                newPosts.ImageUrl = imageUrl;
                //if (!urls.Contains("https:")) urls = "https:" + urls;
                newPosts.PostUrl = urls;

                Posts.Add(newPosts);

            }

            return Posts;
        }
    }

}
