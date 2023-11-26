using System.Drawing;

class Ship
{
    public int Id { get; private set; }
    public int Size { get; private set; }
    public List<Coordinate> Coordinates { get; set; }

    public Ship(int id, int size, List<Coordinate> coordinates)
    {
        Id = id;
        Size = size;
        Coordinates = new List<Coordinate>(size);

        if (coordinates == null || coordinates.Count != size)
        {
            throw new ArgumentException("Invalid number of coordinates");
        }

        Coordinates = coordinates;
    }

    public bool AreCoordinatesValid()
    {
        if (Coordinates.Count < 2)
        {
            return true;
        }

        bool sameColumn = Coordinates.All(c => c.Column == Coordinates[0].Column);
        bool sameRow = Coordinates.All(c => c.Row == Coordinates[0].Row);

        if (!sameColumn && !sameRow)
        {
            return false;
        }

        Coordinates.Sort((c1, c2) => sameColumn ? c1.Row.CompareTo(c2.Row) : c1.Column.CompareTo(c2.Column));

        for (int i = 1; i < Coordinates.Count; i++)
        {
            if ((sameColumn && Coordinates[i].Row != Coordinates[i - 1].Row + 1) ||
                (sameRow && Coordinates[i].Column != Coordinates[i - 1].Column + 1))
            {
                return false;
            }
        }

        return true;
    }

    private bool IsColliding(Coordinate otherCoord)
    {
        return Coordinates.Any(myCoord => myCoord.Row == otherCoord.Row && myCoord.Column == otherCoord.Column);
    }

    public bool IsNotColliding(IEnumerable<Ship> otherShips)
    {
        foreach (var otherShip in otherShips)
        {
            foreach (var otherCoord in otherShip.Coordinates)
            {
                if (IsColliding(otherCoord))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool IsAdjacent(Coordinate coord1, Coordinate coord2)
    {
        return Math.Abs(coord1.Row - coord2.Row) <= 1 && Math.Abs(coord1.Column - coord2.Column) <= 1;
    }

    public bool IsNotAdjacentToOtherShips(IEnumerable<Ship> otherShips)
    {
        foreach (var otherShip in otherShips)
        {
            foreach (var otherCoord in otherShip.Coordinates)
            {
                foreach (var myCoord in Coordinates)
                {
                    if (IsAdjacent(myCoord, otherCoord))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

}

class Battleship : Ship
{
    public Battleship(int id, List<Coordinate> coordinates) : base(id, 4, coordinates) { }
}

class Cruiser : Ship
{
    public Cruiser(int id, List<Coordinate> coordinates) : base(id, 3, coordinates) { }
}

class Destroyer : Ship
{
    public Destroyer(int id, List<Coordinate> coordinates) : base(id, 2, coordinates) { }
}
 
class Submarine : Ship
{
    public Submarine(int id, List<Coordinate> coordinates) : base(id, 1, coordinates) { }
}
