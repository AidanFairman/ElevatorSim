using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorSim
{
    public class Sim
    {
        private int time = 0;
        
        //public int pcount = 0;
        public LinkedList<Person> fl1 = new LinkedList<Person>();
        public LinkedList<Person> fl2 = new LinkedList<Person>();
        public Queue<Person> fl1Q = new Queue<Person>();
        public Queue<Person> fl2Q = new Queue<Person>();
        LinkedList<Person> remove = new LinkedList<Person>();

        public int Time
        {
            get
            {
                return time;
            }

            /*set
            {
                time = value;
            }*/
        }

        public void go(Queue<SimEvent> queue)
        {
           
            queue.Enqueue(new SimEvent(EventType.Initialize, time, EventTarg.Elevator, "INIT", SimObject.floor.first));
            ElevatorCar ec = new ElevatorCar(SimConsts.points[(int)SimConsts.point.E1f], null, queue);
            queue.Enqueue(new SimEvent(EventType.Spawn, time, EventTarg.Elevator, ec.Name, SimObject.floor.first));
            while (time < 50 || Person.Count > 0)
            {
                
                Person p = Person.Create(null, time);
                if (p != null)
                {
                    queue.Enqueue(new SimEvent(EventType.Spawn, time, EventTarg.Person, p.Name, p.Fl));
                    if (p.Fl == SimObject.floor.first)
                    {
                        fl1.AddLast(p);
                    }
                    else
                    {
                        fl2.AddLast(p);
                    }
                }
                foreach(Person pe in fl1)
                {
                    pe.SimThink(this);
                    if (pe.needRemoval)
                    {
                        pe.needRemoval = false;
                        remove.AddLast(pe);
                    }
                }

                foreach(Person pe in remove)
                {
                    fl1.Remove(pe);
                }
                remove.Clear();

                foreach(Person pe in fl2)
                {
                    pe.SimThink(this);
                    if (pe.needRemoval)
                    {
                        pe.needRemoval = false;
                        remove.AddLast(pe);
                    }
                }

                foreach (Person pe in remove)
                {
                    fl2.Remove(pe);
                }
                remove.Clear();

                ec.SimThink(this);
                ++time;
            }
            
        }
    }
}
