using System;

using Otodidak.Models;
using Otodidak.ViewModels;

using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Otodidak.Views
{
    public partial class HeaderPage : ContentPage
    {
        HalamanUtamaViewModel vm;

        public HeaderPage()
        {
            InitializeComponent();
            BindingContext = vm = new HalamanUtamaViewModel();
            //BindingContext = vm = new HalamanUtamaViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Post;
            if (item == null)
                return;

            await Navigation.PushModalAsync(new NavigationPage(new ItemDetailPage(new ItemDetailViewModel(item))));

            // Manually deselect item
            HeaderListView.SelectedItem = null;
        }
        

        protected override void OnAppearing()
        {

            base.OnAppearing();
            if (vm.HeaderPosts.Count == 0)
                vm.LoadHeaderCommand.Execute(null);


        }
    }
}
