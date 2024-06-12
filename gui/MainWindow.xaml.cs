using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BattleshipsLib;

namespace gui
{
    public partial class MainWindow : Window
    {
        private List<Button> clickedButtons = new List<Button>();
        private BattleshipGame game;
        private bool showShips = true;
        private int step = 1;
        public MainWindow()
        {
            Application.Current.DispatcherUnhandledException += OnDispatcherUnhandledException;
            InitializeComponent();
            InitializeGameBoard();
            game = new BattleshipGame();
        }
        private void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show($"An unexpected error occurred: {e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            //prevent application from closing
            e.Handled = true;
        }

        private void InitializeGameBoard()
        {
            string[] rowLabels = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            for (int i = 0; i < rowLabels.Length; i++)
            {
                GameBoard.Children.Add(new TextBlock
                {
                    Text = rowLabels[i],
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                });
                Grid.SetRow(GameBoard.Children[^1], i + 1);
                Grid.SetColumn(GameBoard.Children[^1], 0);
            }

            for (int i = 0; i < 10; i++)
            {
                GameBoard.Children.Add(new TextBlock
                {
                    Text = (i + 1).ToString(),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                });
                Grid.SetRow(GameBoard.Children[^1], 0);
                Grid.SetColumn(GameBoard.Children[^1], i + 1);
            }

            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    Button button = new Button
                    {
                        Margin = new Thickness(1),
                        Tag = $"{rowLabels[row]}{col + 1}",
                        ToolTip = new ToolTip { Content = $"Coordinate: {rowLabels[row]}{col + 1}" },
                        Background = new SolidColorBrush(Colors.LightGray),
                        Content = "",
                };

                    button.Click += Button_Click;

                    Grid.SetRow(button, row + 1);
                    Grid.SetColumn(button, col + 1);
                    GameBoard.Children.Add(button);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                if (clickedButtons.Contains(button))
                {
                    button.Background = new SolidColorBrush(Colors.LightGray);
                    clickedButtons.Remove(button);
                }
                else
                {
                    button.Background = new SolidColorBrush(Colors.Blue);
                    clickedButtons.Add(button);
                }
            }
        }


        private void ClearSelection_Click(object sender, RoutedEventArgs e)
        {
            ClearSelection();
        }

        private void ClearSelection()
        {
            foreach (Button button in clickedButtons)
            {
                button.Background = new SolidColorBrush(Colors.LightGray);
            }
            clickedButtons.Clear();

            ShowChoosenFields();
        }

        private void ShowChoosenFields()
        {
            if (showShips)
            {
                MarkButtonsByCoordinates(game.GetCoordinatesOfPlacedShips(BoardToggle.IsChecked ?? true), Colors.Green);
            }
            else
            {
                MarkButtonsByCoordinates(game.GetHitCoordinates(BoardToggle.IsChecked ?? true), "x");
                MarkButtonsByCoordinates(game.GetMissedCoordinates(BoardToggle.IsChecked ?? true), "o");
            }

            MarkButtonsByCoordinates(GetCoordinatesFromClickedButtons(), Colors.Blue);
        }

        private void ClearBoard()
        {
            foreach (UIElement element in GameBoard.Children)
            {
                if (element is Button button)
                {
                    button.Background = new SolidColorBrush(Colors.LightGray);
                    button.Content = "";
                }
            }
        }

        private void MarkButtonsByCoordinates(List<Coordinate> coordinates, Color color)
        {
            foreach (Coordinate coordinate in coordinates)
            {
                string coordinateStr = coordinate.ToString();

                foreach (UIElement element in GameBoard.Children)
                {
                    if (element is Button button && button.Tag.ToString() == coordinateStr)
                    {
                        button.Background = new SolidColorBrush(color);
                    }
                }
            }
        }

        private void MarkButtonsByCoordinates(List<Coordinate> coordinates, string character)
        {
            foreach (Coordinate coordinate in coordinates)
            {
                string coordinateStr = coordinate.ToString();

                foreach (UIElement element in GameBoard.Children)
                {
                    if (element is Button button && button.Tag.ToString() == coordinateStr)
                    {
                        button.Content = character;
                    }
                }
            }
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            RestartGame();
        }

        private void RestartGame()
        {
            game = new BattleshipGame();
            ClearSelection();
            ClearBoard();
            step = 1;

            StatusText.Text = "Select 4 fields";
        }

        private List<Coordinate> GetCoordinatesFromClickedButtons()
        {
            List<Coordinate> coordinates = new List<Coordinate>();

            foreach (Button button in clickedButtons)
            {
                if (button.Tag is string tag)
                {
                    Coordinate coordinate = new Coordinate(tag);
                    coordinates.Add(coordinate);
                }
            }

            return coordinates;
        }

        private void HandleGameStep(object sender, RoutedEventArgs e)
        {
            switch (step)
            {
                case 1:
                    game.PlaceShip(4, GetCoordinatesFromClickedButtons());
                    ClearSelection();

                    if (game.GetMissingShipsCount(4) == 0)
                    {
                        StatusText.Text = "Select 3 fields";
                        step++;
                    }

                    break;
                case 2:
                    game.PlaceShip(3, GetCoordinatesFromClickedButtons());
                    ClearSelection();

                    if (game.GetMissingShipsCount(3) == 0)
                    {
                        StatusText.Text = "Select 2 fields";
                        step++;
                    }
                    break;
                case 3:
                    game.PlaceShip(2, GetCoordinatesFromClickedButtons());
                    ClearSelection();

                    if (game.GetMissingShipsCount(2) == 0)
                    {
                        StatusText.Text = "Select 1 field";
                        step++;
                    }
                    break;
                case 4:
                    game.PlaceShip(1, GetCoordinatesFromClickedButtons());
                    ClearSelection();

                    if (game.GetMissingShipsCount(1) == 0)
                    {
                        StatusText.Text = "Your turn";
                        showShips = false;
                        ClearBoard();
                        ShowChoosenFields();
                        step++;
                    }
                    break;
                case 5:
                    if (GetCoordinatesFromClickedButtons().Count > 1)
                    {
                        throw new Exception("Select single field to attack");
                    }
                    game.Attack(GetCoordinatesFromClickedButtons().First());
                    ClearSelection();

                    if (game.isGameOver())
                    {
                        String playerName = game.IsPlayerVictory() ? "Player" : "Computer";
                        MessageBox.Show($"{playerName} has won the game", "Game has ended", MessageBoxButton.OK, MessageBoxImage.Information);
                        RestartGame();
                    }

                    break;
                default:

                    break;
            }
        }

        private void ShowShips(object sender, RoutedEventArgs e)
        {
            showShips = true;

            ClearBoard();

            ShowChoosenFields();
        }

        private void ShowShoots(object sender, RoutedEventArgs e)
        {
            showShips = false;

            ClearBoard();

            ShowChoosenFields();
        }

        private void BoardToggle_Checked(object sender, RoutedEventArgs e)
        {
            ClearBoard();

            ShowChoosenFields();
        }

        private void BoardToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ClearBoard();

            ShowChoosenFields();
        }
    }
}