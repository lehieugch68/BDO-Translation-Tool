using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression;
using Microsoft.Win32;
using System.Diagnostics;
using System.Net;

namespace BDOTranslationTool
{
    public partial class BDOTranslationTool : Form
    {
        string _AppPath = AppDomain.CurrentDomain.BaseDirectory;
        string _GamePath;
        bool _Installing = false, _Uninstalling = false, _Decompressing = false, _Downloading = false, _Merging = false;
        Dictionary<string, string> translator = new Dictionary<string, string>();
        public BDOTranslationTool()
        {
            InitializeComponent();
        }

        private void ReportProgress(int percent)
        {
            if (percent <= 100)
            {
                this.progressBar.BeginInvoke((MethodInvoker)delegate ()
                {
                    progressBar.Value = percent;
                });
            }
        }

        private void ReportStatus(string status)
        {
            this.Status.BeginInvoke((MethodInvoker)delegate ()
            {
                Status.Text = status;
            });
        }

        private void Write_Log(string text)
        {
            this.Log.BeginInvoke((MethodInvoker)delegate ()
            {
                Log.Items.Add(text);
                Log.SelectedIndex = Log.Items.Count - 1;
            });
        }

        private void BDOTranslationTool_Load(object sender, EventArgs e)
        {
            string registryPath = @"SOFTWARE\Wow6432Node\BlackDesert_ID";
            try
            {
                _GamePath = Registry.LocalMachine.OpenSubKey(registryPath).GetValue("Path").ToString();
                GamePath.Text = _GamePath;
            }
            catch
            {
                MessageBox.Show("Không tìm thấy thư mục cài đặt Black Desert Online!\nVui lòng chọn đường dẫn thủ công.", "Thông báo");
            }
            translator.Add("Sú", "");
            translator.Add("Lê Hiếu", "");
            foreach (string key in translator.Keys)
            {
                selectTranslator.Items.Add(key);
            }
            selectTranslator.SelectedIndex = 0;
        }

        private void Browser_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    _GamePath = fbd.SelectedPath;
                    GamePath.Text = _GamePath;
                }
            }
        }

        private void Install_Click(object sender, EventArgs e)
        {
            if (!_Installing && !_Uninstalling && !_Decompressing)
            {
                string sourceFile = $"{_GamePath}\\ads\\languagedata_en.loc";
                string encryptFile = $"{_AppPath}\\languagedata_en.loc";
                string decryptFile = $"{_AppPath}\\languagedata_en.tsv";
                string translationFile = $"{_AppPath}\\BDO_Translation.tsv";
                if (File.Exists(sourceFile))
                {
                    _Installing = true;
                    ReportProgress(0);
                    Task.Run(() => decrypt(decompress(sourceFile), decryptFile)).GetAwaiter().OnCompleted(() =>
                    {
                        if (!_Installing) return;
                        ReportProgress(25);
                        Task.Run(() => Replace_Text(decryptFile, translationFile)).GetAwaiter().OnCompleted(() =>
                        {
                            if (!_Installing) return;
                            ReportProgress(50);
                            Task.Run(() => compress(encrypt(decryptFile), decryptFile)).GetAwaiter().OnCompleted(() =>
                            {
                                if (!_Installing) return;
                                ReportStatus("Đang sao chép");
                                ReportProgress(75);
                                Task.Run(() => CopyFile(encryptFile, sourceFile)).GetAwaiter().OnCompleted(() =>
                                {
                                    if (!_Installing) return;
                                    _Installing = false;
                                    ReportStatus("Cài đặt thành công!");
                                    ReportProgress(100);
                                });
                            });
                        });
                    });
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tệp languagedata_en.loc!", "Thông báo");
                }
            }

        }

        private void buttonDecompress_Click(object sender, EventArgs e)
        {
            if (!_Installing && !_Uninstalling && !_Decompressing)
            {
                string sourceFile = $"{_GamePath}\\ads\\languagedata_en.loc";
                string decryptFile = $"{_AppPath}\\languagedata_en.tsv";
                string translationFile = $"{_AppPath}\\BDO_Translation.tsv";
                if (File.Exists(translationFile))
                {
                    DialogResult dialogResult = MessageBox.Show("Phát hiện tệp BDO_Translation.tsv\nBạn có chắc muốn ghi đè tệp này?", "Thông báo", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (!File.Exists(sourceFile))
                        {
                            MessageBox.Show("Không tìm thấy tệp languagedata_en.loc!", "Thông báo");
                            return;
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                _Decompressing = true;
                ReportProgress(0);
                Task.Run(() => { decrypt(decompress(sourceFile), decryptFile); }).GetAwaiter().OnCompleted(() =>
                {
                    if (!_Decompressing) return;
                    ReportProgress(33);
                    Task.Run(() => {
                        File.WriteAllBytes(translationFile, File.ReadAllBytes(decryptFile));
                    }).GetAwaiter().OnCompleted(() =>
                    {
                        ReportProgress(67);
                        Task.Run(() => { Remove_Duplicate(translationFile); }).GetAwaiter().OnCompleted(() =>
                        {
                            _Decompressing = false;
                            ReportStatus("Giải nén thành công!");
                            ReportProgress(100);
                        });
                    });
                });
            }
        }

        private void CopyFile(string sourceFile, string destinationFile)
        {
            ReportProgress(0);
            try
            {
                if (File.Exists(sourceFile))
                {
                    string directory = Path.GetDirectoryName(destinationFile);
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                    File.Copy(sourceFile, destinationFile, true);
                }
            }
            catch (Exception e)
            {
                if (_Installing) _Installing = false;
                if (_Decompressing) _Decompressing = false;
                if (_Uninstalling) _Uninstalling = false;
                MessageBox.Show("Đã xảy ra lỗi!\n\n" + e, "Thông báo");
                ReportStatus("Chưa rõ");
            }
        }

        private void Replace_Text(string sourceFile, string translationFile)
        {
            if (!_Installing && !_Decompressing) return;
            try
            {
                ReportStatus($"Đang sao chép bản dịch");
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                using (StreamReader reader = new StreamReader(translationFile))
                {
                    while (!reader.EndOfStream)
                    {
                        string[] content = reader.ReadLine().Split(new string[] { "\t" }, StringSplitOptions.None);
                        if (content.Length > 1 && content[0] != "<null>" && !content[0].StartsWith("https://") && !string.IsNullOrWhiteSpace(content[0]) && !string.IsNullOrWhiteSpace(content[1]))
                        {
                            try
                            {
                                dictionary.Add(content[0], content[1]);
                            }
                            catch
                            {

                            }
                        }
                    }
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    using (StreamWriter temp = new StreamWriter(stream, Encoding.Unicode))
                    {
                        using (StreamReader reader = new StreamReader(sourceFile))
                        {
                            while (!reader.EndOfStream)
                            {
                                string[] content = reader.ReadLine().Split(new string[] { "\t" }, StringSplitOptions.None);
                                string value;
                                if (!string.IsNullOrWhiteSpace(content[5]) && dictionary.TryGetValue(content[5], out value))
                                {
                                    Write_Log($"{content[5]} => {value}");
                                    content[5] = value;
                                }
                                temp.WriteLine(string.Join("\t", content));
                            }
                        }
                        temp.Flush();
                        FileStream writeStream = new FileStream(sourceFile, FileMode.Create);
                        BinaryWriter writeBinary = new BinaryWriter(writeStream);
                        writeBinary.Write(stream.ToArray());
                        writeBinary.Close();
                    }
                }
                dictionary.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show("Đã xảy ra lỗi!\n\n" + e, "Thông báo");
                if (_Installing) _Installing = false;
                if (_Decompressing) _Decompressing = false;
                ReportStatus("Chưa rõ");
            }
        }

        private void Remove_Duplicate(string file)
        {
            if (!_Installing && !_Decompressing) return;
            try
            {
                int total = File.ReadAllLines(file).Count();
                int count = 0;
                List<string> lines = new List<string>();
                using (MemoryStream stream = new MemoryStream())
                {
                    using (var temp = new StreamWriter(stream, Encoding.Unicode))
                    {
                        using (StreamReader reader = new StreamReader(file))
                        {
                            while (!reader.EndOfStream)
                            {
                                count++;
                                ReportStatus($"Đang kiểm tra những câu bị trùng ({count}/{total} dòng)");
                                string[] content = reader.ReadLine().Split(new string[] { "\t" }, StringSplitOptions.None);
                                if (content.Length > 1 && !string.IsNullOrWhiteSpace(content[5]) && content[5] != "<null>" && !content[5].StartsWith("http://") && !lines.Contains(content[5]))
                                {
                                    temp.WriteLine($"{content[5]}\t");
                                    lines.Add(content[5]);
                                }
                            }
                        }
                        temp.Flush();
                        FileStream writeStream = new FileStream(file, FileMode.Create);
                        BinaryWriter writeBinary = new BinaryWriter(writeStream);
                        writeBinary.Write(stream.ToArray());
                        writeBinary.Close();
                    }
                }
                lines.Clear();
            }
            catch (Exception e)
            {
                MessageBox.Show("Đã xảy ra lỗi!\n\n" + e, "Thông báo");
                if (_Installing) _Installing = false;
                if (_Decompressing) _Decompressing = false;
                ReportStatus("Chưa rõ");
            }
        }
        private MemoryStream decompress(string file)
        {
            ReportStatus("Đang giải nén");
            MemoryStream stream = new MemoryStream();
            try
            {
                using (var input = File.OpenRead(file))
                {
                    input.Seek(6, SeekOrigin.Current);
                    using (var deflateStream = new DeflateStream(input, CompressionMode.Decompress, true))
                    {
                        deflateStream.CopyTo(stream);
                    }
                }
            }
            catch
            {
                ReportStatus("Đã xảy ra lỗi");
                if (_Installing) _Installing = false;
                if (_Decompressing) _Decompressing = false;
            }
            return stream;
        }

        private void compress(MemoryStream stream, string file)
        {
            if (!_Installing) return;
            stream.Position = 0;
            byte[] input = stream.ToArray();
            byte[] size = BitConverter.GetBytes(Convert.ToUInt32(input.Length));
            ReportStatus($"Đang nén ({Convert.ToUInt32(input.Length)} byte)");
            Deflater compressor = new Deflater();
            compressor.SetLevel(Deflater.BEST_SPEED);
            compressor.SetInput(input);
            compressor.Finish();
            MemoryStream bos = new MemoryStream(input.Length);
            byte[] buf = new byte[1024];
            while (!compressor.IsFinished)
            {
                int count = compressor.Deflate(buf);
                bos.Write(buf, 0, count);
            }
            try
            {
                string directory = Path.GetDirectoryName(file);
                string filename = Path.GetFileNameWithoutExtension(file);
                FileStream writeStream = new FileStream($"{directory}\\{filename}.loc", FileMode.Create);
                BinaryWriter writeBinary = new BinaryWriter(writeStream);
                writeBinary.Write(size);
                writeBinary.Write(bos.ToArray());
                writeBinary.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Đã xảy ra lỗi!\n\n" + e, "Thông báo");
                if (_Installing) _Installing = false;
                ReportStatus("Chưa rõ");
            }
        }

        private void decrypt(MemoryStream stream, string decryptFile)
        {
            if (!_Installing && !_Decompressing) return;
            try
            {
                stream.Position = 0;
                using (var reader = new BinaryReader(stream))
                {
                    double total = reader.BaseStream.Length;
                    ReportStatus($"Đang giải mã ({total} byte)");
                    using (var output = new StreamWriter(decryptFile, false, Encoding.Unicode))
                    {

                        while (reader.BaseStream.Position != reader.BaseStream.Length)
                        {
                            UInt32 strSize = reader.ReadUInt32();
                            UInt32 strType = reader.ReadUInt32();
                            UInt32 strID1 = reader.ReadUInt32();
                            UInt16 strID2 = reader.ReadUInt16();
                            byte strID3 = reader.ReadByte();
                            byte strID4 = reader.ReadByte();
                            string str = Encoding.Unicode.GetString(reader.ReadBytes(Convert.ToInt32(strSize * 2))).Replace("\n", "<lf>");
                            reader.ReadBytes(4);
                            output.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", strType, strID1, strID2, strID3, strID4, str);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Đã xảy ra lỗi!\n\n" + e, "Thông báo");
                if (_Installing) _Installing = false;
                if (_Decompressing) _Decompressing = false;
                ReportStatus("Chưa rõ");
            }
        }

        private MemoryStream encrypt(string file)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                ReportStatus($"Đang mã hóa");
                using (var reader = new StreamReader(file))
                {
                    BinaryWriter writeBinary = new BinaryWriter(stream);
                    byte[] zeroes = { (byte)0, (byte)0, (byte)0, (byte)0 };
                    while (!reader.EndOfStream)
                    {
                        string[] content = reader.ReadLine().Split(new string[] { "\t" }, StringSplitOptions.None);
                        byte[] strType = BitConverter.GetBytes(Convert.ToUInt32(content[0]));
                        byte[] strID1 = BitConverter.GetBytes(Convert.ToUInt32(content[1]));
                        byte[] strID2 = BitConverter.GetBytes(Convert.ToUInt16(content[2]));
                        byte strID3 = Convert.ToByte(content[3]);
                        byte strID4 = Convert.ToByte(content[4]);
                        string str = content[5].Replace("<lf>", "\n");
                        byte[] strBytes = Encoding.Unicode.GetBytes(str);
                        byte[] strSize = BitConverter.GetBytes(str.Length);
                        writeBinary.Write(strSize);
                        writeBinary.Write(strType);
                        writeBinary.Write(strID1);
                        writeBinary.Write(strID2);
                        writeBinary.Write(strID3);
                        writeBinary.Write(strID4);
                        writeBinary.Write(strBytes);
                        writeBinary.Write(zeroes);
                    }
                    reader.Close();
                }
            }
            catch
            {
                ReportStatus("Đã xảy ra lỗi");
                if (_Installing) _Installing = false;
                if (_Decompressing) _Decompressing = false;
            }
            return stream;
        }
        private void linkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/lehieugch68/BDO-Translation-Tool");
        }
        private void linkVHG_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://viethoagame.com/threads/pc-black-desert-online-viet-hoa.222/");
        }
    }
}
