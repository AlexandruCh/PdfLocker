using iTextSharp.text.pdf;
using Meridian.Helper;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PdfLocker.Utils
{
    public class PdfUtils
    {
        static int ProcessedCounter { get; set; }
        static int ErrCounter { get; set; }
        static readonly Logger _log = LogManager.GetCurrentClassLogger();

        public PdfUtils()
        {
            //Initialize Properties
            ErrCounter = 0;
            ProcessedCounter = 0;
        }

        /// <summary>
        /// Array of Pdf file paths
        /// </summary>
        private string[] PdfList
        {
            get
            {
                var folderPath = FileUtils.GetExecutingDirectoryPath();
                string[] files = Directory.GetFiles(folderPath, "*.pdf",
                                        SearchOption.AllDirectories);

                return files;
            }
        }

        /// <summary>
        /// Make pdf customization run on separate async threads
        /// </summary>
        /// <returns></returns>
        private async Task CustomizePDFParallelAsync()
        {
            List<Task> tasks = new List<Task>();

            //Disable print button for each pdf in pdf list
            PdfList.ToList()
                .ForEach(filePath =>
                {
                    tasks.Add(Task.Run(() => CustomizePdf(filePath)));
                });

            await Task.WhenAll(tasks);

        }

        /// <summary>
        /// CustomizePdf - Disable Print button and add metadata
        /// </summary>
        /// <param name="pdfIn"></param>
        private void CustomizePdf(string pdfIn)
        {
            try
            {
                var fileName = new FileInfo(pdfIn).Name.MakeUnique();
                var pdfOut = Path.Combine(Path.Combine(Path.GetTempPath(), "PdfStorage"), fileName);
                var reader = new PdfReader(pdfIn, null);
                var xmlPath = Path.Combine(Path.GetDirectoryName(pdfIn), Path.GetFileNameWithoutExtension(pdfIn) + ".xml");

                using (FileStream fileStream = new FileStream(pdfOut, FileMode.Create))
                {
                    var stamper = new PdfStamper(reader, fileStream);
                    stamper.SetEncryption(null, null, PdfWriter.HideWindowUI, PdfWriter.STRENGTH40BITS);

                    stamper.MoreInfo = new XmlUtils(xmlPath).XmlProps;
                    stamper.Close();
                }
                reader.Close();

                //Replace input file with output one
                File.Delete(pdfIn);
                File.Move(pdfOut, pdfIn);

                ProcessedCounter++;
                _log.Info("Disabled print button for pdf file located at: " + pdfIn);
            }
            catch (Exception exp)
            {
                if (exp.HResult == -2146232800)
                    _log.Warn("Pdf file: " + pdfIn + " already processed!" + Environment.NewLine);
                else
                    _log.Error(exp.Message + Environment.NewLine);

                ErrCounter++;
            }

        }

        /// <summary>
        /// Run async job to disable pdf print button and add metadata
        /// </summary>
        /// <returns></returns>
        public async Task StartCustomization()
        {
            Stopwatch s = new Stopwatch();
            s.Start();
            await CustomizePDFParallelAsync();
            s.Stop();

            //Send alert when disable job finished
            string alertMsg = "Job finished in: " + s.Elapsed.TotalSeconds + " seconds"
                + Environment.NewLine + "Files succesfully processed: " + ProcessedCounter
                + Environment.NewLine + "Errors found: " + ErrCounter
                + Environment.NewLine + "Please check log at path: " + ConfigManager.LogPath;

            MessageBox.Show(alertMsg);
        }
    }
}
