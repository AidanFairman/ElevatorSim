using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorSim
{
    public enum EventType
    {
        Initialize,
        Spawn,
        Seek,
        Load,
        Unload,
        Terminate,
        Idle
    }

    public enum EventTarg
    {
        Person,
        Elevator
    }

    public class SimEvent
    {
        public EventType eType;
        public int time;
        public EventTarg eTarg;
        public string targName;
        public SimObject.floor flr;

        public SimEvent(EventType typ, int tim, EventTarg targ, string name, SimObject.floor fl)
        {
            eType = typ;
            time = tim;
            eTarg = targ;
            targName = name;
            flr = fl;
        }
    }
}
