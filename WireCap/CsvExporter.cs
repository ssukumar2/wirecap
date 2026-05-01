using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace WireCap
{
    public class CsvExporter
    {
        private readonly string _filePath;
        private readonly StreamWriter _writer;
        private bool _headerWritten;

        public CsvExporter(string filePath)
        {
            _filePath = filePath;
            _writer = new StreamWriter(filePath, append: true);
            _headerWritten = File.Exists(filePath) && new FileInfo(filePath).Length > 0;
        }

        public void WriteHeader(IEnumerable<string> columns)
        {
            if (_headerWritten) return;
            _writer.WriteLine("timestamp," + string.Join(",", columns));
            _writer.Flush();
            _headerWritten = true;
        }

        public void WriteRow(DateTime timestamp, Dictionary<string, double> values)
        {
            var parts = new List<string>
            {
                timestamp.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            };
            foreach (var kvp in values)
            {
                parts.Add(kvp.Value.ToString("F3", CultureInfo.InvariantCulture));
            }
            _writer.WriteLine(string.Join(",", parts));
            _writer.Flush();
        }

        public void Close()
        {
            _writer?.Close();
        }

        public static string GenerateFilename(string prefix = "wirecap")
        {
            return $"{prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";
        }
    }
}
