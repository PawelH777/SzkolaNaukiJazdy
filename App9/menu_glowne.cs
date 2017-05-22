using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using MySql.Data.MySqlClient;
using System.Data;

namespace App9
{
    [Activity(Label = "App9", MainLauncher = true, Icon = "@drawable/icon")]
    public class menu_glowne : Activity
    {

        private EditText progEmail, progPass;
        private Button btnInsert, registerButton;
        private TextView txtSysLog;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.menu_glowne);
            List<string> konta = new List<string>();
            konta.Add("kursantem");
            konta.Add("instruktorem");
            progEmail = FindViewById<EditText>(Resource.Id.progEmail);
            progPass = FindViewById<EditText>(Resource.Id.progPass);
            btnInsert = FindViewById<Button>(Resource.Id.progBtn);
            txtSysLog = FindViewById<TextView>(Resource.Id.progSysLog);
            registerButton = FindViewById<Button>(Resource.Id.registerButton);
            Spinner spinner = this.FindViewById<Spinner>(Resource.Id.konta);
            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, konta);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            spinner.Adapter = adapter;
            btnInsert.Click += delegate 
            {
                MySqlConnection con = new MySqlConnection("host=s44.hekko.net.pl;user=tkknives_osk;password=politechnikaopolska;database=tkknives_osk;default command timeout=0;");
                if (con.State == ConnectionState.Closed)
                {
                    string whoiswho;
                    whoiswho = spinner.SelectedItem.ToString();
                    con.Open();
                    if (whoiswho == "kursantem")
                    {
                        MySqlCommand queryString = new MySqlCommand("select haslo_k from kursanci where email_k like (@email)", con);
                        queryString.Parameters.AddWithValue("@email", progEmail.Text);
                        queryString.ExecuteNonQuery();
                        var wynik = queryString.ExecuteScalar().ToString();
                        if (wynik == progPass.Text)
                        {
                            con.Close();
                            StartActivity(typeof(menu_kursanta));
                            Finish();
                        }
                        else
                        {
                            Toast.MakeText(this, "Błędny email lub hasło", ToastLength.Short).Show();
                        }
                        con.Close();
                    }
                    else if(whoiswho == "instruktorem")
                    {
                        MySqlCommand queryString = new MySqlCommand("select haslo_i from instruktorzy where email_i like (@email)", con);
                        queryString.Parameters.AddWithValue("@email", progEmail.Text);
                        queryString.ExecuteNonQuery();
                        var wynik = queryString.ExecuteScalar().ToString();
                        if (wynik == progPass.Text)
                        {
                            con.Close();
                            StartActivity(typeof(menu_instruktora));
                            Finish();
                        }
                        else
                        {
                            Toast.MakeText(this, "Błędny email lub hasło", ToastLength.Short).Show();
                        }
                        con.Close();
                    }
                }
            };
            registerButton.Click += delegate {
                StartActivity(typeof(rejestracja));
                Finish();
            };
        }
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            spinner.GetItemAtPosition(e.Position);
        }
    }
}

