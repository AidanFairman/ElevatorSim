using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElevatorSim
{
    public class Person : SimObject
    {
        public static Texture2D Texture;
        private static int count = 0;
        
        private static int created;
        private static float travelRate = ((float)(SimConsts.points[(int)SimConsts.point.PS1].X - SimConsts.points[(int)SimConsts.point.PQ1].X) / SimConsts.time[(int)SimConsts.times.t2]) / 60.0f;
        private static Random rand = new Random();
        private bool rodeElevator = false;
        public bool needRemoval = false;

        public static int Count
        {
            get
            {
                return count;
            }
        }

        //returns initialized person object, or null if it's not time
        public static Person Create(Texture2D texture, int time)
        {
            
            if (count < SimConsts.MAX_LOBBY_SIZE && (time % SimConsts.time[(int)SimConsts.times.t1]) == 0 && created < SimConsts.PERSON_LIMIT)
            {
                ++count;
                ++created;
                int nran = rand.Next();
                if (nran % 2 == 0)
                {
                    Person p = new Person(SimConsts.points[(int)SimConsts.point.PS1], texture, floor.first);
                    p.Action = AIMode.Init;
                    return p;
                }
                else
                {
                    Person p = new Person(SimConsts.points[(int)SimConsts.point.PS2], texture, floor.second);
                    p.Action = AIMode.Init;
                    return p;
                }
            }
            else
            {
                return null;
            }
        }

        /*~Person()
        {
            --count;
        }*/

        public Person(Vector2 position, Texture2D texture, floor f) : base(position, "Person", AIMode.Seek, f, null)
        {
            if (Texture == null)
            {
                Texture = texture;
            }
            
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
                    if (rodeElevator)
                    {
                        if(position.X < Target.X)
                        {
                            position.X += travelRate;
                        }
                    }
                    else
                    {
                        if (position.X > Target.X)
                        {
                            position.X -= travelRate;
                        }
                        else
                        {
                            Action = AIMode.Idle;
                        }
                    }
                    break;
                case AIMode.Load:
                    position = SimConsts.points[(int)SimConsts.point.PH];
                    break;
                case AIMode.Unload:
                    if(Fl == floor.first)
                    {
                        rodeElevator = true;
                        position = SimConsts.points[(int)SimConsts.point.PQ1];
                        Action = AIMode.Seek;
                        Target = SimConsts.points[(int)SimConsts.point.PS1];
                    }
                    else
                    {
                        rodeElevator = true;
                        position = SimConsts.points[(int)SimConsts.point.PQ2];
                        Action = AIMode.Seek;
                        Target = SimConsts.points[(int)SimConsts.point.PS2];
                    }
                    break;
                default:
                    break;
            }
        }

        public override void draw(SpriteBatch sb)
        {
            sb.Draw(Texture, position, Color.White);
        }

        public void postElevatorSeek()
        {
            nextSimAction = SimConsts.time[(int)SimConsts.times.t2];
            Action = AIMode.Seek;
            rodeElevator = true;
        }

        public override void SimThink(Sim s)
        {
            switch (Action)
            {
                case AIMode.Idle:
                    break;
                case AIMode.Init:
                    lifetime = 0;
                    nextSimAction = SimConsts.time[(int)SimConsts.times.t2];
                    Action = AIMode.Seek;
                    break;
                case AIMode.Load:
                    break;
                case AIMode.Seek:
                    if (nextSimAction > 0)
                    {
                        --nextSimAction;
                    }
                    else if(!rodeElevator)
                    {
                        needRemoval = true;
                        Action = AIMode.Idle;
                        SEQ.Enqueue(new SimEvent(EventType.Idle, s.Time, EventTarg.Person, this.Name, Fl));
                        if (Fl == floor.first)
                        {
                            s.fl1Q.Enqueue(this);
                            
                        }
                        else
                        {
                            s.fl2Q.Enqueue(this);
                        }
                    }
                    else
                    {
                        SEQ.Enqueue(new SimEvent(EventType.Terminate, s.Time, EventTarg.Person, this.Name, Fl));
                        needRemoval = true;
                        count--;
                    }
                    break;
                case AIMode.Unload:
                    break;
                default:
                    break;
            }
            ++lifetime;
        }
    }
}
