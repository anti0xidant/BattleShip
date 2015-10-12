using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessingGame
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // ask for the player's name
            Console.Write("Please Enter your Name: ");
            string name = Console.ReadLine();

            // generating our random number
            Random random = new Random();
            int theAnswer = random.Next(1, 21);

            // loop until the player has guessed the random number
            int playerGuess;
            string playerInput;
            bool isNumberGuessed = false;
            do
            {
                // we have to get the player input
                Console.Write("{0}, Enter your guess: ", name);
                playerInput = Console.ReadLine();

                // we need to see if they entered a number
                if (int.TryParse(playerInput, out playerGuess))
                {
                    // check to see did they guess the number
                    if (playerGuess == theAnswer)
                    {
                        // congratulate them!
                        Console.WriteLine("{0}, YOU GOT IT!!!", name.ToUpper());
                        isNumberGuessed = true;
                    }
                    else
                    {
                        if ((playerGuess >= 1) && (playerGuess <= 20))
                        {
                            // they didn't guess the number
                            if (playerGuess > theAnswer)
                            {
                                Console.WriteLine("{0}, Too High!", name);
                            }
                            else
                            {
                                Console.WriteLine("{0}, Too Low!", name);
                            }
                        }
                        else
                        {
                            // they guessed a number outside of 1-20
                            Console.WriteLine("Invalid guess, {0}, please enter between 1 and 20", name);
                        }
                    }
                }
                else
                {
                    // they didn't enter a valid number
                    Console.WriteLine("That wasn't a valid number, {0}!", name);
                }

            } while (!isNumberGuessed);

            Console.WriteLine("{0}, Press any key to quit.", name);
            Console.ReadLine();
        }
    }
}
