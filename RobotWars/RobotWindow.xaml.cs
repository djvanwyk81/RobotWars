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
using DataLayer;

namespace RobotWars
{
    /// <summary>
    /// Interaction logic for RobotWindow.xaml
    /// </summary>
    public partial class RobotWindow : Window
    {
        public string RobotName = String.Empty;
        public System.Drawing.Color Color;
        public RobotFaction Faction;
        public Directions Direction;
        public Position Position = new Position(0, 0);

        public RobotWindow()
        {
            InitializeComponent();

            this.LoadLookups();
        }

        private void LoadLookups()
        {
            cbFaction.ItemsSource = Enum.GetValues(typeof(RobotFaction));
            cbDirection.ItemsSource = HelperClasses.GetGenericEnumDescription(typeof(Directions));
        }

        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.RobotName = tbName.Text;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            #region Validation
            if (String.IsNullOrWhiteSpace(tbName.Text))
            {
                System.Windows.MessageBox.Show("Name must have a value");
                return;
            }

            if (this.Color == null)
            {
                System.Windows.MessageBox.Show("Color mus be picked");
                return;
            }

            if (cbFaction.SelectedValue == null)
            {
                System.Windows.MessageBox.Show("Faction must be selected");
                return;
            }

            if (cbDirection.SelectedValue == null)
            {
                System.Windows.MessageBox.Show("Direction must be selected");
                return;
            }

            if ((String.IsNullOrWhiteSpace(tbX.Text)) || (String.IsNullOrWhiteSpace(tbY.Text)))
            {
                System.Windows.MessageBox.Show("Position must have a values");
                return;
            }
            #endregion

            this.Faction = (RobotFaction)cbFaction.SelectedValue;
            this.Direction = (Directions)cbDirection.SelectedIndex;
            this.Position = new Position(Convert.ToInt32(tbX.Text), Convert.ToInt32(tbY.Text));
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
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

        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.Color = colorDialog.Color;
                btnColor.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(Color.A, Color.R, Color.G, Color.B));
            }
        }
    }
}