using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using SaveImageToDatabaseSampleApp.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(EntryWithClearButtonRenderer))]
namespace SaveImageToDatabaseSampleApp.iOS
{
	public class EntryWithClearButtonRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (this.Control == null)
			{
				return;
			}

			var entry = this.Control as UITextField;

			entry.ClearButtonMode = UITextFieldViewMode.WhileEditing;

		}
	}
}