using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HawkeyeLogo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string defaultDirectory = @"c:\temp";
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        public int ExportedSize
        {
            get { return (int)GetValue(ExportedSizeProperty); }
            set { SetValue(ExportedSizeProperty, value); }
        }

        public static readonly DependencyProperty ExportedSizeProperty = DependencyProperty.Register(
            "ExportedSize", typeof(int), typeof(MainWindow), new PropertyMetadata(256));
        
        public bool? SaveAllSizes
        {
            get { return (bool?)GetValue(SaveAllSizesProperty); }
            set { SetValue(SaveAllSizesProperty, value); }
        }

        public static readonly DependencyProperty SaveAllSizesProperty = DependencyProperty.Register(
            "SaveAllSizes", typeof(bool?), typeof(MainWindow), new PropertyMetadata(true));
        
        public string OutputDirectory
        {
            get { return (string)GetValue(OutputDirectoryProperty); }
            set { SetValue(OutputDirectoryProperty, value); }
        }

        public static readonly DependencyProperty OutputDirectoryProperty = DependencyProperty.Register(
            "OutputDirectory", typeof(string), typeof(MainWindow), new PropertyMetadata(defaultDirectory));       

        private void Save(int size)
        {
            var controls = new Control[]
            {
                h1, h2, h3, h4
            };

            var saved = Cursor;
            Cursor = Cursors.Wait;
            try
            {
                foreach (var control in controls)
                {
                    var filename = Path.Combine(OutputDirectory, control.Name + "_" + size + ".png");
                    control.SaveTo(size, size, filename);
                }
            }
            finally { Cursor = saved; }
        }

        private void SaveAll()
        {
            var sizes = new int[] { 16, 24, 32, 64, 128, 256, 512, 1024 };
            foreach (var size in sizes)
                Save(size);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SaveAllSizes.HasValue && SaveAllSizes.Value)
                SaveAll();
            else Save(ExportedSize);
        }
    }
}
