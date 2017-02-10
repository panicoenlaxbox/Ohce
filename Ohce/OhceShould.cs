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
            var interpreter = GivenAnOhceInterpreter();
            interpreter.Echo("sergio").Should().Be("oigres");
        }

        [Fact]
        public void return_greet_and_reserve_with_palindrome_word()
        {
            var interpreter = GivenAnOhceInterpreter();
            interpreter.Echo("ana").Should().Be("ana\n¡Bonita palabra!");
        }

        [Fact]
        public void return_stop_and_bye_your_name()
        {
            var interpreter = GivenAnOhceInterpreter(name:"Pedro");
            interpreter.Echo("Stop!").Should().Be("Adios Pedro");
        }

        private static OhceInterpreter GivenAnOhceInterpreter(string name="anyName")
        {
            return new OhceInterpreter(name);
        }
    }

    public class OhceInterpreter
    {
        private string _name;

        public OhceInterpreter(string name)
        {
            _name = name;
        }

        public string Echo(string value)
        {
            if (value == "Stop!")
            {
                return $"Adios {_name}";
            }
            var reverse = new string(value.Reverse().ToArray());
            if (value == reverse)
            {
                return $"{reverse}\n¡Bonita palabra!";
            }
            return reverse;
        }
    }
}
