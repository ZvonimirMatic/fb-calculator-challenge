using System;
using System.Collections.Generic;
using System.IO;

namespace CalculatorChallenge
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Directory.GetCurrentDirectory() + "\\calculator_challenge.txt";
            var resultString = "";
            using (StreamReader file = new StreamReader(path))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var expressionArray = line.Split(' ');
                    var expressionQueue = new Queue<string>();
                    for (var i = 1; i < expressionArray.Length; i++)
                        expressionQueue.Enqueue(expressionArray[i]);
                    var expression = new Expression(expressionQueue);
                    var number = Convert.ToInt32(expressionArray[0]);
                    var reverseExpression = expression.BuildReverseExpression(number);
                    var xValue = reverseExpression.CalculateExpression();
                    resultString += Math.Round(xValue) == xValue ? $"{xValue} " : "Err ";
                }
            }
            Console.WriteLine(resultString);
        }
    }
}
