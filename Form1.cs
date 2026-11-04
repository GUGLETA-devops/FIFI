using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FindItFoundIt
{
    public class FifiForm : Form
    {
        private class Korisnik
        {
            public string Ime, Email, Lozinka;
        }

        private class Predmet
        {
            public int Id;
            public string Naziv, Opis, Kategorija, Lokacija, Status, Fotografija, VlasnikEmail;
            public DateTime Datum;
        }

        private class Poruka
        {
            public string Posiljalac, Primalac, Tekst;
            public DateTime Datum;
        }

        private readonly List<Korisnik> korisnici = new List<Korisnik>();
        private readonly List<Predmet> predmeti = new List<Predmet>();
        private readonly List<Poruka> poruke = new List<Poruka>();
        private readonly string[] kategorije = { "Elektronika", "Dokumenti", "Kljucevi", "Odeca", "Torbe", "Ostalo" };
        private Korisnik ulogovaniKorisnik;
        private int sledeciId = 1;
        private readonly Panel sadrzaj = new Panel();
        private readonly Label naslov = new Label();
        private readonly Label stanje = new Label();

        public FifiForm()
        {
            Text = "FIFI - Find It / Found It";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(800, 600);
            Size = new Size(1020, 740);
            BackColor = Color.FromArgb(244, 247, 251);
            Font = new Font("Segoe UI", 10f);

            Panel vrh = new Panel { Dock = DockStyle.Top, Height = 86, BackColor = Color.FromArgb(34, 56, 89) };
            naslov.Text = "FIFI  |  FIND IT / FOUND IT";
            naslov.ForeColor = Color.White;
            naslov.Font = new Font("Segoe UI", 19, FontStyle.Bold);
            naslov.Location = new Point(24, 13);
            naslov.AutoSize = true;
            stanje.ForeColor = Color.FromArgb(222, 230, 245);
            stanje.Location = new Point(26, 53);
            stanje.AutoSize = true;
            vrh.Controls.Add(naslov);
            vrh.Controls.Add(stanje);

            sadrzaj.Dock = DockStyle.Fill;
            sadrzaj.AutoScroll = true;
            sadrzaj.Padding = new Padding(25);
            Controls.Add(sadrzaj);
            Controls.Add(vrh);
            PrikaziPocetnu();
        }

        private FlowLayoutPanel NovaStrana(string naslovStrane)
        {
            sadrzaj.Controls.Clear();
            stanje.Text = ulogovaniKorisnik == null ? "Niste prijavljeni (gost)" : "Prijavljen: " + ulogovaniKorisnik.Ime;
            FlowLayoutPanel p = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(10),
                BackColor = Color.White
            };
            p.Controls.Add(new Label
            {
                Text = naslovStrane,
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                AutoSize = true,
                Margin = new Padding(7, 8, 7, 22)
            });
            sadrzaj.Controls.Add(p);
            return p;
        }

        private Button Dugme(string tekst, Action akcija)
        {
            Button b = new Button
            {
                Text = tekst,
                Width = 280,
                Height = 43,
                Margin = new Padding(7, 5, 7, 5),
                BackColor = Color.FromArgb(48, 104, 179),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            b.FlatAppearance.BorderSize = 0;
            b.Click += (sender, args) => akcija();
            return b;
        }

        private TextBox Polje(FlowLayoutPanel panel, string natpis, bool lozinka = false, bool viseRedova = false)
        {
            panel.Controls.Add(new Label { Text = natpis, AutoSize = true, Margin = new Padding(7, 12, 7, 2) });
            TextBox polje = new TextBox
            {
                Width = 520,
                Height = viseRedova ? 85 : 29,
                Multiline = viseRedova,
                UseSystemPasswordChar = lozinka,
                Margin = new Padding(7, 0, 7, 6)
            };
            panel.Controls.Add(polje);
            return polje;
        }

        private ComboBox Izbor(FlowLayoutPanel panel, string natpis, IEnumerable<string> opcije)
        {
            panel.Controls.Add(new Label { Text = natpis, AutoSize = true, Margin = new Padding(7, 12, 7, 2) });
            ComboBox c = new ComboBox { Width = 520, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(7, 0, 7, 6) };
            c.Items.AddRange(opcije.Cast<object>().ToArray());
            if (c.Items.Count > 0) c.SelectedIndex = 0;
            panel.Controls.Add(c);
            return c;
        }

        private void Nazad(FlowLayoutPanel panel)
        {
            panel.Controls.Add(Dugme("← Nazad na početnu", PrikaziPocetnu));
        }

        private void PrikaziPocetnu()
        {
            FlowLayoutPanel p = NovaStrana("Početna strana");
            if (ulogovaniKorisnik == null)
            {
                p.Controls.Add(Dugme("Prijava", Prijava));
                p.Controls.Add(Dugme("Registracija", Registracija));
                p.Controls.Add(Dugme("Nastavi bez naloga", Meni));
            }
            else
            {
                p.Controls.Add(Dugme("Otvori glavni meni", Meni));
                p.Controls.Add(Dugme("Odjava", () => { ulogovaniKorisnik = null; PrikaziPocetnu(); }));
            }
            p.Controls.Add(Dugme("Izlaz", Close));
        }

        private void Registracija()
        {
            FlowLayoutPanel p = NovaStrana("Registracija");
            TextBox ime = Polje(p, "Ime i prezime");
            TextBox email = Polje(p, "Email");
            TextBox lozinka = Polje(p, "Lozinka (najmanje 6 karaktera)", true);
            p.Controls.Add(Dugme("Registruj se", () =>
            {
                string e = email.Text.Trim();
                if (string.IsNullOrWhiteSpace(ime.Text) || !e.Contains("@") || lozinka.Text.Length < 6)
                {
                    MessageBox.Show("Unesi ime, ispravan email i lozinku od najmanje 6 znakova.");
                    return;
                }
                if (korisnici.Any(k => string.Equals(k.Email, e, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Ovaj email je već registrovan.");
                    return;
                }
                korisnici.Add(new Korisnik { Ime = ime.Text.Trim(), Email = e, Lozinka = lozinka.Text });
                MessageBox.Show("Registracija je uspešna. Sada se prijavi.");
                Prijava();
            }));
            Nazad(p);
        }

        private void Prijava()
        {
            FlowLayoutPanel p = NovaStrana("Prijava");
            TextBox email = Polje(p, "Email");
            TextBox lozinka = Polje(p, "Lozinka", true);
            p.Controls.Add(Dugme("Prijavi se", () =>
            {
                ulogovaniKorisnik = korisnici.FirstOrDefault(k =>
                    string.Equals(k.Email, email.Text.Trim(), StringComparison.OrdinalIgnoreCase)
                    && k.Lozinka == lozinka.Text);
                if (ulogovaniKorisnik == null)
                {
                    MessageBox.Show("Pogrešan email ili lozinka.");
                    return;
                }
                Meni();
            }));
            Nazad(p);
        }

        private void Meni()
        {
            FlowLayoutPanel p = NovaStrana("Glavni meni");
            p.Controls.Add(Dugme("Prijavi izgubljen predmet", () => PrijaviPredmet("Izgubljen")));
            p.Controls.Add(Dugme("Prijavi pronađen predmet", () => PrijaviPredmet("Pronadjen")));
            p.Controls.Add(Dugme("Pretraga predmeta", Pretraga));
            p.Controls.Add(Dugme("Svi predmeti", () => PrikaziPredmete(predmeti.ToList(), "Svi predmeti")));
            p.Controls.Add(Dugme("Mapa - lokacije (tekstualno)", PrikaziMapu));
            p.Controls.Add(Dugme("Poruke", PorukeMeni));
            p.Controls.Add(Dugme("Moj profil", Profil));
            p.Controls.Add(Dugme("Početna / odjava", PrikaziPocetnu));
        }

        private void PrijaviPredmet(string status)
        {
            FlowLayoutPanel p = NovaStrana("Prijavi predmet — " + (status == "Izgubljen" ? "izgubljen" : "pronađen"));
            TextBox naziv = Polje(p, "Naziv predmeta");
            TextBox opis = Polje(p, "Opis", viseRedova: true);
            ComboBox kategorija = Izbor(p, "Kategorija", kategorije);
            TextBox lokacija = Polje(p, "Lokacija");
            TextBox fotografija = Polje(p, "Putanja do fotografije (opciono)");
            p.Controls.Add(Dugme("Sačuvaj predmet", () =>
            {
                if (string.IsNullOrWhiteSpace(naziv.Text))
                {
                    MessageBox.Show("Naziv predmeta je obavezan.");
                    return;
                }
                Predmet novi = new Predmet
                {
                    Id = sledeciId++,
                    Naziv = naziv.Text.Trim(),
                    Opis = opis.Text.Trim(),
                    Kategorija = kategorija.SelectedItem.ToString(),
                    Lokacija = lokacija.Text.Trim(),
                    Status = status,
                    Fotografija = fotografija.Text.Trim(),
                    VlasnikEmail = ulogovaniKorisnik == null ? "Gost" : ulogovaniKorisnik.Email,
                    Datum = DateTime.Now
                };
                predmeti.Add(novi);
                MessageBox.Show("Predmet je prijavljen. ID: " + novi.Id);
                Meni();
            }));
            p.Controls.Add(Dugme("← Glavni meni", Meni));
        }

        private void Pretraga()
        {
            FlowLayoutPanel p = NovaStrana("Pretraga predmeta");
            TextBox tekst = Polje(p, "Naziv, opis ili lokacija");
            ComboBox status = Izbor(p, "Status", new[] { "Svi", "Izgubljen", "Pronadjen" });
            p.Controls.Add(Dugme("Pretraži", () =>
            {
                string upit = tekst.Text.Trim();
                string filter = status.SelectedItem.ToString();
                List<Predmet> rezultati = predmeti.Where(x =>
                    (x.Naziv.IndexOf(upit, StringComparison.OrdinalIgnoreCase) >= 0 ||
                     x.Opis.IndexOf(upit, StringComparison.OrdinalIgnoreCase) >= 0 ||
                     x.Lokacija.IndexOf(upit, StringComparison.OrdinalIgnoreCase) >= 0) &&
                    (filter == "Svi" || x.Status == filter)).ToList();
                PrikaziPredmete(rezultati, "Rezultati pretrage");
            }));
            p.Controls.Add(Dugme("← Glavni meni", Meni));
        }

        private void PrikaziPredmete(List<Predmet> lista, string naslovStrane)
        {
            FlowLayoutPanel p = NovaStrana(naslovStrane);
            if (lista.Count == 0)
                p.Controls.Add(new Label { Text = "Nema predmeta.", AutoSize = true });
            foreach (Predmet predmet in lista)
            {
                Predmet x = predmet;
                p.Controls.Add(new Label
                {
                    Text = string.Format("#{0}  {1}  |  {2}  |  {3}  |  {4:dd.MM.yyyy}", x.Id, x.Naziv, x.Status, x.Lokacija, x.Datum),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Margin = new Padding(7, 12, 7, 2)
                });
                p.Controls.Add(Dugme("Detalji / pošalji poruku (#" + x.Id + ")", () => Detalji(x)));
            }
            p.Controls.Add(Dugme("← Glavni meni", Meni));
        }

        private void Detalji(Predmet x)
        {
            FlowLayoutPanel p = NovaStrana("Detalji predmeta #" + x.Id);
            string info = "Naziv: " + x.Naziv + "\nOpis: " + x.Opis +
                          "\nKategorija: " + x.Kategorija + "\nStatus: " + x.Status +
                          "\nLokacija: " + x.Lokacija + "\nDatum: " + x.Datum.ToString("dd.MM.yyyy") +
                          "\nFotografija: " + (x.Fotografija == "" ? "Nije uneta" : x.Fotografija) +
                          "\nKontakt: " + x.VlasnikEmail;
            p.Controls.Add(new Label { Text = info, AutoSize = true, Margin = new Padding(7, 5, 7, 20) });
            if (ulogovaniKorisnik != null && x.VlasnikEmail != "Gost" &&
                !string.Equals(x.VlasnikEmail, ulogovaniKorisnik.Email, StringComparison.OrdinalIgnoreCase))
            {
                TextBox poruka = Polje(p, "Poruka vlasniku", viseRedova: true);
                p.Controls.Add(Dugme("Pošalji poruku", () =>
                {
                    if (string.IsNullOrWhiteSpace(poruka.Text)) return;
                    poruke.Add(new Poruka
                    {
                        Posiljalac = ulogovaniKorisnik.Email,
                        Primalac = x.VlasnikEmail,
                        Tekst = poruka.Text.Trim(),
                        Datum = DateTime.Now
                    });
                    MessageBox.Show("Poruka je sačuvana.");
                    Meni();
                }));
            }
            p.Controls.Add(Dugme("← Svi predmeti", () => PrikaziPredmete(predmeti.ToList(), "Svi predmeti")));
        }

        private void PrikaziMapu()
        {
            FlowLayoutPanel p = NovaStrana("Lokacije predmeta — tekstualni prikaz");
            foreach (Predmet x in predmeti)
                p.Controls.Add(new Label { Text = x.Id + ". " + x.Naziv + " — " + x.Lokacija + " (" + x.Status + ")", AutoSize = true });
            p.Controls.Add(new Label { Text = "Za pravu interaktivnu mapu potreban je dodatni map servis.", AutoSize = true, Margin = new Padding(7, 20, 7, 10) });
            p.Controls.Add(Dugme("← Glavni meni", Meni));
        }

        private void PorukeMeni()
        {
            if (ulogovaniKorisnik == null)
            {
                MessageBox.Show("Prijavi se da bi video poruke.");
                Prijava();
                return;
            }
            FlowLayoutPanel p = NovaStrana("Moje poruke");
            List<Poruka> moje = poruke.Where(x => x.Posiljalac == ulogovaniKorisnik.Email ||
                                                     x.Primalac == ulogovaniKorisnik.Email).ToList();
            if (moje.Count == 0) p.Controls.Add(new Label { Text = "Još nemaš poruka.", AutoSize = true });
            foreach (Poruka x in moje)
                p.Controls.Add(new Label
                {
                    Text = x.Datum.ToString("dd.MM.yyyy HH:mm") + "  " + x.Posiljalac + " → " + x.Primalac + "\n" + x.Tekst,
                    AutoSize = true,
                    Margin = new Padding(7, 9, 7, 13)
                });
            p.Controls.Add(Dugme("← Glavni meni", Meni));
        }

        private void Profil()
        {
            if (ulogovaniKorisnik == null)
            {
                MessageBox.Show("Nisi prijavljen.");
                Prijava();
                return;
            }
            FlowLayoutPanel p = NovaStrana("Moj profil");
            p.Controls.Add(new Label { Text = "Ime: " + ulogovaniKorisnik.Ime + "\nEmail: " + ulogovaniKorisnik.Email, AutoSize = true });
            p.Controls.Add(Dugme("Moje prijave", () => PrikaziPredmete(
                predmeti.Where(x => x.VlasnikEmail == ulogovaniKorisnik.Email).ToList(), "Moje prijave")));
            p.Controls.Add(Dugme("← Glavni meni", Meni));
        }
    }
}
