using System;
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
        public MainForm()
        {
            InitializeComponent();
            generatorInit();
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            string dir = "";
            if (!SelectFolder(out dir)) return;
            rtxt.Clear();
            resizeImages = chkUseMaxDimension.Checked;
            resizeImageMaxDimension = Convert.ToInt32(txtMaxImageDimension.Text);

            bgw.RunWorkerAsync(dir);
            //generate(dir);
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


        // generator stuff


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

        private string Get_DocumentXml(string materialName, string originalFilePath, double ratio, double scale)
        {
            string textureFileName = Path.GetFileName(originalFilePath);
            string xScale = (ratio * scale).ToString().Replace(",", ".");
            string yScale = scale.ToString().Replace(",", ".");
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><materialDocument xmlns=\"http://sketchup.google.com/schemas/sketchup/1.0/material\" xmlns:mat=\"http://sketchup.google.com/schemas/sketchup/1.0/material\" xmlns:r=\"http://sketchup.google.com/schemas/1.0/references\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://sketchup.google.com/schemas/sketchup/1.0/material http://sketchup.google.com/schemas/sketchup/1.0/material.xsd\">"
                + "<mat:material name=\"" + materialName 
                + "\" type=\"1\" colorRed=\"255\" colorGreen=\"255\" colorBlue=\"255\" colorizeType=\"0\" trans=\"0.5\" useTrans=\"0\" hasTexture=\"1\">"
                + "<mat:texture textureFilename=\"" + textureFileName //originalFilePath
                + "\" xScale=\""+xScale+"\" yScale=\""+yScale+ "\" avgColor=\"4294967295\"><mat:images>"
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

        BackgroundWorker bgw;
        bool resizeImages = false;
        int resizeImageMaxDimension = 2048;
        private void generatorInit()
        {
            bgw = new BackgroundWorker();
            bgw.WorkerReportsProgress = true;
            bgw.DoWork += bgw_DoWork;
            bgw.ProgressChanged += bgw_ProgressChanged;

        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState == null)
            {
                progressBar.Maximum = e.ProgressPercentage;
                progressBar.Value = 0;
                rtxt.Clear();
            }
            else
            {
                progressBar.Value = e.ProgressPercentage;
                rtxt.AppendText((string)e.UserState);
            }
        }

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            generate((string)e.Argument);
        }

        private void generate(string dir)
        {
            string[] files = GetImageFiles(dir);
            bgw.ReportProgress(files.Length, null);
            for (int i=0; i<files.Length;i++)
            {
                bgw.ReportProgress(i, files[i]);
                string res = generateMaterialFrom(files[i]);
                bgw.ReportProgress(i, " -> " + res + "\n");
            }
            bgw.ReportProgress(files.Length, "[DONE]");
        }

        private string generateMaterialFrom(string filePath)
        {
            string fileDir = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileName(filePath);
            string materialName = Path.GetFileNameWithoutExtension(fileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            string materialFileName = fileNameWithoutExtension + ".skm";
            string materialFilePath = fileDir + "\\" + materialFileName;

            MemoryStream references_xml_ms = new MemoryStream(UTF8Encoding.Default.GetBytes(references_xml));
            
            FileStream fs = new FileStream(materialFilePath, FileMode.Create);
            ZipOutputStream zos = new ZipOutputStream(fs);
            zos.UseZip64 = UseZip64.Off;
            zos.SetLevel(9);
            
            Image srcImg = Image.FromFile(filePath);
          
            zos.AddStringEntry(REFERENCES_XML_NAME, references_xml);
            zos.AddStringEntry(DOCUMENT_XML_NAME, Get_DocumentXml(materialName, filePath, ((double)srcImg.Width/(double)srcImg.Height)));
            zos.AddStringEntry(DOCUMENT_PROPERTIES_XML_NAME, Get_DocumentPropertiesXml(materialName));
            if (resizeImages == false)
                zos.AddImageEntry("ref\\" + fileName, srcImg, GetImageFormatFromFileName(fileName));
            else
                zos.AddImageEntry("ref\\" + fileName, srcImg.ResizeImage(resizeImageMaxDimension, false), GetImageFormatFromFileName(fileName));
            zos.AddImageEntry(DOC_THUMBNAIL_PNG_NAME, srcImg.ResizeImage(256, true).ChangeFormat(PixelFormat.Format16bppRgb565), ImageFormat.Png);

            zos.Finish();
            zos.Close();
            fs.Close();
            return materialFileName;
        }

        private string getDimensionsFast(string dir)
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
    public static class ZipExt
    {
        public static void AddStreamEntry(this ZipOutputStream zos, string entryName, MemoryStream ms)
        {
            ZipEntry ze = new ZipEntry(entryName);
            ze.DateTime = DateTime.Now;
            zos.PutNextEntry(ze);
            ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(ms, zos, new byte[ms.Length]);
            zos.CloseEntry();
        }

        public static void AddStringEntry(this ZipOutputStream zos, string entryName, string text)
        {
            byte[] bytes = UTF8Encoding.Default.GetBytes(text);
            MemoryStream ms = new MemoryStream(bytes);
            zos.AddStreamEntry(entryName, ms);
        }

        public static void AddImageEntry(this ZipOutputStream zos, string entryName, Image image, ImageFormat imageFormat)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, imageFormat);
            ms = new MemoryStream(ms.ToArray());
            zos.AddStreamEntry(entryName, ms);
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
    public static class ImageExt
    {
        public static Image ChangeFormat(this Image image, PixelFormat pixelFormat)
        {
            var destImage = new Bitmap(image.Width,image.Height, pixelFormat);
            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.DrawImage(image, 0, 0);
            }
            return (Image)destImage;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Image ResizeImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return (Image)destImage;
        }

        public static Image ResizeImage(this Image image, int maxDimension, bool enlarge)
        {
            if ((enlarge == false) && (image.Width < maxDimension) && (image.Height < maxDimension))
                return image; // no changes
            int newWidth = image.Width;
            int newHeight = image.Height;

            if (image.Width > image.Height) // landscape
            {
                // example: w512 h256  -> 256/512=0.5  -> 0.5*maxDimension
                newWidth = maxDimension;
                newHeight = (image.Height * maxDimension) / image.Width;
            }
            else if (image.Width < image.Height) // portrait
            {
                newWidth = (image.Width * maxDimension) / image.Height;
                newHeight = maxDimension;
            }
            else // (image.Width == image.Height) 1:1 ratio
            {
                if (image.Width == maxDimension)
                    return image; // no changes

                newWidth = maxDimension;
                newHeight = maxDimension;
            }

            return image.ResizeImage(newWidth, newHeight);
        }
    }
}
