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
    public class ToiletBlock : CSType
    {
        public override string name { get; set; } = Nach0Config.TypePrefix + "Toilet";
        public override string icon { get; set; } = Nach0Config.ModIconFolder + "Toilet.png";
        public override bool? isPlaceable { get; set; } = true;
        public override string onPlaceAudio { get; set; } = "dirtPlace";
        public override string onRemoveAudio { get; set; } = "grassDelete";
        public override bool? isSolid { get; set; } = true;
        public override string sideall { get; set; } = "dirt";
        public override string sideyp { get; set; } = Nach0Config.TexturePrefix + "Toilet";
        public override List<string> categories { get; set; } = new List<string>()
        {
            "essential",
            "toilet",
            "NACH0"
        };
    }
    public class ToiletRecipe : ICSRecipe
    {
        public string name => Nach0Config.CrafterRecipePrefix + "Toilet";

        public List<RecipeItem> requires => new List<RecipeItem>()
        {
            { new RecipeItem(ColonyBuiltIn.ItemTypes.DIRT.Name, 1) }
        };

        public List<RecipeResult> results => new List<RecipeResult>()
        {
            { new RecipeResult(Nach0Config.TypePrefix + "Toilet", 1) }
        };

        public CraftPriority defaultPriority => CraftPriority.Medium;
        public bool isOptional => false;
        public int defaultLimit => 2;
        public string Job => Nach0Config.BaseJobPrefix + "crafter";
    }
    public class ToiletTexture : CSTextureMapping
    {
        public const string NAME = Nach0Config.TexturePrefix + "Toilet";
        public override string name => NAME;
        public override string albedo => Nach0Config.ModTextureFolder + "albedo/" + "Toilet.png";
    }
    public class ToiletRegister : IRoamingJobObjective
    {
        public string name => "Toilet";
        public float WorkTime => 5f;
        public ItemId ItemIndex => ItemId.GetItemId(Nach0Config.TypePrefix + "Toilet");
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
    public class CleanToilet : IRoamingJobObjectiveAction
    {
        public string name => ToiletConstants.CLEAN;

        public float TimeToPreformAction => 10f;

        public string AudioKey => "grassDelete";

        public ItemId ObjectiveLoadEmptyIcon => ItemId.GetItemId(Nach0Config.IndicatorTypePrefix + "Clean");

        public ItemId PreformAction(Colony colony, RoamingJobState state)
        {
            var retval = ItemId.GetItemId(Nach0Config.IndicatorTypePrefix + "Clean");

            if (colony.OwnerIsOnline())
            {
                if (state.GetActionEnergy(ToiletConstants.CLEAN) < .75f)
                {
                    var repaired = false;
                    var requiredForClean = new List<InventoryItem>();
                    var returnedFromClean = new List<InventoryItem>();
                    var stockpile = colony.Stockpile;
                    int itemReqMult = 1;


                    if (state.GetActionEnergy(ToiletConstants.CLEAN) < .50f)
                    {
                        itemReqMult++;
                    }
                    if (state.GetActionEnergy(ToiletConstants.CLEAN) < .30f)
                    {
                        itemReqMult++;
                    }
                    if (state.GetActionEnergy(ToiletConstants.CLEAN) < .10f)
                    {
                        itemReqMult++;
                    }

                    requiredForClean.Add(new InventoryItem(ColonyBuiltIn.ItemTypes.BUCKETWATER.Name, 1 * itemReqMult));
                    returnedFromClean.Add(new InventoryItem(ColonyBuiltIn.ItemTypes.BUCKETEMPTY.Name, 1 * itemReqMult));

                    if (stockpile.Contains(requiredForClean))
                    {
                        stockpile.TryRemove(requiredForClean);
                        repaired = true;
                        stockpile.Add(returnedFromClean);
                    }
                    else
                    {
                        foreach (var item in requiredForClean)
                            if (!stockpile.Contains(item))
                            {
                                retval = ItemId.GetItemId(item.Type);
                                break;
                            }
                    }

                    if (repaired)
                    {
                        state.ResetActionToMaxLoad(ToiletConstants.CLEAN);
                    }
                }
            }

            return retval;
        }
    }
}
