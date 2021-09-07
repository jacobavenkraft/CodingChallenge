using CodingChallenge.Interfaces;
using System.Windows;
using System.Windows.Input;

namespace CodingChallenge.Framework.AttachedBehaviors
{
    public static class FrameworkElementAttachedBehaviors
    {
        public static DependencyProperty LoadedCommandProperty =
            DependencyProperty.RegisterAttached(
                "LoadedCommand",
                typeof(ICommand),
                typeof(FrameworkElementAttachedBehaviors),
                new PropertyMetadata(null, OnLoadedCommandChanged));

        private static void OnLoadedCommandChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is FrameworkElement frameworkElement
                && args.NewValue is ICommand argsCommand)
            {
                frameworkElement.Loaded += (o, e) =>
                {
                    argsCommand.Execute(null);
                };
            }
        }

        public static ICommand GetLoadedCommand(DependencyObject dependencyObject)
        {
            return (ICommand)dependencyObject.GetValue(LoadedCommandProperty);
        }

        public static void SetLoadedCommand(DependencyObject dependencyObject, ICommand command)
        {
            dependencyObject.SetValue(LoadedCommandProperty, command);
        }

        public static DependencyProperty SizeChangeListenerProperty =
            DependencyProperty.RegisterAttached(
                "SizeChangeListener",
                typeof(ISizeChangeListener),
                typeof(FrameworkElementAttachedBehaviors),
                new PropertyMetadata(null, OnSizeChangeListenerChanged));

        private static void OnSizeChangeListenerChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is FrameworkElement frameworkElement
                && args.NewValue is ISizeChangeListener listener)
            {
                frameworkElement.SizeChanged += (s, e) =>
                {
                    listener.SizeChanged(e.NewSize);
                };
            }
        }

        public static ISizeChangeListener GetSizeChangeListener(DependencyObject dependencyObject)
        {
            return (ISizeChangeListener)dependencyObject.GetValue(SizeChangeListenerProperty);
        }

        public static void SetSizeChangeListener(DependencyObject dependencyObject, ISizeChangeListener listener)
        {
            dependencyObject.SetValue(SizeChangeListenerProperty, listener);
        }

        public static DependencyProperty MouseMoveListenerProperty =
            DependencyProperty.RegisterAttached(
                "MouseMoveListener",
                typeof(IMouseMoveListener),
                typeof(FrameworkElementAttachedBehaviors),
                new PropertyMetadata(null, OnMouseMoveListenerChanged));

        private static void OnMouseMoveListenerChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            if (dependencyObject is FrameworkElement frameworkElement
                && args.NewValue is IMouseMoveListener listener)
            {
                frameworkElement.MouseMove += (s, e) =>
                {
                    var isEnabled = GetMouseMoveListenerEnabled(frameworkElement);
                    if (isEnabled)
                    {
                        listener.MouseMove(e.GetPosition(frameworkElement));
                    }
                };
            }
        }

        public static IMouseMoveListener GetMouseMoveListener(DependencyObject dependencyObject)
        {
            return (IMouseMoveListener)dependencyObject.GetValue(MouseMoveListenerProperty);
        }

        public static void SetMouseMoveListener(DependencyObject dependencyObject, IMouseMoveListener listener)
        {
            dependencyObject.SetValue(MouseMoveListenerProperty, listener);
        }

        public static DependencyProperty MouseMoveListenerEnabledProperty =
            DependencyProperty.RegisterAttached(
                "MouseMoveListenerEnabled",
                typeof(bool),
                typeof(FrameworkElementAttachedBehaviors),
                new PropertyMetadata(true));

        public static bool GetMouseMoveListenerEnabled(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(MouseMoveListenerEnabledProperty);
        }

        public static void SetMouseMoveListenerEnabled(DependencyObject dependencyObject, bool isEnabled)
        {
            dependencyObject.SetValue(MouseMoveListenerEnabledProperty, isEnabled);
        }
    }
}
