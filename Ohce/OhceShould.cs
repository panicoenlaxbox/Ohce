using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ohce
{
    public class OhceShould
    {
        private readonly IConsole _console = Substitute.For<IConsole>();
        private readonly ITimeProvider _timeProvider = Substitute.For<ITimeProvider>();
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
            interpreter.Echo("sergio");
            _console.Received(1).Write("oigres");
        }

        [Fact]
        public void return_greet_and_reserve_with_palindrome_word()
        {
            var interpreter = GivenAnOhceInterpreter();
            interpreter.Echo("ana");
            _console.Received(1).Write("ana\n¡Bonita palabra!");
        }

        [Fact]
        public void return_stop_and_bye_your_name()
        {
            var interpreter = GivenAnOhceInterpreter(name: "Pedro");
            interpreter.Echo("Stop!");
            _console.Received(1).Write("Adios Pedro");
        }

        [Fact]
        public void greet_at_night()
        {
            _timeProvider.GetCurrentTime().Returns(new DateTime(2017, 02, 10, 20, 30, 00));
            GivenAnOhceInterpreter(name: "Pedro");
            _console.ReceivedCalls().Count().Should().Be(1);
            _console.Received(1).Write("Buenas noches Pedro");
        }


        [Fact]
        public void greet_in_the_morning()
        {
            _timeProvider.GetCurrentTime().Returns(new DateTime(2017, 02, 10, 06, 30, 00));
            GivenAnOhceInterpreter(name: "Pedro");
            _console.ReceivedCalls().Count().Should().Be(1);
            _console.Received(1).Write("Buenos días Pedro");
        }

        [Fact]
        public void greet_in_the_evening()
        {
            _timeProvider.GetCurrentTime().Returns(new DateTime(2017, 02, 10, 16, 30, 00));
            GivenAnOhceInterpreter(name: "Pedro");
            _console.ReceivedCalls().Count().Should().Be(1);
            _console.Received(1).Write("Buenas tardes Pedro");
        }


        private OhceInterpreter GivenAnOhceInterpreter(string name = "anyName")
        {
            return new OhceInterpreter(name, _console, _timeProvider);
        }
    }

    public interface ITimeProvider
    {
        DateTime GetCurrentTime();
    }

    public interface IConsole
    {
        void Write(string value);
    }

    public class OhceInterpreter
    {
        private readonly string _name;
        private IConsole _console;
        private readonly ITimeProvider _timeProvider;

        public OhceInterpreter(string name, IConsole console, ITimeProvider timeProvider)
        {
            _console = console;
            _timeProvider = timeProvider;
            _name = name;
            GreetByTime();
        }

        private void GreetByTime()
        {
            if (IsMorning())
            {
                _console.Write($"Buenos días {_name}");
            }
            else if (IsEvening())
            {
                _console.Write($"Buenas tardes {_name}");
            }
            else
            {
                _console.Write($"Buenas noches {_name}");
            }
        }

        private bool IsEvening()
        {
            var hour = _timeProvider.GetCurrentTime().Hour;
            return hour >= 12 && hour < 20;
        }

        private bool IsMorning()
        {
            var hour = _timeProvider.GetCurrentTime().Hour;
            return hour >= 6 && hour <= 12;
        }

        public void Echo(string value)
        {
            if (value == "Stop!")
            {
                _console.Write($"Adios {_name}");
            }
            var reverse = new string(value.Reverse().ToArray());
            if (value == reverse)
            {
                _console.Write($"{reverse}\n¡Bonita palabra!");
            }
            _console.Write(reverse);
        }
    }
}
