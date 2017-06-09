using Otodidak.Models;

namespace Otodidak.ViewModels
{
	public class ItemDetailViewModel : DetailPostViewModel
	{
		public Post Item { get; set; }
		public ItemDetailViewModel(Post item = null)
		{
			Title = item.Title;
			Item = item;
		}

		int quantity = 1;
		public int Quantity
		{
			get { return quantity; }
			set { SetProperty(ref quantity, value); }
		}
	}
}