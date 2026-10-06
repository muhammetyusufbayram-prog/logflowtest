using System;
using System.Collections.Generic;
namespace logfloww;
// Boru hattında taşınacak paketimiz (değişmez/immutable veri yapısı)
public record LogRecord(
    DateTimeOffset Timestamp,
    string ClientIp,
    string Method,
    string Path,
    int Status,
    long Bytes,
    string UserAgent,
    IReadOnlyDictionary<string, string> Attributes, // İlerleyen haftalarda eklenecek veriler için esnek alan
    string Raw                                      // Ham log satırının kendisi
);