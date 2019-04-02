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
        string romLocation = @"C:\Users\Michael\Documents\Tomba stuff\Tomba! (USA).bin";
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Open_Click(object sender, EventArgs e)
        {
            BinaryReader rom = new BinaryReader(File.OpenRead(romLocation));
            BigDataExtract(rom, out string[] sector16Info);
            for (int i = 0; i < sector16Info.Length; i++)
            {
                lsbx_Sector16.Items.Add(sector16Info[i].ToString());
            }
        }

        private void BigDataExtract(BinaryReader rom, out string[] sector16Info)
        {
            //starting variables
            List<string> methodInformation = new List<string>();
            int romPosition = 0x9300; //sector 16 TODO:start at sector 4 to check what region it is based on license string to export
            string information = "";
            int infoLength = 0;

            //string pulling
            romPosition = 0x9319; //standard identifier
            infoLength = 5;
            ReadIsoString(rom, romPosition, infoLength, out information);
            methodInformation.Add(information);
            romPosition += infoLength;
            infoLength = 1;
            ReadIso(rom, romPosition, infoLength, out information);
            methodInformation.Add(information);
            
            //assigning list to out array
            sector16Info = methodInformation.ToArray();
        }

        private void ReadIsoString(BinaryReader rom, int romPosition, int infoLength, out string information)
        {
            rom.BaseStream.Position = romPosition;
            char[] output = rom.ReadChars(infoLength);
            information = new string(output);
        }

        //private void Export()
        //{
        //  write Region:{0}, sector16Info[0].tostriong, etc
        //}

        //notes repository

        //sector 4: license info
        //sectors 5-11: playstation logo (used to compare with the bios as a piracy check)
        //sector 16: primary volume description

        //0C = minutes (01-26, increments on 0D = 59)
        //0D = seconds (00-59, increments on 0E = 75)
        //0E = sectors (00-74)

        //0F = appears to always be 02h, not sure why, doesnt seem to line up with frames despite being the next part of the readout

        //sector 17: volume descriptor set terminator

        //first 00 02 00 02 (0C-0F at start of sector)
        //max 26 30 29 02 may be 119129 not 119130(last sector according to hxd)
    }
}
