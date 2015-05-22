using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace DBGui.Controls
{
    public abstract class BaseConverter<T, TFrom, TTo> : MarkupExtension, IValueConverter
        where T : BaseConverter<T, TFrom, TTo>, new()
    {
        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            return new T();
        }

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return Convert( (TFrom) value );
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            return ConvertBack( (TTo) value );
        }

        protected abstract TTo Convert( TFrom value );

        protected virtual TFrom ConvertBack( TTo value )
        {
            throw new NotSupportedException();
        }
    }

    public sealed class StringJoiner : BaseConverter<StringJoiner, IEnumerable<string>, string>
    {
        protected override string Convert( IEnumerable<string> value )
        {
            return value == null ? null : ( value.GetEnumerator().MoveNext() ? string.Join( "; ", value ) : null );
        }
    }
}