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
using System.Linq;
using System.Threading;
using System.Collections.ObjectModel;
using System.Net;

using CitizenFX.Core;
using CitizenFX.Core.Native;
using Config.Reader;

using DispatchSystem.sv.External;
using DispatchSystem.Common.DataHolders.Storage;

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

            Log.WriteLine("DispatchSystem.Server by BlockBa5her loaded");
            SendAllMessage("DispatchSystem", new[] { 0, 0, 0 }, "DispatchSystem.Server by BlockBa5her loaded");
        }
    }
}