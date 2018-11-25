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
using Syncfusion.Presentation;
using System.Diagnostics;
using System.IO;
using Syncfusion.Windows.Shared;

namespace Cloning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChromelessWindow
    {
        Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();

        public MainWindow()
        {
            InitializeComponent();			
            ImageSourceConverter img = new ImageSourceConverter();
            string file = @"..\..\..\..\..\..\..\Common\Images\Presentation\ppt_header.png";
            this.image1.Source = (ImageSource)img.ConvertFromString(file);
            this.Icon = (ImageSource)img.ConvertFromString(@"..\..\..\..\..\..\..\Common\Images\Presentation\App.ico");
            this.txtFile.Text = "Template.pptx";
            this.txtFile.Tag = @"..\..\..\..\..\..\..\Common\Data\Presentation\Template.pptx";
            this.destTextBox.Text = "Essential Presentation.pptx";
            this.destTextBox.Tag = @"..\..\..\..\..\..\..\Common\Data\Presentation\Essential Presentation.pptx";
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog1.InitialDirectory = Path.GetFullPath(@"..\..\..\..\..\..\..\Common\Data\Presentation\");
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "PowerPoint Presentations|*.pptx";
            Nullable<bool> result = openFileDialog1.ShowDialog();

            //Get the selected file name and display in a TextBox
            if (result == true)
            {
                this.txtFile.Text = openFileDialog1.SafeFileName;
                this.txtFile.Tag = openFileDialog1.FileName;
            }
        }


        private void destButton_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog1.InitialDirectory = Path.GetFullPath(@"..\..\..\..\..\..\..\Common\Data\Presentation\");
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "PowerPoint Presentations|*.pptx";
            Nullable<bool> result = openFileDialog1.ShowDialog();

            //Get the selected file name and display in a TextBox
            if (result == true)
            {
                this.destTextBox.Text = openFileDialog1.SafeFileName;
                this.destTextBox.Tag = openFileDialog1.FileName;
            }
        }

        private void btnGeneateDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtFile.Text))
                {
                    MessageBox.Show("Please browse the files to generate document", "Input missing", MessageBoxButton.OK);
                }
                else
                {
                    if (this.radioDestination.IsChecked == true)
                    {
                        //Creates presentation instance for source
                        IPresentation sourcePresentation = Presentation.Open(txtFile.Tag.ToString());

                        //Clones the first slide of the presentation
                        ISlide slide = sourcePresentation.Slides[0].Clone();

                        //Creates instance for the destination presentation
                        IPresentation destinationPresentation = Presentation.Open(destTextBox.Tag.ToString());

                        //Adding the cloned slide to the destination presentation by using Destination option.
                        destinationPresentation.Slides.Add(slide, PasteOptions.UseDestinationTheme, sourcePresentation);

                        //Closing the Source presentation
                        sourcePresentation.Close();

                        //Saving the Destination presentaiton.
                        destinationPresentation.Save("ClonedUsingDestination.pptx");

                        //Closing the destination presentation
                        destinationPresentation.Close();

                        if (MessageBox.Show("First slide is cloned and added as last slide to the Presentation,Do you want to view the resultant Presentation?", "Finished Cloning",
                                  MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            System.Diagnostics.Process.Start("ClonedUsingDestination.pptx");

                        }
                    }
                    else
                    {
                        //Creates instance for the source presentation
                        IPresentation sourcePresentation = Presentation.Open(txtFile.Tag.ToString());

                        //Clones the first slide of the presentation
                        ISlide slide = sourcePresentation.Slides[0].Clone();

                        //Creates instance for the destination presentation
                        IPresentation destinationPresentation = Presentation.Open(destTextBox.Tag.ToString());

                        //Adding the cloned slide to the destination presentation by using Destination option.
                        destinationPresentation.Slides.Add(slide, PasteOptions.SourceFormatting, sourcePresentation);

                        //Closing the Source presentation
                        sourcePresentation.Close();

                        //Saving the Destination presentaiton.
                        destinationPresentation.Save("ClonedUsingSource.pptx");

                        //Closing the destinatin presentation
                        destinationPresentation.Close();

                        if (MessageBox.Show("First slide is cloned and added as last slide to the Presentation,Do you want to view the resultant Presentation?", "Finished Cloning",
                                  MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            System.Diagnostics.Process.Start("ClonedUsingSource.pptx");

                        }
                    }
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Please close the generated Presentation", "File is open", MessageBoxButton.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("This file could not be cloned , please contact Syncfusion Direct-Trac system at http://www.syncfusion.com/support/default.aspx for any queries. ", "OOPS..Sorry!",
                        MessageBoxButton.OK);
                this.Close();
            }
        }		    
    }
}





