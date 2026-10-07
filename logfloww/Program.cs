using System;
using System.IO;
using logfloww;

class Program
{
    static void Main(string[] args)
    {
        var parser = new ParserStage();
        var emitter = new CollectingEmitter<LogRecord>();

        // Yönergeye göre dosya yolu belirlenir (varsayılan: data/access-small.log)
        string filePath = args.Length > 0 ? args[0] : Path.Combine("data", "access-small.log");

        // Göreli yol kontrolü (klasörler üst seviyedeyse bulabilmesi için)
        if (!File.Exists(filePath) && File.Exists(Path.Combine("..", "..", "..", filePath)))
        {
            filePath = Path.Combine("..", "..", "..", filePath);
        }

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                parser.ProcessLine(line, emitter);
            }

            // ConsoleSink biçiminde yapılandırılmış kayıtları yazdırma
            foreach (var record in emitter.Items)
            {
                Console.WriteLine($"[{record.Timestamp}] {record.ClientIp} -> {record.Method} {record.Path} ({record.Status} - {record.Bytes} bytes)");
            }

            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Toplam Hatalı Satır Sayısı: {parser.InvalidLineCount}");
        }
        else
        {
            Console.WriteLine($"Hata: '{filePath}' dosyası bulunamadı.");
            Console.WriteLine("Lütfen proje dizinine 'data/access-small.log' dosyasını ekleyin.");
        }
    }
}
