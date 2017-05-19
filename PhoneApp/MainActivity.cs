using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace PhoneApp
{
    [Activity(Label = "PhoneApp", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        static readonly System.Collections.Generic.List<string> PhoneNumbers =
                                  new System.Collections.Generic.List<string>();


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var PhoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            var TranslateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            var CallButton = FindViewById<Button>(Resource.Id.CallButton);
            var CallHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);
            var btnValidarActividad = FindViewById<Button>(Resource.Id.btnValidarActividad);


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
                     // Agregar el número marcado a la lista de números marcados
                     PhoneNumbers.Add(TranslatedNumber);
                     // Habilitar el botón CallHistotyButton
                     CallHistoryButton.Enabled = true;

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
            CallHistoryButton.Click += (sender, e) =>
         {
             var Intent = new Android.Content.Intent(this, typeof(CallHistoryActivity));
             Intent.PutStringArrayListExtra("phone_numbers", PhoneNumbers);
             StartActivity(Intent);
         };
            btnValidarActividad.Click += (s, a) =>
            {
                var intent = new Android.Content.Intent(this, typeof(ValidarActivity));
                StartActivity(intent);
            };

        }


    }
}