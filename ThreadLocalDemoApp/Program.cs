using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadLocalDemoApp
{
    class ThreadLocalDemo
    {

        // Demonstrates:
        //      ThreadLocal(T) constructor
        //      ThreadLocal(T).Value
        //      One usage of ThreadLocal(T)

        class Juttu
        {
            public int age;
            public string name;
        }
        static void Main()
        {
            // Thread-Local variable that yields a name for a thread
            /* ThreadLocal<string> ThreadName = new ThreadLocal<string>(() =>
             {
                 return "Thread" + Thread.CurrentThread.ManagedThreadId;
             });
            */

            ThreadLocal<Juttu> ThreadName = new ThreadLocal<Juttu>(() =>
            {
                Juttu dummy = new Juttu();
                dummy.name = "Thread" + Thread.CurrentThread.ManagedThreadId;
                dummy.age = Thread.CurrentThread.ManagedThreadId;
                return dummy;
            });

            // Action that prints out ThreadName for the current thread
            Action action = () =>
            {
                // If ThreadName.IsValueCreated is true, it means that we are not the
                // first action to run on this thread.
                bool repeat = ThreadName.IsValueCreated;


               Console.WriteLine("ThreadName = {0} {1}", ThreadName.Value.name, repeat ? "(repeat)" : "");

                //Console.WriteLine("ThreadName = {0} {1}", ThreadName.Value, repeat ? "(repeat)" : "");
            };

            // Launch eight of them.  On 4 cores or less, you should see some repeat ThreadNames
            Parallel.Invoke(action, action, action, action, action, action, action, action,
                action, action, action, action, action, action, action, action,
                action, action, action, action, action, action, action, action);

            // Dispose when you are done
            ThreadName.Dispose();

            //-----------------------------------------
            //Timer test

            // First interval = 5000ms; subsequent intervals = 1000ms
            System.Threading.Timer tmr = new Timer(Tick, "tick...", 2500, 1000);
            tmr.Dispose();         // This both stops the timer and cleans up.

            // tämä on .New oma timer
            System.Timers.Timer tmrner = new System.Timers.Timer(500);
            tmrner.Elapsed += Tock;
            tmrner.Start();

            Console.ReadLine();

            tmrner.Dispose();

            Console.ReadLine();

        }
        static void Tick(object data)
        {
            // This runs on a pooled thread
            Console.WriteLine(data);          // Writes "tick..."
        }

        static void Tock(object sender, EventArgs e)
        {
            Console.WriteLine("Tock...");
        }
    }
}
