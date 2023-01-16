using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using DevExpress.Internal;
using System.Linq.Expressions;

namespace DesktopRemoteApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var program = textBox1.Text.ToString();

            try
            {
                using (var process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo(program);
                    process.Start();
                }

                string processName = Path.GetFileNameWithoutExtension(program);
                processChecker.Interval = 100;
                processChecker.Tick += (s, eventArgs) =>
                {
                    bool isRunning = Process.GetProcessesByName(processName).Any(p => p.MainModule.FileName == program);
                    if (isRunning)
                    {
                        statusStrip1.Items[0].Text = "Status: Application '" + processName + "' Running";
                    }
                    else
                    {
                        var allProcess = Process.GetProcesses();

                        if (allProcess.Length > 0)
                        {
                            statusStrip1.Items[0].Text = "Status: Application '" + processName + "' Closed";
                        }
                        else
                        {
                            statusStrip1.Items[0].Text = "Status: There is no application running";
                        }
                    }
                };
                processChecker.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: '{0}'", ex);
            }
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            using (OpenFileDialog)
            {
                OpenFileDialog.Title = "Choose EXE File";
                OpenFileDialog.Filter = "CSV Files (*.exe)|*.exe";
                OpenFileDialog.CheckFileExists = true;
                OpenFileDialog.CheckPathExists = true;
                OpenFileDialog.FileName = "";

                if (OpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = OpenFileDialog.FileName;
                }
            }
        }
    }
}