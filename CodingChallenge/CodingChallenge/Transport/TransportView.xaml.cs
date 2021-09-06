using CodingChallenge.Interfaces;
using System.Windows.Controls;

namespace CodingChallenge.Transport
{
    /// <summary>
    /// Interaction logic for TransportView.xaml
    /// </summary>
    public partial class TransportView : UserControl
    {
        public TransportView()
        {
            InitializeComponent();
        }

        public ITransportController Controller => (DataContext as TransportViewModel).Controller;
    }
}
