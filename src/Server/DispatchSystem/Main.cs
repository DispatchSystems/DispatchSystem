using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using CitizenFX.Core;

using static DispatchSystem.sv.Common;

namespace DispatchSystem.sv
{
    public partial class DispatchSystem : BaseScript
    {
        public DispatchSystem()
        {
            Log.Create("dispatchsystem.log");

            InitializeComponents();
            RegisterEvents();

            Tick += OnTick;

            Log.WriteLine("DispatchSystem.Server by BlockBa5her loaded");
            SendAllMessage("DispatchSystem", new[] { 0, 0, 0 }, "DispatchSystem.Server by BlockBa5her loaded");
        }

        /*
         * Below is just for executing something on the main thread
        */

        private static volatile ConcurrentQueue<Action> callbacks;
        async Task OnTick()
        {
            // Executing all of the callback methods available
            while (callbacks.TryDequeue(out Action queue))
                queue();

            await Delay(0);
        }
        internal static void Invoke(Action method) => callbacks.Enqueue(method); // Adding method for execution in main thread
    }
}