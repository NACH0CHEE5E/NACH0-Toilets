using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NACH0
{
    class Nach0Config
    {
        public const string Name = "NACH0";
        public const string TypePrefix = Name + ".Types.";
        public const string IndicatorTypePrefix = TypePrefix + "Indicator.";
        public const string ResearchPrefix = Name + ".Research.";
        public const string JobPrefix = Name + ".Jobs.";
        public const string TexturePrefix = Name + ".Textures.";
        public const string JobGuardPrefix = Name + ".Jobs.Guard.";
        public const string BaseJobPrefix = "pipliz.";
        public const string RecipePrefix = Name + ".";
        public const string CrafterRecipePrefix = RecipePrefix + "crafter.";
        public const string ModName = "Happiness";
        public const string ModFullName = Name + "." + ModName;
        public const string GamedataFolder = "gamedata";
        public const string ModsFolder = GamedataFolder + "/mods";
        public const string ModFolder = ModsFolder + "/" + Name + "/" + ModName;
        public const string ModGamedataFolder = ModFolder + "/gamedata";
        public const string ModIconFolder = ModGamedataFolder + "/textures/icons/";
        public const string ModTextureFolder = ModGamedataFolder + "/textures/materials/";
    }
}
