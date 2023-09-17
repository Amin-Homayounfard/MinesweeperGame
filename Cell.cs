public class Cell {
    public int Row { get; }
    public int Column { get;}
    public string Label { get; }
    public bool Marked { get; set; }
    public bool Revealed {get; set;}
    public bool Selected { get; set; }
    public virtual CellType GetCellType() => CellType.Empty;

    public Cell(int row, int column, string label = "-") {
        Row = row;
        Column = column;
        Selected = false;
        Marked = false;
        Revealed = false;
        Label = label;
    }
}

public class EmptyCell : Cell {
    public EmptyCell(int row, int column) : base(row, column) {}
    public override CellType GetCellType() => CellType.Empty;
}

public class NumberCell : Cell {
    public int NumberOfNearbyMines {get;}
    public NumberCell(int row, int column, int numberOfNearbyMines) : base(row, column, numberOfNearbyMines.ToString()) {
        NumberOfNearbyMines = numberOfNearbyMines;
    }
    public override CellType GetCellType() => CellType.Number;
}

public class MineCell : Cell {
    public bool Neutralized {get; set;}
    //private string unicodeChar = "\u25CF";

    public MineCell(int row, int column) : base(row, column, "*") {}
    public override CellType GetCellType() => CellType.Mine;
}
public enum CellType {Empty, Mine, Number}
