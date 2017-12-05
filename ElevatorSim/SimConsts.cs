using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ElevatorSim
{
    public static class SimConsts
    {
        public enum point
        {
            E2f = 0,    //elevator 2nd floor
            E1f,        //elevator 1st floor
            PQ2,        //person queue 2nd floor
            PQ1,        //person queue 1st floor
            PS2,        //person spawn 2nd floor
            PS1,        //person spawn 1st floor
            PH          //person holding for faux elevator ride
        }

        public enum times
        {
            t = 0,  //t is current time
            t1,     //t1 is person spawn rate
            t2,     //t2 is person travel time
            t3,     //t3 is elevator travel time
            t4      //t4 is person load/unload time
        }
        public const int ELEVATOR_CAR_CAPACITY = 10;
        public const int MAX_LOBBY_SIZE = 25;
        public const int PERSON_LIMIT = 50;
        public static Vector2[] points = {   new Vector2(1, 32), new Vector2(1, 448),
                                             new Vector2(40, 32), new Vector2(40, 448),
                                             new Vector2(801, 32), new Vector2(801, 448),
                                             new Vector2(-100, -100)   };
        
        public static int[] time = new int[5];

        public static void initialize()
        {
            time[(int)times.t] = 0;
            time[(int)times.t1] = 2;
            time[(int)times.t2] = 8;
            time[(int)times.t3] = 4;
            time[(int)times.t4] = 1;
        }
    }
}
