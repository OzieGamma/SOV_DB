namespace DBGui
{
    using System;
    using System.Windows.Forms;
    using System.Windows.Input;

    using DB;

    public class ImportCsvCommand : ICommand
    {
        private bool importing;

        public bool Importing
        {
            get
            {
                return this.importing;
            }
            set
            {
                this.importing = value;
                if (this.CanExecuteChanged != null)
                {
                    this.CanExecuteChanged(null, null);
                }
            }
        }

        public bool CanExecute(object obj)
        {
            return !this.Importing;
        }

        public void Execute(object obj)
        {
            this.Importing = true;

            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string directory = dialog.SelectedPath;
                

                // Launch import
                new Import(directory, new ConsoleOutput()).FromCsv().Wait();

                this.Importing = false;
            }

            else
            {
                this.Importing = false;
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
