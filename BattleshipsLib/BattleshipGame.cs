using System.Numerics;
using System.Reflection.Emit;

namespace BattleshipsLib
{
    public class BattleshipGame
    {
        private Player player;
        private Player opponent;
        private CoordinateGenerator shootsGenerator = new CoordinateGenerator();

        public BattleshipGame()
        {
            player = new Player();
            opponent = new Player();

            ShipPlacementGenerator generator = new ShipPlacementGenerator(opponent);
            generator.PlaceAllShipsRandomly();
        }

        public void PlaceShip(int size, List<Coordinate> coordinates)
        {
            player.PlaceShip(size, coordinates);

        }

        public bool Attack(Coordinate coord)
        {
            player.Shoot(coord);
            bool isHit = opponent.IsHit(coord);

            opponent.Shoot(shootsGenerator.GetRandomCoordinate());

            return isHit;
        }

        public bool isGameOver()
        {
            return player.IsDefeated(opponent.Shoots) || opponent.IsDefeated(player.Shoots);
        }

        public bool IsPlayerVictory()
        {
            return opponent.IsDefeated(player.Shoots);
        }


        public int GetMissingShipsCount(int size)
        {
            return player.GetMissingShipsCount(size);
        }

        public List<Coordinate> GetCoordinatesOfPlacedShips(bool forOpponent = true)
        {
            if (!forOpponent)
                return player.Board.Where(pair => pair.Value != 0).Select(pair => pair.Key).ToList();
            else
                return opponent.Board.Where(pair => pair.Value != 0).Select(pair => pair.Key).ToList();
        }


        public List<Coordinate> GetHitCoordinates(bool forOpponent = true)
        {
            var hitCoordinates = new List<Coordinate>();
            Player p = forOpponent ? opponent : player;
            Player o = forOpponent ? player : opponent;

            foreach (var shoot in p.Shoots)
            {
                if (o.Board.ContainsKey(shoot) && o.Board[shoot] != 0)
                {
                    hitCoordinates.Add(shoot);
                }
            }
            return hitCoordinates;
        }

        public List<Coordinate> GetMissedCoordinates(bool forOpponent = true)
        {
            var missedCoordinates = new List<Coordinate>();
            Player p = forOpponent ? opponent : player;
            Player o = forOpponent ? player : opponent;

            foreach (var shoot in p.Shoots)
            {
                if (!o.Board.ContainsKey(shoot) || o.Board[shoot] == 0)
                {
                    missedCoordinates.Add(shoot);
                }
            }
            return missedCoordinates;
        }
    }

}
