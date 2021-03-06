using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace ExcelReporting.UI.Utilities
{
    public partial class FileEntry : UserControl
    {
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(FileEntry), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public static DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(FileEntry), new PropertyMetadata(null));

        public string Text
        {
            get => GetValue(TextProperty) as string;
            set => SetValue(TextProperty, value);
        }

        public string Description
        {
            get => GetValue(DescriptionProperty) as string;
            set => SetValue(DescriptionProperty, value);
        }

        public FileEntry() { InitializeComponent(); }

        private void BrowseFolder(object sender, RoutedEventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = @"Excel files (*.xls?)|*.xls?|All files (*.*)|*.*";
                //dlg.Description = Description;
                //dlg.SelectedPath = Text;
                //dlg.ShowNewFolderButton = true;
                dlg.FileName = Text;
                var result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Text = dlg.FileName;
                    var be = GetBindingExpression(TextProperty);
                    be?.UpdateSource();
                }
            }
        }
    }
}