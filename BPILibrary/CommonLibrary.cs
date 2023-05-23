using System;
using System.Data;
using System.IO.Compression;
using System.Reflection;
using System.Text;

namespace BPILibrary
{
    public static class CommonLibrary
    {
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static DataTable ListToDataTable<T>(List<T> list, string auditUser, string auditAction, DateTime auditDate, string _tableName)
        {
            DataTable dt = new DataTable(_tableName);

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            dt.Columns.Add(new DataColumn("AuditUser", Nullable.GetUnderlyingType(auditUser.GetType()) ?? auditUser.GetType()));
            dt.Columns.Add(new DataColumn("AuditAction", Nullable.GetUnderlyingType(auditAction.GetType()) ?? auditAction.GetType()));
            dt.Columns.Add(new DataColumn("AuditActionDate", Nullable.GetUnderlyingType(auditDate.GetType()) ?? auditDate.GetType()));

            foreach (T t in list)
            {
                DataRow row = dt.NewRow();

                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
                }
                row["AuditUser"] = auditUser;
                row["AuditAction"] = auditAction;
                row["AuditActionDate"] = auditDate;

                dt.Rows.Add(row);
            }
            return dt;
        }

        public static void saveFiletoDirectory(string path, byte[] content)
        {
            string dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using FileStream fs = new(path, FileMode.Create);
            Stream stream = new MemoryStream(content);
            stream.CopyTo(fs);
        }

        public static void saveFiletoDirectoryAsZip(string path, string originalName, byte[] content)
        {
            string dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (var compressedStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(compressedStream, ZipArchiveMode.Create, false))
                {
                    FileInfo fi = new FileInfo(originalName);
                    var entry = archive.CreateEntry(fi.Name);

                    using (var original = new MemoryStream(content))
                    {
                        using (var zipEntryStream = entry.Open())
                        {
                            original.CopyTo(zipEntryStream);
                        }
                    }
                }

                using FileStream fs = new(path, FileMode.Create);
                Stream stream = new MemoryStream(compressedStream.ToArray());
                stream.CopyTo(fs);
            }
        }

        // data processing
        public static byte[] compressData(byte[] data)
        {
            byte[] compressedData = new byte[0];

            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                compressedData = compressedStream.ToArray();
            }

            return compressedData;
        }

        public static byte[] decompressData(byte[] data)
        {
            byte[] decompressedData = new byte[0];

            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                decompressedData = resultStream.ToArray();
            }

            return decompressedData;
        }

        public static byte[] getFileStream(string uploadPath, string type, string filename, DateTime date, string id)
        {
            string path = Path.Combine(uploadPath, type, date.Year.ToString(), date.Month.ToString(), date.Day.ToString(), id, Path.GetFileName(filename));

            return File.ReadAllBytes(path);
        }

        public static byte[] getFileStreamfromZip(string filename, string originalName, byte[] data)
        {
            byte[] compressedData = new byte[0];

            Stream stream = new MemoryStream(data);
            ZipArchive archive = new ZipArchive(stream);

            FileInfo fi = new FileInfo(originalName);
            var entryData = archive.GetEntry(fi.Name);
            var content = entryData.Open();

            using (var ms = new MemoryStream())
            {
                content.CopyTo(ms);
                compressedData = ms.ToArray();
            }

            return compressedData;
        }

        public static bool deleteFilefromDirectory(string uploadPath, string type, string filename, DateTime date)
        {
            string path = Path.Combine(uploadPath, type, date.Year.ToString(), date.Month.ToString(), date.Day.ToString(), Path.GetFileName(filename));

            if (File.Exists(path))
            {
                File.Delete(path);

                return true;
            }

            return false;
        }

        public static string RandomString(int length, string type)
        {
            var random = new Random();
            string chars;

            switch (type)
            {
                case "ALPHA":
                    chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;
                case "NUM":
                    chars = "0123456789";
                    break;
                case "ALPHANUM":
                    chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    break;
                default:
                    chars = "";
                    throw new Exception("Parameter type from Common Library RandomString is not Relevant");
            }

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static MemoryStream ListToCSV<T>(List<T> list, string separator)
        {
            MemoryStream res = new();

            using (StreamWriter sw = new(res, new UTF8Encoding(false), 8192, true))
            {
                var info = typeof(T).GetProperties();
                // header
                sw.WriteLine(string.Join(separator, info.Select(x => x.Name)));
                // data
                foreach (var dt in list)
                {
                    sw.WriteLine(string.Join(separator, info.Select(prop => prop.GetValue(dt))));
                }
            }

            res.Position = 0;
            return res;
        }

        //
    }
}
