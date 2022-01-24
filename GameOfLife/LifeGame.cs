using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using Yasai;
using Yasai.Graphics;
using Yasai.Structures.DI;

namespace GameOfLife
{
    public class LifeGame : Game
    {
        private Grid grid;

        private const int TILE_SIZE = 32;
        private const int TICK_RATE = 30;

        public override void Load(DependencyContainer dependencies)
        {
            base.Load(dependencies);

            // randomly generate the grid
            var rand = new Random();
            bool[][] situation = new bool[10][];
            for (int y = 0; y < 10; y++)
            {
                var row = new bool[10];
                for (int x = 0; x < 10; x++)
                {
                    row[x] = rand.Next(10) > 4;
                }

                situation[y] = row;
            }
            
            grid = new Grid(situation);

            // create the cells
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    Root.Add(new DrawableCell
                    {
                        Position = new Vector2(x * (TILE_SIZE + 2), y * (TILE_SIZE + 2)),
                        Coord = new Vector2i(x, y),
                        Size = new Vector2(TILE_SIZE),
                        Visible = grid.Situation[y][x]
                    });
                }
            }
        }

        private int timer;
        public override void Update(FrameEventArgs args)
        {
            base.Update(args);
            timer++; // <- this sucks

            if (timer % TICK_RATE == 0)
                tick();
        }

        private void tick()
        {
            grid.Tick();

            foreach (IDrawable d in Root)
            {
                if (d is DrawableCell cell)
                {
                    Vector2i pos = cell.Coord;
                    cell.Visible = grid.Situation[pos.Y][pos.X];
                }
            }
        }
    }
}