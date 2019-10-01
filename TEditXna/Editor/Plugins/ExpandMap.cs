using System;
using System.Threading.Tasks;
using TEdit.MvvmLight.Threading;
using TEditXna.ViewModel;
using TEditXNA.Terraria;

namespace TEditXna.Editor.Plugins
{
    public class ExpandMap : BasePlugin
    {
        public World NewWorld
        {
            get; private set;
        }

        public ExpandMap(WorldViewModel worldViewModel) : base(worldViewModel)
        {
            Name = "Expand Map";
            NewWorld = worldViewModel.CurrentWorld;
            //DataContext = worldViewModel.CurrentWorld;
        }

        public override void Execute()
        {
            if (_wvm.CurrentWorld == null)
                return;

            ExpandMapPluginView view = new ExpandMapPluginView(_wvm.CurrentWorld);
            if (view.ShowDialog() == false)
            {
                return;
            }

            World w = _wvm.CurrentWorld;

            UpdateWorld(w);

        }

        private async void UpdateWorld(World w)
        {
            await Task.Factory.StartNew(() =>
            {

                w.SpawnX = w.TilesWide / 2;
                w.SpawnY = (int)Math.Max(0, w.GroundLevel - 10);
                w.GroundLevel = (int)w.GroundLevel;
                w.RockLevel = (int)w.RockLevel;
                w.BottomWorld = w.TilesHigh * 16;
                w.RightWorld = w.TilesWide * 16;
                w.Tiles = new Tile[w.TilesWide, w.TilesHigh];
                var cloneTile = new Tile();
                for (int y = 0; y < w.TilesHigh; y++)
                {


                    if (y == (int)w.GroundLevel - 10)
                        cloneTile = new Tile { WireRed = false, IsActive = true, LiquidType = LiquidType.None, LiquidAmount = 0, Type = 2, U = -1, V = -1, Wall = 2 };
                    if (y == (int)w.GroundLevel - 9)
                        cloneTile = new Tile { WireRed = false, IsActive = true, LiquidType = LiquidType.None, LiquidAmount = 0, Type = 0, U = -1, V = -1, Wall = 2 };
                    else if (y == (int)w.GroundLevel + 1)
                        cloneTile = new Tile { WireRed = false, IsActive = true, LiquidType = LiquidType.None, LiquidAmount = 0, Type = 0, U = -1, V = -1, Wall = 0 };
                    else if (y == (int)w.RockLevel)
                        cloneTile = new Tile { WireRed = false, IsActive = true, LiquidType = LiquidType.None, LiquidAmount = 0, Type = 1, U = -1, V = -1, Wall = 0 };
                    else if (y == w.TilesHigh - 182)
                        cloneTile = new Tile();
                    for (int x = 0; x < w.TilesWide; x++)
                    {
                        w.Tiles[x, y] = (Tile)cloneTile.Clone();
                    }
                }
                return w;
            })
            .ContinueWith(t => _wvm.CurrentWorld = t.Result, TaskFactoryHelper.UiTaskScheduler)
            .ContinueWith(t => _wvm.RenderEntireWorld())
                .ContinueWith(t =>
                {
                    _wvm.FinalizeWorldUpdate(t);
                }, TaskFactoryHelper.UiTaskScheduler);
        }
    }
}
