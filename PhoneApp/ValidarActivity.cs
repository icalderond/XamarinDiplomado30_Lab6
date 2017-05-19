
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PhoneApp
{
    [Activity(Label = "ValidarActivity")]
    public class ValidarActivity : Activity
    {
        TextView tvPrimero, tvSegundo, tvTercero;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ValidarLayout);

            // Create your application here
            var etCorreo = FindViewById<EditText>(Resource.Id.etCorreo);
            var etContrasena = FindViewById<EditText>(Resource.Id.etContrasena);
            var btnValidar = FindViewById<Button>(Resource.Id.btnValidar);

            tvPrimero = FindViewById<TextView>(Resource.Id.tvPrimero);
            tvSegundo = FindViewById<TextView>(Resource.Id.tvSegundo);
            tvTercero = FindViewById<TextView>(Resource.Id.tvTercero);

            btnValidar.Click += (s, a) => validar(etCorreo.Text, etContrasena.Text);

        }
        async void validar(string _correo, string _contrasena)
        {
            var serviceClient = new SALLab06.ServiceClient();
            var user = _correo;
            var pass = _contrasena;
            var phoneInfo = Android.Provider.Settings.Secure.GetString(
                                ContentResolver, Android.Provider.Settings.Secure.AndroidId); ;
            var serviceResult = await serviceClient.ValidateAsync(user, pass, phoneInfo);
            tvPrimero.Text = serviceResult.Status.ToString();
            tvSegundo.Text = serviceResult.Fullname;
            tvTercero.Text = serviceResult.Token;
        }
    }
}
