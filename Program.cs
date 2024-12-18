using System;
using System.Collections.Generic;
using System.IO;

namespace ToDoListApp
{
    class Program
    {
        private static List<Gorev> gorevler = new List<Gorev>();
        private const string dosyaAdi = "gorevler.txt";

        static void Main()
        {
            GorevleriYukle();

            while (true)
            {
                Console.WriteLine("\n=== Yapılacaklar Listesi ===");
                Console.WriteLine("1. Görev Ekle");
                Console.WriteLine("2. Görevleri Listele");
                Console.WriteLine("3. Görev Sil");
                Console.WriteLine("4. Görevi Tamamlandı Olarak İşaretle");
                Console.WriteLine("5. Çıkış");
                Console.Write("Seçiminizi yapın: ");

                string secim = Console.ReadLine();
                switch (secim)
                {
                    case "1":
                        GorevEkle();
                        break;
                    case "2":
                        GorevListele();
                        break;
                    case "3":
                        GorevSil();
                        break;
                    case "4":
                        GorevTamamla();
                        break;
                    case "5":
                        GorevleriKaydet();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                        break;
                }
            }
        }

        static void GorevEkle()
        {
            Console.Write("\nYeni görev adı: ");
            string ad = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(ad))
            {
                gorevler.Add(new Gorev { Ad = ad, Tamamlandi = false });
                Console.WriteLine("Görev başarıyla eklendi!");
            }
            else
            {
                Console.WriteLine("Görev adı boş olamaz.");
            }
        }

        static void GorevListele()
        {
            if (gorevler.Count == 0)
            {
                Console.WriteLine("\nHenüz eklenmiş bir görev yok.");
                return;
            }

            Console.WriteLine("\n=== Görevler ===");
            for (int i = 0; i < gorevler.Count; i++)
            {
                string durum = gorevler[i].Tamamlandi ? "[Tamamlandı]" : "[Bekliyor]";
                Console.WriteLine($"{i + 1}. {durum} {gorevler[i].Ad}");
            }
        }

        static void GorevSil()
        {
            GorevListele();
            if (gorevler.Count == 0) return;

            Console.Write("\nSilmek istediğiniz görevin numarasını girin: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= gorevler.Count)
            {
                gorevler.RemoveAt(index - 1);
                Console.WriteLine("Görev başarıyla silindi!");
            }
            else
            {
                Console.WriteLine("Geçersiz bir seçim yaptınız.");
            }
        }

        static void GorevTamamla()
        {
            GorevListele();
            if (gorevler.Count == 0) return;

            Console.Write("\nTamamlandı olarak işaretlemek istediğiniz görevin numarasını girin: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= gorevler.Count)
            {
                gorevler[index - 1].Tamamlandi = true;
                Console.WriteLine("Görev başarıyla tamamlandı!");
            }
            else
            {
                Console.WriteLine("Geçersiz bir seçim yaptınız.");
            }
        }

        static void GorevleriKaydet()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(dosyaAdi))
                {
                    foreach (var gorev in gorevler)
                    {
                        sw.WriteLine($"{gorev.Ad}|{gorev.Tamamlandi}");
                    }
                }

                Console.WriteLine("Görevler başarıyla kaydedildi!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }

        static void GorevleriYukle()
        {
            if (!File.Exists(dosyaAdi)) return;

            try
            {
                using (StreamReader sr = new StreamReader(dosyaAdi))
                {
                    string satir;
                    while ((satir = sr.ReadLine()) != null)
                    {
                        var veri = satir.Split('|');
                        if (veri.Length == 2)
                        {
                            gorevler.Add(new Gorev
                            {
                                Ad = veri[0],
                                Tamamlandi = bool.Parse(veri[1])
                            });
                        }
                    }
                }

                Console.WriteLine("Görevler başarıyla yüklendi!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }
        }
    }

    class Gorev
    {
        public string Ad { get; set; }
        public bool Tamamlandi { get; set; }
    }
}
