using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DBGui.Controls
{
    // from http://stackoverflow.com/questions/4793030/wpf-reset-listbox-scroll-position-when-itemssource-changes
    public static class ScrollToTop
    {
        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached
            (
                "Enabled",
                typeof( bool ),
                typeof( ScrollToTop ),
                new UIPropertyMetadata( false, OnEnabledPropertyChanged )
            );

        public static bool GetEnabled( DependencyObject obj )
        {
            return (bool) obj.GetValue( EnabledProperty );
        }

        public static void SetEnabled( DependencyObject obj, bool value )
        {
            obj.SetValue( EnabledProperty, value );
        }

        private static void OnEnabledPropertyChanged( DependencyObject dpo, DependencyPropertyChangedEventArgs e )
        {
            ItemsControl itemsControl = dpo as ItemsControl;
            if ( itemsControl != null )
            {
                DependencyPropertyDescriptor dependencyPropertyDescriptor =
                        DependencyPropertyDescriptor.FromProperty( ItemsControl.ItemsSourceProperty, typeof( ItemsControl ) );
                if ( dependencyPropertyDescriptor != null )
                {
                    if ( (bool) e.NewValue == true )
                    {
                        dependencyPropertyDescriptor.AddValueChanged( itemsControl, ItemsSourceChanged );
                    }
                    else
                    {
                        dependencyPropertyDescriptor.RemoveValueChanged( itemsControl, ItemsSourceChanged );
                    }
                }
            }
        }

        private static void ItemsSourceChanged( object sender, EventArgs e )
        {
            ItemsControl itemsControl = sender as ItemsControl;
            EventHandler eventHandler = null;
            eventHandler = new EventHandler( delegate
            {
                if ( itemsControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated )
                {
                    ScrollViewer scrollViewer = GetVisualChild<ScrollViewer>( itemsControl ) as ScrollViewer;
                    scrollViewer.ScrollToTop();
                    itemsControl.ItemContainerGenerator.StatusChanged -= eventHandler;
                }
            } );
            itemsControl.ItemContainerGenerator.StatusChanged += eventHandler;
        }

        private static T GetVisualChild<T>( DependencyObject parent ) where T : Visual
        {
            T child = default( T );
            int numVisuals = VisualTreeHelper.GetChildrenCount( parent );
            for ( int i = 0; i < numVisuals; i++ )
            {
                Visual v = (Visual) VisualTreeHelper.GetChild( parent, i );
                child = v as T;
                if ( child == null )
                {
                    child = GetVisualChild<T>( v );
                }
                if ( child != null )
                {
                    break;
                }
            }
            return child;
        }
    }
}
