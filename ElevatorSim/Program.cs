using System;
using System.Collections.Generic;

namespace ElevatorSim
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Queue<SimEvent> simOut = new Queue<SimEvent>();
            SimConsts.initialize();
            Sim s = new Sim();
            s.go(simOut);
            
            var game = new Game1();
            game.setQueue(simOut);
                game.Run();
        }
    }
#endif
}
