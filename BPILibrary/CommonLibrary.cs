using BPILibrary.Models;
using System;
using System.Data;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
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

        public static MemoryStream ListToCSV<T>(List<T> list, string separator, bool isHeader)
        {
            MemoryStream res = new();

            using (StreamWriter sw = new(res, new UTF8Encoding(false), 8192, true))
            {
                var info = typeof(T).GetProperties();
                // header
                if (isHeader)
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

        public static async Task<ResultModel<string>> sendEmail(string from, List<string> to, List<string>? cc, NetworkCredential creds, string subject, string body, bool isHtml, int port, string host, bool enableSSL)
        {
            ResultModel<string> res = new();

            try
            {
                using (SmtpClient smtp = new())
                {
                    MailMessage msg = new();

                    msg.From = new(from);
                    if (to.Count > 0) { to.ForEach(x => { msg.To.Add(x); }); } else { throw new Exception("To Recipient Data Not Defined !"); }
                    if (cc != null) { if (cc.Count > 0) { cc.ForEach(x => { msg.CC.Add(x); }); } else { throw new Exception("CC Recipient Data Not Defined !"); } }

                    string templateBody =
                        "<table style=\"width: 100%; background-color: #f2f2f2;\">" +
                            "<tbody>" +
                                "<tr>" +
                                    "<td style=\"width: 15%;\"></td>" +
                                    "<td style=\"width: 70%; padding-bottom: 10px;\">" +
                                        "<table style=\"width: 100%; background-color: #fff; border-radius: 8px;\">" +
                                            "<tbody>" +
                                                "<tr>" +
                                                    "<td style=\"width:100%; text-align: center; color: #fff; font-size: 20px; margin: 0 0 4px; background-color: #0090da; padding: 25px 16px; border-top-left-radius: 8px; border-top-right-radius: 8px;\"><b>BPI Application Auto Email Client</b></td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style=\"width: 100%; text-align: left; margin: 0 0 4px; padding: 25px 16px;\">" +
                                                        $"<p>Hi <b>{string.Join(",", to)}</b>,</p>" +
                                                        $"<p>We would like to inform you about information below.</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</tbody>" +
                                        "</table>" +
                                    "</td>" +
                                    "<td style=\"width: 15%;\"></td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td style=\"width: 15%;\"></td>" +
                                    "<td style=\"width: 70%; padding-bottom: 10px;\">" +
                                        "<table style=\"width: 100%; background-color: #fff; border-radius: 8px;\">" +
                                            "<tbody>" +
                                                "<tr>" +
                                                    "<td style=\"width: 100%; text-align: center; margin: 0 0 4px; padding: 15px 8px;\"><b>INFORMATION !</b></td>" +
                                                "</tr>" +
                                                "<tr>" +
                                                    "<td style=\"width: 100%; text-align: left; margin: 0 0 4px; padding: 25px 16px;\">" +
                                                        $"<p>{body}</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</tbody>" +
                                        "</table>" +
                                    "</td>" +
                                    "<td style=\"width: 15%;\"></td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td style=\"width: 15%;\"></td>" +
                                    "<td style=\"width: 70%; padding-bottom: 10px;\">" +
                                        "<table style=\"width: 100%; background-color: #fff; border-radius: 8px;\">" +
                                            "<tbody>" +
                                                "<tr>" +
                                                    "<td style=\"width: 100%; text-align: center; margin: 0 0 4px; padding: 25px 16px;\">" +
                                                        "<p>Please Access our Web Application at <a href=\"https://bpi.mitra10.com\">BPI Application (bpi.mitra10.com)</a></p>" +
                                                        "<p>THIS MESSAGE WAS SENT TO YOU BY AN AUTO EMAIL AGENT PLEASE DONT REPLY THIS MESSAGE</p>" +
                                                        "<hr />" +
                                                        "<p>PT. CATUR MITRA SEJATI SENTOSA (MITRA10)</p>" +
                                                    "</td>" +
                                                "</tr>" +
                                            "</tbody>" +
                                        "</table>" +
                                    "</td>" +
                                    "<td style=\"width: 15%;\"></td>" +
                                "</tr>" +
                            "</tbody>" +
                        "</table";

                    if (subject.Length > 0) { msg.Subject = subject; } else { throw new Exception("Subject Empty !"); }
                    if (body.Length > 0) { msg.Body = templateBody; } else { throw new Exception("Body Empty !"); }
                    msg.IsBodyHtml = isHtml;

                    smtp.Credentials = creds;
                    smtp.Port = port;
                    smtp.Host = host;
                    smtp.EnableSsl = enableSSL;

                    smtp.Send(msg);
                }

                res = new ResultModel<string>
                {
                    Data = "Send Email Success",
                    isSuccess = true,
                    ErrorCode = "00",
                    ErrorMessage = ""
                };
            }
            catch (Exception ex)
            {
                res = new ResultModel<string>
                {
                    Data = "Exception",
                    isSuccess = false,
                    ErrorCode = "99",
                    ErrorMessage = "Exception : " + ex.Message
                };
            }

            return res;
        }

        //
    }
}
