using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiplomskiRad
{
    public partial class XXTEAPage : UserControl
    {
        XXTEA xxtea;
        Watcher ws;
        byte[] value1;
        byte[] loadFile;

        string outputFolder;
        public XXTEAPage()
        {
            xxtea = new XXTEA();
            InitializeComponent();
        }

        private void button_WOC1_Click(object sender, EventArgs e)
        {

            value1 = this.xxtea.GenerateRandomKey();
            SetKeyTbx();

        }
        private void SetKeyTbx()
        {
            tbK.Text = BitConverter.ToString(value1).Replace("-", "");
        }

        private void button_WOC3_Click(object sender, EventArgs e)
        {
            bool empty = string.IsNullOrEmpty(tbK.Text);

            if (empty)
            {
                MessageBox.Show("Set a value for the key.",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                return;
            }
            else
            {
                try
                {
                    uint[] q = xxtea.PretvoriByteToUInt(value1);
                    xxtea.Initialize(q);
                    MessageBox.Show("You have successfully set the value.",
                    "Information",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Information);
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void button_WOC4_Click(object sender, EventArgs e)
        {
            bool empty = string.IsNullOrEmpty(tbK.Text);

            if (empty)
            {
                MessageBox.Show("Set a value for the key.",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                return;
            }
            else
            {
                try
                {
                    using (var folderBrowserDialog = new FolderBrowserDialog())
                    {
                        DialogResult result = folderBrowserDialog.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            folderTB.Text = folderBrowserDialog.SelectedPath;

                            ws = new Watcher(folderBrowserDialog.SelectedPath, richTextBox2);
                            Cipher.SetActiveRichBox(richTextBox2);

                            Task.Run(async () =>
                            {
                                await ws.WatchForNewFilesAsync();
                            });
                        }
                    }

                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.ToString(),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void button_WOC5_Click(object sender, EventArgs e)
        {
            bool empty = string.IsNullOrEmpty(tbK.Text);
            bool empty2 = string.IsNullOrEmpty(folderTB.Text);

            if (empty)
            {
                MessageBox.Show("Set a value for the key.",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                return;
            }
            else if (empty2)
            {
                MessageBox.Show("Set a monitored folder.",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                return;
            }
            else
            {
                try
                {
                    using (var folderBrowserDialog = new FolderBrowserDialog())
                    {
                        DialogResult result = folderBrowserDialog.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            textBox1.Text = folderBrowserDialog.SelectedPath;
                            Cipher.SetOutputPath(folderBrowserDialog.SelectedPath);

                            outputFolder=folderBrowserDialog.SelectedPath;
                        }
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.ToString(),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private async void button_WOC6_Click(object sender, EventArgs e)
        {
            bool empty = string.IsNullOrEmpty(tbK.Text);

            if (empty)
            {
                MessageBox.Show("Set a value for the key.",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                return;
            }
            else
            {
                try
                {
                    OpenFileDialog dialog = new OpenFileDialog();

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = dialog.FileName;
                        byte[] fileContent;


                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                        {
                            int fileSize = (int)fileStream.Length;
                            fileContent = new byte[fileSize];

                            await fileStream.ReadAsync(fileContent, 0, fileSize);
                        }

                        loadFile = fileContent;
                        richTextBox1.Text = Convert.ToBase64String(loadFile);
                    }
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.ToString(),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void button_WOC7_Click(object sender, EventArgs e)
        {
            bool empty = string.IsNullOrEmpty(tbK.Text);
            if (empty)
            {
                MessageBox.Show("Set a value for the key.",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                return;
            }
            else
            {
                try
                {
                        richTextBox1.Clear();
                        byte[] encryptedContent = xxtea.Decrypt(loadFile);
                        richTextBox1.Text = Encoding.UTF8.GetString(encryptedContent);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(),
                                    "Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                    return;
                }
            }
        }
    }
}
