using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
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
            Sector16 sector16 = new Sector16();
            Sector17 sector17 = new Sector17();
            StreamWriter output = new StreamWriter(Properties.Settings.Default.outputPath);
            lbl_Status.Text = "Extracting Information..";
            Sector16Extract(rom, sector16, output);
            Sector17Extract(rom, sector17, output);
            output.Close();
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

        private void Sector16Extract(BinaryReader rom, Sector16 sector16, StreamWriter output)
        {
            //starting variables
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
            Array.Reverse(byteInfo); //big endian
            sector16.pathTable3BlockNo = ByteConvertInt32(byteInfo);
            romPosition += infoLength; //path table 4 block number
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            Array.Reverse(byteInfo);
            sector16.pathTable4BlockNo = ByteConvertInt32(byteInfo);
            romPosition += 38; //vol set id, root directory record is its own method
            infoLength = 128;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.volSetID = ByteConvertString(byteInfo);
            romPosition += infoLength; //publisher id
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.publishID = ByteConvertString(byteInfo);
            romPosition += infoLength; //data prep id
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.dataPrepID = ByteConvertString(byteInfo);
            romPosition += infoLength; //application id
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.applicationID = ByteConvertString(byteInfo);
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
            //TODO: need better error display for improper dates/all zero dates, mainly for mod/expire/effect timestamps
            romPosition += infoLength; //volume creation timestamp
            infoLength = 16;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.volCreationTimestamp = ByteConvertDateTime(byteInfo);
            //TODO: unsure of how to format timezone byte, seems to just be 00h. Not exported currently
            romPosition += infoLength; //timezone
            infoLength = 1;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.timezone = byteInfo[0];
            romPosition += infoLength; //volume modification timestamp
            infoLength = 16;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.volModifyTimestamp = ByteConvertDateTime(byteInfo);
            romPosition += infoLength + 1; //volume expiration timestamp
            infoLength = 16;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.volExpireTimestamp = ByteConvertDateTime(byteInfo);
            romPosition += infoLength + 1; //volume effective timestamp
            infoLength = 16;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.volEffectTimestamp = ByteConvertDateTime(byteInfo);
            romPosition += infoLength + 1; //file structure version
            infoLength = 1;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.fileStructVer = byteInfo[0];
            romPosition += infoLength; //reserved for future
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.reservedForFuture = byteInfo[0];
            romPosition += 142; //cd xa identifying signature, skipping over app use area because 00 filled
            infoLength = 8;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector16.cdxaIdSig = ByteConvertString(byteInfo);
            lbl_Status.Text = "Finished extracting sector 16, outputting to file..";
            Sector16InfoExport(sector16, output);
        }

        private void Sector17Extract(BinaryReader rom, Sector17 sector17, StreamWriter output)
        {
            int romPosition = 0x9C48; //vol desc type
            byte[] byteInfo;
            int infoLength = 1;

            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector17.volDescType = byteInfo[0];
            romPosition++; //standard identifier
            infoLength = 5;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector17.stdID = ByteConvertString(byteInfo);
            romPosition += infoLength; //terminator version
            infoLength = 1;
            ReadIso(rom, romPosition, infoLength, out byteInfo);
            sector17.terminatorVer = byteInfo[0];
            lbl_Status.Text = "Finished extracting sector 17, outputting to file..";
            Sector17InfoExport(sector17, output);
        }

        private void Sector16InfoExport(Sector16 sector16, StreamWriter output)
        {
            output.WriteLine("Sector 16 Information");
            output.WriteLine("----------");
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
            output.WriteLine(string.Format("Path Table 1 Block Number: {0}", sector16.pathTable1BlockNo));
            output.WriteLine(string.Format("Path Table 2 Block Number: {0}", sector16.pathTable2BlockNo));
            output.WriteLine(string.Format("Path Table 3 Block Number: {0}", sector16.pathTable3BlockNo));
            output.WriteLine(string.Format("Path Table 4 Block Number: {0}", sector16.pathTable4BlockNo));
            output.WriteLine(string.Format("Volume Set Identifier: {0}", sector16.volSetID));
            output.WriteLine(string.Format("Publisher Identifier: {0}", sector16.publishID));
            output.WriteLine(string.Format("Data Preparer Identifier: {0}", sector16.dataPrepID));
            output.WriteLine(string.Format("Application Identifier: {0}", sector16.applicationID));
            output.WriteLine(string.Format("Volume Creation Timestamp: {0:yyyy-MM-dd HH:mm:ss:ff}", sector16.volCreationTimestamp));
            output.WriteLine(string.Format("Volume Modification Timestamp: {0:yyyy-MM-dd HH:mm:ss:ff}", sector16.volModifyTimestamp));
            output.WriteLine(string.Format("Volume Expiration Timestamp: {0:yyyy-MM-dd HH:mm:ss:ff}", sector16.volExpireTimestamp));
            output.WriteLine(string.Format("Volume Effective Timestamp: {0:yyyy-MM-dd HH:mm:ss:ff}", sector16.volEffectTimestamp));
            output.WriteLine(string.Format("File Structure Version: {0:X2}", sector16.fileStructVer));
            output.WriteLine(string.Format("Reserved For Future: {0:X2}", sector16.reservedForFuture));
            output.WriteLine(string.Format("CD-XA Identifying Signature: {0}", sector16.cdxaIdSig));
            lbl_Status.Text = "Export of sector 16 information finished!";
        }

        private void Sector17InfoExport(Sector17 sector17, StreamWriter output)
        {

            output.WriteLine("");
            output.WriteLine("Sector 17 Information");
            output.WriteLine("----------");
            output.WriteLine(string.Format("Volume Descriptor Type: {0:X2}", sector17.volDescType));
            output.WriteLine(string.Format("Standard ID: {0}", sector17.stdID));
            output.Write(string.Format("Terminator Version: {0}", sector17.terminatorVer));
            lbl_Status.Text = "Export of sector 17 information finished!";
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
            string strDate = Encoding.Default.GetString(byteInfo);
            strDate = string.Format("{0}-{1}-{2} {3}:{4}:{5}:{6}", strDate.Substring(0, 4), strDate.Substring(4, 2), strDate.Substring(6, 2), strDate.Substring(8, 2), strDate.Substring(10, 2), strDate.Substring(12, 2), strDate.Substring(14, 2));
            try
            {
                return DateTime.ParseExact(strDate, "yyyy-MM-dd HH:mm:ss:ff", CultureInfo.InvariantCulture);
            }
            catch
            {
                return DateTime.ParseExact("1000-01-01 01:01:01:01", "yyyy-MM-dd HH:mm:ss:ff", CultureInfo.InvariantCulture);
            }
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
            public string volSetID;
            public string publishID;
            public string dataPrepID;
            public string applicationID;
            public string copyrightFilename;
            public string abstractFilename;
            public string biblioFilename;
            public DateTime volCreationTimestamp;
            public byte timezone;
            public DateTime volModifyTimestamp;
            public DateTime volExpireTimestamp;
            public DateTime volEffectTimestamp;
            public byte fileStructVer;
            public byte reservedForFuture;
            public string cdxaIdSig;
        }

        public class Sector17
        {
            public byte volDescType;
            public string stdID;
            public byte terminatorVer;
        }
        //TODO: these
        public class PathTable
        {
            public byte lengthDirName;
            public byte extAttRecLen;
            public Int32 dirLogicBlockNo;
            public Int16 parentDirNo;

        }

        public class DirectoryDescriptor
        {

        }

        //notes repository

        //I dont want to talk about it
        //return DateTime.Parse(strDate);
        //return DateTime.ParseExact(strDate, "yyyy-MM-dd HH:mm:ss:ff", CultureInfo.InvariantCulture);
        //long longVar2 = BitConverter.to
        //Double longVar = BitConverter.ToDouble(byteInfo, 0);
        //long longVar = BitConverter.ToInt32(byteInfo, 0);
        //return new DateTime().AddMilliseconds(longVar);
        //DateTime tempDT = new DateTime().AddMilliseconds(longVar);
        //return tempDT.ToString("YYYYMMDDHHMMSSFF");

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
        //for root directory later
        //len 34 = 21h

    }
}
