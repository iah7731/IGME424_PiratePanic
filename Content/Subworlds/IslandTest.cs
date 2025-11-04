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

namespace PiratePanic.Content.Subworlds
{
    internal class IslandTest : Subworld
    {

        public override bool ShouldSave => true;
        public override bool NoPlayerSaving => true;

        public override int Width => 1005;
        public override int Height => 409;

        public override List<GenPass> Tasks => new()
        {

        };
    }
}
