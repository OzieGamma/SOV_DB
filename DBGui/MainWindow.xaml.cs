using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using DB;
using WinForms = System.Windows.Forms;

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
            var dialog = new WinForms.FolderBrowserDialog();
            if ( dialog.ShowDialog() == WinForms.DialogResult.OK )
            {
                await Database.ImportFromDirectoryAsync( dialog.SelectedPath );
            }
        }


        private async void PeopleSearchButton_Click( object sender, RoutedEventArgs e )
        {
            var table = await Database.ExecuteQueryAsync(
@"SELECT Id, FirstName, LastName, Gender, Trivia, Quotes, BirthDate, DeathDate, BirthName, ShortBio, SpouseInfo, Height
  FROM Person
  WHERE FirstName LIKE '%" + PeopleSearchBox.Text + "%' OR LastName LIKE '%" + PeopleSearchBox.Text + "%';" );
            PeopleView.ItemsSource = table.DefaultView;

        }

        private async void PeopleView_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            if ( e.AddedItems.Count != 1 )
            {
                return;
            }
            int personId = (int) ( (DataRowView) e.AddedItems[0] ).Row.ItemArray[0];
            var table = await Database.ExecuteQueryAsync(
@"SELECT Production.Id, Production.Title, ProductionCast.CastRole
  FROM Production JOIN ProductionCast ON Production.Id = ProductionCast.ProductionId
  WHERE ProductionCast.PersonId = " + personId + ";" );
            PersonCharactersView.ItemsSource = table.DefaultView;

            var table2 = await Database.ExecuteQueryAsync(
@"SELECT Name
  FROM AlternativePersonName
  WHERE PersonId = " + personId + ";" );
            PersonNamesView.ItemsSource = table2.DefaultView;
        }

        private async void CharacterSearchButton_Click( object sender, RoutedEventArgs e )
        {
            int productionId = (int) ( (DataRowView) ( (Button) sender ).DataContext ).Row.ItemArray[0];
            Tabs.SelectedItem = ProductionsTab;
            var table = await Database.ExecuteQueryAsync(
@"SELECT Id, Title, ReleaseYear, Genre
  FROM Production
  WHERE Id = " + productionId + ";" );
            ProductionsView.ItemsSource = table.DefaultView;
        }


        private async void ProductionsSearchButton_Click( object sender, RoutedEventArgs e )
        {
            var table = await Database.ExecuteQueryAsync(
@"SELECT Id, Title, ReleaseYear, Genre
  FROM Production
  WHERE Title LIKE '%" + ProductionsSearchBox.Text + "%';" );
            ProductionsView.ItemsSource = table.DefaultView;
        }


        private async void RawQueryButton_Click( object sender, RoutedEventArgs e )
        {
            try
            {
                var table = await Database.ExecuteQueryAsync( RawQueryBox.Text );
                RawQueryResultsGrid.ItemsSource = table.DefaultView;
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.StackTrace );
            }
        }
    }
}