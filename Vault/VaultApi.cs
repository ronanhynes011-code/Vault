using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Vault
{
    public static class VaultApi
    {
        [DllImport("bin\\Vault.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void Initialize();

        [DllImport("bin\\Vault.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void ExecuteAsync(byte[] scriptSource, string[] clientUsers, int numUsers);

        [DllImport("bin\\Vault.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetClients();

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct ClientInfo
        {
            public string version;
            public string name;
            public int id;
        }

        public static bool IsAttached { get; private set; } = false;

        public static void Inject()
        {
            if (!IsRobloxOpen())
                throw new Exception("Roblox is not running.");

            try
            {
                Initialize();
                Thread.Sleep(1500);
                IsAttached = true;
            }
            catch (Exception ex)
            {
                IsAttached = false;
                throw new Exception("Injection failed: " + ex.Message);
            }
        }

        public static void Execute(string scriptSource)
        {
            if (!IsAttached)
                throw new Exception("Not attached to a client. Inject first.");

            string[] clients = GetClientsList().Select(c => c.name).ToArray();
            if (clients.Length == 0)
            {
                IsAttached = false;
                throw new Exception("No clients found. Inject again.");
            }

            ExecuteAsync(Encoding.UTF8.GetBytes(scriptSource), clients, clients.Length);
        }

        public static List<ClientInfo> GetClientsList()
        {
            List<ClientInfo> list = new List<ClientInfo>();
            IntPtr ptr = GetClients();

            while (true)
            {
                ClientInfo info = Marshal.PtrToStructure<ClientInfo>(ptr);
                if (info.name == null) break;
                list.Add(info);
                ptr += Marshal.SizeOf<ClientInfo>();
            }
            return list;
        }

        public static bool IsRobloxOpen()
        {
            return Process.GetProcessesByName("RobloxPlayerBeta").Any();
        }
    }
}