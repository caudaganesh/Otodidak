
using Otodidak.ViewModels;

using Xamarin.Forms;
using static Otodidak.Helpers.WebHelper;

namespace Otodidak.Views
{
	public partial class ItemDetailPage : ContentPage
	{
		ItemDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public ItemDetailPage()
        {
            InitializeComponent();
        }

        public ItemDetailPage(ItemDetailViewModel viewModel)
		{
			InitializeComponent();
            //NavigationPage.SetHasBackButton(this, true);
			BindingContext = this.viewModel = viewModel;
           

        }

        protected override async void OnAppearing()
        {
            var helper = new WebContentHelper();
            await viewModel.ExecuteDetailCommand(viewModel.Item);
            var html = new HtmlWebViewSource
            {
                Html = helper.WrapContent(viewModel.DetailPost.Content)
            };
            ContentWebView.Source = html;
        }

        private void BookmarkButton_Clicked(object sender, System.EventArgs e)
        {

        }

        private void ShareButton_Clicked(object sender, System.EventArgs e)
        {

        }

        private void LinkButton_Clicked(object sender, System.EventArgs e)
        {

        }

        private void ShuffleButton_Clicked(object sender, System.EventArgs e)
        {

        }

        private void RefreshButton_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}
