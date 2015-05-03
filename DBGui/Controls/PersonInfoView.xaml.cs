using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DBGui.Models;

namespace DBGui.Controls
{
    public sealed partial class PersonInfoView : UserControl
    {
        public IList<PersonInfo> Items
        {
            get { return (IList<PersonInfo>) GetValue( ItemsProperty ); }
            set { SetValue( ItemsProperty, value ); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register( "Items", typeof( IList<PersonInfo> ), typeof( PersonInfoView ), new PropertyMetadata( null ) );

        public PersonInfoView()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void Person_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            var info = (PersonInfo) ( (ListViewItem) sender ).DataContext;
            new PersonWindow( info.Id ).Show();
        }
    }
}