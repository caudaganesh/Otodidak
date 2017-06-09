using HtmlAgilityPack;
using Otodidak.Helpers;
using Otodidak.Models;
using Otodidak.Services;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Otodidak.ViewModels
{
	public class BaseViewModel : ObservableObject
	{
		/// <summary>
		/// Get the azure service instance
		/// </summary>
		public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

		bool isHeaderBusy = false;
		public bool IsHeaderBusy
		{
			get { return isHeaderBusy; }
			set { SetProperty(ref isHeaderBusy, value); }
		}

        bool isBodyBusy = false;
        public bool IsBodyBusy
        {
            get { return isBodyBusy; }
            set { SetProperty(ref isBodyBusy, value); }
        }

        bool isCategoryBusy = false;
        public bool IsCategoryBusy
        {
            get { return isCategoryBusy; }
            set { SetProperty(ref isCategoryBusy, value); }
        }

        /// <summary>
        /// Private backing field to hold the title
        /// </summary>
        string title = string.Empty;
		/// <summary>
		/// Public property to set and get the title of the item
		/// </summary>
		public string Title
		{
			get { return title; }
			set { SetProperty(ref title, value); }
		}

        public HtmlDocument htmlDocument = new HtmlDocument();
        public HtmlDocument innerHtmlDocument = new HtmlDocument();

        public class Grouping<K, T> : ObservableRangeCollection<T>
        {
            public K Key { get; private set; }

            public Grouping(K key, IEnumerable<T> items)
            {
                Key = key;
                foreach (var item in items)
                    this.Items.Add(item);
            }
        }
    }
}

