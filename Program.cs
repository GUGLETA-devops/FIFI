
using System;
using System.Collections.Generic;
using System.Linq;

namespace projekat_find_it_found_it
{
    class Korisnik
    {
        public string Ime = "";
        public string Email = "";
        public string Lozinka = "";
    }

    class Predmet
    {
        public int Id;
        public string Naziv = "";
        public string Opis = "";
        public string Kategorija = "";
        public string Lokacija = "";
        public string Status = "";
        public string Fotografija = "";
        public string VlasnikEmail = "";
        public DateTime Datum = DateTime.Now;
    }

    class Poruka
    {
        public string Posiljalac = "";
        public string Primalac = "";
        public string Tekst = "";
        public DateTime Datum = DateTime.Now;
    }

    internal class Program
    {
        static List<Korisnik> korisnici = new();
        static List<Predmet> predmeti = new();
        static List<Poruka> poruke = new();

        static Korisnik? ulogovaniKorisnik;
        static int sledeciId = 1;

        static string[] kategorije =
        {
            "Elektronika", "Dokumenti", "Kljucevi",
            "Odeca", "Torbe", "Ostalo"
        };

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n===== FIFI =====");
                Console.WriteLine("1. Prijava");
                Console.WriteLine("2. Registracija");
                Console.WriteLine("3. Nastavi bez naloga");
                Console.WriteLine("0. Izlaz");
                Console.Write("Izbor: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Prijava();
                        break;
                    case "2":
                        Registracija();
                        break;
                    case "3":
                        Pocetna();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Nepostojeca opcija.");
                        break;
                }
            }
        }

        static void Registracija()
        {
            Console.Write("Ime i prezime: ");
            string ime = Console.ReadLine() ?? "";

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Lozinka: ");
            string lozinka = Console.ReadLine() ?? "";

            if (ime.Trim() == "" ||
                !email.Contains("@") ||
                lozinka.Length < 6)
            {
                Console.WriteLine(
                    "Unesi ime, ispravan email i lozinku od najmanje 6 znakova.");
                return;
            }

            if (korisnici.Any(k =>
                k.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("Ovaj email je vec registrovan.");
                return;
            }

            korisnici.Add(new Korisnik
            {
                Ime = ime,
                Email = email,
                Lozinka = lozinka
            });

            Console.WriteLine("Registracija je uspesna.");
        }

        static void Prijava()
        {
            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Lozinka: ");
            string lozinka = Console.ReadLine() ?? "";

            ulogovaniKorisnik = korisnici.FirstOrDefault(k =>
                k.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                && k.Lozinka == lozinka);

            if (ulogovaniKorisnik == null)
            {
                Console.WriteLine("Pogresan email ili lozinka.");
                return;
            }

            Console.WriteLine("Dobrodosao/la, " + ulogovaniKorisnik.Ime);
            Pocetna();
        }

        static void Pocetna()
        {
            while (true)
            {
                Console.WriteLine("\n===== FIFI - POCETNA =====");
                Console.WriteLine("1. Prijavi izgubljen predmet");
                Console.WriteLine("2. Prijavi pronadjen predmet");
                Console.WriteLine("3. Pretraga predmeta");
                Console.WriteLine("4. Svi predmeti");
                Console.WriteLine("5. Mapa - prikaz lokacija");
                Console.WriteLine("6. Poruke");
                Console.WriteLine("7. Moj profil");
                Console.WriteLine("8. Odjava");
                Console.WriteLine("0. Izlaz");
                Console.Write("Izbor: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        PrijaviPredmet("Izgubljen");
                        break;
                    case "2":
                        PrijaviPredmet("Pronadjen");
                        break;
                    case "3":
                        Pretraga();
                        break;
                    case "4":
                        PrikaziPredmete(predmeti);
                        break;
                    case "5":
                        PrikaziMapu();
                        break;
                    case "6":
                        PorukeMeni();
                        break;
                    case "7":
                        Profil();
                        break;
                    case "8":
                        ulogovaniKorisnik = null;
                        return;
                    case "0":
                        Environment.Exit(0);
                        return;
                    default:
                        Console.WriteLine("Nepostojeca opcija.");
                        break;
                }
            }
        }

        static void PrijaviPredmet(string status)
        {
            Predmet p = new Predmet();
            p.Id = sledeciId++;
            p.Status = status;

            Console.Write("Naziv predmeta: ");
            p.Naziv = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(p.Naziv))
            {
                Console.WriteLine("Naziv je obavezan.");
                return;
            }

            Console.Write("Opis predmeta: ");
            p.Opis = Console.ReadLine() ?? "";

            Console.WriteLine("\nKategorije:");
            for (int i = 0; i < kategorije.Length; i++)
                Console.WriteLine($"{i + 1}. {kategorije[i]}");

            Console.Write("Izbor kategorije: ");
            if (!int.TryParse(Console.ReadLine(), out int broj) ||
                broj < 1 || broj > kategorije.Length)
            {
                Console.WriteLine("Neispravna kategorija.");
                return;
            }

            p.Kategorija = kategorije[broj - 1];

            Console.Write("Lokacija: ");
            p.Lokacija = Console.ReadLine() ?? "";

            Console.Write("Putanja do fotografije (opciono): ");
            p.Fotografija = Console.ReadLine() ?? "";

            p.VlasnikEmail = ulogovaniKorisnik?.Email ?? "Gost";
            p.Datum = DateTime.Now;

            predmeti.Add(p);

            Console.WriteLine("Predmet je prijavljen. ID: " + p.Id);
        }

        static void Pretraga()
        {
            Console.Write("Naziv ili opis: ");
            string tekst = Console.ReadLine() ?? "";

            Console.WriteLine("1. Svi");
            Console.WriteLine("2. Izgubljeni");
            Console.WriteLine("3. Pronadjeni");
            Console.Write("Filter statusa: ");

            string status = Console.ReadLine() ?? "";

            var rezultati = predmeti.Where(p =>
                (p.Naziv.Contains(tekst,
                    StringComparison.OrdinalIgnoreCase) ||
                 p.Opis.Contains(tekst,
                    StringComparison.OrdinalIgnoreCase) ||
                 p.Lokacija.Contains(tekst,
                    StringComparison.OrdinalIgnoreCase))
                &&
                (status == "1" ||
                 (status == "2" && p.Status == "Izgubljen") ||
                 (status == "3" && p.Status == "Pronadjen"))
            ).ToList();

            PrikaziPredmete(rezultati);
        }

        static void PrikaziPredmete(List<Predmet> lista)
        {
            if (lista.Count == 0)
            {
                Console.WriteLine("Nema predmeta.");
                return;
            }

            foreach (Predmet p in lista)
            {
                Console.WriteLine("\n----------------------");
                Console.WriteLine("ID: " + p.Id);
                Console.WriteLine("Naziv: " + p.Naziv);
                Console.WriteLine("Status: " + p.Status);
                Console.WriteLine("Kategorija: " + p.Kategorija);
                Console.WriteLine("Lokacija: " + p.Lokacija);
                Console.WriteLine("Datum: " + p.Datum.ToString("dd.MM.yyyy"));
                Console.WriteLine("Unos fotografije: " +
                    (string.IsNullOrWhiteSpace(p.Fotografija)
                        ? "Nije uneta" : p.Fotografija));

                Console.WriteLine("1. Detalji");
                Console.WriteLine("2. Posalji poruku");
                Console.WriteLine("Enter - dalje");

                string izbor = Console.ReadLine() ?? "";

                if (izbor == "1")
                    Detalji(p);
                else if (izbor == "2")
                    PosaljiPoruku(p);
            }
        }

        static void Detalji(Predmet p)
        {
            Console.WriteLine("\n===== DETALJI =====");
            Console.WriteLine("Naziv: " + p.Naziv);
            Console.WriteLine("Opis: " + p.Opis);
            Console.WriteLine("Kategorija: " + p.Kategorija);
            Console.WriteLine("Status: " + p.Status);
            Console.WriteLine("Lokacija: " + p.Lokacija);
            Console.WriteLine("Datum: " + p.Datum);
            Console.WriteLine("Kontakt nalog: " + p.VlasnikEmail);
        }

        static void PrikaziMapu()
        {
            Console.WriteLine("\n===== LOKACIJE PREDMETA =====");

            foreach (Predmet p in predmeti)
                Console.WriteLine(
                    $"{p.Id}. {p.Naziv} - {p.Lokacija} ({p.Status})");

            Console.WriteLine(
                "Ovo je tekstualni prikaz. Za pravu mapu potreban je map servis.");
        }

        static void PosaljiPoruku(Predmet p)
        {
            if (ulogovaniKorisnik == null)
            {
                Console.WriteLine("Prijavi se da bi slao poruke.");
                return;
            }

            if (p.VlasnikEmail == "Gost" ||
                p.VlasnikEmail == ulogovaniKorisnik.Email)
            {
                Console.WriteLine("Nije moguce poslati poruku ovom korisniku.");
                return;
            }

            Console.Write("Poruka: ");
            string tekst = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(tekst))
                return;

            poruke.Add(new Poruka
            {
                Posiljalac = ulogovaniKorisnik.Email,
                Primalac = p.VlasnikEmail,
                Tekst = tekst
            });

            Console.WriteLine("Poruka je sacuvana.");
        }

        static void PorukeMeni()
        {
            if (ulogovaniKorisnik == null)
            {
                Console.WriteLine("Prijavi se da bi video poruke.");
                return;
            }

            var mojePoruke = poruke.Where(p =>
                p.Posiljalac == ulogovaniKorisnik.Email ||
                p.Primalac == ulogovaniKorisnik.Email).ToList();

            if (mojePoruke.Count == 0)
            {
                Console.WriteLine("Jos nemas poruka.");
                return;
            }

            foreach (Poruka p in mojePoruke)
            {
                Console.WriteLine("----------------------");
                Console.WriteLine(p.Posiljalac + " -> " + p.Primalac);
                Console.WriteLine(p.Tekst);
                Console.WriteLine(p.Datum);
            }
        }

        static void Profil()
        {
            if (ulogovaniKorisnik == null)
            {
                Console.WriteLine("Nisi prijavljen.");
                return;
            }

            Console.WriteLine("\n===== MOJ PROFIL =====");
            Console.WriteLine("Ime: " + ulogovaniKorisnik.Ime);
            Console.WriteLine("Email: " + ulogovaniKorisnik.Email);
            Console.WriteLine("\nMoje prijave:");

            var mojePrijave = predmeti
                .Where(p => p.VlasnikEmail == ulogovaniKorisnik.Email)
                .ToList();

            PrikaziPredmete(mojePrijave);
        }
    }
}
