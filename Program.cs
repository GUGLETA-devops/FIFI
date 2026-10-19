using System;
using System.Collections.Generic;

namespace projekat_find_it_found_it
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var predmeti = new List<string>();

            while (true)
            {
                Console.WriteLine("\n=== Find It, Found It ===");
                Console.WriteLine("1. Prijavi izgubljen predmet");
                Console.WriteLine("2. Prijavi pronađen predmet");
                Console.WriteLine("3. Prikaži prijavljene predmete");
                Console.WriteLine("0. Izlaz");
                Console.Write("Izaberi opciju: ");

                string izbor = Console.ReadLine();

                if (izbor == "0")
                {
                    Console.WriteLine("Doviđenja!");
                    break;
                }


                if (izbor == "1" || izbor == "2")
                {
                    Console.Write("Naziv predmeta: ");
                    string naziv = Console.ReadLine() ?? "";

                    Console.Write("Lokacija: ");
                    string lokacija = Console.ReadLine() ?? "";

                    string vrsta = izbor == "1" ? "Izgubljen" : "Pronađen";
                    predmeti.Add($"{vrsta}: {naziv} — lokacija: {lokacija}");

                    Console.WriteLine("Predmet je prijavljen.");
                }
                else if (izbor == "3")
                {
                    if (predmeti.Count == 0)
                    {
                        Console.WriteLine("Još nema prijavljenih predmeta.");
                    }
                    else
                    {
                        foreach (string predmet in predmeti)
                        {
                            Console.WriteLine("- " + predmet);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ta opcija ne postoji. Pokušaj ponovo.");
                }
            }
        }
    }
}