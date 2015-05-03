using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DBGui.Models;

namespace DBGui.Controls
{
    public partial class ProductionInfoView
    {
        public IList<ProductionInfo> Items
        {
            get { return (IList<ProductionInfo>) GetValue( ItemsProperty ); }
            set { SetValue( ItemsProperty, value ); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register( "Items", typeof( IList<ProductionInfo> ), typeof( ProductionInfoView ), new PropertyMetadata( null ) );

        public ProductionInfoView()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void Item_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            var info = (ProductionInfo) ( (ListViewItem) sender ).DataContext;
            new ProductionWindow( info.Id ).Show();
        }
    }
}