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
            Sector16 Sector16 = new Sector16();
            lbl_Status.Text = "Extracting Information..";
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
            sector16.volDescType = byteInfo[0];
            romPosition++; //standard identifier
            infoLength = 5;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.stdID = ByteConvertString(byteInfo);
            romPosition += infoLength; //volume descriptor version
            infoLength = 1;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.volDescVer = byteInfo[0];
            romPosition += 2; //system identifier
            infoLength = 32;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.sysID = ByteConvertString(byteInfo);
            romPosition += infoLength; //volume indentifier
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.volID = ByteConvertInt32(byteInfo);
            romPosition += infoLength + 8; //volume space size, skipping 8 reserved bytes
            infoLength = 4;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.volSpaceSize = ByteConvertInt32(byteInfo);
            romPosition += (infoLength * 2) + 32; //volume set size, skipping 4 bytes that are a mirror of volume spize size
            infoLength = 2;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.volSetSize = ByteConvertInt16(byteInfo);
            romPosition += (infoLength * 2); //volume sequence number
            infoLength = 2;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.volSeqNum = ByteConvertInt16(byteInfo);
            romPosition += (infoLength * 2); //logical block size in bytes
            infoLength = 2;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.logicalBlockSize = ByteConvertInt16(byteInfo);
            romPosition += (infoLength * 2); //path table size
            infoLength = 4;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.pathTableSize = ByteConvertInt32(byteInfo);
            romPosition += (infoLength * 2); //path table 1 block number
            infoLength = 4;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.pathTable1BlockNo = ByteConvertInt32(byteInfo);
            romPosition += infoLength; //path table 2 block number
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.pathTable2BlockNo = ByteConvertInt32(byteInfo);
            romPosition += infoLength; //path table 3 block number
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.pathTable3BlockNo = ByteConvertInt32(byteInfo);
            romPosition += infoLength; //path table 4 block number
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.pathTable4BlockNo = ByteConvertInt32(byteInfo);
            romPosition += 38; //vol set id, root directory record is its own method
            infoLength = 128;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.volSetId = ByteConvertString(byteInfo);
            romPosition += infoLength; //publisher id
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.publishId = ByteConvertString(byteInfo);
            romPosition += infoLength; //data prep id
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.dataPrepId = ByteConvertString(byteInfo);
            romPosition += infoLength; //application id
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.applicationId = ByteConvertString(byteInfo);
            romPosition += infoLength; //copyright filename
            infoLength = 37;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.copyrightFilename = ByteConvertString(byteInfo);
            romPosition += infoLength; //abstract filename
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.abstractFilename = ByteConvertString(byteInfo);
            romPosition += infoLength; //bibliographic filename
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.biblioFilename = ByteConvertString(byteInfo);
            romPosition += infoLength; //volume creation timestamp
            infoLength = 17;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.volCreationTimestamp2 = ByteConvertDateTime(byteInfo);
            //sector16.volCreationTimestamp = ByteConvertString(byteInfo);

            //len 34 = 21h

            //TODO: more information
            lbl_Status.Text = "Finished extracting, outputting file..";
            Sector16InfoExport(sector16);
        }

        private void Sector16InfoExport(Sector16 sector16)
        {
            StreamWriter output = new StreamWriter(Properties.Settings.Default.outputPath);
            output.WriteLine(string.Format("Volume Descriptor Type: {0:X2}", sector16.volDescType));
            output.WriteLine(string.Format("Standard ID: {0}", sector16.stdID));
            output.WriteLine(string.Format("Volume Description Version: {0:X2}", sector16.volDescVer));
            output.WriteLine(string.Format("System Identifier: {0}", sector16.sysID));
            output.WriteLine(string.Format("Volume Identifier: {0}", sector16.volID));
            output.WriteLine(string.Format("Volume Space Size: {0}", sector16.volSpaceSize));
            output.WriteLine(string.Format("Volume Set Size: {0:X4}h, {0}", sector16.volSetSize));
            output.WriteLine(string.Format("Volume Sequence Number: {0:X4}h, {0}", sector16.volSeqNum));
            output.WriteLine(string.Format("Logical Block Size: {0:X4}h, {0} bytes", sector16.logicalBlockSize));
            output.WriteLine(string.Format("Path Table Size: {0:X8}h, {0} bytes", sector16.pathTableSize));
            output.WriteLine(string.Format("Path Table 1 Block Number: {0:X8}h", sector16.pathTable1BlockNo));
            output.WriteLine(string.Format("Path Table 2 Block Number: {0:X8}h", sector16.pathTable2BlockNo));
            output.WriteLine(string.Format("Path Table 3 Block Number: {0:X8}h", sector16.pathTable3BlockNo));
            output.WriteLine(string.Format("Path Table 4 Block Number: {0:X8}h", sector16.pathTable4BlockNo));
            output.WriteLine(string.Format("Volume Set Identifier: {0}", sector16.volSetId));
            output.WriteLine(string.Format("Publisher Identifier: {0}", sector16.publishId));
            output.WriteLine(string.Format("Data Preparer Identifier: {0}", sector16.dataPrepId));
            output.WriteLine(string.Format("Application Identifier: {0}", sector16.applicationId));
            output.WriteLine(string.Format("Volume Creation Timestamp: {0}", sector16.volCreationTimestamp));
            output.WriteLine(string.Format("Volume Creation Timestamp: {0:yyyy/MM/dd hh:MM}", sector16.volCreationTimestamp2));

            //TODO: a lot more of this
            output.Close();
            lbl_Status.Text = "Export finished!";
        }

        private void ReadIso(BinaryReader rom, int romPosition, int infoLength, out byte[] byteInfo)
        {
            rom.BaseStream.Position = romPosition;
            byteInfo = rom.ReadBytes(infoLength);
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

        private DateTime ByteConvertDateTime(byte[] byteInfo)
        {
            long longVar2 = BitConverter.to
            long longVar = BitConverter.ToInt64(byteInfo, 0);
            return new DateTime().AddMilliseconds(longVar);
            //DateTime tempDT = new DateTime().AddMilliseconds(longVar);
            //return tempDT.ToString("YYYYMMDDHHMMSSFF");
        }

        public class Sector16
        {
            public byte volDescType;
            public string stdID;
            public byte volDescVer;
            public string sysID;
            public Int32 volID;
            public Int32 volSpaceSize;
            public Int16 volSetSize;
            public Int16 volSeqNum;
            public Int16 logicalBlockSize;
            public Int32 pathTableSize;
            public Int32 pathTable1BlockNo;
            public Int32 pathTable2BlockNo;
            public Int32 pathTable3BlockNo;
            public Int32 pathTable4BlockNo;
            public string volSetId;
            public string publishId;
            public string dataPrepId;
            public string applicationId;
            public string copyrightFilename;
            public string abstractFilename;
            public string biblioFilename;
            public string volCreationTimestamp;
            public DateTime volCreationTimestamp2;
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
