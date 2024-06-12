using System.Text;

class Player
{
    public Dictionary<Coordinate, int> Board { get; private set; }
    public Dictionary<int, Ship> Ships { get; private set; }

    public List<Coordinate> Shoots { get; private set; }

    public readonly Dictionary<int, int> allowedShips = new Dictionary<int, int> { { 4, 1 }, { 3, 2 }, { 2, 3 }, { 1, 4 } };
    public Player()
    {
        Board = new Dictionary<Coordinate, int>();
        Ships = new Dictionary<int, Ship>();
        Shoots = new List<Coordinate>();
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        foreach (Row row in Enum.GetValues(typeof(Row)))
        {
            for (int column = 1; column < 11; column++)
            {
                Board.Add(new Coordinate(row, column), 0);
            }
        }
    }

    public void Shoot(Coordinate coordinate)
    {
        if (Shoots.Contains(coordinate))
        {
            throw new InvalidOperationException("Cannot select the same coordinate again.");
        }

        Shoots.Add(coordinate);
    }


    public bool IsHit(Coordinate coordinate)
    {
        if (!Board.ContainsKey(coordinate))
        {
            throw new ArgumentOutOfRangeException("Coordinate is out of the board's bounds or not initialized.");
        }

        return Board[coordinate] != 0;
    }

    public bool IsDefeated(List<Coordinate> opponentShoots)
    {
        foreach (var ship in Ships.Values)
        {
            foreach (var coordinate in ship.Coordinates)
            {
                if (!opponentShoots.Contains(coordinate))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void PlaceShip(int size, List<Coordinate> coordinates)
    {
        if (!IsValidShipSize(coordinates.Count)) throw new Exception("Invalid ship size");

        int nextId = Ships.Count + 1;

        var ship = new Ship(nextId, size, coordinates);

        ship.CheckCoordinatesValidity();
        ship.CheckCollision(Ships.Values);
        ship.CheckAdjacent(Ships.Values);

        Ships.Add(ship.Id, ship);

        foreach (var coord in coordinates)
        {
            Board[coord] = ship.Id;
        }

    }

    private bool IsValidShipSize(int size)
    {
        if (!allowedShips.ContainsKey(size)) return false;

        int currentCount = Ships.Values.Count(s => s.Size == size);
        return currentCount < allowedShips[size];
    }

    public int GetMissingShipsCount(int size)
    {
        if (!allowedShips.ContainsKey(size))
        {
            throw new ArgumentException("Invalid ship size.");
        }

        int currentCount = Ships.Values.Count(s => s.Size == size);
        return allowedShips[size] - currentCount;
    }


    public string GetAllShipsInfo()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var shipEntry in Ships)
        {
            Ship ship = shipEntry.Value;
            sb.AppendLine($"Ship ID: {ship.Id}, Size: {ship.Size}");
            sb.Append("Coordinates: ");
            foreach (var coord in ship.Coordinates)
            {
                sb.Append($"{coord} ");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

}
