﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace SketchupMaterialGenerator
{
    public partial class MainForm : Form
    {
        string DOCUMENT_XML_NAME = "document.xml";
        string REFERENCES_XML_NAME = "references.xml";
        string DOCUMENT_PROPERTIES_XML_NAME = "documentProperties.xml";
        string DOC_THUMBNAIL_PNG_NAME = "doc_thumbnail.png";

        string references_xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><references xmlns=\"http://sketchup.google.com/schemas/1.0/references\" xmlns:r=\"http://sketchup.google.com/schemas/1.0/references\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://sketchup.google.com/schemas/1.0/references http://sketchup.google.com/schemas/1.0/references.xsd\" />";

        private string Get_DocumentPropertiesXml(string materialName)
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><documentProperties xmlns=\"http://sketchup.google.com/schemas/1.0/documentproperties\" xmlns:dp=\"http://sketchup.google.com/schemas/1.0/documentproperties\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://sketchup.google.com/schemas/1.0/documentproperties http://sketchup.google.com/schemas/1.0/documentproperties.xsd\">"
                +"<dp:title>" + materialName + "</dp:title>"
                +"<dp:description></dp:description><dp:creator></dp:creator><dp:keywords></dp:keywords><dp:lastModifiedBy></dp:lastModifiedBy><dp:revision>0</dp:revision>"
                +"<dp:created>2022-03-21T16:07:58Z</dp:created><dp:modified>2022-03-21T16:07:58Z</dp:modified>"
                +"<dp:thumbnail>doc_thumbnail.png</dp:thumbnail>"
                +"<dp:generator dp:name=\"Material\" dp:version=\"1\" /></documentProperties>";
        }

        private string Get_DocumentXml(string materialName, string originalFilePath, double scale)
        {
            string textureFileName = Path.GetFileName(originalFilePath);
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><materialDocument xmlns=\"http://sketchup.google.com/schemas/sketchup/1.0/material\" xmlns:mat=\"http://sketchup.google.com/schemas/sketchup/1.0/material\" xmlns:r=\"http://sketchup.google.com/schemas/1.0/references\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://sketchup.google.com/schemas/sketchup/1.0/material http://sketchup.google.com/schemas/sketchup/1.0/material.xsd\">"
                + "<mat:material name=\"" + materialName 
                + "\" type=\"1\" colorRed=\"255\" colorGreen=\"255\" colorBlue=\"255\" colorizeType=\"0\" trans=\"0.5\" useTrans=\"0\" hasTexture=\"1\">"
                + "<mat:texture textureFilename=\"" + textureFileName //originalFilePath
                + "\" xScale=\""+scale.ToString().Replace(",",".")+"\" yScale=\"1.0\" avgColor=\"4294967295\"><mat:images>"
                + "<mat:image id=\"1\" path=\""+textureFileName+"\" />"
                + "</mat:images></mat:texture></mat:material></materialDocument>";
        }


        public string[] GetImageFiles(string dir)
        {
            return Directory.GetFiles(dir, "*.*", chkSearchSubfolders.Checked?SearchOption.AllDirectories:SearchOption.TopDirectoryOnly)
                .Where(s => s.ToLower().EndsWith(".png", ".bmp", ".jpg", ".jpeg", ".jpe", ".gif", ".tiff")).ToArray<string>();
        }

        private ImageFormat GetImageFormatFromFileName(string filePath)
        {
            string ext = Path.GetExtension(filePath).ToLower();
            //rtxt.AppendText(ext + "\n");
            if (ext == ".bmp") return ImageFormat.Bmp;
            else if (ext == ".png") return ImageFormat.Png;
            else if (ext == ".jpg" || ext == ".jpe" || ext == ".jpeg") return ImageFormat.Jpeg;
            else if (ext == ".gif") return ImageFormat.Gif;
            else if (ext == ".tiff") return ImageFormat.Tiff;
            else return ImageFormat.Jpeg;
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private bool SelectFolder(out string path)
        {
            //CommonOpenFileDialog = cofd new CommonOpenFileDialog();
            path = "";
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ValidateNames = false;
            ofd.CheckFileExists = false;
            ofd.FileName = "(select folder)";
            ofd.Title = "Select the folder";
            ofd.InitialDirectory = Application.StartupPath;

            DialogResult dr = ofd.ShowDialog();

            if (dr != DialogResult.OK) return false;
            path = Path.GetDirectoryName(ofd.FileName);
            return true;
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            string dir = "";
            if (!SelectFolder(out dir)) return;
            rtxt.Clear();
            generate(dir);

        }

        private void AddStreamEntry(string entryName, MemoryStream ms, ZipOutputStream zos)
        {
            ZipEntry ze = new ZipEntry(entryName);
            ze.DateTime = DateTime.Now;
            zos.PutNextEntry(ze);
            StreamUtils.Copy(ms, zos, new byte[ms.Length]);
            zos.CloseEntry();
        }

        private void AddStringEntry(string entryName, string text, ZipOutputStream zos)
        {
            byte[] bytes = UTF8Encoding.Default.GetBytes(text);
            MemoryStream ms = new MemoryStream(bytes);
            AddStreamEntry(entryName, ms, zos);
        }

        private void AddImageEntry(string entryName, Image image, ImageFormat imageFormat, ZipOutputStream zos)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, imageFormat);
            ms = new MemoryStream(ms.ToArray());
            AddStreamEntry(entryName, ms, zos);
        }

        private void generateMaterialFrom(string filePath)
        {
            string fileDir = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string materialName = Path.GetFileNameWithoutExtension(fileName);
            string filePathWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string materialFilePath = fileDir + "\\" + filePathWithoutExtension + ".skm";

            MemoryStream references_xml_ms = new MemoryStream(UTF8Encoding.Default.GetBytes(references_xml));
            
            FileStream fs = new FileStream(materialFilePath, FileMode.Create);
            ZipOutputStream zos = new ZipOutputStream(fs);
            zos.UseZip64 = UseZip64.Off;
            zos.SetLevel(0);
            
            Image srcImg = Image.FromFile(filePath);
          
            AddStringEntry(REFERENCES_XML_NAME, references_xml, zos);
            AddStringEntry(DOCUMENT_XML_NAME, Get_DocumentXml(materialName, filePath, ((double)srcImg.Width/(double)srcImg.Height)), zos);
            AddStringEntry(DOCUMENT_PROPERTIES_XML_NAME, Get_DocumentPropertiesXml(materialName), zos);
            AddImageEntry("ref\\" + fileName, srcImg, GetImageFormatFromFileName(fileName), zos);
            AddImageEntry(DOC_THUMBNAIL_PNG_NAME, srcImg, ImageFormat.Png, zos);

            zos.Finish();
            zos.Close();
            fs.Close();
            rtxt.AppendText(materialFilePath + " [DONE]\n");
        }

        private string getDimensions(string dir)
        {
            string res = "";
            // Get all files from sourcefolder, including subfolders.
            string[] sourceFiles = GetImageFiles(dir);
            foreach (string file in sourceFiles)
            {
                using (Stream stream = File.OpenRead(file))
                {
                    using (Image sourceImage = Image.FromStream(stream, false, false))
                    {
                        int w = sourceImage.Width;
                        int h = sourceImage.Height;
                        double r = (double)w / (double)h;
                        res += file + " W:" + w + ", H:" + h + ", ratio:" + r + "\n";
                    }
                }
            }
            return res;
        }

        private void generate(string dir)
        {
            // Get all files from sourcefolder, including subfolders.
            string[] sourceFiles = GetImageFiles(dir);
            foreach (string file in sourceFiles)
            {
                generateMaterialFrom(file);
                
            }
        }
    }

    public static class StringExt
    {
        public static bool EndsWith(this string ts, params string[] any)
        {
            for (int i = 0; i < any.Length; i++)
            {
                if (ts.EndsWith(any[i])) return true;
            }
            return false;
        }
    }
}
