new MinesweeperGame().MainMenu();

public class MinesweeperGame {
    private Player player = new Player();
    private Board board = new Board();
    private BoardRendrer rendrer = new BoardRendrer();
    private Coordinates boardDimension;
    private int numberOfTotalMines;
    private int numberOfRemaningMines;
    private bool mineExploded;

    public MinesweeperGame(int x = 7, int y = 7, int numberOfTotalMines = 7) {
        boardDimension = new Coordinates(x, y);
        board.MineRevealed += OnMineExploded;
        board.MineMarked += OnMineMarked;
        board.MineUnmarked += OnMineUnmarked;
        Initialize(boardDimension, numberOfTotalMines);
    }
    private void Initialize(Coordinates boardDimension, int numberOfTotalMines) {
        this.boardDimension = boardDimension;
        this.numberOfTotalMines = numberOfTotalMines;
        numberOfRemaningMines = numberOfTotalMines;
        mineExploded = false;
    }
    public void Run() {
        board.Initialize(boardDimension, numberOfTotalMines);
        while (!mineExploded)
        {  
        rendrer.Draw(board);
        rendrer.DisplayInstructions();
        player.HandleInput(board);
        if (HasWon()) break;
        }
        DisplayFinalBoard();
        Console.ReadKey();
        MainMenu();
    }

    private void DisplayFinalBoard() {
        if (mineExploded) {
            board.RevealAll();
            rendrer.Draw(board);
            Console.WriteLine($"You Lost! You were just {numberOfRemaningMines} mines away.");
        }
        else {
            board.MarkAllMines();
            rendrer.Draw(board);
            Console.WriteLine("You Won!");
        }
    }

    private void OnMineExploded() {
        mineExploded = true;
    }

    private void OnMineMarked() {
        numberOfRemaningMines -= 1;
    }

    private void OnMineUnmarked() {
        numberOfRemaningMines += 1;
    }

    private bool HasWon() {
        int numberOfTotalCells = boardDimension.X * boardDimension.Y;
        if (board.NumberOfRevealedCells == numberOfTotalCells - numberOfTotalMines)
        {
            return true;
        }
        return false;
    }

    public void MainMenu() {
        Console.CursorVisible = false;
        Menu menu = Menu.Play;
        bool isSelected = false;
        ConsoleColor white = ConsoleColor.White;
        ConsoleColor blue = ConsoleColor.Blue;
        ConsoleColor darkRed = ConsoleColor.DarkRed;
        while (!isSelected)
        {
            Console.Clear();
            Console.ForegroundColor = darkRed;
            Console.WriteLine("Minesweeper Game");
            Console.ForegroundColor = menu == Menu.Play ? blue : white;
            Console.WriteLine("Play");
            Console.ForegroundColor = menu == Menu.Options ? blue : white;
            Console.WriteLine("Options");
            Console.ForegroundColor = menu == Menu.Exit ? blue : white;
            Console.WriteLine("Exit");
            ConsoleKey key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                if (menu != Menu.Play) menu--;
                break;
                case ConsoleKey.DownArrow:
                if (menu != Menu.Exit) menu++;
                break;
                case ConsoleKey.Enter:
                isSelected = true;
                break;
            }
        }

        switch (menu) {
            case Menu.Play:
            Initialize(boardDimension, numberOfTotalMines);
            Run();
            break;
            case Menu.Options:
            ChangeOptions();
            break;
            case Menu.Exit:
            break;
        }
    }

    private void ChangeOptions() {
        Console.Clear();
        Console.Write("Number of rows: ");
        int x = int.Parse(Console.ReadLine()!);
        Console.Write("Number of columns: ");
        int y = int.Parse(Console.ReadLine()!);
        boardDimension = new Coordinates(x, y);
        Console.Write("Number of total mines: ");
        int z = int.Parse(Console.ReadLine()!);
        numberOfTotalMines = z;
        MainMenu();
    }
}
enum Menu {Play, Options, Exit}

public struct Coordinates {
    public int X { get;}
    public int Y { get;}

    public Coordinates(int x, int y) {
        X = x; Y = y;
    }
    public static Coordinates operator +(Coordinates a, Coordinates b) {
        return new Coordinates(a.X + b.X, a.Y + b.Y);
    }
}


