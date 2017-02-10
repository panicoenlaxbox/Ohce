using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Ohce
{
    public class OhceShould
    {
        //return the reverse echo of the word
        //When you introduce a palindrome, ohce likes it and after reverse-echoing it, it adds ¡Bonita palabra!
        //ohce knows when to stop, you just have to write Stop! and it'll answer Adios < your name > and end.
        //Between 20 and 6 hours, ohce will greet you saying: ¡Buenas noches < your name >!
        //Between 6 and 12 hours, ohce will greet you saying: ¡Buenos días<your name >!
        //Between 12 and 20 hours, ohce will greet you saying: ¡Buenas tardes < your name >!

        //        $ ohce Pedro
        //> ¡Buenos días Pedro!
        //$ hola
        //> aloh
        //$ oto
        //> oto
        //> ¡Bonita palabra!
        //$ stop
        //> pots
        //$ Stop!
        //> Adios Pedro

        [Fact]
        public void return_reserve_word()
        {
            var interpreter = new OhceInterpreter();
            interpreter.Echo("sergio").Should().Be("oigres");
        }

        [Fact]
        public void return_greet_and_reserve_with_palindrome_word()
        {
            var interpreter = new OhceInterpreter();
            interpreter.Echo("ana").Should().Be("ana\n¡Bonita palabra!");
        }
    }

    public class OhceInterpreter
    {
        public string Echo(string value)
        {
            var reverse = new string(value.Reverse().ToArray());
            if (value == reverse)
            {
                return $"{reverse}\n¡Bonita palabra!";
            }
            return reverse;
        }
    }
}
