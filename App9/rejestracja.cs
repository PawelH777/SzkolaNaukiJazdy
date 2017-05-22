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
    [Activity(Label = "Activity1")]
    public class rejestracja : Activity
    {
        public override void OnBackPressed()
        {
            StartActivity(typeof(menu_glowne));
            Finish();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //ustawianie interfejsu widocznego na ekranie
            SetContentView(Resource.Layout.rejestracja);
            //stworzenie listy do spinnera i dodanie do niej kursów
            List<string> kursy = new List<string>();
            kursy.Add("A2");
            kursy.Add("A");
            kursy.Add("B");
            kursy.Add("B+E");
            kursy.Add("C");
            kursy.Add("C+E");
            kursy.Add("D");
            kursy.Add("D+E");
            //zdefiniowanie zmiennej spinner i pobranie do niej identyfikatora spinneru z layout1
            Spinner spinner = this.FindViewById<Spinner>(Resource.Id.Kursy);
            //dalej sa operacje odpowiadajace za spinner
            ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, kursy);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            spinner.Adapter = adapter;
            //zdefiniowanie pozosta³ych obiektów z layout1
            Button returnButton = FindViewById<Button>(Resource.Id.returnButton);
            EditText editImie = FindViewById<EditText>(Resource.Id.editImie);
            EditText editNazwisko = FindViewById<EditText>(Resource.Id.editNazwisko);
            EditText editHaslo = FindViewById<EditText>(Resource.Id.editHaslo);
            EditText editPowtorzHaslo = FindViewById<EditText>(Resource.Id.editPowtorzHaslo);
            EditText editEmail = FindViewById<EditText>(Resource.Id.editEmail);
            Spinner Kursy = FindViewById<Spinner>(Resource.Id.Kursy);
            returnButton.Click += delegate
            {
                //walidacja rejestracji
                bool registerflag = true;
                if (String.IsNullOrEmpty(editImie.Text))
                {
                    registerflag = false;
                    editImie.SetError("Nie wpisano imienia", null);
                }
                if (editImie.Text.Any(char.IsDigit))
                {
                    registerflag = false;
                    editImie.SetError("Imiê posiada symbole", null);
                }
                if (String.IsNullOrEmpty(editNazwisko.Text))
                {
                    registerflag = false;
                    editNazwisko.SetError("Nie wpisano nazwiska", null);
                }
                if (editNazwisko.Text.Any(char.IsDigit))
                {
                    registerflag = false;
                    editNazwisko.SetError("Nazwisko posiada symbole", null);
                }
                if (editHaslo.Text.Length < 6 || editHaslo.Text.Length > 20)
                {
                    registerflag = false;
                    editHaslo.SetError("Haslo jest zbyt krótkie lub zbyt d³ugie (musi mieæ ono od 6 do 20 symboli)", null);
                }
                if (editHaslo.Text != editPowtorzHaslo.Text)
                {
                    registerflag = false;
                    editPowtorzHaslo.SetError("Has³a ze sob¹ siê nie zgadzaj¹", null);
                }
                if (!Android.Util.Patterns.EmailAddress.Matcher(editEmail.Text).Matches())
                {
                    registerflag = false;
                    editEmail.SetError("Email jest wpisany niepoprawnie", null);
                }
                //jesli walidacja przebiegla pomyslnie to przechodzimy do ifa wrzucaj¹cego nowy rekord do bazy
                if (registerflag == true)
                {
                    Toast.MakeText(this, "Rejestracja zosta³a przeprowadzona pomyœlnie", ToastLength.Short).Show();
                    MySqlConnection con = new MySqlConnection("host=s44.hekko.net.pl;user=tkknives_osk;password=politechnikaopolska;database=tkknives_osk;default command timeout=0;");
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        int id;
                        MySqlCommand cmd = new MySqlCommand("INSERT INTO kursanci(email_k, imie_k, nazwisko_k, haslo_k, zatwierdzenie_k, data_konta_k) VALUES(@email,@imie, @nazwisko, @haslo, @zatwierdzenie, now())", con);
                        cmd.Parameters.AddWithValue("@email", editEmail.Text);
                        cmd.Parameters.AddWithValue("@imie", editImie.Text);
                        cmd.Parameters.AddWithValue("@nazwisko", editNazwisko.Text);
                        cmd.Parameters.AddWithValue("@haslo", editHaslo.Text);
                        cmd.Parameters.AddWithValue("@zatwierdzenie", 0);
                        cmd.ExecuteNonQuery();
                        MySqlCommand query = new MySqlCommand("select id_k from kursanci", con);
                        query.ExecuteNonQuery();
                        var wynik = query.ExecuteScalar();
                        id = Convert.ToInt32(wynik);
                        MySqlCommand cme = new MySqlCommand("insert into kursy_kursantow(id_k, id_i, kurs, pozostale_godziny, data_rozpoczecia) values(@id_k, @id_i, @kurs, @pozostale_godziny, now())", con);
                        cme.Parameters.AddWithValue("@id_k", id);
                        cme.Parameters.AddWithValue("@id_i", 1);
                        cme.Parameters.AddWithValue("@kurs", spinner.SelectedItem.ToString());
                        cme.Parameters.AddWithValue("@pozostale_godziny", 30);
                        cme.ExecuteNonQuery();

                    }
                    con.Close();
                    
                    StartActivity(typeof(menu_glowne));
                    Finish();
                }

            };
        }
        //funkcja która dziala gdy zmieniamy kurs w spinnerze
        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("Wybrany kurs to {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }
    }
}