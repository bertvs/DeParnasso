using System;
using System.Linq;

namespace DeParnasso.Terminal
{
    static class Program
    {

        // test hidden parallell check

        // unittests

        // ciphered bass from harmonised melody, harmony from ciphered bass

        // implement rests in melody


        // new: harmonisation in I,IV,V using The Lost Chord Stephen Taylor


        // work with barnumbers, how?

        // read and write .cp files, also with .ly

        // todo: melody operations : inversion, retrograde, retrograde inversion, augmentation, diminution, move.

        // harmony get root / analyse
        // todo: chord class, property of harmony (eg CMaj, etc.), derive based on tones in harmony
        // todo: functional harmony: the function of a chord related to the tonal center (key, degree)
        // chord Has root, quality (minor, major, augmented, diminished, half-diminished, dominant), inversion, position(open vs closed)

        // todo: pitch enharmonize up/down

        // todo: counterpoint (input: 1 or 2 melodies), with options (gradus ad parnassum), grade output based on quality and number of operations needed

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Enter one of the following commands, or type 'exit' to exit:");
                var taskList = typeof(TaskLibrary).GetMethods().Where(m => m.DeclaringType == typeof(TaskLibrary));

                foreach (var item in taskList)
                {
                    Console.WriteLine("> " + item.Name.ToLower());
                }

                var command = Console.ReadLine();

                if (command.ToLower() == "exit")
                {
                    break;
                }

                var task = taskList.SingleOrDefault(m => m.Name.ToLower() == command);

                if (task == null)
                {
                    Console.WriteLine($"Could not find task with name {command}!");
                    Console.WriteLine();
                    continue;
                }

                Console.WriteLine($"Executing task {task.Name}...");
				bool result;

                try
                {
                    result = (bool)task.Invoke(new TaskLibrary(), new object[] { });
                }
                catch (Exception e)
                {
                    result = false;
                    Console.WriteLine($"An exception occurred: {e.InnerException.Message}");
                }

                Console.WriteLine($"Task {task.Name} executed, result is {result}.");
                Console.WriteLine();
            }
        }
    }
}
