public class BoardRendrer {
    //private string unicodeChar = "\u2691";
    public void Draw(Board board) {
        Console.Clear();
        string borderLine = String.Concat(Enumerable.Repeat("+---", board.Dimension.Y));
        for (int i = 0; i < board.Dimension.X; i++)
        {
            Console.Write(borderLine); Console.WriteLine("+");
            for (int j = 0; j < board.Dimension.Y; j++)
            {
                Cell cell = board.ContentsOf(i, j);
                Console.Write($"| ");
                AdjustConsoleColor(cell);
                if (cell.Revealed) Console.Write($"{cell.Label}");
                else if (cell.Marked) Console.Write("#");
                else Console.Write($" ");
                Console.ResetColor();
                Console.Write($" ");
            }
            Console.WriteLine("|");
        }
        Console.Write(borderLine); Console.WriteLine("+");
    }

    private void AdjustConsoleColor(Cell cell) {
        if (cell.Selected) Console.BackgroundColor = ConsoleColor.DarkGray;
        if (cell.Revealed) {
            ConsoleColor color = cell.GetCellType() switch
            {
                CellType.Mine => ConsoleColor.DarkRed,
                CellType.Number => ConsoleColor.DarkCyan,
                _ => ConsoleColor.White,

            };
            Console.ForegroundColor = color;
        }
        else if (cell.Marked) Console.ForegroundColor = ConsoleColor.Green;
    }

    private void AdjustConsoleColor() {
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.ForegroundColor = ConsoleColor.Black;
    }

    public void DisplayInstructions() {
        Console.WriteLine();
        AdjustConsoleColor();
        Console.Write("Navigate:");
        Console.ResetColor();
        Console.WriteLine(" \u2190 " + " \u2191 " + " \u2192 " + " \u2193 ");
        AdjustConsoleColor();
        Console.Write("Reveal:");
        Console.ResetColor();
        Console.WriteLine(" Spacebar");
        AdjustConsoleColor();
        Console.Write("Toggle Mark:");
        Console.ResetColor();
        Console.WriteLine(" Enter");
    }
}
