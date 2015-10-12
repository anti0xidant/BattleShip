using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BattleShip.BLL.GameLogic;
using BattleShip.BLL.Requests;
using BattleShip.BLL.Responses;
using BattleShip.BLL.Ships;

namespace BattleShip.UI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Player p1 = new Player(1);
            Player p2 = new Player(2);

            GamePlay BattleShip = new GamePlay();

            BattleShip.PlayGame(p1, p2);
        }
    }
}