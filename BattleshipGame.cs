using System;
using System.Diagnostics.CodeAnalysis;

class BattleshipGame
{
    private Player player;
    private Player opponent;
    private bool opponentTurn;

    public BattleshipGame(bool opponentFirst)
    {
        player = new Player();
        opponent = new Player();
        opponentTurn = opponentFirst;
    }

    public void PlaceShip(int size, List<Coordinate> coordinates)
    {
        player.PlaceShip(size, coordinates);
    }

    public bool Attack(Coordinate coord)
    {
        CurrentPlayer().Shoot(coord);
        bool isHit =  OpponentPlayer().IsHit(coord);
        SwapTurn();

        return isHit;
    }

    public bool isGameOver() {
        return OpponentPlayer().IsDefeated();
    }

    private Player CurrentPlayer()
    {
        if (opponentTurn)
        {
            return opponent;
        }
        else
        {
            return  player;
        }
    }
    private Player OpponentPlayer()
    {
        if (!opponentTurn)
        {
            return opponent;
        }
        else
        {
            return  player;
        }
    }

    private void SwapTurn()
    {
        opponentTurn = !opponentTurn;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Random random = new Random();
        bool opponentFirst = random.Next(2) == 0;

        BattleshipGame game = new BattleshipGame(opponentFirst);



    }
}
