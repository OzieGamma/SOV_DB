using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DBGui.Controls
{
    public abstract class WindowBase : Window, INotifyPropertyChanged
    {
        private readonly ProgressIndicator _progressIndicator;
        private bool _loaded;

        public WindowBase()
        {
            Width = 800;
            Height = 800;
            Title = "Database project";

            DataContext = this;
            this.GetType().GetMethod( "InitializeComponent" ).Invoke( this, null );

            _progressIndicator = new ProgressIndicator();
            _progressIndicator.Visibility = Visibility.Collapsed;
            ( (Grid) Content ).Children.Add( _progressIndicator );
        }

        protected override void OnActivated( EventArgs e )
        {
            base.OnActivated( e );

            if ( !_loaded )
            {
                _loaded = true;
                Load();
            }
        }

        protected virtual void Load() { }

        protected async void DoAsync( Func<Task> action )
        {
            _progressIndicator.Visibility = Visibility.Visible;
            try
            {
                await action();
            }
            finally
            {
                _progressIndicator.Visibility = Visibility.Collapsed;
            }
        }

        #region INotifyPropertyChanged
        protected void Set<T>( ref T field, T value, [CallerMemberName] string propertyName = "" )
        {
            if ( !object.ReferenceEquals( field, value ) )
            {
                field = value;
                OnPropertyChanged( propertyName );
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged( string propertyName )
        {
            var evt = PropertyChanged;
            if ( evt != null )
            {
                evt( this, new PropertyChangedEventArgs( propertyName ) );
            }
        }
        #endregion
    }
}