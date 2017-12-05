using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ElevatorSim
{
    class Background
    {
        public static Texture2D Texture;

        public Background(Texture2D texture)
        {
            if (Texture == null)
            {
                Texture = texture;
            }
        }
    }
}
