using SubworldLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;


namespace PiratePanic.Content.Subworlds
{
    internal class DaveEJonesIsland : Subworld
    {

        public override bool ShouldSave => true;
        public override bool NoPlayerSaving => true;

        public override int Width => 1000;
        public override int Height => 1000;
        public override List<GenPass> Tasks => new List<GenPass>()
        {

        };


    }
}
