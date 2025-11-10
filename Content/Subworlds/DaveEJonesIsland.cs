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

namespace PiratePanic.Content.Subworlds
{
    internal class DaveEJonesIsland : Subworld
    {

        public override bool ShouldSave => true;
        public override bool NoPlayerSaving => false;

        public override int Width => 1046;
        public override int Height => 630;

        public override List<GenPass> Tasks => new()
        {

        };


        public override void OnLoad()
        {
            Main.worldSurface = 350;
            Main.rockLayer = Height + 1;
            Main.maxTilesY = 1200;

            Console.WriteLine("DaveEJonesIsland Subworld Loaded");
            Console.WriteLine(StructureHelper.API.Generator.GetStructureDimensions("Content/Subworlds/DaveEJonesIsland.shstruct", ModContent.GetInstance<PiratePanic>(), false));
            StructureHelper.API.Generator.GenerateStructure("Content/Subworlds/DaveEJonesIsland.shstruct", 
                new Point16(40,300), 
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
                        Main.spawnTileX = x;
                        Main.spawnTileY = y;
                        Main.tile[x, y].TileType = TileID.WoodBlock;
                    }
                }
            }

            
        }

    }
}
