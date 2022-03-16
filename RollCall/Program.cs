using System;
using System.Collections.Generic;
using System.Linq;

namespace RollCall
{

    /*   
     *At the start of class, I like to call roll. I like to go through my list of students in alphabetical order.Where possible, I like to call students by their first names.Of course, if two students have the same first name,
     *I have to also give the last name so they know who I’m calling. Write a program to help me out. Given a class roll, it is going to tell how I should call the names.

    Input
    Input consists of up to  names, one per line, terminated by the end of file.Each line contains a first and a last name for a particular person.First and last names use to  letters (a–z),
    always starting with an uppercase letters first followed by only lowercase letters.No two people will have exactly the same first and last names.

    Output
    Print the list of names, one per line, sorted by last name. If two or more people have the same last name, order these people by first name. Where the first name is unambiguous,
    just list the first name.If two people have the same first name, also list their last names to resolve the ambiguity.

    Sample Input 1	Sample Output 1
    Will Smith
    Agent Smith
    Peter Pan
    Micky Mouse
    Minnie Mouse
    Peter Gunn
    Peter Gunn
    Micky
    Minnie
    Peter Pan
    Agent
    Will



        https://open.kattis.com/problems/rollcall
    */
    class Program
    {
        static void Main(string[] args)
        {
            List<Person> people = new List<Person>();

            string line;
            while ((line = Console.ReadLine()) != null && line != "x")
            {
                people.Add(Person.Parse(line));
            }

            people = people.OrderBy(p => p.LastName).ToList();


            for (int i = 0; i < people.Count; i++)
            {
                var peopleWithSameLastName = people.Where(p => p != people[i]).Where(p => p.LastName == people[i].LastName).ToList();

                if (peopleWithSameLastName.Count > 0)
                {
                    peopleWithSameLastName.Add(people[i]);
                    peopleWithSameLastName = peopleWithSameLastName.OrderBy(p => p.FirstName).ToList();

                    int lastIndex = 200;

                    for (int y = 0; y < peopleWithSameLastName.Count; y++)
                    {
                        int index = people.IndexOf(peopleWithSameLastName[y]);
                        if (lastIndex > index)
                        {
                            lastIndex = index;
                        }
                    }

                    foreach(var person in peopleWithSameLastName)
                    {
                        people.Remove(person);
                    }

                    people.InsertRange(lastIndex, peopleWithSameLastName);
                    
                }

            }

            foreach(var person in people)
            {
                int peopleWithSameFirstNameCount = people.Where(p => p != person).Where(p => p.FirstName == person.FirstName).Count();

                if (peopleWithSameFirstNameCount > 0)
                {
                    Console.WriteLine(person.FirstName + " " + person.LastName);
                }
                else
                {
                    Console.WriteLine(person.FirstName);
                }

            }
        }
    }

    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static Person Parse(string personInformation)
        {
            string[] splitName = personInformation.Split(' ');
            return new Person { FirstName = splitName[0], LastName = splitName[1] };
        }
    }
}