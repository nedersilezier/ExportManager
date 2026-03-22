using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace ExportManager.Behaviors
{
    public class ClosingBehavior: Behavior<Window>
    {
        public ICommand ClosingCommand
        {
            get => (ICommand)GetValue(ClosingCommandProperty);
            set => SetValue(ClosingCommandProperty, value);
        }

        public static readonly DependencyProperty ClosingCommandProperty =
            DependencyProperty.Register(nameof(ClosingCommand), typeof(ICommand), typeof(ClosingBehavior));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Closing += OnWindowClosing;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Closing -= OnWindowClosing;
            base.OnDetaching();
        }

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            if (ClosingCommand?.CanExecute(e) == true)
            {
                ClosingCommand.Execute(e);
            }
        }
    }
}
