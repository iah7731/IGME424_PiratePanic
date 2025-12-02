using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using StructureHelper;
using System.Diagnostics;
using log4net.Repository.Hierarchy;
using Terraria.ID;
using System.IO.Pipes;
using PiratePanic.Content.NPCs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PiratePanic.Content.Subworlds
{
    internal class DaveEJonesIsland : Subworld
    {

        public override bool ShouldSave => true;
        public override bool NoPlayerSaving => false;
        public override bool NormalUpdates => false;
        public static bool loaded = false;

        public override int Width => 1046;
        public override int Height => 630;

        public override List<GenPass> Tasks => new()
        {
            new PassLegacy("SpawnNPC", (progress, configuration) =>
            {
                Vector2 pos = new Vector2(928, 537) * 16;
                NPC.NewNPC(null, (int)pos.X, (int)pos.Y, ModContent.NPCType<CursedPirate>());
            })
        };

        public override void OnEnter()
        {
            Main.dayTime = false;
            Main.time = 3600;
            Main.moonPhase = 0;
        }

        public override void OnLoad()
        {
            if (loaded == false)
            {
                Main.worldSurface = 350;
                Main.rockLayer = Height + 1;

                Console.WriteLine("DaveEJonesIsland Subworld Loaded");
                Console.WriteLine(StructureHelper.API.Generator.GetStructureDimensions("Content/Subworlds/DaveEJonesIsland.shstruct", ModContent.GetInstance<PiratePanic>(), false));
                StructureHelper.API.Generator.GenerateStructure("Content/Subworlds/DaveEJonesIsland.shstruct",
                    new Point16(40, 300),
                    ModContent.GetInstance<PiratePanic>(),
                    false,
                    false,
                    GenFlags.None);

                for (int x = 0; x < Main.maxTilesX; x++)
                {
                    for (int y = 0; y < Main.maxTilesY; y++)
                    {
                        Tile tile = Main.tile[x, y];
                        if (tile != null && tile.HasTile && tile.TileType == TileID.CrispyHoneyBlock)
                        {
                            Main.tile[x, y].TileType = TileID.WoodBlock;
                            Main.spawnTileX = x;
                            Main.spawnTileY = y;
                        }
                        else if (tile != null && tile.HasTile && tile.TileType == TileID.HayBlock)
                        {
                            Main.tile[x, y].TileType = TileID.ObsidianBrick;
                        }
                    }
                }
                loaded = true;
            }

        }

    }
}
