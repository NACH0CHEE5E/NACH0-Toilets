using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Happiness;
using Pipliz;
using Shared;
using Chatting;
using Pandaros.Settlers.Entities;
using Pandaros.Settlers.Jobs.Roaming;


namespace NACH0.Happiness
{
    class FancyToilets : IHappinessCause
    {
        public static Pandaros.Settlers.localization.LocalizationHelper LocalizationHelper { get; private set; } = new Pandaros.Settlers.localization.LocalizationHelper(Nach0Config.Name + ".Happiness");

        public float Evaluate(Colony colony)
        {
            var cs = ColonyState.GetColonyState(colony);
            if (colony != null && cs.ItemsInWorld.ContainsKey(Pandaros.Settlers.Models.ItemId.GetItemId(Nach0Config.TypePrefix + "PorcelainToilet")))
            {
                var toiletCount = cs.ItemsInWorld[Pandaros.Settlers.Models.ItemId.GetItemId(Nach0Config.TypePrefix + "PorcelainToilet")];
                if (colony.FollowerCount >= toiletCount * 10)
                {
                    return toiletCount * 3;
                }
                else
                {
                    return colony.FollowerCount * 3 / 10;
                }
                
            }
            else
            {
                return 0;
            }

        }

        public string GetDescription(Colony colony, Players.Player player)
        {
            return LocalizationHelper.LocalizeOrDefault("FancyToilets", player);
        }

    }

}
