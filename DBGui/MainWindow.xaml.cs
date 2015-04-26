using System.Windows;
using System.Windows.Forms;
using DB;

namespace DBGui
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private async void ImportMenu_Click( object sender, RoutedEventArgs e )
        {
            var dialog = new FolderBrowserDialog();
            if ( dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                await DatabaseImport.ImportFromDirectoryAsync( dialog.SelectedPath );
            }
        }
    }
}