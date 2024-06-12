using System.Data.Common;

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
        Column = CheckColumnRange(column);
    }

    public Coordinate(string coordinate)
    {
        if (string.IsNullOrEmpty(coordinate) || coordinate.Length < 2)
        {
            throw new ArgumentException("Invalid coordinate string");
        }

        char rowPart = char.ToUpper(coordinate[0]);
        if (!Enum.TryParse(rowPart.ToString(), out Row row))
        {
            throw new ArgumentException("Invalid row in coordinate string");
        }

        if (!int.TryParse(coordinate.Substring(1), out int column))
        {
            throw new ArgumentException("Invalid column in coordinate string");
        }

        Row = row;
        Column = CheckColumnRange(column);
    }

    private static int CheckColumnRange(int column)
    {
        if (column < 1 || column > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(column), "Column must be between 1 and 10");
        }
        return column;
    }

    public static Row IntToRow(int value)
    {
        if (!Enum.IsDefined(typeof(Row), value))
        {
            throw new ArgumentException("Invalid row value.");
        }
        return (Row)value;
    }

    public override string ToString()
    {
        return $"{Row}{Column}";
    }

    public override bool Equals(object obj)
    {
        if (obj is Coordinate otherCoordinate)
        {
            return this.Row == otherCoordinate.Row && this.Column == otherCoordinate.Column;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column);
    }
}