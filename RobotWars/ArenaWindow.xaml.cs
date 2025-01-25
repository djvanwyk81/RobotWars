using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RobotWars
{
    /// <summary>
    /// Interaction logic for ArenaWindow.xaml
    /// </summary>
    public partial class ArenaWindow : Window
    {
        public int HeightValue = 10;
        public int WidthValue = 10;
        public ArenaWindow()
        {
            InitializeComponent();
        }

        private void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is null)
                return;

            int result = 0;
            if (!int.TryParse((sender as System.Windows.Controls.TextBox).Text, out result))
            {
                (sender as System.Windows.Controls.TextBox).Foreground = System.Windows.Media.Brushes.Red;
                if (btnOK != null)
                    btnOK.IsEnabled = false;
            }
            else
            {
                (sender as System.Windows.Controls.TextBox).Foreground = System.Windows.Media.Brushes.Black;
                if (btnOK != null)
                    btnOK.IsEnabled = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.HeightValue = Convert.ToInt32(tbHeight.Text);
            this.WidthValue = Convert.ToInt32(tbWidth.Text);
            this.DialogResult = true;
            this.Close();
        }
    }
}