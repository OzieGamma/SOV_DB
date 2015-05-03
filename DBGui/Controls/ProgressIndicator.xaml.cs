using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DBGui.Controls
{
    public partial class ProgressIndicator
    {
        public ProgressIndicator()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            HorizontalContentAlignment = HorizontalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Stretch;

            Grid.SetColumnSpan( this, 100 );
            Grid.SetRowSpan( this, 100 );
            Margin = new Thickness( 0 );
            Padding = new Thickness( 0 );
            Background = Brushes.White;

            InitializeComponent();
        }
    }
}