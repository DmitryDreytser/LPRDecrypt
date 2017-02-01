using System;
using System.IO;
using System.Text;
using System.Windows;


namespace LPRDecode
{
    public partial class Form1 : Window, System.Windows.Markup.IComponentConnector
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static byte[] UU2bin(Stream input)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            int len = (int)input.Length;

            if (len == 0)
                throw new ArgumentNullException("input"); ;

            int resultLength = 3 * len / 4 + 1;
            byte[] output = new byte[resultLength];

            int didx = 0;
            byte A, B, C, D;
            while (didx < resultLength)
            {
                A = (byte)(input.ReadByte() - 0x20);
                B = (byte)(input.ReadByte() - 0x20);
                C = (byte)(input.ReadByte() - 0x20);
                D = (byte)(input.ReadByte() - 0x20);

                try
                {
                    output[didx++] = (byte)((byte)((A << 2)) | (byte)((B >> 4) & 3));
                    output[didx++] = (byte)((byte)((B << 4)) | (byte)((C >> 2) & 0xf));
                    output[didx++] = (byte)((byte)((C << 6)) | (byte)(D & 0x3f));
                }
                catch
                {

                }
            }
            return output;
        }


        private void DecodeButton_Click(object sender, EventArgs e)
        {
            byte[] passBinData = UU2bin(new MemoryStream(Encoding.ASCII.GetBytes(UUEPass.Text)));

            int v37 = passBinData.Length;

            byte[] net = new byte[256];

            byte[] key = Encoding.ASCII.GetBytes("ADMIN_SERVICE_PARAMS");

            int v14 = 0;

            do
            {
                net[v14] = (byte)v14;
                ++v14;
            }
            while (v14 < 256);

            byte v15 = 0;
            int v17 = 0;
            int v18 = 0;
            int v19 = 0;
            int v20 = 0;
            int v41 = key.Length;

            sbyte v39 = 0;

            int v16 = 0;
            do
            {
                v17 = net[v16];
                v18 = v39;
                v15 += (byte)(net[v16] + key[v39]);
                v19 = net[v15];
                v20 = net[v15] ^ net[v16];
                net[v16] = (byte)v20;
                net[v15] ^= (byte)v20;
                net[v16++] ^= net[v15];
                v39 = (sbyte)((v18 + 1) % v41);
            }
            while (v16 < 256);

            byte v36_LOBYTE = 0;
            byte v36_BYTE1 = 0;
            int v21 = 0;
            int v22 = 0;
            int v25 = 0;
            byte v26 = 0;

            if (v37 > 0)
            {
                do
                {
                    v36_LOBYTE += 1;

                    v22 = net[v36_LOBYTE];
                    v36_BYTE1 += (byte)((sbyte)v22);

                    v25 = net[v36_BYTE1] ^ v22;
                    net[v36_LOBYTE] = (byte)v25;
                    net[v36_BYTE1] ^= (byte)v25;

                    net[v36_LOBYTE] ^= net[v36_BYTE1];

                    v26 = (byte)(net[(byte)(net[v36_BYTE1] + net[v36_LOBYTE])] ^ passBinData[v21]);
                    passBinData[v21++] = v26;
                }
                while (v21 < v37);
            }

            UUEPass.Text = Encoding.GetEncoding(1251).GetString(passBinData);


        }
    }
}
