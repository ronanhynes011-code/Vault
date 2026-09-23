using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Vault
{
    public static class VaultApi
    {
        private const string SocketHost = "127.0.0.1";
        private const int SocketPort = 6969;
        private const int ConnectTimeoutMs = 5000;

        public static bool IsAttached { get; private set; }

        private static string BackendDir
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "backend"); }
        }

        public static void Inject()
        {
            if (!IsRobloxOpen())
                throw new Exception("Roblox is not running. Launch it and join a game first.");

            string injectorPath = Path.Combine(BackendDir, "YuB-X-Injector.exe");
            string modulePath = Path.Combine(BackendDir, "example_dll.dll");

            if (!File.Exists(injectorPath))
                throw new Exception("Injector not found: " + injectorPath);
            if (!File.Exists(modulePath))
                throw new Exception("Module DLL not found: " + modulePath);

            ProcessStartInfo psi = new ProcessStartInfo(injectorPath);
            psi.WorkingDirectory = BackendDir;
            psi.UseShellExecute = true;
            Process.Start(psi);

            // Poll for the Module's TCP server to come up inside Roblox
            bool alive = false;
            for (int i = 0; i < 30; i++)
            {
                Thread.Sleep(400);
                if (IsSocketAlive()) { alive = true; break; }
            }

            if (!alive)
                throw new Exception("Injection failed. Module did not start its TCP server on port 6969. Close the injector console window and try again.");

            IsAttached = true;
        }

        public static void Execute(string script)
        {
            if (!IsAttached)
                throw new Exception("Not attached. Click Inject first.");
            if (string.IsNullOrEmpty(script))
                throw new Exception("Script is empty.");

            byte[] scriptBytes = Encoding.UTF8.GetBytes(script);
            byte[] lengthBytes = BitConverter.GetBytes(scriptBytes.Length);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(lengthBytes);

            using (TcpClient client = new TcpClient())
            {
                IAsyncResult result = client.BeginConnect(SocketHost, SocketPort, null, null);
                bool connected = result.AsyncWaitHandle.WaitOne(ConnectTimeoutMs);
                if (!connected)
                {
                    IsAttached = false;
                    throw new Exception("Could not reach Module on port 6969. Re-inject.");
                }
                client.EndConnect(result);

                using (NetworkStream stream = client.GetStream())
                {
                    stream.Write(lengthBytes, 0, 4);
                    stream.Write(scriptBytes, 0, scriptBytes.Length);
                    stream.Flush();
                }
            }
        }

        public static bool IsRobloxOpen()
        {
            return Process.GetProcessesByName("RobloxPlayerBeta").Any();
        }

        private static bool IsSocketAlive()
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    IAsyncResult result = client.BeginConnect(SocketHost, SocketPort, null, null);
                    bool connected = result.AsyncWaitHandle.WaitOne(500);
                    if (!connected) return false;
                    client.EndConnect(result);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
