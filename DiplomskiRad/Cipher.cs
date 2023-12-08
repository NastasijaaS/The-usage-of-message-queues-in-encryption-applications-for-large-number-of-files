using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiplomskiRad
{
    class Cipher
    {
        private static UserControl activeUser;
        private static RichTextBox txtLog;
        private static string outputpath;
        Stopwatch stopwatch;
        public Cipher()
        {
            stopwatch = new Stopwatch();
        }
        public static void SetActiveRichBox(RichTextBox txt)
        {
            txtLog = txt;
        }
        public static void SetActiveUserControl(UserControl uc)
        {
            activeUser = uc;
        }
        public static void SetOutputPath(string output)
        {
            outputpath = output;
        }

        public async Task EncryptFileAsync(string filePath)
        {
            try
            {
                if (activeUser is AESPage)
                {
                    var cipherAES = new AES();
                    byte[] fileContent;

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        int fileSize = (int)fileStream.Length;
                        fileContent = new byte[fileSize];

                        await fileStream.ReadAsync(fileContent, 0, fileSize);
                    }

                    stopwatch.Start();
                    byte[] encryptedContent = await cipherAES.EncryptFileAES(fileContent);
                    stopwatch.Stop();

                    long elapsedTime = stopwatch.ElapsedMilliseconds;
                    string fileName = Path.GetFileName(filePath);

                    string encryptedOutputPath = Path.Combine(outputpath, fileName);
                    using (FileStream fs = File.Create(encryptedOutputPath))
                    {
                        await fs.WriteAsync(encryptedContent, 0, encryptedContent.Length);
                    }

                    string message = $"File encrypted: {filePath},Elapsed time: {elapsedTime} ms {Environment.NewLine}";

                    AppendTextToLog(message);

                }
                else if (activeUser is XXTEAPage)
                {
                    var cipherXXTEA = new XXTEA();
                    byte[] fileContent;

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        int fileSize = (int)fileStream.Length;
                        fileContent = new byte[fileSize];

                        await fileStream.ReadAsync(fileContent, 0, fileSize);
                    }

                    stopwatch.Start();
                    byte[] encryptedContent = cipherXXTEA.Encrypt(fileContent);
                    stopwatch.Stop();
                    long elapsedTime = stopwatch.ElapsedMilliseconds;

                    string fileName = Path.GetFileName(filePath);

                    string encryptedOutputPath = Path.Combine(outputpath, fileName);
                    using (FileStream fs = File.Create(encryptedOutputPath))
                    {
                        await fs.WriteAsync(encryptedContent, 0, encryptedContent.Length);
                    }

                    string message = $"File encrypted: {filePath},Elapsed time: {elapsedTime} ms {Environment.NewLine}";
                    AppendTextToLog(message);
                }
                else
                {
                    var cipherRC6 = new RC6();
                    byte[] fileContent;

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        int fileSize = (int)fileStream.Length;
                        fileContent = new byte[fileSize];

                        await fileStream.ReadAsync(fileContent, 0, fileSize);
                    }
                    stopwatch.Start();
                    byte[] encryptedContent = cipherRC6.Encrypt(fileContent);
                    stopwatch.Stop();
                    long elapsedTime = stopwatch.ElapsedMilliseconds;

                    string fileName = Path.GetFileName(filePath);

                    string encryptedOutputPath = Path.Combine(outputpath, fileName);
                    using (FileStream fs = File.Create(encryptedOutputPath))
                    {
                        await fs.WriteAsync(encryptedContent, 0, encryptedContent.Length);
                    }

                    string message = $"File encrypted: {filePath},Elapsed time: {elapsedTime} ms {Environment.NewLine}";
                    AppendTextToLog(message);
                }
            }
            catch(IOException ex)
            {
                throw new Exception("An exception occurred during IO cipher part: ", ex);
            }

            catch(Exception ex)
            {
                throw new Exception("An exception occurred during cipher part: ", ex);
            }
        }
    
        private void AppendTextToLog(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.BeginInvoke(new MethodInvoker(delegate
                {
                    txtLog.AppendText(message);
                }));
            }
            else
            {
                txtLog.AppendText(message);
            }
        }

    }

}

