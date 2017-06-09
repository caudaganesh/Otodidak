using System;
using System.Linq;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Net;
using Otodidak.Models;
using Otodidak.Helpers;
using Xamarin.Forms;
using Otodidak.ViewModels;
using System.Diagnostics;

namespace Wikihow_Indonesia.ViewModels
{
    public class CategoryViewModel:BaseViewModel
    {
       

        public ObservableRangeCollection<Post> CategoryPosts { get; set; }
        public ObservableRangeCollection<Grouping<string, Post>> ItemsGrouped { get; set; }

        public Command LoadCategoryCommand { get; set; }

        public CategoryViewModel()
        {
            CategoryPosts = new ObservableRangeCollection<Post>();
            ItemsGrouped = new ObservableRangeCollection<Grouping<string, Post>>();
            LoadCategoryCommand=new Command(async () => await ExecuteCategoryCommand());
        }

        private async Task ExecuteCategoryCommand()
        {
            if (IsCategoryBusy)
                return;

            IsCategoryBusy = true;

            try
            {
                CategoryPosts.Clear();
                await getCategory();
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
                IsCategoryBusy = false;
            }
        }
        

        //public static ObservableCollection<Post> CategoryPosts { get; set; } = new ObservableCollection<Post>();

        public async Task getCategory()
        {
            if (CategoryPosts == null) CategoryPosts = new ObservableRangeCollection<Post>();
            await getCategoryData();
            
        }

        public async Task getCategoryData()
        {
            if (CategoryPosts.Count != 0)
                return;

            string htmlPage="";
            try
            {

                using (var client = new HttpClient())
                {
                    htmlPage = await client.GetStringAsync("http://id.wikihow.com/Istimewa:Categorylisting").ConfigureAwait(false);
                }


                htmlDocument.LoadHtml(htmlPage);
                var innerText = htmlDocument.DocumentNode
                    .Descendants("div")
                    .Where(o => o.GetAttributeValue("class", "") == "section_text").FirstOrDefault();

                htmlDocument.LoadHtml(innerText.OuterHtml);
                var divs = htmlDocument.DocumentNode
                    .Descendants("div")
                    .Where(o => o.GetAttributeValue("class", "") == "thumbnail").ToList();

                foreach (var div in divs)
                {
                    Post newPosts = new Post();
                    string imageUrl = "";
                    string title = "";
                    string url = "";
                    

                    imageUrl = Regex.Match(div.OuterHtml, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    url = Regex.Match(div.OuterHtml, "<a.+?href=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;

                    var match = Regex.Match(div.OuterHtml, @"(<p.*?>.*?</p>)", RegexOptions.Singleline);
                    title = Regex.Replace(match.Groups[1].Value, @"\s*<.*?>\s*", "");

                    title = WebUtility.HtmlDecode(title);

                    newPosts.Title = title;
                    newPosts.ImageUrl = imageUrl;
                    newPosts.PostUrl = "http://id.wikihow.com" + url;

                    CategoryPosts.Add(newPosts);

                }
                

                var sorted = from item in CategoryPosts
                             orderby item.Title
                             group item by item.Title[0].ToString() into itemGroup
                             select new Grouping<string, Post>(itemGroup.Key, itemGroup);

               var itemsGrouped = new ObservableRangeCollection<Grouping<string, Post>>(sorted);

                ItemsGrouped.ReplaceRange(itemsGrouped);
            }
            catch (Exception)
            {

            }

        }

        
    }

    
}