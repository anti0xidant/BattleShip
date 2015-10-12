using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShip.BLL.Requests;

namespace BattleShip.UI
{
    public class Player
    {
        public string PlayerName { get; set; }
        private readonly int _playerNumber;
        public int BattleShipWinCount;

        public Player(int playerNumber)
        {
            _playerNumber = playerNumber;
        }

        public void GetName()
        {
            Console.Clear();
            Console.Write("Player {0}, what is your name? ", _playerNumber);
            PlayerName = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Greetings, Captain {0}!", PlayerName);
            Console.ReadLine();
        }
    }
}