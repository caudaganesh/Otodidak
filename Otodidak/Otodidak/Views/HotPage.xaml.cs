using Otodidak.Models;
using Otodidak.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Otodidak.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HotPage : ContentPage
	{
        HalamanUtamaViewModel vm;
		public HotPage ()
		{
			InitializeComponent ();
            this.BindingContext = vm = new HalamanUtamaViewModel();
            //vm = this.BindingContext as HalamanUtamaViewModel;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Post;
            if (item == null)
                return;
            //var NewPage = new MasterDetailPage();
            var NewPage = (new NavigationPage(new ItemDetailPage(new ItemDetailViewModel(item))));
            await Navigation.PushModalAsync(NewPage);
            
            // Manually deselect item
            HeaderListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            if (vm.BodyPosts.Count == 0)
                vm.LoadBodyCommand.Execute(null);
        }
    }

    
}
