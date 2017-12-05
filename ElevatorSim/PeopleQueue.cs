using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ElevatorSim
{
    class PeopleQueue
    {
        Vector2 position;
        Queue<Person> queue;

        PeopleQueue(Vector2 vect, byte capacity)
        {
            queue = new Queue<Person>(capacity);
            position = new Vector2(vect.X, vect.Y);
        }
    }
}
