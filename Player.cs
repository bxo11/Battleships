class Player
{
    public Dictionary<Coordinate, int> Board { get; private set; }
    public Dictionary<int, Ship> Ships { get; private set; }

    public List<Coordinate> Shoots { get; private set; }

    public Player()
    {
        Board = new Dictionary<Coordinate, int>();
        Ships = new Dictionary<int, Ship>();
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
        Shoots.Add(coordinate);
    }

    public bool IsHit(Coordinate coordinate)
    {
        return Board[coordinate] != 0;
    }

    public bool IsDefeated()
    {
        foreach (var ship in Ships.Values)
        {
            foreach (var coordinate in ship.Coordinates)
            {
                if (!Shoots.Contains(coordinate))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void PlaceShip(int size, List<Coordinate> coordinates) {
    int nextId = Ships.Count + 1;

    var ship = new Ship(nextId,size, coordinates);
    
    if (ship.AreCoordinatesValid() && ship.IsNotColliding(Ships.Values) && ship.IsNotAdjacentToOtherShips(Ships.Values))
    {
        Ships.Add(ship.Id, ship);

        foreach (var coord in coordinates)
        {
            Board[coord] = ship.Id;
        }
    }
}

 
}
