using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShip.BLL.GameLogic;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;
using BattleShip.BLL.Ships;

namespace BattleShip.UI
{
    internal class GamePlay
    {
        private PlaceShipRequest _PSR;
        private Board _b1;
        private Board _b2;
        private FireShotResponse _response;
        private Coordinate _c;

        public GamePlay()
        {
            _b1 = new Board();
            _b2 = new Board();
        }

        public void PlayGame(Player p1, Player p2)
        {
            displayWelcomeScreen();
            
            p1.GetName();
            p2.GetName();

            PlaceShips(p1, _b2);
            PlaceShips(p2, _b1);

            bool winner = false;
            int alternator = 0;
            string repeat = "";
            do
            {
                while (!winner)
                {
                    Console.Clear();

                    if (alternator%2 == 0)
                    {
                        winner = MakeAMove(_b1, p1);
                        alternator++;
                        if (winner)
                        {
                            Console.Clear();
                            Console.WriteLine("{0} WINS!!!!!", p1.PlayerName.ToUpper());
                        }
                    }
                    else
                    {
                        winner = MakeAMove(_b2, p2);
                        alternator++;
                        if (winner)
                        {
                            Console.Clear();
                            Console.WriteLine("{0} WINS!!!!", p2.PlayerName.ToUpper());
                        }
                    }
                }
                Console.Write("Press (Q) to quit: ");
                repeat = Console.ReadLine();
            } while (repeat.ToUpper() != "Q");

        }

        public static void displayWelcomeScreen()
        {
            Console.WriteLine(@"
         
                                           |
                                       ____|____
                                      [].[].[].[]
                           __          \+++++++/            ___
<<::::::::::::::::::::::::|__|          |_____|            |___\======> 
          /_______________|  |__________|_____|____________|   |____________    
          \                                                                 |
           \   ( )   ( )   ( )   ( )   ( )    ( )   ( )   ( )   ( )   ( )   |                      
            \                                                               |
             |                      Battleship v1.0                         |
            /                                                               |
           /________________________________________________________________/");


            Console.WriteLine("\n\n\n                               Press ENTER to continue..");
            Console.ReadLine();
        }

        public void PlaceShips(Player p, Board b)
        {
            _PSR = new PlaceShipRequest();

            Console.Clear();
            Console.WriteLine("It's {0}'s turn to place ships.\nPress ENTER to continue...", p.PlayerName);
            Console.ReadLine();
            for (int i = 0; i < 5; i++)
            {
                _PSR.ShipType = (ShipType) i;

                do
                {
                    while (true)
                    {
                        Console.Clear();
                        b.drawBoard(b.ShipListTranslator(b.shipPlacement));

                        Console.WriteLine("({0}/5)Place your {1}", i + 1, ((ShipType)i));
                        
                        Console.Write("Please enter a coordinate: ");
                        string coordinateInput = Console.ReadLine();

                        int result, x, y;

                        if (coordinateInput == "")
                        {
                            Console.WriteLine("Please enter a something.");
                            Console.WriteLine("Try again...");
                            Console.ReadLine();
                        }
                        else
                        {
                            coordinateInput = coordinateInput.ToUpper();

                            if (coordinateInput[0] >= 65 && coordinateInput[0] <= 74)
                            {
                                if (int.TryParse(coordinateInput.Substring(1), out result) &&
                                    (result >= 1 && result <= 10))
                                {
                                    x = coordinateInput[0] - 64;
                                    y = result;
                                    _PSR.Coordinate = new Coordinate(x, y);

                                    break;
                                }
                                else
                                {
                                    Console.WriteLine(
                                        "Invalid entry. Y coordinate must be a number between 1 and 10. Try again...");
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                Console.WriteLine(
                                    "Invalid entry. X coordinate must be a letter between A and J. Try again...");
                                Console.ReadLine();
                            }
                            Console.Clear();
                        }
                        }

                    
                   
                    while (true)
                    {
                        int result;
                        Console.Clear();
                        b.drawBoard(b.ShipListTranslator(b.shipPlacement));
                        Console.WriteLine("Good. Now enter a number for direction:");
                        Console.Write("1) Up\n2) Down\n3) Left\n4) Right\nEntry: ");

                        if (int.TryParse(Console.ReadLine(), out result) && (result >= 1 && result <= 4))
                        {
                            _PSR.Direction = (ShipDirection) result - 1;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid entry. Please pick a number betwee 1 and 4...");
                            Console.ReadLine();
                        }
                    }
                } while (!isPlacementValid(b.PlaceShip(_PSR)));

                Console.WriteLine("Thank you! Press ENTER to submit...");
                Console.ReadLine();
                Console.Clear();
            }
        }

        public bool isPlacementValid(ShipPlacement placement)
        {
            if (placement == ShipPlacement.Overlap)
            {
                Console.WriteLine("\nYour placement overlaps another ship.");
                Console.WriteLine("Press ENTER to try again...");
                Console.ReadLine();
                Console.Clear();
                return false;
            }
            else if (placement == ShipPlacement.NotEnoughSpace)
            {
                Console.WriteLine("\nNot enough space.");
                Console.WriteLine("Press ENTER to try again...");
                Console.ReadLine();
                Console.Clear();
                return false;
            }

            return true;
        }

        public bool MakeAMove(Board b, Player player)
        {
            Console.Clear();

            _response = new FireShotResponse();

            bool goodEntry = false;
            bool winner = false;

            while (!goodEntry)
            {
                b.drawBoard(b.DictionaryTranslator(b.ShotHistory));

                Console.WriteLine("{0}, it is your turn to shoot.", player.PlayerName);
                Console.Write("Enter a coordinate: ");
                
                string coordinateInput = Console.ReadLine();

                if (coordinateInput == "iddqd")
                {
                    Console.WriteLine("God mode activated. Press ENTER to continue...");
                    Console.ReadLine();
                    Console.Clear();
                    b.drawBoard(b.ShipListTranslator(b.shipPlacement));
                }

                else if (coordinateInput == "")
                {
                    Console.WriteLine("You need to enter something...");
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    coordinateInput = coordinateInput.ToUpper();
                    int y;

                    if (int.TryParse(coordinateInput.Substring(1), out y))
                    {
                        int x = coordinateInput[0] - 64;
                        _c = new Coordinate(x, y);
                        _response = b.FireShot(_c);

                        switch (_response.ShotStatus)
                        {
                            case ShotStatus.Invalid:
                                Console.WriteLine(
                                    "Out of bounds. Pick an X coordinate between A-J and Y coordinate between 1-10");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            case ShotStatus.Duplicate:
                                Console.WriteLine("You've fired at that position already. Try again...");
                                Console.ReadLine();
                                Console.Clear();
                                break;
                            case ShotStatus.Miss:
                                Console.WriteLine("Missed!");
                                Console.ReadLine();
                                Console.Clear();
                                goodEntry = true;
                                break;
                            case ShotStatus.Hit:
                                Console.WriteLine("Hit!!!");
                                Console.ReadLine();
                                Console.Clear();
                                goodEntry = true;
                                break;
                            case ShotStatus.HitAndSunk:
                                Console.WriteLine("You sunk it!!");
                                Console.ReadLine();
                                Console.Clear();
                                goodEntry = true;
                                break;
                            case ShotStatus.Victory:
                                goodEntry = true;
                                winner = true;
                                player.BattleShipWinCount++;
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Y coordinate must be an interger.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
            }
            return winner;
        }
    }
}