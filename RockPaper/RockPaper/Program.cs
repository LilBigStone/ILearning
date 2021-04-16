using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace RockPaper
{
    internal class Program
    {
        public static byte[] GetRandomKey()
        {
            RNGCryptoServiceProvider rngCrypt = new RNGCryptoServiceProvider();
            byte[] tokenBuffer = new byte[128];        // `int32` takes 4 bytes in C#
            rngCrypt.GetBytes(tokenBuffer);
            return tokenBuffer;
            
        }
        public static string GetHMAC(string data)
        {
            byte[] hmac = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(data));
            StringBuilder builder = new StringBuilder();  
            for (int i = 0; i < hmac.Length; i++)  
            {  
                builder.Append(hmac[i].ToString("x2"));  
            } 
            return builder.ToString();
        }
        
        public static void Main(string[] args)
        {
            string[] gameVariants = args;
            while(true){
                string argsString;
                
                if (gameVariants.Length == 0) {
                    Console.WriteLine("Please Enter arguments. Example: Rock Paper Scissors");
                    argsString = Console.ReadLine();
                    gameVariants = argsString.Split(' ');
                }
                else if (gameVariants.Length % 2 == 0)
                {
                    Console.WriteLine("Please enter odd number of arguments. Example: Rock Paper Scissors");
                    argsString = Console.ReadLine();
                    gameVariants = argsString.Split(' ');
                }
                else
                {
                    for (int i = 0; i < gameVariants.Length; i++)
                    {
                        if ( gameVariants.Count(s => s.Equals(gameVariants[i])) > 1)
                        {
                            Console.WriteLine("Please enter not duplicates arguments. Example: Rock Paper Scissors"); 
                            argsString = Console.ReadLine();
                            gameVariants = argsString.Split(' ');
                            continue; 
                        }
                    }
                    break;
                }
            }
            
            byte[] secureKey = GetRandomKey();
            Random rnd = new Random(BitConverter.ToInt32(secureKey,0));
            int computerTurn = rnd.Next(1, gameVariants.Length+1);
            string hmac = GetHMAC(BitConverter.ToString(secureKey));
            
            Console.WriteLine("HMAC:{0}", hmac);
            hmac = GetHMAC(computerTurn.ToString());
            
            Console.WriteLine("Available moves:");
            for (int i = 1; i <= gameVariants.Length; i++)
            {
                Console.WriteLine("{0} - {1}",i,gameVariants[i-1]);
            }
            Console.WriteLine("0 - Exit\nEnter ur move:");
            int urMove;
            while (true)
            {
                string enterTurn = Console.ReadLine();
                if (int.TryParse(enterTurn,out urMove))
                {
                    if (urMove >= 0 && urMove <= gameVariants.Length)
                    {
                        if (urMove == 0)
                        {
                            return;
                        }
                        break;
                    }

                }
                else
                {
                    Console.WriteLine("Invalid Enter. Please try again");
                }
            }
            List<int> win = new List<int>();
            for (int i = urMove-2,k=0; k < gameVariants.Length/2; --i,k++)
            {
                if (i < 0)
                {
                    win.Add(gameVariants.Length + i);  
                }
                else
                {
                    win.Add(i);  
                }
                
            }
            
            bool check = false;
            
            foreach (var i in win)
            {
                if (i == computerTurn-1)
                { 
                    check = true;
                }

            }
            Console.WriteLine("Your move:{0}\nComputer move: {1}", gameVariants[urMove-1],gameVariants[computerTurn-1]);
            if (urMove == computerTurn)
            {
                Console.WriteLine("Draw");  
            }
            else if (check)
            {
                Console.WriteLine("you Win!");
            }
            else
            { 
                Console.WriteLine("you Defeat!");
            }
            Console.WriteLine("HMAC: {0}", hmac);
            
            
        }
    }
}