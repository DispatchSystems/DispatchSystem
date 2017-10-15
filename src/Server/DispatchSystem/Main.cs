/*
 * Information:
 * 
 * 
 * 
 *                __THIS IS A PRE-RELEASE__
 *                -------------------------
 *             There may be some features missing
 *         There may be some bugs in the features here
 * 
 * 
 * 
 * DispatchSystem made by BlockBa5her
 * 
 * Protected under the MIT License
*/

using System;
using System.Collections.Generic;
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
            RegisterCommands();

            Tick += OnTick;

            Log.WriteLine("DispatchSystem.Server by BlockBa5her loaded");
            SendAllMessage("DispatchSystem", new[] { 0, 0, 0 }, "DispatchSystem.Server by BlockBa5her loaded");
        }

        /*
         * Below is just for executing something on the main thread
        */

        private volatile static List<Action> callbacks;
        async Task OnTick()
        {
            // Executing all of the callback methods available
            for (int i = 0; i < callbacks.Count; i++)
            {
                callbacks[i]();
                callbacks.RemoveAt(i);
            }

            /*
            try
            {

            }
            catch (InvalidOperationException)
            {
#if DEBUG
                Log.WriteLine("Callback added to list while execution of others were staged");
#else
                Log.WriteLineSilent("Callback added to list while execution of others were staged");
#endif
            }
            */

            await Delay(0);
        }
        internal static void Invoke(Action method) => callbacks.Add(method); // Adding method for execution in main thread
    }
}