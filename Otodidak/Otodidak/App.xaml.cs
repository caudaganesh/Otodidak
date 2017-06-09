using Otodidak.ViewModels;
using Otodidak.Views;
using System.Threading.Tasks;
using Wikihow_Indonesia.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Otodidak
{
    //public static class ViewModelLocator
    //{
    //    static HalamanUtamaViewModel huVM;
    //    public static HalamanUtamaViewModel HalamanUtamaViewModel
    //    => huVM ?? (huVM = new HalamanUtamaViewModel());

    //    static CategoryViewModel catVM;
    //    public static CategoryViewModel CategoryViewModel
    //    => catVM ?? (catVM = new CategoryViewModel());

    //    static ItemDetailViewModel detVM;
    //    public static ItemDetailViewModel ItemDetailViewModel
    //    => detVM ?? (detVM = new ItemDetailViewModel());
    //}

    public partial class App : Application
	{
        

        //public static HalamanUtamaViewModel vm;
        public App()
		{
            //vm = new HalamanUtamaViewModel();
			InitializeComponent();
			SetMainPage();
		}


        protected override void OnStart()
        {
            base.OnStart();
            //ViewModelLocator.HalamanUtamaViewModel.LoadBodyCommand.Execute(null);
            //ViewModelLocator.HalamanUtamaViewModel.LoadHeaderCommand.Execute(null);
        }

        
        public static void SetMainPage()
		{
            
            Current.MainPage = new TabbedPage
            {
                

                Children =
                {
                    new NavigationPage(new HeaderPage())
                    {
                        Title = "Halaman Utama",

                        Icon = "ic_home_white_24dp.png"
                    },
                    new NavigationPage(new HotPage())
                    {
                        Title = "Hot",

                        Icon = "HotIcon.png"
                    },
                    new NavigationPage(new CategoryPage())
                    {
                        Title = "Kategori",
                        Icon = "ic_apps_white_24dp.png"
                    },
                    new NavigationPage(new FavoritePage())
                    {
                        Title = "Favorit",
                        Icon = "ic_favorite_white_24dp.png"
                    },
                    new NavigationPage(new BookmarkPage())
                    {
                        Title = "Markah",
                        Icon = "ic_bookmark_white_24dp.png"
                    },
                }
            };
        }
	}
}
