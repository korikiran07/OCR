using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Support.V7.App;
using Android.Provider;
using Android.Graphics;
using Android.Gms.Vision.Texts;
using Android.Util;
using Android.Gms.Vision;
using Java.Lang;

namespace OCR
{
	[Activity(Label = "OCR", MainLauncher = true, Icon = "@mipmap/icon",Theme="@style/Theme.AppCompat.Light.NoActionBar")]
	public class MainActivity : AppCompatActivity
	{

		/// <summary>
		/// Poc of Optical Character Recognition by google play service vision
		/// </summary>

		ImageView imgSample;
		Button btnCapture;
		TextView lblResult;

		Bitmap bitmap;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.Main);
            FnInitialization();
			FnClickEvents();
		}
		void FnInitialization()
		{
			imgSample = FindViewById<ImageView>(Resource.Id.imgSample);
			btnCapture = FindViewById<Button>(Resource.Id.btnCapture);
			lblResult = FindViewById<TextView>(Resource.Id.lblResult);

			bitmap = BitmapFactory.DecodeResource(ApplicationContext.Resources, Resource.Drawable.itr4);
			 imgSample.SetImageBitmap(bitmap);
             FnReadText(bitmap);

		}
		void FnClickEvents()
		{
			btnCapture.Click += delegate (object sender, System.EventArgs e)
			{
				FnTakePicture();
			};
		}


		void FnTakePicture()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			StartActivityForResult(intent, 1);
		}
		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			if (resultCode == Android.App.Result.Ok && requestCode == 1)
			{
				bitmap = (Bitmap)data.Extras.Get("data");
				imgSample.SetImageBitmap(bitmap);
				FnReadText(bitmap);
			}
		}

		void FnReadText(Bitmap image)
		{
			TextRecognizer textRecognizer = new TextRecognizer.Builder(this.Application.ApplicationContext).Build();
			if (!textRecognizer.IsOperational)
			{
				Log.Error("ERROR", "Detector dependencies are not yet available");
			}
			else
			{
				Frame frame = new Frame.Builder().SetBitmap(image).Build();
				SparseArray items = textRecognizer.Detect(frame);
				StringBuilder strBuilder = new StringBuilder();
				for (int i = 0; i < items.Size(); i++)
				{
					TextBlock item = (TextBlock)items.ValueAt(i);
					strBuilder.Append(item.Value);
					strBuilder.Append("\n");
				}
				lblResult.Text = strBuilder.ToString();
			}
		}

	}
}

