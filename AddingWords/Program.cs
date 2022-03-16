using System;
using System.Collections.Generic; 
using System.Linq;

/*Psychologists at Wassamatta University believe that humans are able to more easily deal with words than with numbers. So they have devised experiments to find out if this is true. In an interesting twist,
 * one of their experiments deals with using words which represent numbers. Rather than adding numbers, they want to add words. You are a research programmer on the project,
 * and your job is to write a program that demonstrates this ability.

Input
Input is a sequence of up to  commands, one per line, ending at end of file. Each command is a definition, a calculation, or a clear. All tokens within a command are separated by single spaces.

A definition has the format def x y where x is a variable name and y is a an integer in the range . No two variables are ever defined to have the same numeric value at the same time. 
If x had been defined previously, defining it again erases its old definition. Variable names are up to  lowercase characters, each character from the range a to z.

A calculation command starts with the word calc, and is followed by one to  more variable names separated by addition or subtraction operators. The end of a calc command is an equals sign. For example:

calc foo + bar - car =
The clear command instructs your program to forget all of its definitions.

Output
Your program should produce no output for definitions, but for calculations it should produce the value of the calculation. If there is not a word for the result, or some word in the calculation has not been defined,
then the result of the calculation should be unknown. The word unknown is never used as a variable in the input.

Sample input: 
def foo 3
calc foo + bar =
def bar 7
def programming 10
calc foo + bar =
def is 4
def fun 8
calc programming - is + fun =
def fun 1
calc programming - is + fun =
clear


Sample output:
foo + bar = unknown
foo + bar = programming
programming - is + fun = unknown
programming - is + fun = bar
 * 
 * 
 * 
 * 
 * https://open.kattis.com/problems/addingwords
*/


namespace AddingWords
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            while ((line = Console.ReadLine()) != null && line != "x")
            {
                string[] lineSplit = line.Split(' ');
                switch(lineSplit[0])
                {
                    case "def":
                        CommandProcessor.AddDefinition(line);
                        break;
                    case "calc":
                        if (line.EndsWith("="))
                        {
                            CommandProcessor.Calc(line);
                        }
                        break;
                    case "clear":
                        CommandProcessor.Clear();
                        break;
                }
            }
        }
    }

    static class CommandProcessor
    {
        private static Dictionary<string, int> definitions = new Dictionary<string, int>();

        public static void Clear()
        {
            definitions = new Dictionary<string, int>();
        }

        public static void AddDefinition(string command)
        {
            string[] commandSplit = command.Split(' ');
            string name = commandSplit[1];
            int value = int.Parse(commandSplit[2]);
            if (name != "unknown")
            {
                if (definitions.ContainsKey(name))
                {
                    definitions.Remove(name);
                    definitions.Add(name, value);
                }
                else
                {
                    if (!definitions.Values.Contains(value))
                    {
                        definitions.Add(name, value);
                    }
                    else throw new InvalidOperationException($"Value {value} already exists within this context.");
                }
            }
            else throw new InvalidOperationException("'unknown' is not a valid definition. It is reserved by the system.");
        }

        public static void Calc(string command)
        {
            string answer = "";
            string[] commandSplit = command.Split(' ');

            int total = 0;

            string currentOperator = "";
            bool definitionNotFound = false;

            for (int i = 1; i < commandSplit.Length; i++)
            {
                if (i == 1)
                {
                    bool foundValue = definitions.TryGetValue(commandSplit[i], out int value);
                    if (!foundValue)
                    {
                        definitionNotFound = true;
                    }

                    answer += commandSplit[i] + " ";
                    total = value;
                }

                switch (commandSplit[i])
                {
                    case "+":
                        currentOperator = "+";
                        answer += currentOperator;
                        answer += " ";
                        break;
                    case "-":
                        currentOperator = "-";
                        answer += currentOperator;
                        answer += " ";
                        break;
                }

                if (currentOperator == "+" && commandSplit[i] != "+" && commandSplit[i] != "-" && commandSplit[i] != "=")
                {
                    answer += commandSplit[i] + " ";

                    bool foundValue = definitions.TryGetValue(commandSplit[i], out int value);
                    total += value;

                    if (!foundValue)
                    {
                        definitionNotFound = true;
                    }
                }
                else if (currentOperator == "-" && commandSplit[i] != "+" && commandSplit[i] != "-" && commandSplit[i] != "=")
                {
                    answer += commandSplit[i] + " ";

                    bool foundValue = definitions.TryGetValue(commandSplit[i], out int value);
                    total -= value;

                    if (!foundValue)
                    {
                        definitionNotFound = true;
                    }
                }

            }

            if (!definitionNotFound)
            {
                var definition = definitions.Where(d => d.Value == total).ToList();
                if (definition.Count > 0)
                {
                    answer += "= " + definition[0].Key;
                }
                else
                {
                    answer += "= unknown";
                }
            }
            else
            {
                answer += "= unknown";
            }
            Console.WriteLine(answer);
        }


    }

}