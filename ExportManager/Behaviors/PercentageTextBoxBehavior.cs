using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ExportManager.Behaviors
{
    public class PercentageTextBoxBehavior
    {
        public static bool GetIsPercentage(DependencyObject obj) => (bool)obj.GetValue(IsPercentageProperty);
        public static void SetIsPercentage(DependencyObject obj, bool value) => obj.SetValue(IsPercentageProperty, value);

        public static readonly DependencyProperty IsPercentageProperty =
            DependencyProperty.RegisterAttached("IsPercentage", typeof(bool), typeof(PercentageTextBoxBehavior),
                new PropertyMetadata(false, OnChanged));

        private static void OnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb)
            {
                tb.PreviewTextInput += (s, ev) =>
                {
                    var textBox = (TextBox)s;
                    string newText = textBox.Text.Insert(textBox.CaretIndex, ev.Text);
                    newText = newText.Replace("%", "");
                    if (!int.TryParse(newText, out int value) || value < 0 || value > 100)
                    {
                        ev.Handled = true;
                    }
                };
                DataObject.AddPastingHandler(tb, (s, ev) =>
                {
                    if (ev.DataObject.GetDataPresent(typeof(string)))
                    {
                        string pasteText = (string)ev.DataObject.GetData(typeof(string));
                        var textBox = (TextBox)s;
                        string newText = textBox.Text.Insert(textBox.CaretIndex, pasteText);
                        newText = newText.Replace("%", "");
                        if (!int.TryParse(newText, out int value) || value < 0 || value > 100)
                            ev.CancelCommand();
                    }
                    else ev.CancelCommand();
                });
            }
        }
    }
}
