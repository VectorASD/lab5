using Avalonia.Controls;
using Figurator.ViewModels;

namespace Figurator.Views {
    public partial class MainWindow: Window {
        public MainWindow() {
            InitializeComponent();
            DataContext = new MainWindowViewModel(this);
        }
    }
}