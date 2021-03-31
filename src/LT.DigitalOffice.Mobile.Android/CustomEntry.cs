using Android.Graphics.Drawables;
using EntryValidationBorder.Droid.Renderers;
using LT.DigitalOffice.Mobile;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace EntryValidationBorder.Droid.Renderers
{
    [System.Obsolete]
    public class ExtendedEntryRenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control == null || e.NewElement == null) return;

			UpdateBorders();
		}

		void UpdateBorders()
		{
			GradientDrawable shape = new GradientDrawable();

			shape.SetShape(ShapeType.Rectangle);
			shape.SetCornerRadius(15);
			shape.SetColor(Android.Graphics.Color.ParseColor("#F1F1EF"));

			Control.SetBackground(shape);
		}

	}
}