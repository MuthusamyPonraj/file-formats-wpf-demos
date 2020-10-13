#region Copyright Syncfusion Inc. 2001-2020.
// Copyright Syncfusion Inc. 2001-2020. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using Syncfusion.Windows.Shared;
using Syncfusion.Pdf.Interactive;
using syncfusion.demoscommon.wpf;

namespace syncfusion.pdfdemos.wpf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PDFConformance : DemoControl
    {
        # region Constructor
        /// <summary>
        /// Window constructor
        /// </summary>
        public PDFConformance()
        {
            InitializeComponent();

        }
        #endregion
        #region Dispose
        protected override void Dispose(bool disposing)
        {
            //Release all resources
            base.Dispose(disposing);
        }
        # endregion
        # region Events
        /// <summary>
        /// Creates PDF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PdfDocument document = null;
            int conformanceValue = this.comboBox.SelectedIndex;
            switch (conformanceValue)
            {
                case 0:
                    document = new PdfDocument(PdfConformanceLevel.Pdf_A1A);
                    break;

                case 1:
                    document = new PdfDocument(PdfConformanceLevel.Pdf_A1B);
                    break;

                case 2:
                    document = new PdfDocument(PdfConformanceLevel.Pdf_A2A);
                    break;

                case 3:
                    document = new PdfDocument(PdfConformanceLevel.Pdf_A2B);
                    break;

                case 4:
                    document = new PdfDocument(PdfConformanceLevel.Pdf_A2U);
                    break;

                case 5:
                case 6:
                case 7:
                    {
                        if (conformanceValue == 5)
                            document = new PdfDocument(PdfConformanceLevel.Pdf_A3A);
                        else if (conformanceValue == 6)
                            document = new PdfDocument(PdfConformanceLevel.Pdf_A3B);
                        else
                            document = new PdfDocument(PdfConformanceLevel.Pdf_A3U);

                        PdfAttachment attachment = new PdfAttachment(@"Assets\PDF\PDF_A.rtf");
                        attachment.Relationship = PdfAttachmentRelationship.Alternative;
                        attachment.ModificationDate = DateTime.Now;
                        attachment.Description = "PDF_A";
                        attachment.MimeType = "application/xml";
                        document.Attachments.Add(attachment);
                        break;
                    }
                case 8:
                    document = new PdfDocument(PdfConformanceLevel.Pdf_X1A2001);
                    break;
            }
            //Add a page
            PdfPage page = document.Pages.Add();

            //Create Pdf graphics for the page
            PdfGraphics g = page.Graphics;
            FileStream imageStream = new FileStream(@"Assets\PDF\AdventureCycle.jpg", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            PdfFont font = new PdfTrueTypeFont(new Font("Arial", 14));
            //Load the image from the disk.
            PdfImage img = PdfImage.FromStream(imageStream);
            //Draw the image in the specified location and size.
            g.DrawImage(img, new RectangleF(150, 30, 200, 100));

            PdfTextElement textElement = new PdfTextElement("Adventure Works Cycles, the fictitious company on which the AdventureWorks sample databases are based," +
                               " is a large, multinational manufacturing company. The company manufactures and sells metal and composite bicycles to North American, " +
                               "European and Asian commercial markets. While its base operation is located in Bothell, Washington with 290 employees, several regional" +
                               " sales teams are located throughout their market base.")
            {
                Font = font
            };
            PdfLayoutResult layoutResult = textElement.Draw(page, new RectangleF(0, 150, page.GetClientSize().Width, page.GetClientSize().Height));

            document.Save("PDFConformance.pdf");

            //Message box confirmation to view the created PDF document.
            if (MessageBox.Show("Do you want to view the PDF file?", "PDF File Created",
                MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                //Launching the PDF file using the default Application.[Acrobat Reader]
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo = new System.Diagnostics.ProcessStartInfo("PDFConformance.pdf") { UseShellExecute = true };
                process.Start();

            }

        }
        # endregion
    }
}
