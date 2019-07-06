using BlockTypes;
using Jobs;
using NPC;
using Pandaros.Settlers.Items;
using Pandaros.Settlers.Jobs.Roaming;
using Pandaros.Settlers.Models;
using Pipliz;
using Recipes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Pandaros.Settlers;

namespace NACH0.Toilets
{
    public class PorcelainToiletBlock : CSType
    {
        public override string name { get; set; } = Nach0Config.TypePrefix + "PorcelainToilet";
        public override string icon { get; set; } = Nach0Config.ModIconFolder + "PorcelainToilet.png";
        public override bool? isPlaceable { get; set; } = true;
        public override string onPlaceAudio { get; set; } = "stonePlace";
        public override string onRemoveAudio { get; set; } = "stoneDelete";
        public override bool? isSolid { get; set; } = true;
        public override string sideall { get; set; } = Nach0Config.TexturePrefix + "PorcelainToilet";
        public override List<string> categories { get; set; } = new List<string>()
        {
            "essential",
            "toilet",
            "NACH0"
        };
    }
    public class PorcelainToiletRecipe : ICSRecipe
    {
        public string name => Nach0Config.RecipePrefix + "kilnjob.PorcelainToilet";

        public List<RecipeItem> requires => new List<RecipeItem>()
        {
            { new RecipeItem(ColonyBuiltIn.ItemTypes.PORCELAINRAW.Name, 6) }
        };

        public List<RecipeResult> results => new List<RecipeResult>()
        {
            { new RecipeResult(Nach0Config.TypePrefix + "PorcelainToilet", 1) }
        };

        public CraftPriority defaultPriority => CraftPriority.Medium;
        public bool isOptional => false;
        public int defaultLimit => 10;
        public string Job => Nach0Config.BaseJobPrefix + "kilnjob";
    }
    public class PorcelainToiletTexture : CSTextureMapping
    {
        public const string NAME = Nach0Config.TexturePrefix + "PorcelainToilet";
        public override string name => NAME;
        public override string albedo => Nach0Config.ModTextureFolder + "albedo/" + "PorcelainToilet.png";
    }
    public class PorcelainToiletRegister : IRoamingJobObjective
    {
        public string name => "PorcelainToilet";
        public float WorkTime => 5f;
        public ItemId ItemIndex => ItemId.GetItemId(Nach0Config.TypePrefix + "PorcelainToilet");
        public Dictionary<string, IRoamingJobObjectiveAction> ActionCallbacks { get; } = new Dictionary<string, IRoamingJobObjectiveAction>()
        {
            { ToiletConstants.CLEAN, new CleanToilet() }
        };

        public string ObjectiveCategory => "toilet";

        public void DoWork(Colony colony, RoamingJobState toiletState)
        {
            if (colony.OwnerIsOnline())
                if (toiletState.GetActionEnergy(ToiletConstants.CLEAN) > 0 &&
                    toiletState.NextTimeForWork < Pipliz.Time.SecondsSinceStartDouble)
                {
                    if (TimeCycle.IsDay)
                    {
                        toiletState.SubtractFromActionEnergy(ToiletConstants.CLEAN, 0.05f);
                    }
                    else
                    {
                        toiletState.SubtractFromActionEnergy(ToiletConstants.CLEAN, 0.02f);
                    }

                    toiletState.NextTimeForWork = toiletState.RoamingJobSettings.WorkTime + Pipliz.Time.SecondsSinceStartDouble;


                }
        }
    }
}
