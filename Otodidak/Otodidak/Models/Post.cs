using System;
using System.Collections.Generic;
using System.Text;

namespace Otodidak.Models
{
    public class Post:BaseDataObject
    {
        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private string imageUrl;

        public string ImageUrl
        {
            get { return imageUrl; }
            set { SetProperty(ref imageUrl, value); }
        }

        private string postUrl;

        public string PostUrl
        {
            get { return postUrl; }
            set {SetProperty(ref postUrl, value); }
        }



    }
}
