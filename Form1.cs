using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using DevExpress.Internal;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;

namespace DesktopRemoteApplication
{
    public partial class Form1 : Form
    {
        /*#region Button Disable variable and Dependencies
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
        #endregion*/

        #region Form Process Variable and Dependencies
        private bool _isRunning = false;

        private UdpClient _udpClient;
        private IPEndPoint _remoteEndPoint;
        private bool _isListening;
        #endregion

        #region Form Process 
        public Form1()
        {
            InitializeComponent();

            // _hookID = SetHook(_proc);
        }

        private void StartProcess()
        {
            if (_isRunning) return;
            _isRunning = true;

            var program = pathBox.Text.ToString();

            try
            {
                if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(program)).Any(p => p.MainModule.FileName == program))
                {
                    statusStrip1.Items[0].Text = "Status: Application is already running";
                    MessageBox.Show("Application is already running");
                }
                else
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
            _remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555);

            _isListening = true;
            new Thread(() =>
            {
                while (_isListening)
                {
                    try
                    {
                        _udpClient.BeginReceive(DataReceived, null);
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
                dataInBox.Invoke((MethodInvoker)delegate
                {
                    dataInBox.AppendText(receivedMessage);
                });
            }
        }
        #endregion

        /*#region Button Disable
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
        #endregion*/

        #region Form Event
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // UnhookWindowsHookEx(_hookID);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _isListening = false;
            _udpClient.Close();
        }
        #endregion

        #region Button Event
        private void executeButton_Click(object sender, EventArgs e)
        {
            if (_isRunning) return;
            StartProcess();
        }

        private void receiveButton_Click(object sender, EventArgs e)
        {
            if (_isListening) return;
            StartListening();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            dataInBox.Clear();
        }
        #endregion

        #region Text Box Event
        private void pathBox_DoubleClick(object sender, EventArgs e)
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
                    pathBox.Text = OpenFileDialog.FileName;
                }
            }
        }

        private void pathBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (_isRunning) return;
            StartProcess();
        }
        #endregion           
    }
}