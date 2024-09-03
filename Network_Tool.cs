using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;

namespace DEVELOP_Setup_Tool
{
    public static class Network_Tool
    {
        public static bool IsLocalAddress(IPAddress ipAddress)
        {
            // Vérifie si l'adresse est une adresse de boucle locale (127.0.0.0/8)
            byte[] ipBytes = ipAddress.GetAddressBytes();
            return ipBytes[0] == 127;
        }

        public static (string subnet, int cidr) GetLocalSubnet()
        {
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var ip in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            var ipAddress = ip.Address;
                            var subnetMask = ip.IPv4Mask;
                            var ipBytes = ipAddress.GetAddressBytes();
                            var maskBytes = subnetMask.GetAddressBytes();

                            // Calculer le sous-réseau
                            var subnetBytes = new byte[ipBytes.Length];
                            for (int i = 0; i < ipBytes.Length; i++)
                            {
                                subnetBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
                            }
                            var subnet = new IPAddress(subnetBytes).ToString();
                            int cidr = GetCIDR(subnetMask);
                            return (subnet, cidr);
                        }
                    }
                }
            }
            throw new Exception("Aucune adresse IPv4 active trouvée sur l'hôte local.");
        }

        public static int GetCIDR(IPAddress subnetMask)
        {
            byte[] ipBytes = subnetMask.GetAddressBytes();
            int cidr = 0;
            foreach (byte b in ipBytes)
            {
                cidr += Convert.ToString(b, 2).Count(c => c == '1');
            }
            return cidr;
        }
        public static List<string> GetIPRange(string subnet, int cidr)
        {
            List<string> ipRange = new List<string>();

            string[] ipParts = subnet.Split('.');
            int ipInt = (int.Parse(ipParts[0]) << 24) |
                        (int.Parse(ipParts[1]) << 16) |
                        (int.Parse(ipParts[2]) << 8) |
                        int.Parse(ipParts[3]);

            int mask = ~0 << (32 - cidr);

            int startIP = ipInt & mask;
            int endIP = startIP | ~mask;

            for (int ip = startIP + 1; ip < endIP; ip++)
            {
                ipRange.Add(string.Join(".",
                    (ip >> 24) & 0xFF,
                    (ip >> 16) & 0xFF,
                    (ip >> 8) & 0xFF,
                    ip & 0xFF));
            }

            return ipRange;
        }
        public static string GetSubnetAddress(IPAddress ipAddress, IPAddress subnetMask)
        {
            var ipBytes = ipAddress.GetAddressBytes();
            var maskBytes = subnetMask.GetAddressBytes();

            // Calculer le sous-réseau
            var subnetBytes = new byte[ipBytes.Length];
            for (int i = 0; i < ipBytes.Length; i++)
            {
                subnetBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }
            return new IPAddress(subnetBytes).ToString();
        }
    }
}
