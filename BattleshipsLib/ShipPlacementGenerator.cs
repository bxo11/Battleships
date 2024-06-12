using System;
using System.Collections.Generic;
using System.Linq;

 class ShipPlacementGenerator
{
    private readonly Random random;
    private readonly Player player;

    public ShipPlacementGenerator(Player player)
    {
        this.player = player;
        this.random = new Random();
    }

    public void PlaceAllShipsRandomly()
    {
        foreach (var shipSize in player.allowedShips.Keys)
        {
            int shipsToPlace = player.allowedShips[shipSize];
            for (int i = 0; i < shipsToPlace; i++)
            {
                PlaceShipRandomly(shipSize);
            }
        }
    }

    private void PlaceShipRandomly(int size)
    {
        bool shipPlaced = false;
        while (!shipPlaced)
        {
            try
            {
                List<Coordinate> coordinates = GenerateRandomShipCoordinates(size);
                player.PlaceShip(size, coordinates);
                shipPlaced = true;
            }
            catch
            {
                // if an exception is thrown, try again with a new set of coordinates
            }
        }
    }

    private List<Coordinate> GenerateRandomShipCoordinates(int size)
    {
        Row randomRow = Coordinate.IntToRow(random.Next(1, 11)); 
        int randomColumn = random.Next(1, 11);
        Coordinate startCoord = new Coordinate(randomRow, randomColumn);

        bool horizontal = random.Next(0, 2) == 0;
        return GetShipCoordinates(startCoord, size, horizontal);
    }

    private List<Coordinate> GetShipCoordinates(Coordinate start, int size, bool horizontal)
    {
        List<Coordinate> coordinates = new List<Coordinate>();
        for (int i = 0; i < size; i++)
        {
            int row = horizontal ? (int)start.Row : (int)start.Row + i;
            int column = horizontal ? start.Column + i : start.Column;

            if (row < 1 || row > 10 || column < 1 || column > 10)
            {
                throw new ArgumentException("Ship placement is out of bounds.");
            }

            coordinates.Add(new Coordinate((Row)row, column));
        }
        return coordinates;
    }

  
}
