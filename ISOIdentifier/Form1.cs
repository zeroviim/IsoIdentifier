using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISOIdentifier
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            UpdateFilePaths();
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            BinaryReader rom = new BinaryReader(File.OpenRead(Properties.Settings.Default.romPath));
            Sector16 Sector16 = new Sector16();
            Sector16Extract(rom, Sector16);
        }

        private void btn_BinLocation_Click(object sender, EventArgs e)
        {
            OpenFileDialog binLocationPicker;
            binLocationPicker = new OpenFileDialog();
            DialogResult result = new DialogResult();
            result = binLocationPicker.ShowDialog();
            if (result == DialogResult.OK)
            {
                Properties.Settings.Default.romPath = binLocationPicker.FileName;
                Properties.Settings.Default.Save();
            }
            UpdateFilePaths();
        }

        private void btn_Output_Click(object sender, EventArgs e)
        {
            //TODO: folder browser dialog sucks, figure out if we can use a better version somewhere in c# more alike the openfiledialog
            DialogResult result = new DialogResult();
            result = fbd_Output.ShowDialog();
            if (result == DialogResult.OK)
            {
                Properties.Settings.Default.outputPath = fbd_Output.SelectedPath + @"\Iso_Info_Output.txt";
                Properties.Settings.Default.Save();
            }
            UpdateFilePaths();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void UpdateFilePaths()
        {
            lbl_BinLocation.Text = Properties.Settings.Default.romPath;
            lbl_OutputLocation.Text = Properties.Settings.Default.outputPath;
        }

        private void Sector16Extract(BinaryReader rom, Sector16 sector16)
        {
            //starting variables
            List<string> methodInformation = new List<string>();
            int romPosition = 0x9300;
            byte[] byteInfo;
            int infoLength = 0;

            //string pulling
            romPosition += 0x18;//volume descriptor type
            infoLength = 1;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.VolDescType = byteInfo[0];
            romPosition++; //standard identifier
            infoLength = 5;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.stdID = ByteConvertString(byteInfo);
            romPosition += infoLength; //volume descriptor version
            infoLength = 1;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.VolDescVer = byteInfo[0];
            romPosition += 2; //system identifier
            infoLength = 32;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.SysID = ByteConvertString(byteInfo);
            romPosition += infoLength; //volume indentifier
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.VolID = ByteConvertInt32(byteInfo);
            romPosition += infoLength + 8; //volume space size, skipping 8 reserved bytes
            infoLength = 4;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.VolSpaceSize = ByteConvertInt32(byteInfo);
            romPosition += infoLength + 36; //volume set size, skipping 4 bytes that are a mirror of volume spize size
            infoLength = 2;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.VolSetSize = ByteConvertInt16(byteInfo);
            //TODO: more information
            Export(sector16);
        }

        private void ReadIso(BinaryReader rom, int romPosition, int infoLength, out byte[] byteInfo)
        {
            rom.BaseStream.Position = romPosition;
            byteInfo = rom.ReadBytes(infoLength);
        }

        private void Export(Sector16 sector16)
        {
            StreamWriter output = new StreamWriter(Properties.Settings.Default.outputPath);
            output.WriteLine(string.Format("Volume Descriptor Type: {0:X2}", sector16.VolDescType));
            output.WriteLine(string.Format("Standard ID: {0}", sector16.stdID));
            output.WriteLine(string.Format("Volume Description Version: {0:X2}", sector16.VolDescVer));
            output.WriteLine(string.Format("System Identifier: {0}", sector16.SysID));
            output.WriteLine(string.Format("Volume Identifier: {0}", sector16.VolID));
            output.WriteLine(string.Format("Volume Space Size: {0}", sector16.VolSpaceSize));
            output.WriteLine(string.Format("Volume Set Size: {0:X4}", sector16.VolSetSize));
            //TODO: a lot more of this
            output.Close();
        }

        private string ByteConvertString(byte[] byteInfo)
        {
            return Encoding.Default.GetString(byteInfo);
        }

        private int ByteConvertInt32(byte[] byteInfo)
        {
            return BitConverter.ToInt32(byteInfo, 0);
        }

        private Int16 ByteConvertInt16(byte[] byteInfo)
        {
            return BitConverter.ToInt16(byteInfo, 0);
        }

        public class Sector16
        {
            public byte VolDescType;
            public string stdID;
            public byte VolDescVer;
            public string SysID;
            public Int32 VolID;
            public Int32 VolSpaceSize;
            public Int16 VolSetSize;
        }


        //notes repository

        //Open sector file as cd sector size 2352

        //volume space size, number of logical blocks, 2x32bit
        //5A D1 01 00, 00 01 D1 5A
        //90 209 1 0 

        //1,523,646,720
        //119130, first one is little endian, second is big endian

        //sector 4: license info
        //sectors 5-11: playstation logo (used to compare with the bios as a piracy check)
        //sector 16: primary volume description

        //0C = minutes (00-26, increments on 0D = 59)
        //0D = seconds (00-59, increments on 0E = 75)
        //0E = sectors (00-74)

        //0F = appears to always be 02h, not sure why, doesnt seem to line up with frames despite being the next part of the readout

        //sector 17: volume descriptor set terminator

        //first 00 02 00 02 (0C-0F at start of sector)
        //max 26 30 29 02 may be 119129 not 119130(last sector according to hxd)

        //this is an idea of making a custom multi variable type list to store the specific data of the iso sector in the right formats
        //not used currently

    }
}
