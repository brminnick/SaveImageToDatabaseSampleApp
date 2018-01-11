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

			if (Control == null)
				return;			

            Control.ClearButtonMode = UITextFieldViewMode.WhileEditing;
		}
	}
}