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
                title = div.InnerText;
                urls = Regex.Match(div.OuterHtml, "<a.+?href=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

                title = WebUtility.HtmlDecode(title);

                newPosts.Title = title;
                newPosts.ImageUrl = imageUrl;
                if (!urls.Contains("http:")) urls = "http:" + urls;
                newPosts.PostUrl = urls;

                Posts.Add(newPosts);

            }

            return Posts;
        }
    }

}
