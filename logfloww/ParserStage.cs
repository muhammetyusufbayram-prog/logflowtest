using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace logfloww;
public class ParserStage
{
    // Common Log Format (CLF) için regex kalıbı
    private static readonly Regex ClfRegex = new Regex(
        @"^(\S+) \S+ \S+ \[(.*?)\] ""(\S+) (\S+)(?: \S+)?"" (\d{3}) (\d+|-)(?: ""(.*?)"" ""(.*?)""|.*)?$",
        RegexOptions.Compiled
    );

    // Hatalı satır sayacı
    public int InvalidLineCount { get; private set; } = 0;

    public void ProcessLine(string rawLine, CollectingEmitter<LogRecord> emitter)
    {
        // Boş satır kontrolü
        if (string.IsNullOrWhiteSpace(rawLine))
        {
            InvalidLineCount++;
            return;
        }

        var match = ClfRegex.Match(rawLine.Trim());
        if (!match.Success)
        {
            InvalidLineCount++;
            return;
        }

        try
        {
            string ip = match.Groups[1].Value;
            // Tarih okuma
            string dateStr = match.Groups[2].Value;
            DateTimeOffset timestamp = DateTimeOffset.ParseExact(dateStr, "dd/MMM/yyyy:HH:mm:ss zzz", System.Globalization.CultureInfo.InvariantCulture);

            string method = match.Groups[3].Value;
            string path = match.Groups[4].Value;
            int status = int.Parse(match.Groups[5].Value);
            long bytes = match.Groups[6].Value == "-" ? 0 : long.Parse(match.Groups[6].Value);
            string userAgent = match.Groups[8].Success ? match.Groups[8].Value : "-";

            // Başarıyla ayrışan veriden yeni kayıt oluştur
            var record = new LogRecord(
                timestamp, ip, method, path, status, bytes, userAgent,
                new Dictionary<string, string>(), rawLine
            );

            // Ürünü emitter'a gönder
            emitter.Emit(record);
        }
        catch
        {
            InvalidLineCount++;
        }
    }
}