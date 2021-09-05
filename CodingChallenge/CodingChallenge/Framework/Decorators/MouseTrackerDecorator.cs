using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CodingChallenge.Framework.Decorators
{
    public class MouseTrackerDecorator : Decorator
    {
        static readonly DependencyProperty MousePositionProperty;

        static MouseTrackerDecorator()
        {
            MousePositionProperty = DependencyProperty.Register("MousePosition", typeof(Point), typeof(MouseTrackerDecorator));
        }

        public override UIElement Child
        {
            get => base.Child;
            set
            {
                if (base.Child != null)
                {
                    base.Child.MouseMove -= Child_MouseMove;
                }
                base.Child = value;
                base.Child.MouseMove += Child_MouseMove;
            }
        }

        private void Child_MouseMove(object sender, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(base.Child);

            MousePosition = mousePosition;
        }

        public Point MousePosition
        {
            get => (Point)GetValue(MouseTrackerDecorator.MousePositionProperty);
            set => SetValue(MouseTrackerDecorator.MousePositionProperty, value);
        }
    }
}
