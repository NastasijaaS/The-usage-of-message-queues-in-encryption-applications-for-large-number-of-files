using System;
using System.Collections.Generic;


namespace DiplomskiRad
{
    class RC6
    {
        private const int W = 32;
        private const int R = 20;
        private static uint[] key = new uint[2 * R + 4];

        public RC6()
        {

        }

        public void SetKey(string key)
        {
            uint val;
            UInt32.TryParse(key, out val);
            KeyGen(val);
        }

        private uint RightShift(uint z_value, int z_shift)
        {
            return ((z_value >> z_shift) | (z_value << (W - z_shift)));
        }

        private uint LeftShift(uint z_value, int z_shift)
        {
            return ((z_value << z_shift) | (z_value >> (W - z_shift)));
        }


        private void KeyGen(uint dwKey)
        {
            try
            {
                uint P32 = 0xB7E15163;
                uint Q32 = 0x9E3779B9;

                uint i, A, B;

                key[0] = P32;

                for (i = 1; i < 2 * R + 4; i++)
                    key[i] = key[i - 1] + Q32;

                i = A = B = 0;
                int v = 3 * ( 2 * R + 4);

                for (int s = 1; s <= v; s++)
                {
                    A = key[i] = LeftShift(key[i] + A + B, 3);
                    B = dwKey = LeftShift(dwKey + A + B, (int)(A + B));
                    i = (i + 1) % (2 * R + 4);
                }
            }
            catch(Exception ex)
            {
                throw new Exception("An exception occurred during RC6 KeyGen: ", ex);
            }
        }

        public byte[] Encrypt(byte[] byteText)
        {
            try
            {
                List<byte> resultData = new List<byte>();
                int i = byteText.Length;
                while (i % 16 != 0)
                    i++;
                byte[] text = new byte[i];
                byteText.CopyTo(text, 0);

                uint[] pom = new uint[4];

                for (i = 0; i < text.Length; i += 16)
                {
                    pom[0] = BitConverter.ToUInt32(text, i);
                    pom[1] = BitConverter.ToUInt32(text, i + 4);
                    pom[2] = BitConverter.ToUInt32(text, i + 8);
                    pom[3] = BitConverter.ToUInt32(text, i + 12);

                    pom[1] = (pom[1] + key[0]);
                    pom[3] = (pom[3] + key[1]);

                    for (int j = 1; j <= R; j++)
                    {

                        uint t = LeftShift((pom[1] * (2 * pom[1] + 1)), (int)(Math.Log((double)W) / Math.Log(2.0)));
                        uint u = LeftShift((pom[3] * (2 * pom[3] + 1)),(int)(Math.Log((double)W) / Math.Log(2.0)));

                        pom[0] = (LeftShift(pom[0] ^ t,(int)u) + key[2 * j]);
                        pom[2] = (LeftShift(pom[2] ^ u, (int)t) + key[2 * j + 1]);

                        uint temp = pom[0];
                        pom[0] = pom[1];
                        pom[1] = pom[2];

                        pom[2] = pom[3];
                        pom[3] = temp;
                    }
                    pom[0] = (pom[0] + key[2 * R + 2]);
                    pom[2] = (pom[2] + key[2 * R + 3]);

                    resultData.AddRange(PretvoriUIntUByte(pom));
                }

                return resultData.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occurred during RC6 encryption: ", ex);
            }
        }

        public byte[] Decrypt(byte[] text)
        {
            try
            {
                List<byte> resultData = new List<byte>();
                uint[] pom = new uint[4];

                for (int i = 0; i < text.Length; i += 16)
                {
                    pom[0] = BitConverter.ToUInt32(text, i);
                    pom[1] = BitConverter.ToUInt32(text, i + 4);
                    pom[2] = BitConverter.ToUInt32(text, i + 8);
                    pom[3] = BitConverter.ToUInt32(text, i + 12);

                    pom[0] = (pom[0] - key[2 * R + 2]);
                    pom[2] = (pom[2] - key[2 * R + 3]);

                    for (int j = R; j >= 1; j--)
                    {
                        uint temp = pom[3];
                        pom[3] = pom[2];
                        pom[2] = pom[1];
                        pom[1] = pom[0];
                        pom[0] = temp;

                        uint t = LeftShift((pom[1] * (2 * pom[1] + 1)), (int)(Math.Log((double)W) / Math.Log(2.0)));
                        uint u = LeftShift((pom[3] * (2 * pom[3] + 1)), (int)(Math.Log((double)W) / Math.Log(2.0)));

                        pom[0] = (RightShift((pom[0] - key[2 * j]), (int)u)) ^ t;
                        pom[2] = (RightShift((pom[2] - key[2 * j + 1]), (int)t)) ^ u;

                    }
                    pom[1] = (pom[1] - key[0]);
                    pom[3] = (pom[3] - key[1]);

                    resultData.AddRange(PretvoriUIntUByte(pom));

                }

                return resultData.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occurred during RC6 decryption: ", ex);
            }
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

    }
}
