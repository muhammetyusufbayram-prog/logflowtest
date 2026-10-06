using System;
using logfloww;

class Program
{
    static void Main(string[] args)
    {
        var parser = new ParserStage();
        var emitter = new CollectingEmitter<LogRecord>();

        // Örnek log satırı
        string sampleLine = @"127.0.0.1 - - [10/Oct/2000:13:55:36 -0700] ""GET /apache_pb.gif HTTP/1.0"" 200 2326 ""http://www.example.com/start.html"" ""Mozilla/4.08 [en] (Win98; I)""";

        parser.ProcessLine(sampleLine, emitter);

        // Ekran çıktısı (ConsoleSink mantığı)
        foreach (var record in emitter.Items)
        {
            Console.WriteLine($"[{record.Timestamp}] {record.ClientIp} -> {record.Method} {record.Path} ({record.Status} - {record.Bytes} bytes) - {record.UserAgent}");
        }

        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Toplam Hatalı Satır Sayısı: {parser.InvalidLineCount}");
    }
}