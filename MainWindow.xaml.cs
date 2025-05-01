using Newtonsoft.Json;
using System.Windows;

namespace CITest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string json = JsonConvert.SerializeObject(new
            {
                name = "Test1",
            });
            MessageBox.Show(json);
        }
    }
}