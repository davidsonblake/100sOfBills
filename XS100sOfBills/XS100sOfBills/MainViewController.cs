using System;
using UIKit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Foundation;
using CoreGraphics;

namespace XS100sOfBills
{
	public class MainViewController : UIViewController
	{
		public List<UIImage> Images;

		public MainViewController ()
			:base(null, null)
		{
		}

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Images = new List<UIImage> ();
			var table = new UITableView (new CGRect (0, 70, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height - 70));
			table.Source = new MyTableSource (this);
			Add (table);

			var btn = new UIButton (new CGRect (0, 20, 80, 50));
			btn.SetTitle ("Reload", UIControlState.Normal);
			btn.TouchUpInside += async (sender, e) => await DownloadImages(table);
			Add (btn);

			await DownloadImages (table);

		}

		private async Task DownloadImages(UITableView table)
		{
			var rnd = new Random ();
			var client = new HttpClient ();
			Images.Clear ();

			for (int i = 0; i < 100; i++) {
				var num1 = rnd.Next (0, 9);
				var num2 = rnd.Next (0, 9);
				var bytes = await client.GetByteArrayAsync (string.Format ("http://fillmurray.com/20{0}/20{1}", num1, num2));
				Images.Add(UIImage.LoadFromData(NSData.FromArray(bytes)));
				table.ReloadData ();
			}
		}
	}

	public class MyTableSource : UITableViewSource
	{
		private readonly MainViewController _vc;

		public MyTableSource(MainViewController vc)
		{
			_vc = vc;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return _vc.Images.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = new UITableViewCell (UITableViewCellStyle.Default, "cell");
			cell.ImageView.Image = _vc.Images[(int)indexPath.Item];
			return cell;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return 200;
		}
	}
}

