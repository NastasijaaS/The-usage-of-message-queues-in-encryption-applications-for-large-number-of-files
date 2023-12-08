using System;
using System.Security.Cryptography;


namespace DiplomskiRad
{
    class XXTEA
    {
        private static uint[] kljuc;
        uint delta = 0x9e3779b9;

        public XXTEA()
        {
            kljuc = new UInt32[4];
        }

        public void Initialize(uint[] kljucc)
        {
            kljuc = kljucc;
        }

        public byte[] Encrypt(byte[] source)
        {
            try
            {
                uint[] v = PretvoriByteToUInt(source);

                int n = v.Length;
                int q = Convert.ToInt32(Math.Floor(Convert.ToDecimal(6 + 52 / n)));

                uint z = v[n - 1], y = v[0];
                uint e, sum = 0;

                while (q-- > 0)
                {
                    sum += delta;
                    e = sum >> 2 & 3;
                    for (int p = 0; p < n; p++)
                    {
                        y = v[(p + 1) % n];
                        z = v[p] += MX(sum, y, z, p, e, kljuc);
                    }
                }
                byte[] arr = PretvoriUIntUByte(v);
                return arr;
            }
            catch(Exception ex)
            {
                throw new Exception("An exception occurred during XXTEA encryption: ", ex);
            }
        }

        public byte[] Decrypt(byte[] source)
        {
            try
            {
                uint[] v = PretvoriByteToUInt(source);
                int n = v.Length;

                uint q = Convert.ToUInt32(Math.Floor(Convert.ToDecimal(6 + 52 / n)));

                uint z = v[n - 1], y = v[0];
                uint mx, e, sum = q * delta;

                while (sum != 0)
                {
                    e = sum >> 2 & 3;
                    for (int p = n - 1; p >= 0; p--)
                    {
                        z = v[p > 0 ? p - 1 : n - 1];
                        mx = MX(sum, y, z, p, e, kljuc);
                        y = v[p] -= mx;
                    }
                    sum -= delta;
                }

                byte[] arr = PretvoriUIntUByte(v);
                return arr;
            }
            catch(Exception ex)
            {
                throw new Exception("An exception occurred during XXTEA decryption: ", ex);
            }
        }

        private static UInt32 MX(UInt32 sum, UInt32 y, UInt32 z, Int32 p, UInt32 e, UInt32[] k)
        {
            return (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z);
        }

        public uint[] PretvoriByteToUInt(byte[] niz)
        {
            int duzina_niz = niz.Length;
            int duzina = duzina_niz / 4;
            if (duzina_niz % 4 != 0)
                duzina++;
            uint[] povratna_vrendost = new uint[duzina];
            int j = 0;
            byte[] pom = new byte[4];
            for (int i = 0; i < duzina_niz; i = i + 4)
            {
                for (int k = 0; k < 4; k++)
                {
                    if (i + k < duzina_niz)
                        pom[k] = niz[i + k];
                    else
                        pom[k] = 0;
                }
                povratna_vrendost[j++] = BitConverter.ToUInt32(pom, 0);
            }
            return povratna_vrendost;
        }

        public byte[] PretvoriUIntUByte(uint[] niz)
        {
            int broj_bajtova = niz.Length * 4;
            byte[] povratna_vrednost = new byte[broj_bajtova];
            byte[] intBytes;
            int j = 0;
            for (int i = 0; i < broj_bajtova; i = i + 4)
            {
                intBytes = BitConverter.GetBytes(niz[j++]);
                povratna_vrednost[i] = intBytes[0];
                povratna_vrednost[i + 1] = intBytes[1];
                povratna_vrednost[i + 2] = intBytes[2];
                povratna_vrednost[i + 3] = intBytes[3];
            }
            return povratna_vrednost;
        }

        public byte[] GenerateRandomKey()
        {
            byte[] key = GenerateRandomBytes(16);
            return key;
        }


        static byte[] GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
