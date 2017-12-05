using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System;

namespace ElevatorSim
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        LinkedList<SimObject> SimThings;
        Queue<SimEvent> simOut;
        ElevatorCar elevate;
        Background bg;
        Texture2D personTexture;
        Texture2D elevatorTexture;
        int updates = 0;
        StreamWriter outFile;
        Color color;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public void setQueue(Queue<SimEvent> simQ)
        {
            simOut = simQ;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            
            SimThings = new LinkedList<SimObject>();
            //simOut = new Queue<SimEvent>();
            // TODO: Add your initialization logic here
            
            outFile = new StreamWriter("bufferEvents.txt", false);
            
            
            /*for (int i = 0; i < simOut.Count; ++i)
            {
                output(simOut.Peek());
                simOut.Enqueue(simOut.Dequeue());

            }*/
            color = Color.CornflowerBlue;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //person = new Person("Bob", Vector2.Zero, Content.Load<Texture2D>("Person"));
            personTexture = Content.Load<Texture2D>("Person");
            elevatorTexture = Content.Load<Texture2D>("ColorBox");
            bg = new Background(Content.Load<Texture2D>("Background"));
            elevate = new ElevatorCar(SimConsts.points[(int)SimConsts.point.E1f], elevatorTexture, simOut);
            Console.WriteLine("Height: {0}", GraphicsDevice.Viewport.Height);
            Console.WriteLine("Width: {0}", GraphicsDevice.Viewport.Width);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) || simOut.Count == 0)
                this.Exit();
               

            while (simOut.Count > 0 && simOut.Peek().time <= (updates / 60))
            {
                process(simOut.Dequeue());
            }
            
            foreach (SimObject so in SimThings)
            {
                so.Update(gameTime);
            }
            ++updates;
            /* if (Keyboard.GetState().IsKeyDown(Keys.S))
             {
                 person.Position += new Vector2(0, 1);
             }
             if (Keyboard.GetState().IsKeyDown(Keys.W))
             {
                 person.Position -= new Vector2(0, 1);
             }
             if (Keyboard.GetState().IsKeyDown(Keys.A))
             {
                 person.Position -= new Vector2(1, 0);
             }
             if (Keyboard.GetState().IsKeyDown(Keys.D))
             {
                 person.Position += new Vector2(1, 0);
             }
             */

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(color);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(Background.Texture, new Rectangle((int)Vector2.Zero.X, (int)Vector2.Zero.Y, 800, 480), Color.White);

            foreach (SimObject so in SimThings)
            {
                if (so is Person)
                {
                    so.draw(spriteBatch);
                }
            }
            elevate.draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void process(SimEvent se)
        {
            switch (se.eType)
            {
                case EventType.Initialize:
                    updates = 0;
                    break;
                case EventType.Spawn:
                    if (se.eTarg == EventTarg.Person)
                    {
                        if (se.flr == SimObject.floor.first)
                        {
                            Person p = new Person(SimConsts.points[(int)SimConsts.point.PS1], personTexture, se.flr);
                            p.Name = se.targName;
                            p.Target = SimConsts.points[(int)SimConsts.point.PQ1];
                            SimThings.AddLast(p);
                        }
                        else
                        {
                            Person p = new Person(SimConsts.points[(int)SimConsts.point.PS2], personTexture, se.flr);
                            p.Name = se.targName;
                            p.Target = SimConsts.points[(int)SimConsts.point.PQ2];
                            SimThings.AddLast(p);
                        }
                    }
                    else
                    {
                        elevate = new ElevatorCar(SimConsts.points[(int)SimConsts.point.E1f], elevatorTexture, simOut);
                        elevate.Name = se.targName;
                        SimThings.AddLast(elevate);
                    }
                    break;
                case EventType.Seek:
                    foreach (SimObject so in SimThings)
                    {
                        if (so.Name == se.targName)
                        {
                            so.Fl = se.flr;
                            so.ChangeMode(SimObject.AIMode.Seek);
                        }
                    }
                    break;
                case EventType.Load:
                    foreach (SimObject so in SimThings)
                    {
                        if (so.Name == se.targName)
                        {
                            so.Fl = se.flr;
                            so.ChangeMode(SimObject.AIMode.Load);
                        }
                    }
                    break;
                case EventType.Unload:
                    foreach (SimObject so in SimThings)
                    {
                        if (so.Name == se.targName)
                        {
                            so.Fl = se.flr;
                            so.ChangeMode(SimObject.AIMode.Unload);
                        }
                    }
                    break;
                case EventType.Terminate:
                    if (se.eTarg == EventTarg.Elevator)
                    {
                        //MessageBox.Show("End of simulation", "End of simulation", new string[] { "OK" });
                        this.Exit();
                    }
                    SimObject kill = null;
                    foreach (SimObject so in SimThings)
                    {
                        if (so.Name == se.targName)
                        {
                            kill = so;
                        }
                    }

                    SimThings.Remove(kill);
                    kill = null;

                    break;
                default:
                    break;
            }
        }

        private void output(SimEvent se)
        {
            string tname = se.targName;
            string time = se.time.ToString();
            string target;
            string floor;
            string etype;

            if (se.eTarg == EventTarg.Elevator)
            {
                target = "Elev";
            }
            else
            {
                target = "Prsn";
            }

            if (se.flr == SimObject.floor.first)
            {
                floor = "1st";
            }
            else
            {
                floor = "2nd";
            }

            if (se.eType == EventType.Initialize)
            {
                etype = "Init";
            }
            else if (se.eType == EventType.Spawn)
            {
                etype = "Spwn";
            }
            else if (se.eType == EventType.Load)
            {
                etype = "Load";
            }
            else if (se.eType == EventType.Unload)
            {
                etype = "Ulod";
            }
            else if (se.eType == EventType.Seek)
            {
                etype = "Seek";
            }
            else if (se.eType == EventType.Idle)
            {
                etype = "Idle";
            }
            else
            {
                etype = "Term";
            }

            outFile.WriteLine(String.Format("{0}, {1}, {2}, {3}, {4}", tname, time, target, floor, etype));
        }
    }
}
