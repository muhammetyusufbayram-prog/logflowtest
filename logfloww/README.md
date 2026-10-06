# LogFlow - Artım 2 (CLF Parser & Unit Tests)

Bu proje, Common Log Format (CLF) formatındaki web sunucusu log satırlarını ayrıştırarak tipli nesnelere dönüştüren hattı (pipeline) ve birim testlerini içerir.

## Eklenen Bileşenler
- **LogRecord**: Ayrıştırılan log verilerini tutan tipli veri yapısı.
- **ParserStage**: CLF formatındaki verileri regex/string işleme ile ayrıştıran aşama.
- **CollectingEmitter**: Testler için çıktıları bellekte toplayan test dublörü.
- **LogFlow.Tests**: Ayrıştırıcı ve aşama davranışlarını doğrulayan 8 adet xUnit birim testi.

## Test Kapsama Oranı (Test Coverage)
- **Satır Kapsama (Line Coverage):** %95+
- **Geçen Test Sayısı:** 8 / 8