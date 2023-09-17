public class Board {

    private Cell[,] cells = new Cell[0, 0];
    private Random random = new Random();
    public Coordinates Dimension { get; private set;}
    public Cell? SelectedCell { get; private set; }
    public int NumberOfRevealedCells { get; private set; }

    public event Action? MineRevealed;
    public event Action? MineMarked;
    public event Action? MineUnmarked;

    public Cell ContentsOf(int row, int column) => cells[row, column];
    public Cell ContentsOf(Coordinates point) => cells[point.X, point.Y];
    private Cell? GetSelectedCell() => SelectedCell;
    private void SetCellAsSelected(Cell cell) {
        cell.Selected = true;
        SelectedCell = cell;
    } 

    public void Initialize(Coordinates dimension, int numberOfTotalMines) {
        Dimension = dimension;
        cells = new Cell[dimension.X, dimension.Y];
        for (int i = 0; i < dimension.X; i++)
        {
            for (int j = 0; j < dimension.Y; j++)
            {
                cells[i,j] = new Cell(i, j);
            }
        }
        NumberOfRevealedCells = 0;
        ShuffleMines(numberOfTotalMines);
        CalculateNumberOfNearbyMines();
        cells[0,0].Selected = true;
        SelectedCell = cells[0,0];

    }
    public void Reveal() {
        Cell cell = GetSelectedCell()!;
        if (!cell.Revealed){
            cell.Revealed = true;
            NumberOfRevealedCells += 1;
            switch (cell.GetCellType())
            {
                case CellType.Number:
                break;
                case CellType.Mine:
                MineRevealed?.Invoke();
                break;
                case CellType.Empty:
                Reveal((EmptyCell)GetSelectedCell()!);
                break;
            }
        }
    }

    private void Reveal(EmptyCell cell) {
        List<Cell> nearbyCells = GetNearbyCells(cell);
        foreach (Cell nearbyCell in nearbyCells)
        {
            if (!nearbyCell.Revealed)
            {
                switch (nearbyCell.GetCellType())
                {
                    case CellType.Number:
                    nearbyCell.Revealed = true;
                    NumberOfRevealedCells += 1;
                    break;
                    case CellType.Empty:
                    nearbyCell.Revealed = true;
                    NumberOfRevealedCells += 1;
                    Reveal((EmptyCell)nearbyCell);
                    break;
                    case CellType.Mine:
                    break;
                } 
            }
        }
    }

    public void MarkAllMines() {
        for (int i = 0; i < Dimension.X; i++)
        {
            for (int j = 0; j < Dimension.Y; j++)
            {
                if (cells[i,j].GetCellType() == CellType.Mine) cells[i,j].Marked = true;
            }
        }
    }

    public void RevealAll() {
        for (int i = 0; i < Dimension.X; i++)
        {
            for (int j = 0; j < Dimension.Y; j++)
            {
                if (!cells[i,j].Marked || cells[i,j].GetCellType() != CellType.Mine)
                {
                    cells[i,j].Revealed = true;
                }
            }
        }
    }

    public void ToggleMark() {
        Cell selectedCell = GetSelectedCell()!;
        if (selectedCell.Revealed) return;
        bool marked = !selectedCell.Marked;
        selectedCell.Marked = marked;
        if (selectedCell.GetCellType() == CellType.Mine && marked) MineMarked?.Invoke();
        else if (selectedCell.GetCellType() == CellType.Mine && !marked) MineUnmarked?.Invoke();
    }

    public void Navigate(Coordinates direction) {
        Cell cell = GetSelectedCell()!;
        Coordinates newLocation = new Coordinates(cell.Row, cell.Column) + direction;
        if (IsInBounds(newLocation))
        {
            cell.Selected = false;
            SetCellAsSelected(ContentsOf(newLocation));    
        }
        
    }
    private List<Cell> GetNearbyCells(Cell cell) {
        List<Cell> nearbyCells = new List<Cell>();
        Coordinates[] directions = new Coordinates[] {new Coordinates(0, 1), new Coordinates(0, -1),
                                                      new Coordinates(1, 0), new Coordinates(-1, 0),
                                                      new Coordinates(1, 1), new Coordinates(1, -1),
                                                      new Coordinates(-1, 1), new Coordinates(-1, -1)};
        foreach (Coordinates direction in directions)
        {
            Coordinates location = direction + new Coordinates(cell.Row, cell.Column);
            if (IsInBounds(location)) nearbyCells.Add(cells[location.X, location.Y]);
        }
        return nearbyCells;
    }

    private bool IsInBounds(Coordinates coordinates) {
        if (coordinates.X < 0 || coordinates.X >= Dimension.X) return false;
        if (coordinates.Y < 0 || coordinates.Y >= Dimension.Y) return false;
        return true;
    }
    private void ShuffleMines(int numberOfTotalMines) {
        for (int i = 0; i < numberOfTotalMines; i++)
        {
            Coordinates randomCoordinates;
            bool alreadySelected;
            do
            {
            randomCoordinates = GetRandomCoordinates();
            alreadySelected = cells[randomCoordinates.X, randomCoordinates.Y].GetCellType() == CellType.Mine;
            } while (alreadySelected);

            cells[randomCoordinates.X, randomCoordinates.Y] = new MineCell(randomCoordinates.X, randomCoordinates.Y);
        }
    }

    private void CalculateNumberOfNearbyMines() {
        for (int i = 0; i < Dimension.X; i++)
        {
            for (int j = 0; j < Dimension.Y; j++)
            {
                Cell cell = cells[i, j];
                if (cell.GetCellType() != CellType.Mine) 
                {
                    int numberOfNearbyMines = 0;
                    foreach (Cell nearbyCell in GetNearbyCells(cell))
                    {
                        if (nearbyCell.GetCellType() == CellType.Mine) numberOfNearbyMines++;
                    }
                    if (numberOfNearbyMines == 0) cells[i, j] = new EmptyCell(i, j);
                    else cells[i, j] = new NumberCell(i, j, numberOfNearbyMines);
                }
                
            }
        }
    }

    private Coordinates GetRandomCoordinates() {
        int x = random.Next(Dimension.X); 
        int y = random.Next(Dimension.Y);
        return new Coordinates(x, y);

    }
}
