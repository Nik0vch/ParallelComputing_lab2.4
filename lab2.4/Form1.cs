using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab2._4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        private async void btnAnalyze_Click(object sender, EventArgs e)
        {
            string filePath = txtFilePath.Text;

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                MessageBox.Show("Указан не валидный путь файла.");
                return;
            }

            string resultFilePath = filePath.Replace(".txt", "") + " result.txt";

            await Task.Run(() =>
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "lab2.exe",
                    Arguments = $"\"{filePath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                using (Process process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        process.PriorityClass = ProcessPriorityClass.High;
                        process.WaitForExit();
                    }
                }
            });

            if (File.Exists(resultFilePath))
            {
                string result = File.ReadAllText(resultFilePath);
                lblResult.Text = "Самое длинное слово:\n     " + result;
            }
            else
            {
                lblResult.Text = "Result file not found.";
            }
        }
    }
}
