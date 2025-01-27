using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataLayer;

namespace RobotWars
{
    public partial class MainWindow : Window
    {
        private Arena _Arena = null;
        private Robot _LastRobot = null;
        private Dictionary<Robot, System.Windows.Controls.Button> _RobotButtons = new Dictionary<Robot, System.Windows.Controls.Button>();

        public MainWindow()
        {
            InitializeComponent();

            tbAutomation.Text = @"5 5
1 2 N
LMLMLMLMM
3 3 E
MMRMMRMRRM";
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnCreateArena_Click(object sender, RoutedEventArgs e)
        {
            ArenaWindow arenaWindow = new ArenaWindow();

            if (arenaWindow.ShowDialog().Value)
            {
                this.CreateArena(arenaWindow.HeightValue, arenaWindow.WidthValue);
            } // if
        }

        private void btnAddRobot_Click(object sender, RoutedEventArgs e)
        {
            RobotWindow robotWindow = new RobotWindow();
            if (robotWindow.ShowDialog().Value)
            {
                this.CreateRobot(robotWindow.Position, robotWindow.Direction, robotWindow.RobotName, robotWindow.Color, robotWindow.Faction);
            }
        }

        private void Robot_OnMove(Robot sender)
        {
            _RobotButtons[sender].SetValue(Grid.ColumnProperty, sender.Position.X);
            _RobotButtons[sender].SetValue(Grid.RowProperty, _Arena.Width - sender.Position.Y);
        }

        private void miClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lbLog.Items.Clear();
            } // try
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show(ExceptionFormatter.FormatException(exp), "Error clearing Log", MessageBoxButton.OK, MessageBoxImage.Error);
            } // catch
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(tbAutomation.Text.Trim()))
            {
                WriteLog("Nothing to execute");
                return;
            }

            foreach (string line in tbAutomation.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {// This should all be refactored into a business library instead of the UI
                int spaceCount = line.Trim().Where(c => c.Equals(' ')).Count();
                if (spaceCount == 1)
                {
                    #region Arena
                    string[] values = line.Trim().Split(new char[] { ' ' });
                    int aX = -1;
                    int aY = -1;
                    if (!int.TryParse(values[0], out aX))
                    {
                        this.WriteLog($"'{values[0]}' is not a valid arena width");
                        return;
                    }

                    if (!int.TryParse(values[1], out aY))
                    {
                        this.WriteLog($"'{values[1]}' is not a valid arena height");
                        return;
                    }

                    if ((aX < 1) || (aX > int.MaxValue) || (aY < 1) || (aY > int.MaxValue))
                    {
                        this.WriteLog($"Arena height and width must fall between 1 and {int.MaxValue}");
                        return;
                    }

                    this.CreateArena(aX, aY);
                    continue;
                    #endregion
                } // Arena parameters

                if (spaceCount == 2)
                {
                    #region Robot specifications
                    string[] values = line.Trim().Split(new char[] { ' ' });
                    int aX = -1;
                    int aY = -1;
                    string asDirection = values[2].Trim();
                    Directions aDirection;
                    if (!int.TryParse(values[0], out aX))
                    {
                        this.WriteLog($"'{values[0]}' is not a valid X coordinate");
                        return;
                    }

                    if (!int.TryParse(values[1], out aY))
                    {
                        this.WriteLog($"'{values[1]}' is not a valid Y coordinate");
                        return;
                    }
                    switch (asDirection)
                    {
                        case "N": aDirection = Directions.North; break;
                        case "E": aDirection = Directions.East; break;
                        case "S": aDirection = Directions.South; break;
                        case "W": aDirection = Directions.West; break;
                        default: this.WriteLog($"'{asDirection}' is not a valid direction"); return;
                    } // switch

                    this.CreateRobot(new Position(aX, aY), aDirection, $"Robot{_Arena.Robots.Count + 1}", this.RandomizeColor(aX + aY), RobotFaction.Friendly);
                    continue;
                    #endregion
                } // Robot specifications

                #region Commands
                foreach (char command in line)
                {
                    switch (command)
                    {
                        case 'L': _LastRobot.Turn(TurnDirections.Left); break;
                        case 'R': _LastRobot.Turn(TurnDirections.Right);  break;
                        case 'M': _LastRobot.Move(ref _Arena); break;
                        default: this.WriteLog($"'{command}' is not a valid command"); return;
                    } // switch

                    if (cbDebug.IsChecked.Value)
                    WriteLog($"{command} -> {_LastRobot.Position.X} {_LastRobot.Position.Y} {_LastRobot.Direction}");
                } // foreach command
                WriteLog($"{_LastRobot.Position.X} {_LastRobot.Position.Y} {_LastRobot.Direction}");
                #endregion
            } // foreach line
        }

        private void CreateArena(int height, int width)
        {
            if (_Arena!=null)
            { // Clean up old arena in case execution is repeated multiple times
                grdArena.ColumnDefinitions.Clear();
                grdArena.RowDefinitions.Clear();
                foreach (System.Windows.Controls.Button robotButton in _RobotButtons.Values) { robotButton.Visibility = Visibility.Collapsed; } // Hide controls that have not been disposed yet
                _RobotButtons.Clear();
                _Arena = null;
            } // if

            _Arena = new Arena(height, width);
            tbWelcome.Visibility = Visibility.Collapsed;
            btnAddRobot.IsEnabled = true;
            for (int x = 0; x < _Arena.Width; x++)
            {
                grdArena.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            } // for
            for (int y = 0; y < _Arena.Width; y++)
            {
                grdArena.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            } // for
        }

        private void CreateRobot(Position position, Directions direction, string robotName, System.Drawing.Color color, RobotFaction faction)
        {
            if (_Arena.CheckBoundaries(position))
            {
                System.Windows.MessageBox.Show("New robot must be inside arena boundaries", "New robot boundary check", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }  // if

            Robot? collisionRobot = _Arena.CheckRobotCollisions(String.Empty, position);
            if (collisionRobot != null)
            {
                System.Windows.MessageBox.Show($"New robot can't be at the same position as {collisionRobot.Name}", "New robot boundary check", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } // if

            if (_Arena.CheckNames(robotName))
            {
                System.Windows.MessageBox.Show("New robot name already used", "New robot boundary check", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _Arena.Robots.Add(robotName, new Robot(robotName, color, faction, position.X, position.Y, direction));

            System.Windows.Controls.Button newButton = new System.Windows.Controls.Button
            {
                Name = $"btn{robotName}",
                Tag = robotName,
                Content = String.Concat(_Arena.Robots[robotName].DirectionIndicator, " ", robotName),
                Margin = new Thickness(5),
                Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B)),
            };
            newButton.SetValue(Grid.ColumnProperty, position.X);
            newButton.SetValue(Grid.RowProperty, _Arena.Width - position.Y);

            _Arena.Robots[robotName].OnMove += this.Robot_OnMove;

            _RobotButtons.Add(_Arena.Robots[robotName], newButton);
            grdArena.Children.Add(newButton);
            _LastRobot = _Arena.Robots[robotName];
            lbRobots.Visibility = Visibility.Visible;
            lbRobots.Content = $"{_Arena.Robots.Count} robots remaining";
        }

        private System.Drawing.Color RandomizeColor(int seed)
        {
            Random rnd = new Random(seed);
            int R = rnd.Next(256);  // creates a number between 1 and 12
            int G = rnd.Next(256);   // creates a number between 1 and 6
            int B = rnd.Next(256);     // creates a number between 0 and 51

            return System.Drawing.Color.FromArgb(R, G, B);
        }

        private void WriteLog(string text)
        {
            lbLog.Items.Add(text);
            lbLog.ScrollIntoView(lbLog.Items[lbLog.Items.Count - 1]);
        }
    }
}