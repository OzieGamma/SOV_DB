using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DBGui.Controls
{
    public partial class TextInput : UserControl
    {
        public string ButtonText
        {
            get { return (string) GetValue( ButtonTextProperty ); }
            set { SetValue( ButtonTextProperty, value ); }
        }

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register( "ButtonText", typeof( string ), typeof( TextInput ), new PropertyMetadata( null ) );


        public bool IsMultiline
        {
            get { return (bool) GetValue( IsMultilineProperty ); }
            set { SetValue( IsMultilineProperty, value ); }
        }

        public static readonly DependencyProperty IsMultilineProperty =
            DependencyProperty.Register( "IsMultiline", typeof( bool ), typeof( TextInput ), new PropertyMetadata( false ) );


        public TextInput()
        {
            DataContext = this;
            InitializeComponent();
        }

        public event Action<string> Executed;

        private void Button_Click( object sender, RoutedEventArgs e )
        {
            Execute();
        }

        private void Box_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.Key == Key.Enter )
            {
                Execute();
            }
        }

        private void Execute()
        {
            if ( !string.IsNullOrWhiteSpace( Box.Text ) )
            {
                var evt = Executed;
                if ( evt != null )
                {
                    evt( Box.Text.Trim() );
                }
            }
        }
    }
}