﻿using System;
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
            BigDataExtract(rom, out string[] sector16Info);
            Export(sector16Info);
        }

        private void btn_BinLocation_Click(object sender, EventArgs e)
        {
            //todo: file browser to pull location of bin picked by user and store in a string
            OpenFileDialog binLocationPicker;
            binLocationPicker = new OpenFileDialog();
            binLocationPicker.ShowDialog();
            Properties.Settings.Default.romPath = binLocationPicker.FileName;
            UpdateFilePaths();
        }

        private void btn_Output_Click(object sender, EventArgs e)
        {
            //TODO: folder browser dialog sucks, figure out if we can use a better version somewhere in c# more alike the openfiledialog
            fbd_Output.ShowDialog();
            Properties.Settings.Default.outputPath = fbd_Output.SelectedPath + @"\Iso_Info_Output.txt";
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

        private void BigDataExtract(BinaryReader rom, out string[] sector16Info)
        {
            //starting variables
            List<string> methodInformation = new List<string>();
            int romPosition = 0x9300; //sector 16 TODO:start at sector 4 to check what region it is based on license string to export
            //TODO: testing, delete later
            romPosition = 0x0;
            string information = "";
            int infoLength = 0;

            //string pulling
            romPosition += 0x19; //standard identifier
            infoLength = 5;
            ReadIso(rom, romPosition, infoLength, out information);
            methodInformation.Add(information);
            romPosition += infoLength;
            infoLength = 1;
            ReadIso(rom, romPosition, infoLength, out information);
            methodInformation.Add(information);
            //TODO: more information
            
            //assigning list to out array
            sector16Info = methodInformation.ToArray();
        }

        private void ReadIso(BinaryReader rom, int romPosition, int infoLength, out string information)
        {
            rom.BaseStream.Position = romPosition;
            char[] output = rom.ReadChars(infoLength);
            information = new string(output);
        }

        private void Export(string[] information)
        {
            StreamWriter output = new StreamWriter(Properties.Settings.Default.outputPath);
            output.WriteLine(string.Format("Standard ID: {0}", information[0]));
            byte[] version = Encoding.ASCII.GetBytes(information[1]);
            output.WriteLine(string.Format("Volume Description Version: {0:X2}", version[0]));
            //TODO: a lot more of this
            output.Close();
        }

        //notes repository

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
        public interface isoInformationList { }
        public class isoInformation : isoInformationList
        {
            public byte VolDescType;
            public string stdID;
            public byte VolDescVer;

        }


    }
}
