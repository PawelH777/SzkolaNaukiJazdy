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

namespace App9
{
    [Activity(Label = "Activity2")]
    public class menu_kursanta : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Button terminarz, instruktorzy, kurs, statystyki;
            TextView powiadomienie;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menu_kursanta);
            Toast.MakeText(this, "Logowanie zosta³o przeprowadzone pomyœlnie", ToastLength.Short).Show();
            terminarz = FindViewById<Button>(Resource.Id.Terminarz);
            instruktorzy= FindViewById<Button>(Resource.Id.Instruktorzy);
            kurs = FindViewById<Button>(Resource.Id.Kurs);
            statystyki = FindViewById<Button>(Resource.Id.Statystyki);
            powiadomienie = FindViewById<TextView>(Resource.Id.Powiadomienia);
            terminarz.Click += delegate
            {
                StartActivity(typeof(terminarz));
                Finish();
            };
            instruktorzy.Click += delegate
            {
                StartActivity(typeof(Instruktorzy));
                Finish();
            };
            kurs.Click += delegate
            {
                StartActivity(typeof(Kurs));
                Finish();
            };
            statystyki.Click += delegate
            {
                StartActivity(typeof(Statystyki));
                Finish();
            };
            // Create your application here
        }
    }
}