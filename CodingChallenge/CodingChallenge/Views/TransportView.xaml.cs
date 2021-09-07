using CodingChallenge.Interfaces;
using CodingChallenge.Transport;
using System.Windows.Controls;

namespace CodingChallenge.Views
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
