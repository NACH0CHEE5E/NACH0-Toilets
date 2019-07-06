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
    public class Plumber
    {
        public const string job = "Plumber";
        public const string type = job + "Table";
    }
    public class PlumberJobType : CSType
    {
        public override string name => Nach0Config.TypePrefix + Plumber.type;
        public override string icon => Nach0Config.ModIconFolder + Plumber.type + ".png";
        public override string onPlaceAudio => "stonePlace";
        public override string onRemoveAudio => "stoneDelete";
        public override string sideall => "planks";
        public override string sideyp => Nach0Config.TexturePrefix + Plumber.type;
        public override List<string> categories => new List<string>() { "job", "NACH0" }; // whatever you want really
    }
    public class PlumberTexture : CSTextureMapping
    {
        public const string NAME = Nach0Config.TexturePrefix + Plumber.type;
        public override string name => NAME;
        public override string albedo => Nach0Config.ModTextureFolder + "albedo/" + Plumber.type + ".png";
    }
    public class PlumberRecipe : ICSRecipe
    {
        public string name => Nach0Config.CrafterRecipePrefix + Plumber.type;

        public List<RecipeItem> requires => new List<RecipeItem>()
        {
            { new RecipeItem(ColonyBuiltIn.ItemTypes.WORKBENCH.Name, 1) }
        };

        public List<RecipeResult> results => new List<RecipeResult>()
        {
            { new RecipeResult(Nach0Config.TypePrefix + Plumber.type, 1) }
        };

        public CraftPriority defaultPriority => CraftPriority.Medium;
        public bool isOptional => false;
        public int defaultLimit => 2;
        public string Job => Nach0Config.BaseJobPrefix + "crafter";
    }
    public class PlumberSettings : IBlockJobSettings
    {
        static NPCType _Settings;
        public virtual float NPCShopGameHourMinimum { get { return TimeCycle.Settings.SleepTimeEnd; } }
        public virtual float NPCShopGameHourMaximum { get { return TimeCycle.Settings.SleepTimeStart; } }

        static PlumberSettings()
        {
            NPCType.AddSettings(new NPCTypeStandardSettings
            {
                keyName = Nach0Config.JobPrefix + Plumber.job,
                printName = Plumber.job,
                maskColor1 = new Color32(242, 132, 29, 255),
                type = NPCTypeID.GetNextID(),
                inventoryCapacity = 1f
            });

            _Settings = NPCType.GetByKeyNameOrDefault(Nach0Config.JobPrefix + Plumber.job);
        }

        public virtual ItemTypes.ItemType[] BlockTypes => new[]
        {
            ItemTypes.GetType(Nach0Config.TypePrefix + Plumber.type) // the block type placed
        };

        public NPCType NPCType => _Settings;

        public virtual InventoryItem RecruitmentItem => new InventoryItem("coppertools");

        public virtual bool ToSleep => !TimeCycle.IsDay;

        public Pipliz.Vector3Int GetJobLocation(BlockJobInstance instance)
        {
            if (instance is RoamingJob roamingJob)
                return roamingJob.OriginalPosition;

            return Pipliz.Vector3Int.invalidPos;
        }

        public void OnGoalChanged(BlockJobInstance instance, NPCBase.NPCGoal goalOld, NPCBase.NPCGoal goalNew)
        {

        }

        public void OnNPCAtJob(BlockJobInstance instance, ref NPCBase.NPCState state)
        {
            instance.OnNPCAtJob(ref state);
        }

        public void OnNPCAtStockpile(BlockJobInstance instance, ref NPCBase.NPCState state)
        {

        }
    }
    public class PlumberJob : RoamingJob
    {

        public PlumberJob(IBlockJobSettings settings, Pipliz.Vector3Int position, ItemTypes.ItemType type, ByteReader reader) :
            base(settings, position, type, reader)
        {
        }

        public PlumberJob(IBlockJobSettings settings, Pipliz.Vector3Int position, ItemTypes.ItemType type, Colony colony) :
            base(settings, position, type, colony)
        {
        }


        public override List<string> ObjectiveCategories => new List<string>() { "toilet" };
        public override string JobItemKey => Nach0Config.JobPrefix + Plumber.job;
        public override List<ItemId> OkStatus => new List<ItemId>
            {
                ItemId.GetItemId(Nach0Config.IndicatorTypePrefix + "Clean"),
                ItemId.GetItemId(GameLoader.NAMESPACE + ".Waiting")
            };
    }
    public class CleaningIcon : CSType
    {
        public override string name { get; set; } = Nach0Config.IndicatorTypePrefix + "Clean";
        public override string icon { get; set; } = Nach0Config.ModIconFolder + "Clean.png";
    }
    [ModLoader.ModManager]
    public static class PlumberModEntries
    {
        [ModLoader.ModCallback(ModLoader.EModCallbackType.AfterItemTypesDefined, Nach0Config.Name + "." + Plumber.job)]
        [ModLoader.ModCallbackProvidesFor("create_savemanager")]
        public static void AfterDefiningNPCTypes()
        {
            ServerManager.BlockEntityCallbacks.RegisterEntityManager(
                new BlockJobManager<PlumberJob>(
                    new PlumberSettings(),
                    (setting, pos, type, bytedata) => new PlumberJob(setting, pos, type, bytedata),
                    (setting, pos, type, colony) => new PlumberJob(setting, pos, type, colony)
                )
            );
        }
    }
}
