using System;
using System.Collections;
using System.Threading;

namespace G9AssemblyManagement.Core.Class.MultiThreading
{
    public class G9CAsyncTask
    {
        public EventHandler Completed = delegate { };

        private IEnumerable WorkAsync(Action nextStep)
        {
            using (var timer = new Timer(_ => nextStep()))
            {
                timer.Change(0, 500);

                var tick = 0;
                while (tick < 10)
                {
                    // resume upon next timer tick
                    yield return Type.Missing;
                    Console.WriteLine("Tick: " + tick++);
                }
            }

            Completed(this, EventArgs.Empty);
        }

        public void Start()
        {
            IEnumerator enumerator = null;
            Action nextStep = () => enumerator.MoveNext();
            enumerator = WorkAsync(nextStep).GetEnumerator();
            nextStep();
        }
    }
}