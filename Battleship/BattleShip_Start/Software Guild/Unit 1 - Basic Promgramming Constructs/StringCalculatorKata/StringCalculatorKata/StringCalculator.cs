using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StringCalculatorKata
{
    public class StringCalculator
    {
        public static int Add(string numbers)
        {
           
            int sum = 0;
                
            if (numbers == "") // If empty input
            {
                return 0;
            }

            else if (numbers.StartsWith("//")) // If assining a deliminator
            {
                char delimiter = numbers[2]; // capture deliminator
                string substring = numbers.Substring(3); // cut out //[del]

                // fixes the position 0 deliminator problem
                if (substring.StartsWith(",") || substring.StartsWith("\n")) 
                {
                    if (substring.StartsWith(","))
                    {
                        string beginsCommaString = substring.Substring(1);

                        string[] array = beginsCommaString.Split(',', '\n', delimiter);

                        foreach (string parsedStuff in array)
                        {
                            sum += int.Parse(parsedStuff);
                        }
                        return sum;
                    }
                    else
                    {
                        string beginsLineBreak = substring.Substring(1);
                        string[] array = beginsLineBreak.Split(',', '\n', delimiter);

                        foreach (string parsedStuff in array)
                        {
                            sum += int.Parse(parsedStuff);
                        }
                        return sum;
                    }
                }
                else
                {
                    string[] array = substring.Split(',', '\n', delimiter);

                    foreach (string parsedStuff in array)
                    {
                        sum += int.Parse(parsedStuff);

                    }
                    return sum;
                }

            }
            else // if not assining deliminator 
            {
                string[] array = numbers.Split(',', '\n');

                foreach (string parsedStuff in array)
                {
                    sum += int.Parse(parsedStuff);
                }
                return sum;
            }
        }
    }
}

