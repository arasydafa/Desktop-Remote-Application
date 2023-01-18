using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace ClientDesktopRemoteApplication
{
    public partial class FormClient : Form
    {
        private UdpClient _udpClient;
        private IPEndPoint _remoteEndPoint;
        private int _port = 5555;
        private bool _isListening;

        static string filePath;
        static string text;

        public FormClient()
        {
            InitializeComponent();
        }

        private void StartSending()
        {
            _udpClient = new UdpClient(_port);
            _remoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.3.1"), _port);

            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        byte[] data = Encoding.ASCII.GetBytes("This is a test message");
                        _udpClient.Send(data, data.Length, _remoteEndPoint);
                        Console.WriteLine("Sent Data to {0}:{1}", _remoteEndPoint.Address, _remoteEndPoint.Port);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: '{0}'", ex);
                    }
                }
            }).Start();
        }

        private void ParsingFile()
        {
            string[] lines = contentBox.Text.Split('#');

            for (int i = 0; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(',');

                string ipAddress = "";
                string message1 = "";
                string message2 = "";
                string message3 = "";

                switch (data.Length)
                {
                    case 2:
                        ipAddress = data[0];
                        message1 = data[1];
                        PrintData(ipAddress, message1);
                        break;
                    case 3:
                        ipAddress = data[0];
                        message1 = data[1];
                        message2 = data[2];
                        PrintData(ipAddress, message1, message2);
                        break;
                    case 4:
                        ipAddress = data[0];
                        message1 = data[1];
                        message2 = data[2];
                        message3 = data[3];
                        PrintData(ipAddress, message1, message2, message3);
                        break;
                    default:
                        Console.WriteLine("Line does not contain the correct number of elements. Skipping.");
                        break;
                }
            }
        }

        private void PrintData(string ipAddress, string message1, string message2 = "", string message3 = "")
        {
            Console.WriteLine(ipAddress);
            Console.WriteLine(message1);
            Console.WriteLine(message2);
            Console.WriteLine(message3);
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                text = File.ReadAllText(filePath);
                contentBox.Text = text;
                saveButton.Enabled = true;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            text = contentBox.Text;
            File.WriteAllText(filePath, text);

            MessageBox.Show("Text files saved");
        }

        private void contentBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ParsingFile();
            }
        }
    }
}
