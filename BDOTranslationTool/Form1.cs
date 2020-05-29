using System;
using System.ComponentModel;
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
        Dictionary<string, string[]> translator = new Dictionary<string, string[]>();
        public BDOTranslationTool()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.Icon;
        }

        public void ReportProgress(int percent)
        {
            this.progressBar.BeginInvoke((MethodInvoker)delegate ()
            {
                int p = (percent > 100) ? 100 : percent;
                progressBar.Value = p;
            });
        }

        public void ReportStatus(string status)
        {
            this.Status.BeginInvoke((MethodInvoker)delegate ()
            {
                Status.Text = status;
            });
        }

        public void Write_Log(string text)
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
            translator.Add("Sú", new string[] { "https://drive.google.com/uc?export=download&id=1Jy8OiFDu2EXZsz2u0aIOpdxXNs2jrtb_", "BDO_Translation_Su.zip", "Sú", "https://www.facebook.com/visaosang2305" });
            translator.Add("Lê Hiếu", new string[] { "https://drive.google.com/uc?export=download&id=1Oo9el5Z0CHx46EUZ4YPmgl02gcatRuw3", "BDO_Translation_LeHieu.zip", "Lê Hiếu", "https://www.facebook.com/le.anh.hieu.68" });
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

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            string[] value;
            string path = Path.Combine(_AppPath, "translator");
            if (!_Downloading && !_Merging && translator.TryGetValue(selectTranslator.GetItemText(selectTranslator.SelectedItem), out value))
            {
                _Downloading = true;
                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                        Write_Log($"Tạo thư mục: {path}");
                    }
                    using (WebClient download = new WebClient())
                    {
                        download.DownloadProgressChanged += download_ProgressChanged;
                        download.DownloadFileCompleted += download_Completed;
                        download.QueryString.Add("path", $"{path}\\{value[1]}");
                        Write_Log($"Đang tải xuống bản dịch của {value[2]}...");
                        download.DownloadFileAsync(new Uri(value[0]), $"{path}\\{value[1]}");
                    }
                }
                catch (Exception err)
                {
                    Write_Log("Xảy ra lỗi khi tải xuống bản dịch!");
                    MessageBox.Show("Đã xảy ra lỗi!\n\n" + err, "Thông báo");
                    _Downloading = false;
                }
            }
            else
            {
                string msg = _Downloading ? "Đang tải bản dịch!" : "Đang gộp bản dịch!";
                MessageBox.Show(msg, "Thông báo");
            }
        }

        private void download_ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBarDownload.Value = e.ProgressPercentage;
        }

        private void download_Completed(object sender, AsyncCompletedEventArgs e)
        {
            Write_Log($"Đang giải nén bản dịch...");
            string path = ((WebClient)(sender)).QueryString["path"];
            string extractPath = Path.Combine(_AppPath, "translator");
            try
            {
                Task.Run(() =>
                {
                    using (var zip = ZipFile.OpenRead(path))
                    {
                        foreach (var entry in zip.Entries)
                        {
                            string destinationPath = Path.Combine(extractPath, entry.FullName);
                            entry.ExtractToFile(destinationPath, true);
                        }
                    }
                }).GetAwaiter().OnCompleted(() =>
                {
                    _Downloading = false;
                    File.Delete(path);
                    Write_Log("Giải nén bản dịch thành công.");
                });
            }
            catch (Exception err)
            {
                Write_Log("Xảy ra lỗi khi giải nén bản dịch!");
                _Downloading = false;
                MessageBox.Show("Đã xảy ra lỗi!\n\n" + err, "Thông báo");
            }
        }

        private void buttonContact_Click(object sender, EventArgs e)
        {
            string[] value;
            if (translator.TryGetValue(selectTranslator.GetItemText(selectTranslator.SelectedItem), out value))
            {
                try
                {
                    Process.Start(value[3]);
                }
                catch { }
            }
        }

        private void Install_Click(object sender, EventArgs e)
        {
            if (!_Installing && !_Uninstalling && !_Decompressing && !_Merging)
            {
                string backupFile = $"{_GamePath}\\ads\\backup\\languagedata_en.loc";
                string sourceFile = $"{_GamePath}\\ads\\languagedata_en.loc";
                string encryptFile = $"{_AppPath}\\languagedata_en.loc";
                string decryptFile = $"{_AppPath}\\languagedata_en.tsv";
                string translationFile = $"{_AppPath}\\BDO_Translation.tsv";
                if (File.Exists(sourceFile))
                {
                    _Installing = true;
                    ReportProgress(0);
                    if (!File.Exists(backupFile))
                    {
                        CopyFile(sourceFile, backupFile);
                    }
                    Task.Run(() => decrypt(decompress(backupFile), decryptFile)).GetAwaiter().OnCompleted(() =>
                    {
                        if (!_Installing) return;
                        ReportProgress(25);
                        ReportStatus("Đang sao chép bản dịch");
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
            if (!_Installing && !_Uninstalling && !_Decompressing && !_Merging)
            {
                string backupFile = $"{_GamePath}\\ads\\backup\\languagedata_en.loc";
                string sourceFile = File.Exists(backupFile) ? backupFile : $"{_GamePath}\\ads\\languagedata_en.loc";
                string decryptFile = $"{_AppPath}\\languagedata_en.tsv";
                string translationFile = $"{_AppPath}\\BDO_Translation.tsv";
                bool _Overwrite = true;
                if (File.Exists(translationFile))
                {
                    DialogResult dialogResult = MessageBox.Show("Phát hiện tệp BDO_Translation.tsv\nBạn có muốn ghi đè tệp này?", "Thông báo", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes) { }
                    else if (dialogResult == DialogResult.No)
                    {
                        _Overwrite = false;
                    }
                }
                if (!File.Exists(sourceFile))
                {
                    MessageBox.Show("Không tìm thấy tệp languagedata_en.loc!", "Thông báo");
                }
                else
                {
                    _Decompressing = true;
                    ReportProgress(0);
                    Task.Run(() => { decrypt(decompress(sourceFile), decryptFile); }).GetAwaiter().OnCompleted(() =>
                    {
                        if (!_Decompressing) return;
                        if (_Overwrite)
                        {
                            ReportProgress(50);
                            Task.Run(() => Remove_Duplicate(decryptFile, translationFile)).GetAwaiter().OnCompleted(() =>
                            {
                                if (_Decompressing)
                                {
                                    _Decompressing = false;
                                    ReportStatus("Giải nén thành công!");
                                }
                            });
                        }
                        else
                        {
                            if (_Decompressing)
                            {
                                _Decompressing = false;
                                ReportProgress(100);
                                ReportStatus("Giải nén thành công!");
                            }
                        }
                    });
                }
            }
        }

        private void buttonMerge_Click(object sender, EventArgs e)
        {
            string[] value;
            if (!_Installing && !_Uninstalling && !_Decompressing && !_Merging && !_Downloading && translator.TryGetValue(selectTranslator.GetItemText(selectTranslator.SelectedItem), out value))
            {
                string transFile = Path.Combine(Path.Combine(_AppPath, "translator"), $"{Path.GetFileNameWithoutExtension(value[1])}.tsv");
                string destFile = Path.Combine(_AppPath, "BDO_Translation.tsv");
                if (File.Exists(transFile))
                {
                    _Merging = true;
                    ReportStatus($"Đang gộp bản dịch ({value[2]})");
                    Write_Log($"Bắt đầu gộp bản dịch ({value[2]})...");
                    ReportProgress(0);
                    Task.Run(() => Replace_Text(destFile, transFile)).GetAwaiter().OnCompleted(() =>
                    {
                        if (_Merging)
                        {
                            ReportProgress(100);
                            ReportStatus($"Gộp bản dịch thành công ({value[2]})");
                            Write_Log($"Gộp bản dịch thành công ({value[2]})!");
                            _Merging = false;
                        }

                    });
                }
                else
                {
                    MessageBox.Show($"Không tìm thấy tệp:\n{transFile}", "Thông báo");
                }
            }
        }

        private void buttonBackup_Click(object sender, EventArgs e)
        {
            if (!_Installing && !_Decompressing)
            {
                string backupFile = $"{_GamePath}\\ads\\backup\\languagedata_en.loc";
                string sourceFile = $"{_GamePath}\\ads\\languagedata_en.loc";
                if (File.Exists(sourceFile))
                {
                    Write_Log("Đang sao lưu...");
                    Task.Run(() => CopyFile(sourceFile, backupFile)).GetAwaiter().OnCompleted(() =>
                    {
                        Write_Log("Sao lưu tệp gốc thành công!");
                    });
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tệp languagedata_en.loc!", "Thông báo");
                }
            }
        }

        private void buttonRestore_Click(object sender, EventArgs e)
        {
            if (!_Installing && !_Decompressing)
            {
                string backupFile = $"{_GamePath}\\ads\\backup\\languagedata_en.loc";
                string sourceFile = $"{_GamePath}\\ads\\languagedata_en.loc";
                if (File.Exists(backupFile))
                {
                    Write_Log("Đang khôi phục tệp gốc...");
                    Task.Run(() => CopyFile(backupFile, sourceFile)).GetAwaiter().OnCompleted(() =>
                    {
                        Write_Log("Khôi phục tệp gốc thành công!");
                    });
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tệp sao lưu!", "Thông báo");
                }
            }
        }

        private void CopyFile(string sourceFile, string destinationFile)
        {
            try
            {
                if (File.Exists(sourceFile))
                {
                    string directory = Path.GetDirectoryName(destinationFile);
                    if (!Directory.Exists(directory))
                    {
                        Write_Log($"Tạo thư mục: {directory}");
                        Directory.CreateDirectory(directory);
                    }
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
            if (!_Installing && !_Decompressing && !_Merging) return;
            try
            {
                Write_Log($"Đang thêm những câu được dịch...");
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                string[] allLines = File.ReadAllLines(translationFile);
                foreach (string line in allLines)
                {
                    string[] content = line.Split(new string[] { "\t" }, StringSplitOptions.None);
                    content[0] = content[0].TrimStart((char)34).TrimEnd((char)34);
                    content[1] = content[1].TrimStart((char)34).TrimEnd((char)34).Replace($"{(char)34}", "<quot>");
                    if (content.Length > 1 && content[0] != "<null>" && !content[0].StartsWith("http") && !string.IsNullOrWhiteSpace(content[0]) && !string.IsNullOrWhiteSpace(content[1]))
                    {
                        if (!dictionary.ContainsKey(content[0])) dictionary.Add(content[0], content[1]);
                    }
                }
                Write_Log($"Thêm thành công {dictionary.Count()} dòng.");
                Write_Log("Bắt đầu dịch...");
                allLines = File.ReadAllLines(sourceFile);
                int a = _Merging ? 1 : 5;
                int b = _Merging ? 0 : 5;
                int count = 0;
                using (StreamWriter temp = new StreamWriter(sourceFile, false, Encoding.Unicode))
                {
                    foreach (string line in allLines)
                    {
                        string[] content = line.Split(new string[] { "\t" }, StringSplitOptions.None);
                        if (content.Length > 1)
                        {
                            string value;
                            bool isInstalling = _Merging ? string.IsNullOrWhiteSpace(content[a]) : true;
                            if (!string.IsNullOrWhiteSpace(content[b]) && dictionary.TryGetValue(content[b], out value) && isInstalling)
                            {
                                count++;
                                content[a] = value;
                            }
                        }
                        temp.WriteLine(string.Join("\t", content));
                    }
                }
                Write_Log($"Dịch thành công {count} dòng!");
            }
            catch (Exception e)
            {
                ReportStatus("Chưa rõ");
                Write_Log("Đã xảy ra lỗi!");
                if (_Installing) _Installing = false;
                if (_Decompressing) _Decompressing = false;
                if (_Merging) _Merging = false;
                MessageBox.Show("Đã xảy ra lỗi!\n\n" + e, "Thông báo");
            }
        }

        private void Remove_Duplicate(string sourceFile, string transFile)
        {
            if (!_Installing && !_Decompressing) return;
            try
            {
                string[] allLines = File.ReadAllLines(sourceFile);
                Write_Log($"Đang lọc những câu bị trùng (tổng {allLines.Length} dòng)...");
                double total = allLines.Length;
                double count = 0;
                int remove = 0;
                List<string> lines = new List<string>();
                using (var writer = new StreamWriter(transFile, false, Encoding.Unicode))
                {
                    foreach (string line in allLines)
                    {
                        count++;
                        ReportStatus($"Đang lọc những câu bị trùng ({count}/{total} dòng)");
                        ReportProgress((int)(count * 50 / total) + 50);
                        string[] content = line.Split(new string[] { "\t" }, StringSplitOptions.None);
                        if (content.Length > 1 && !string.IsNullOrWhiteSpace(content[5]) && content[5] != "<null>" && !content[5].StartsWith("http") && !lines.Contains(content[5]))
                        {
                            writer.WriteLine($"{content[5]}\t");
                            lines.Add(content[5]);
                        }
                        else
                        {
                            remove++;
                        }
                    }
                }
                Write_Log($"Đã loại bỏ {remove} câu trùng.");
            }
            catch (Exception e)
            {
                if (_Installing) _Installing = false;
                if (_Decompressing) _Decompressing = false;
                ReportStatus("Chưa rõ");
                MessageBox.Show("Đã xảy ra lỗi!\n\n" + e, "Thông báo");
            }
        }
        private MemoryStream decompress(string file)
        {
            ReportStatus("Đang giải nén");
            Write_Log("Bắt đầu giải nén...");
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
                Write_Log($"Giải nén thành công {stream.Length} byte.");
            }
            catch (Exception e)
            {
                ReportStatus("Chưa rõ");
                Write_Log("Xảy ra lỗi khi giải nén tệp!");
                if (_Installing) _Installing = false;
                if (_Decompressing) _Decompressing = false;
                MessageBox.Show("Đã xảy ra lỗi!\n\n" + e, "Thông báo");
            }
            return stream;
        }

        private void compress(MemoryStream stream, string file)
        {
            if (!_Installing) return;
            try
            {
                stream.Position = 0;
                byte[] input = stream.ToArray();
                byte[] size = BitConverter.GetBytes(Convert.ToUInt32(input.Length));
                ReportStatus($"Đang nén");
                Write_Log($"Bắt đầu nén {input.Length} byte...");
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
                byte[] output = bos.ToArray();
                string directory = Path.GetDirectoryName(file);
                string filename = Path.GetFileNameWithoutExtension(file);
                FileStream writeStream = new FileStream($"{directory}\\{filename}.loc", FileMode.Create);
                BinaryWriter writeBinary = new BinaryWriter(writeStream);
                writeBinary.Write(size);
                writeBinary.Write(output);
                Write_Log($"Nén thành công {input.Length} byte => {output.Length} byte.");
                writeBinary.Close();
            }
            catch (Exception e)
            {
                if (_Installing) _Installing = false;
                ReportStatus("Chưa rõ");
                Write_Log("Xảy ra lỗi khi nén tệp!");
                MessageBox.Show("Đã xảy ra lỗi!\n\n" + e, "Thông báo");
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
                    long total = reader.BaseStream.Length;
                    ReportStatus($"Đang giải mã");
                    Write_Log($"Bắt đầu giải mã {total} byte...");
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
                            string str = Encoding.Unicode.GetString(reader.ReadBytes(Convert.ToInt32(strSize * 2))).Replace("\n", "<lf>").Replace($"{(char)34}", "<quot>");
                            if (str.StartsWith("=") || str.StartsWith("+") || str.StartsWith("-")) str = $"'{str}";
                            reader.ReadBytes(4);
                            output.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", strType, strID1, strID2, strID3, strID4, str);
                        }
                        Write_Log($"Giải mã thành công {total} byte => {output.BaseStream.Length} byte.");
                    }
                }
            }
            catch (Exception e)
            {
                if (_Installing) _Installing = false;
                if (_Decompressing) _Decompressing = false;
                ReportStatus("Chưa rõ");
                Write_Log("Xảy ra lỗi khi giải mã tệp!");
                MessageBox.Show("Đã xảy ra lỗi!\n\n" + e, "Thông báo");
            }
        }

        private MemoryStream encrypt(string file)
        {
            MemoryStream stream = new MemoryStream();
            try
            {
                string[] allLines = File.ReadAllLines(file);
                ReportStatus($"Đang mã hóa");
                Write_Log($"Bắt đầu mã hóa {allLines.Length} dòng...");
                BinaryWriter writeBinary = new BinaryWriter(stream);
                byte[] zeroes = { (byte)0, (byte)0, (byte)0, (byte)0 };
                string[] excel_cal_char = { "'+", "'=", "'-" };
                foreach (string line in allLines)
                {
                    string[] content = line.Split(new string[] { "\t" }, StringSplitOptions.None);
                    byte[] strType = BitConverter.GetBytes(Convert.ToUInt32(content[0]));
                    byte[] strID1 = BitConverter.GetBytes(Convert.ToUInt32(content[1]));
                    byte[] strID2 = BitConverter.GetBytes(Convert.ToUInt16(content[2]));
                    byte strID3 = Convert.ToByte(content[3]);
                    byte strID4 = Convert.ToByte(content[4]);
                    string str = content[5].Replace("<lf>", "\n").Replace("<quot>", $"{(char)34}");
                    if (excel_cal_char.Any(character => str.StartsWith(character)))
                    {
                        str = str.TrimStart((char)39);
                    }
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
                Write_Log($"Mã hóa thành công {allLines.Length} dòng => {stream.Length} byte.");
            }
            catch (Exception e)
            {
                if (_Installing) _Installing = false;
                if (_Decompressing) _Decompressing = false;
                ReportStatus("Đã xảy ra lỗi");
                Write_Log("Đã xảy ra lỗi khi mã hóa tệp!");
                MessageBox.Show("Đã xảy ra lỗi!\n\n" + e, "Thông báo");
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
