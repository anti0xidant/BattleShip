using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Policy;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;
using BattleShip.BLL.Ships;

namespace BattleShip.BLL.GameLogic
{
    public class Board
    {
        public Dictionary<Coordinate, ShotHistory> ShotHistory;
        public List<Ship> shipPlacement;
        private Ship[] _ships;
        private int _currentShipIndex;
        

        public Board()
        {
            ShotHistory = new Dictionary<Coordinate, ShotHistory>();
            shipPlacement = new List<Ship>();
            _ships = new Ship[5];
            _currentShipIndex = 0;

        }

        public FireShotResponse FireShot(Coordinate coordinate)
        {
            var response = new FireShotResponse();

            // is this coordinate on the board?
            if (!IsValidCoordinate(coordinate))
            {
                response.ShotStatus = ShotStatus.Invalid;
                return response;
            }

            // did they already try this position?
            if (ShotHistory.ContainsKey(coordinate))
            {
                response.ShotStatus = ShotStatus.Duplicate;
                return response;
            }

            CheckShipsForHit(coordinate, response);
            CheckForVictory(response);

            return response;
        }

        private void CheckForVictory(FireShotResponse response)
        {
            if (response.ShotStatus == ShotStatus.HitAndSunk)
            {
                // did they win?
                if (_ships.All(s => s.IsSunk))
                    response.ShotStatus = ShotStatus.Victory;
            }
        }

        private void CheckShipsForHit(Coordinate coordinate, FireShotResponse response)
        {
            response.ShotStatus = ShotStatus.Miss;

            foreach (var ship in _ships)
            {
                // no need to check sunk ships
                if (ship.IsSunk)
                    continue;

                ShotStatus status = ship.FireAtShip(coordinate);

                switch (status)
                {
                    case ShotStatus.HitAndSunk:
                        response.ShotStatus = ShotStatus.HitAndSunk;
                        response.ShipImpacted = ship.ShipName;
                        ShotHistory.Add(coordinate, Responses.ShotHistory.Hit);
                        break;
                    case ShotStatus.Hit:
                        response.ShotStatus = ShotStatus.Hit;
                        response.ShipImpacted = ship.ShipName;
                        ShotHistory.Add(coordinate, Responses.ShotHistory.Hit);
                        break;
                }

                // if they hit something, no need to continue looping
                if (status != ShotStatus.Miss)
                    break;
            }

            if (response.ShotStatus == ShotStatus.Miss)
            {
                ShotHistory.Add(coordinate, Responses.ShotHistory.Miss);
            }
        }

        private bool IsValidCoordinate(Coordinate coordinate)
        {
            return coordinate.XCoordinate >= 1 && coordinate.XCoordinate <= 10 &&
                   coordinate.YCoordinate >= 1 && coordinate.YCoordinate <= 10;
        }

        public ShipPlacement PlaceShip(PlaceShipRequest request)
        {
            if (_currentShipIndex > 4)
                throw new Exception("You can not add another ship, 5 is the limit!");

            if (!IsValidCoordinate(request.Coordinate))
                return ShipPlacement.NotEnoughSpace;

            Ship newShip = ShipCreator.CreateShip(request.ShipType);
            switch (request.Direction)
            {
                case ShipDirection.Down:
                    return PlaceShipDown(request.Coordinate, newShip);
                case ShipDirection.Up:
                    return PlaceShipUp(request.Coordinate, newShip);
                case ShipDirection.Left:
                    return PlaceShipLeft(request.Coordinate, newShip);
                default:
                    return PlaceShipRight(request.Coordinate, newShip);
            }
        }

        private ShipPlacement PlaceShipRight(Coordinate coordinate, Ship newShip)
        {
            // x coordinate gets bigger
            int positionIndex = 0;
            int maxX = coordinate.XCoordinate + newShip.BoardPositions.Length;

            for (int i = coordinate.XCoordinate; i < maxX; i++)
            {
                var currentCoordinate = new Coordinate(i, coordinate.YCoordinate);

                if (!IsValidCoordinate(currentCoordinate))
                    return ShipPlacement.NotEnoughSpace;

                if (OverlapsAnotherShip(currentCoordinate))
                    return ShipPlacement.Overlap;

                newShip.BoardPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return ShipPlacement.Ok;
        }

        private ShipPlacement PlaceShipLeft(Coordinate coordinate, Ship newShip)
        {
            // x coordinate gets smaller
            int positionIndex = 0;
            int minX = coordinate.XCoordinate - newShip.BoardPositions.Length;

            for (int i = coordinate.XCoordinate; i > minX; i--)
            {
                var currentCoordinate = new Coordinate(i, coordinate.YCoordinate);

                if (!IsValidCoordinate(currentCoordinate))
                    return ShipPlacement.NotEnoughSpace;

                if (OverlapsAnotherShip(currentCoordinate))
                    return ShipPlacement.Overlap;

                newShip.BoardPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return ShipPlacement.Ok;
        }

        private ShipPlacement PlaceShipUp(Coordinate coordinate, Ship newShip)
        {
            // y coordinate gets smaller
            int positionIndex = 0;
            int minY = coordinate.YCoordinate - newShip.BoardPositions.Length;

            for (int i = coordinate.YCoordinate; i > minY; i--)
            {
                var currentCoordinate = new Coordinate(coordinate.XCoordinate, i);

                if (!IsValidCoordinate(currentCoordinate))
                    return ShipPlacement.NotEnoughSpace;

                if (OverlapsAnotherShip(currentCoordinate))
                    return ShipPlacement.Overlap;

                newShip.BoardPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return ShipPlacement.Ok;
        }

        private ShipPlacement PlaceShipDown(Coordinate coordinate, Ship newShip)
        {
            // y coordinate gets bigger
            int positionIndex = 0;
            int maxY = coordinate.YCoordinate + newShip.BoardPositions.Length;

            for (int i = coordinate.YCoordinate; i < maxY; i++)
            {
                var currentCoordinate = new Coordinate(coordinate.XCoordinate, i);
                if (!IsValidCoordinate(currentCoordinate))
                    return ShipPlacement.NotEnoughSpace;

                if (OverlapsAnotherShip(currentCoordinate))
                    return ShipPlacement.Overlap;

                newShip.BoardPositions[positionIndex] = currentCoordinate;
                positionIndex++;
            }

            AddShipToBoard(newShip);
            return ShipPlacement.Ok;
        }

        private void AddShipToBoard(Ship newShip)
        {
            shipPlacement.Add(newShip);
            _ships[_currentShipIndex] = newShip;
            _currentShipIndex++;
        }

        private bool OverlapsAnotherShip(Coordinate coordinate)
        {
            foreach (var ship in _ships)
            {
                if (ship != null)
                {
                    if (ship.BoardPositions.Contains(coordinate))
                        return true;
                }
            }

            return false;
        }

        public char[,] DictionaryTranslator(Dictionary<Coordinate, ShotHistory> shothistory)
        {
            char[,] boardPositions = new char[10, 10];
            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    boardPositions[i, j] = '-';
                }
            }

            foreach (var member in shothistory)
            {
                // convert coordinate into i and j
                int i, j;
                i = member.Key.XCoordinate - 1;
                j = member.Key.YCoordinate - 1;

                // store ShotHistory into boardPositions[i,j]
                char value;
                value = member.Value.ToString()[0];

                boardPositions[i, j] = value;
            }

            return boardPositions;
        }

        public char[,] ShipListTranslator(List<Ship> shipList)
        {
            char[,] boardPositions = new char[10, 10];
            for (int j = 0; j < 10; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    boardPositions[i, j] = '-';
                }
            }

            foreach (Ship ship in shipList)
            {
                foreach (Coordinate coordinate in ship.BoardPositions)
                {
                    // convert coordinate into i and j
                    int i, j;
                    i = coordinate.XCoordinate - 1;
                    j = coordinate.YCoordinate - 1;

                    boardPositions[i, j] = 'X';
                }
            }

            return boardPositions;
        }
        public void drawBoard(char[,] information)
        {
            Console.WriteLine("    A   B   C   D   E   F   G   H   I   J");
            for (int j = 0; j < 10; j++)
            {
                if (j != 9)
                {
                    Console.Write("{0}   ", j + 1);
                }
                else
                {
                    Console.Write("{0}  ", j + 1);
                }
                

                for (int i = 0; i < 10; i++)
                {
                    if (information[i, j] == 'M')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    if (information[i, j] == 'H')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    
                    Console.Write("{0}   ", information[i, j]);
                    Console.ResetColor();

                    if (i == 9)
                    {
                        Console.WriteLine("");
                    }
                }
            }
        }
    }
}
