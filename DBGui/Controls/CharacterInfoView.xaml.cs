using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DBGui.Models;

namespace DBGui.Controls
{
    public sealed partial class CharacterInfoView
    {
        public IList<CharacterInfo> Items
        {
            get { return (IList<CharacterInfo>) GetValue( ItemsProperty ); }
            set { SetValue( ItemsProperty, value ); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register( "Items", typeof( IList<CharacterInfo> ), typeof( CharacterInfoView ), new PropertyMetadata( null ) );

        public CharacterInfoView()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void Item_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            var info = (CharacterInfo) ( (ListViewItem) sender ).DataContext;
            new CharacterWindow( info.Id ).Show();
        }
    }
}