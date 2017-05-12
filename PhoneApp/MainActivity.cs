using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace PhoneApp
{
	[Activity(Label = "PhoneApp", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		TextView tvPrimero, tvSegundo, tvTercero;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			var PhoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
			var TranslateButton = FindViewById<Button>(Resource.Id.TranslateButton);
			var CallButton = FindViewById<Button>(Resource.Id.CallButton);
			tvPrimero = FindViewById<TextView>(Resource.Id.tvPrimero);
			tvSegundo = FindViewById<TextView>(Resource.Id.tvSegundo);
			tvTercero = FindViewById<TextView>(Resource.Id.tvTercero);

			CallButton.Enabled = false;

			var TranslatedNumber = string.Empty;

			TranslateButton.Click += (object sender, System.EventArgs e) =>
		   {
			   var Translator = new PhoneTranslator();
			   TranslatedNumber = Translator.ToNumber(PhoneNumberText.Text);
			   if (string.IsNullOrWhiteSpace(TranslatedNumber))
			   {
				   // No hay número a llamar
				   CallButton.Text = "Llamar";
				   CallButton.Enabled = false;
			   }
			   else
			   {
				   CallButton.Enabled = true;
			   }
		   };
			CallButton.Click += (object sender, System.EventArgs e) =>
			 {
				 // Intentar marcar el número telefónico
				 var CallDialog = new AlertDialog.Builder(this);
				 CallDialog.SetMessage($"Llamar al número {TranslatedNumber}?");
				 CallDialog.SetNeutralButton("Llamar", delegate
				 {
					 // Crear un intento para marcar el número telefónico
					 var CallIntent =
						new Android.Content.Intent(Android.Content.Intent.ActionCall);
					 CallIntent.SetData(
											  Android.Net.Uri.Parse($"tel:{TranslatedNumber}"));

					 StartActivity(CallIntent);
				 });
				 CallDialog.SetNegativeButton("Cancelar", delegate { });
				 // Mostrar el cuadro de diálogo al usuario y esperar una respuesta.
				 CallDialog.Show();
			 };

			validar();
		}

		async void validar()
		{
			var serviceClient = new SALLab05.ServiceClient();
			var user = "mail";
			var pass = "****";
			var phoneInfo = Android.Provider.Settings.Secure.GetString(
								ContentResolver, Android.Provider.Settings.Secure.AndroidId); ;
			var serviceResult = await serviceClient.ValidateAsync(user, pass, phoneInfo);

			tvPrimero.Text = serviceResult.Status.ToString();
			tvSegundo.Text = serviceResult.Fullname;
			tvTercero.Text = serviceResult.Token;
		}
	}
}

