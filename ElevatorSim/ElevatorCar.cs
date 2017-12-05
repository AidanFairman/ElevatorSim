using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ElevatorSim
{
    public class ElevatorCar : SimObject
    {
        public static Texture2D Texture;
        private static float travelRate = ((SimConsts.points[(int)SimConsts.point.E1f].Y - SimConsts.points[(int)SimConsts.point.E2f].Y)/ 3.0f) / 60.0f;
        Queue<Person> occupants = new Queue<Person>(SimConsts.ELEVATOR_CAR_CAPACITY);
        
        public ElevatorCar(Vector2 position, Texture2D texture, Queue<SimEvent> Q) : base(position, "Elevator", AIMode.Load, floor.first, Q)
        {
            if (Texture == null)
            {
                Texture = texture;
            }
            if (position == SimConsts.points[(int)SimConsts.point.E2f])
            {
                Fl = floor.second;
            }
        }

        public void LoadPerson(Person p)
        {
            occupants.Enqueue(p);
        }

        public Person UnloadPerson()
        {
            return occupants.Dequeue();
        }

        public override void Update(GameTime gameTime)
        {
            Think(gameTime);
        }

        private void Think(GameTime GameTime)
        {
            switch (Action)
            {
                case AIMode.Idle:

                    break;
                case AIMode.Seek:
                    if (Fl == floor.first)
                    {
                        if (position.Y < Target.Y)
                        {
                            position.Y += travelRate;
                        }
                    }
                    else
                    {
                        if (position.Y > Target.Y)
                        {
                            position.Y -= travelRate;
                        }
                    }
                    break;
                default:
                    Action = AIMode.Idle;
                    break;
            }//end switch
        }//end think method

        public override void draw(SpriteBatch sb)
        {
            sb.Draw(Texture, position, Color.White);
        }

        public override void ChangeMode(AIMode mode)
        {
            Action = mode;
            if (Fl == floor.first)
            {
                Target = SimConsts.points[(int)SimConsts.point.E1f];
            }
            else
            {
                Target = SimConsts.points[(int)SimConsts.point.E2f];
            }
        }

        public override void SimThink(Sim s)
        {
            switch (Action)
            {
                case AIMode.Idle:
                    break;
                case AIMode.Seek:
                    if (lifetime >= nextSimAction)
                    {
                        Action = AIMode.Unload;
                        SEQ.Enqueue(new SimEvent(EventType.Unload, s.Time, EventTarg.Elevator, this.Name, Fl));
                    }
                    break;
                case AIMode.Load:
                    if (Fl == floor.first)
                    {
                        if (s.fl1Q.Count > 0 && occupants.Count < SimConsts.ELEVATOR_CAR_CAPACITY)
                        {
                            Person p = s.fl1Q.Dequeue();
                            SEQ.Enqueue(new SimEvent(EventType.Load, s.Time, EventTarg.Person, p.Name, Fl));
                            occupants.Enqueue(p);
                        }
                        else
                        {
                            Fl = floor.second;
                            SEQ.Enqueue(new SimEvent(EventType.Seek, s.Time, EventTarg.Elevator, this.Name, Fl));
                            Action = AIMode.Seek;
                            nextSimAction = lifetime + SimConsts.time[(int)SimConsts.times.t3];
                        }
                    }
                    else
                    {
                        if (s.fl2Q.Count > 0 && occupants.Count < SimConsts.ELEVATOR_CAR_CAPACITY)
                        {
                            Person p = s.fl2Q.Dequeue();
                            SEQ.Enqueue(new SimEvent(EventType.Load, s.Time, EventTarg.Person, p.Name, Fl));
                            
                            occupants.Enqueue(p);
                        }
                        else
                        {
                            Fl = floor.first;
                            SEQ.Enqueue(new SimEvent(EventType.Seek, s.Time, EventTarg.Elevator, this.Name, Fl));
                            Action = AIMode.Seek;
                            nextSimAction = lifetime + SimConsts.time[(int)SimConsts.times.t3];
                        }
                    }
                    break;
                case AIMode.Unload:
                    if (Fl == floor.first)
                    {
                        if (occupants.Count > 0)
                        {
                            Person p = occupants.Dequeue();
                            p.postElevatorSeek();
                            SEQ.Enqueue(new SimEvent(EventType.Unload, s.Time, EventTarg.Person, p.Name, Fl));
                            s.fl2.AddLast(p);
                        }
                        else
                        {
                            Action = AIMode.Load;
                            SEQ.Enqueue(new SimEvent(EventType.Load, s.Time, EventTarg.Elevator, this.Name, Fl));
                        }
                    }
                    else
                    {
                        if (occupants.Count > 0)
                        {
                            Person p = occupants.Dequeue();
                            p.postElevatorSeek();
                            SEQ.Enqueue(new SimEvent(EventType.Unload, s.Time, EventTarg.Person, p.Name, Fl));
                            s.fl1.AddLast(p);
                        }
                        else
                        {
                            Action = AIMode.Load;
                            SEQ.Enqueue(new SimEvent(EventType.Load, s.Time, EventTarg.Elevator, this.Name, Fl));
                        }
                    }
                    break;
                default:
                    break;
            }
            ++lifetime;
        }

    }
}
