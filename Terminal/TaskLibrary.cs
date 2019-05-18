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

            var composition = new Composition("C major", "4/4");
            var soprano = new Melody("c'4 e'4 d'4");
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
