using SubworldLibrary;
using PiratePanic.Content.Subworlds;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.ModLoader;



namespace PiratePanic.Content.Biomes
{
	// Shows setting up two basic biomes. For a more complicated example, please request.
	public class PirateIsland : ModBiome
	{
		// public override CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

		// // Select Music
		// //public override int Music => MusicLoader.GetMusicSlot(Mod, "Assets/Music/MysteriousMystery");
        // public override int Music => 22;

		// // // Populate the Bestiary Filter
		// // public override string BestiaryIcon => base.BestiaryIcon;
		// // public override string BackgroundPath => base.BackgroundPath;
		// // public override Color? BackgroundColor => base.BackgroundColor;
		// // public override string MapBackground => BackgroundPath; // Re-uses Bestiary Background for Map Background

		// // Calculate when the biome is active.
		// public override bool IsBiomeActive(Player player) 
        // {
		// 	bool b1 = SubworldSystem.IsActive<DaveEJonesIsland>();
		// 	return b1;
		// }

		// // Declare biome priority. The default is BiomeLow so this is only necessary if it needs a higher priority.
		// public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;
	}
}