using System;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DesktopRemoteApplication
{
    public partial class Form1 : Form
    {
        #region Button Disable variable and Dependencies
        private static int WH_KEYBOARD_LL = 13;
        private static int WM_KEYDOWN = 0x0100;
        private static int WM_SYSKEYDOWN = 0x0104;
        private static IntPtr _hookID = IntPtr.Zero;

        private static LowLevelKeyboardProc _proc = HookCallback;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        #endregion Button Disable variable and Dependencies

        #region Form Process Variable and Dependencies
        private bool _isRunning = false;

        private UdpClient _udpClient;
        private IPEndPoint _remoteEndPoint;
        private bool _isListening;
        #endregion Form Process Variable and Dependencies

        #region Form Process
        public Form1()
        {
            InitializeComponent();

            _hookID = SetHook(_proc);

            pathBox.KeyDown += pathBox_KeyDown;
        }

        private void StartProcess(params string[] ProcessArgs)
        {
            if (_isRunning) return;
            _isRunning = true;

            var program1 = textBox1.Text.ToString();
            var program2 = textBox2.Text.ToString();
            var program3 = textBox3.Text.ToString();

            try
            {
                if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(program1)).Any(p => p.MainModule.FileName == program1) &&
                    Process.GetProcessesByName(Path.GetFileNameWithoutExtension(program2)).Any(p => p.MainModule.FileName == program2) &&
                    Process.GetProcessesByName(Path.GetFileNameWithoutExtension(program3)).Any(p => p.MainModule.FileName == program3))
                {
                    statusStrip1.Items[0].Text = "Status: Application is already running";
                    MessageBox.Show("Application is already running");
                }
                else
                {
                    using (var process1 = new Process())
                    {
                        process1.StartInfo = new ProcessStartInfo(program1);
                        process1.Start();
                    }
                    using (var process2 = new Process())
                    {
                        process2.StartInfo = new ProcessStartInfo(program2);
                        process2.Start();
                    }
                    using (var process3 = new Process())
                    {
                        process3.StartInfo = new ProcessStartInfo(program3);
                        process3.Start();
                    }

                    string processName1 = Path.GetFileNameWithoutExtension(program1);
                    string processName2 = Path.GetFileNameWithoutExtension(program2);
                    string processName3 = Path.GetFileNameWithoutExtension(program3);

                    processChecker1.Interval = 100;
                    processChecker1.Tick += (s, eventArgs) =>
                    {
                        bool isRunning = Process.GetProcessesByName(processName1).Any(p => p.MainModule.FileName == program1);
                        if (isRunning)
                        {
                            statusStrip1.Items[0].Text = "Status: Application '" + processName1 + "' Running";
                        }
                        else
                        {
                            var allProcess = Process.GetProcesses();

                            if (allProcess.Length > 0)
                            {
                                statusStrip1.Items[0].Text = "Status: Application '" + processName1 + "' Closed";
                            }
                            else
                            {
                                statusStrip1.Items[0].Text = "Status: There is no application running";
                            }
                        }
                    };
                    processChecker1.Start();

                    processChecker2.Interval = 100;
                    processChecker2.Tick += (s, eventArgs) =>
                    {
                        bool isRunning = Process.GetProcessesByName(processName2).Any(p => p.MainModule.FileName == program2);
                        if (isRunning)
                        {
                            statusStrip2.Items[0].Text = "Status: Application '" + processName2 + "' Running";
                        }
                        else
                        {
                            var allProcess = Process.GetProcesses();

                            if (allProcess.Length > 0)
                            {
                                statusStrip2.Items[0].Text = "Status: Application '" + processName2 + "' Closed";
                            }
                            else
                            {
                                statusStrip2.Items[0].Text = "Status: There is no application running";
                            }
                        }
                    };
                    processChecker2.Start();

                    processChecker3.Interval = 100;
                    processChecker3.Tick += (s, eventArgs) =>
                    {
                        bool isRunning = Process.GetProcessesByName(processName3).Any(p => p.MainModule.FileName == program3);
                        if (isRunning)
                        {
                            statusStrip3.Items[0].Text = "Status: Application '" + processName3 + "' Running";
                        }
                        else
                        {
                            var allProcess = Process.GetProcesses();

                            if (allProcess.Length > 0)
                            {
                                statusStrip3.Items[0].Text = "Status: Application '" + processName3 + "' Closed";
                            }
                            else
                            {
                                statusStrip3.Items[0].Text = "Status: There is no application running";
                            }
                        }
                    };
                    processChecker3.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: '{0}'", ex);
            }
            _isRunning = false;
        }

        private void StartListening()
        {
            _udpClient = new UdpClient(5555);
            // _remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);
            _remoteEndPoint = new IPEndPoint(IPAddress.Parse("192.168.3.1"), 5555);

            _isListening = true;

            new Thread(() =>
            {
                while (_isListening)
                {
                    try
                    {
                        if (_udpClient != null)
                        {
                            _udpClient.BeginReceive(DataReceived, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred: '{0}'", ex);
                    }
                }
            }).Start();
        }

        private void DataReceived(IAsyncResult asyncResult)
        {
            if (_udpClient != null && _remoteEndPoint != null)
            {
                byte[] receivedBytes = _udpClient.EndReceive(asyncResult, ref _remoteEndPoint);
                string receivedMessage = Encoding.ASCII.GetString(receivedBytes);
                receivedMessage = receivedMessage.TrimStart('[').TrimEnd(']');

                // receivedMessage = Decrypt(receivedMessage);

                /*if (!receivedMessage.Contains(","))
                {
                    dataInBox.Invoke((MethodInvoker)delegate
                    {
                        dataInBox.AppendText(receivedMessage + Environment.NewLine);

                    });

                    pathBox.Invoke((MethodInvoker)delegate
                    {
                        pathBox.Text = receivedMessage;
                    });

                    SimulateExecuteButtonClick();
                }*/
                string[] substrings = receivedMessage.Split(',');

                if (substrings.Length <= 0 && substrings.Length > 3)
                {
                    MessageBox.Show("Data format is not correct, only 1 substrings minimal and 3 substrings maximal are expected but received " + substrings.Length);
                    return;
                }

                for (int i = 0; i < substrings.Length; i++)
                {
                    string substring = substrings[i].Trim();
                    if (i == 0)
                    {
                        textBox1.Invoke((MethodInvoker)delegate { textBox1.Text = substring; });
                    }
                    else if (i == 1)
                    {
                        textBox2.Invoke((MethodInvoker)delegate { textBox2.Text = substring; });
                    }
                    else if (i == 2)
                    {
                        textBox3.Invoke((MethodInvoker)delegate { textBox3.Text = substring; });
                    }
                }

                if (substrings.Length == 1)
                {
                    StartProcess(substrings[0], null, null);
                }
                else if (substrings.Length == 2)
                {
                    StartProcess(substrings[0], substrings[1], null);
                }
                else if (substrings.Length == 3)
                {
                    StartProcess(substrings[0], substrings[1], substrings[2]);
                }

                dataInBox.Invoke((MethodInvoker)delegate
                {
                    dataInBox.AppendText(receivedMessage + Environment.NewLine);
                });
            }
        }

        private string Decrypt(string message)
        {
            int shift = 11;
            char[] characters = message.ToCharArray();

            for (int i = 0; i < characters.Length; i++)
            {
                int charValue = (int)characters[i];
                charValue -= shift;
                if (charValue < 32)
                {
                    charValue = 127 - (32 - charValue);
                }
                characters[i] = (char)charValue;
            }
            return new string(characters);
        }
        #endregion Form Process

        #region Button Disable
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == (int)Keys.LWin || vkCode == (int)Keys.RWin || vkCode == (int)Keys.Tab && (Control.ModifierKeys & Keys.Alt) != 0)
                {
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        #endregion Button Disable

        #region Form Event
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isListening = false;
            if (_udpClient != null)
            {
                _udpClient.Dispose();
                _udpClient.Close();
                _udpClient = null;
            }
        }

        #endregion Form Event

        #region Button Event
        private void executeButton_Click(object sender, EventArgs e)
        {
            /*if (_isRunning) return;
            string command = pathBox.Text;
            StartProcess(command);*/
        }

        private void receiveButton_Click(object sender, EventArgs e)
        {
            if (_isListening) return;

            statusStrip4.Items[0].Text = "Connection Status: Ready to receive UDP data";

            StartListening();
        }

        private void SimulateExecuteButtonClick()
        {
            /* if (_isRunning) return;
             string command = pathBox.Text;
             StartProcess(command);*/

            /*if (executeButton.InvokeRequired)
            {
                executeButton.Invoke(new MethodInvoker(delegate { executeButton.PerformClick(); }));
            }
            else
            {
                executeButton.PerformClick();
            }*/
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            pathBox.Clear();
            dataInBox.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }
        #endregion Button Event

        #region Text Box Event
        private void pathBox_DoubleClick(object sender, EventArgs e)
        {
            using (OpenFileDialog)
            {
                OpenFileDialog.Title = "Choose EXE File";
                OpenFileDialog.Filter = "EXE Files (*.exe)|*.exe";
                OpenFileDialog.CheckFileExists = true;
                OpenFileDialog.CheckPathExists = true;
                OpenFileDialog.FileName = "";

                if (OpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pathBox.Text = OpenFileDialog.FileName;
                }
            }
        }

        private void pathBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (_isRunning) return;
                string command = pathBox.Text;
                StartProcess(command);
            }
        }
        #endregion Text Box Event
    }
}