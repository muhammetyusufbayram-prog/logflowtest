using System;
using Xunit;
using logfloww;


public class ParserStageTests
{
    // 1. Test: Geçerli ve doðru bir log satýrýnýn ayrýþtýrýlmasý
    [Fact]
    public void Test1_GecerliSatir()
    {
        var parser = new ParserStage();
        var emitter = new CollectingEmitter<LogRecord>();
        string line = @"127.0.0.1 - - [10/Oct/2000:13:55:36 -0700] ""GET /apache_pb.gif HTTP/1.0"" 200 2326 ""http://www.example.com/start.html"" ""Mozilla/4.08 [en] (Win98; I)""";

        parser.ProcessLine(line, emitter);

        Assert.Single(emitter.Items);
        Assert.Equal(0, parser.InvalidLineCount);
        Assert.Equal("127.0.0.1", emitter.Items[0].ClientIp);
    }

    // 2. Test: Boþ satýr verildiðinde hata sayacýnýn artmasý
    [Fact]
    public void Test2_BosSatir()
    {
        var parser = new ParserStage();
        var emitter = new CollectingEmitter<LogRecord>();

        parser.ProcessLine("   ", emitter);

        Assert.Empty(emitter.Items);
        Assert.Equal(1, parser.InvalidLineCount);
    }

    // 3. Test: Durum kodu sayý yerine harf (ABC) olduðunda hata vermesi
    [Fact]
    public void Test3_HataliDurumKodu()
    {
        var parser = new ParserStage();
        var emitter = new CollectingEmitter<LogRecord>();
        string line = @"127.0.0.1 - - [10/Oct/2000:13:55:36 -0700] ""GET /index.html HTTP/1.0"" ABC 2326";

        parser.ProcessLine(line, emitter);

        Assert.Empty(emitter.Items);
        Assert.Equal(1, parser.InvalidLineCount);
    }

    // 4. Test: Adres içinde sorgu parametreleri (?q=test) olan satýr
    [Fact]
    public void Test4_QueryStringIcerenSatir()
    {
        var parser = new ParserStage();
        var emitter = new CollectingEmitter<LogRecord>();
        string line = @"192.168.1.1 - - [10/Oct/2000:13:55:36 +0000] ""GET /search?q=test&lang=tr HTTP/1.1"" 200 512 ""-"" ""Mozilla/5.0""";

        parser.ProcessLine(line, emitter);

        Assert.Single(emitter.Items);
        Assert.Equal("/search?q=test&lang=tr", emitter.Items[0].Path);
    }

    // 5. Test: Eksik alan içeren yarým kalmýþ satýr
    [Fact]
    public void Test5_EksikAlan()
    {
        var parser = new ParserStage();
        var emitter = new CollectingEmitter<LogRecord>();
        string line = @"127.0.0.1 - - [10/Oct/2000:13:55:36 -0700]";

        parser.ProcessLine(line, emitter);

        Assert.Empty(emitter.Items);
        Assert.Equal(1, parser.InvalidLineCount);
    }

    // 6. Test: Geçersiz formatta yazýlmýþ tarih alaný
    [Fact]
    public void Test6_HataliZamanDamgasi()
    {
        var parser = new ParserStage();
        var emitter = new CollectingEmitter<LogRecord>();
        string line = @"127.0.0.1 - - [HATALI_TARIH] ""GET /index.html HTTP/1.0"" 200 2326";

        parser.ProcessLine(line, emitter);

        Assert.Empty(emitter.Items);
        Assert.Equal(1, parser.InvalidLineCount);
    }

    // 7. Test: Baþýnda ve sonunda fazla boþluk olan satýr
    [Fact]
    public void Test7_FazladanBosluk()
    {
        var parser = new ParserStage();
        var emitter = new CollectingEmitter<LogRecord>();
        string line = @"   127.0.0.1 - - [10/Oct/2000:13:55:36 -0700] ""GET /index.html HTTP/1.0"" 200 2326 ""-"" ""Mozilla/5.0""   ";

        parser.ProcessLine(line, emitter);

        Assert.Single(emitter.Items);
        Assert.Equal(0, parser.InvalidLineCount);
    }

    // 8. Test: Týrnak iþaretleri içinde uzun ve boþluklu User-Agent bilgisi
    [Fact]
    public void Test8_BoslukIcerenUserAgent()
    {
        var parser = new ParserStage();
        var emitter = new CollectingEmitter<LogRecord>();
        string line = @"10.0.0.1 - - [10/Oct/2000:13:55:36 +0000] ""GET /home HTTP/1.1"" 200 100 ""-"" ""Mozilla/5.0 (Windows NT 10.0; Win64; x64)""";

        parser.ProcessLine(line, emitter);

        Assert.Single(emitter.Items);
        Assert.Equal("Mozilla/5.0 (Windows NT 10.0; Win64; x64)", emitter.Items[0].UserAgent);
    }
}