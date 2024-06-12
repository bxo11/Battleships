using System;
using System.Collections.Generic;

public class CoordinateGenerator
{
    private HashSet<Coordinate> generatedCoordinates;
    private Random random;
    private const int MaxColumn = 10;

    public CoordinateGenerator()
    {
        generatedCoordinates = new HashSet<Coordinate>();
        random = new Random();
    }

    public Coordinate GetRandomCoordinate()
    {
        Coordinate coordinate;
        do
        {
            Row row = (Row)random.Next(1, Enum.GetNames(typeof(Row)).Length + 1);
            int column = random.Next(1, MaxColumn + 1);
            coordinate = new Coordinate(row, column);
        }
        while (generatedCoordinates.Contains(coordinate));

        generatedCoordinates.Add(coordinate);
        return coordinate;
    }

}

