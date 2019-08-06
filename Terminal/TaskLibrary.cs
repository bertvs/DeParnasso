using DeParnasso.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DeParnasso.Harmoniser.MatterMethod;

namespace DeParnasso.Terminal
{
    public class TaskLibrary
    {
        public bool Test()
        {
            Console.WriteLine("testing...");

            var composition = new Composition("F major", "4/4");
            var soprano = new Melody("f'4 c''4 a'4 g'4 f'4");
            composition.AddVoice("Soprano", soprano);
            var harmoniser = new Worker(new Configuration(), composition);
            harmoniser.Execute();
            return true;
        }

        public bool TestMethod2()
        {
            Console.WriteLine("TestMethod2");
            return false;
        }
    }
}
