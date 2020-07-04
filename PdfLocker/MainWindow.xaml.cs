using Meridian.Utils;
using PdfLocker.Utils;
using System.Threading.Tasks;
using System.Windows;

namespace PdfLocker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Main Entry Point
        /// </summary>
        public MainWindow()
        {
            //Delete content of Tmp folder
            new InitialSetup();
            InitializeComponent();
            
            //Start Pdf Customization
            Task task = Task.Run(async () => await new PdfUtils().StartCustomization());
            task.Wait();
            
            //End Job
            App.Current.MainWindow.Close();
        }


    }
}
