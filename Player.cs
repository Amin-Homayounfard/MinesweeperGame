public class Player {
    public void HandleInput(Board board) {
        while (true)
        {
            ConsoleKey key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.RightArrow:
                board.Navigate(new Coordinates(0, 1));
                break;
                case ConsoleKey.LeftArrow:
                board.Navigate(new Coordinates(0, -1));
                break;
                case ConsoleKey.UpArrow:
                board.Navigate(new Coordinates(-1, 0));
                break;
                case ConsoleKey.DownArrow:
                board.Navigate(new Coordinates(1, 0));
                break;
                case ConsoleKey.Spacebar:
                board.Reveal();
                break;
                case ConsoleKey.Enter:
                board.ToggleMark();
                break;
                default:
                continue;
            }
            return;
        }
    }
}
