using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ElevatorSim
{
    public abstract class SimObject
    {
        public enum AIMode
        {
            Idle,
            Seek,
            Load,
            Unload,
            Init
        }

        public enum floor
        {
            first,
            second
        }
        public Vector2 position;
        public Vector2 Target { get; set; }
        public string Name { get; set; }
        protected AIMode Action { get; set; }
        public static int serial = 0;
        public floor Fl { get; set; }
        protected int lifetime;
        protected int nextSimAction;
        protected static Queue<SimEvent> SEQ;

        public SimObject(Vector2 Position, string name, AIMode action, floor f, Queue<SimEvent> Q)
        {
            if (Q != null && SEQ == null)
            {
                SEQ = Q;
            }
            Name = String.Format("{0}{1}", name, serial);
            ++serial;
            position = Position;
            Action = action;
            Fl = f;
        }

        public virtual void ChangeMode(AIMode mode)
        {
            Action = mode;
        }

        public abstract void SimThink(Sim s);
        public abstract void Update(GameTime gameTime);
        public abstract void draw(SpriteBatch sb);
        

    }
}
