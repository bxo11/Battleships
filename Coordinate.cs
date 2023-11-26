public enum Row
{
    A = 1,
    B = 2,
    C = 3,
    D = 4,
    E = 5,
    F = 6,
    G = 7,
    H = 8,
    I = 9,
    J = 10
}

public class Coordinate
{
    public int Column { get; }
    public Row Row { get; }

    public Coordinate(Row row, int column)
    {
        Row = row;
        Column = column;
    }
}