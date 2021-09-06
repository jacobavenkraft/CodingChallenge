using System.Windows;
using System.Windows.Input;

namespace CodingChallenge.Framework.AttachedBehaviors
{
    public static class WindowAttachedBehaviors
    {
        public static DependencyProperty ClosedCommandProperty =
            DependencyProperty.RegisterAttached(
                "ClosedCommand",
                typeof(ICommand),
                typeof(WindowAttachedBehaviors),
                new PropertyMetadata(null, OnClosedCommandChanged));

        private static void OnClosedCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is Window window
                && args.NewValue is ICommand argsCommand)
            {
                window.Closed += (o, e) =>
                {
                    argsCommand.Execute(null);
                };
            }
        }

        public static ICommand GetClosedCommand(DependencyObject dependencyObject)
        {
            return (ICommand)dependencyObject.GetValue(ClosedCommandProperty);
        }

        public static void SetClosedCommand(DependencyObject dependencyObject, ICommand command)
        {
            dependencyObject.SetValue(ClosedCommandProperty, command);
        }
    }
}
