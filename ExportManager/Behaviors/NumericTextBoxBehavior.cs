using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExportManager.Behaviors
{
    public class NumericTextBoxBehavior
    {
        public static bool GetIsPositiveInteger(DependencyObject obj) => (bool)obj.GetValue(IsPositiveIntegerProperty);
        public static void SetIsPositiveInteger(DependencyObject obj, bool value) => obj.SetValue(IsPositiveIntegerProperty, value);

        public static readonly DependencyProperty IsPositiveIntegerProperty =
            DependencyProperty.RegisterAttached("IsPositiveInteger", typeof(bool), typeof(NumericTextBoxBehavior),
                new PropertyMetadata(false, OnChanged));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb)
            {
                tb.PreviewTextInput += (s, ev) => ev.Handled = !ev.Text.All(char.IsDigit);
                DataObject.AddPastingHandler(tb, (s, ev) =>
                {
                    if (ev.DataObject.GetDataPresent(typeof(string)))
                    {
                        string text = (string)ev.DataObject.GetData(typeof(string));
                        if (!text.All(char.IsDigit)) ev.CancelCommand();
                    }
                    else ev.CancelCommand();
                });
            }
        }
    }
}
