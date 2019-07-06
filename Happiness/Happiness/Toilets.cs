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
using Pandaros.Settlers.Models;
using BlockTypes;
using Jobs;
using NPC;
using Pandaros.Settlers.Items;
using Recipes;
using System.Collections.ObjectModel;
using UnityEngine;
using Pandaros.Settlers;


namespace NACH0.Happiness
{
    class Toilets : IHappinessCause
    {
        //public static Dictionary<Colony, int> toiletCount_Dict = new Dictionary<Colony, int>();
        public static Pandaros.Settlers.localization.LocalizationHelper LocalizationHelper { get; private set; } = new Pandaros.Settlers.localization.LocalizationHelper(Nach0Config.Name + ".Happiness");

        public float Evaluate(Colony colony)
        {
            if (colony != null && colony.FollowerCount > 15 )
            {
                //toiletCount = RoamingJobManager.Objectives[colony].Select(s => s.Value.RoamObjective == "toilet").ToList();
                    var toiletCount = 0;
                    //ServerLog.LogAsyncMessage(new LogMessage("<color=blue>Number of toilets: " + toiletCount + "    (before foreach)</color>", UnityEngine.LogType.Log));
                //var toilets = RoamingJobManager.Objectives[colony].Where(s => s.Value.RoamingJobSettings.ObjectiveCategory == "toilet").Select(s => s.Value).ToList();
                    var toilets = RoamingJobManager.Objectives[colony]["toilet"].Values;
                    foreach (var toilet in toilets)
                    {
                        //ServerLog.LogAsyncMessage(new LogMessage("<color=purple>The foreach runs</color>", UnityEngine.LogType.Log));
                        var levelOfClean = toilet.ActionEnergy[NACH0.Toilets.ToiletConstants.CLEAN];
                    //var levelOfClean = toilet.
                        if (levelOfClean > 0.50f)
                        {
                            toiletCount ++;
                            //ServerLog.LogAsyncMessage(new LogMessage("<color=blue>Number of toilets: " + toiletCount + "</color>", UnityEngine.LogType.Log));
                        }

                    }
                    //ServerLog.LogAsyncMessage(new LogMessage("<color=blue>Number of toilets: " + toiletCount + "</color>", UnityEngine.LogType.Log));
                    int toiletsSupplied = toiletCount * 10;
                    int toiletsNeeded = colony.FollowerCount;

                    if (toiletsSupplied <= toiletsNeeded)
                    {
                        int happinessAmount = toiletsSupplied - toiletsNeeded;
                        return happinessAmount;
                    }
                    else
                    {
                        return 0;
                    }
            }
            else
            {
                return 0;
            }

        }

        public string GetDescription(Colony colony, Players.Player player)
        {
            return LocalizationHelper.LocalizeOrDefault("NotEnoughToilets", player);
        }

    }

    [ChatCommandAutoLoader]
    public class SeeToiletsCommand : IChatCommand
    {
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if (!chat.Equals("?nach0toilets"))
            {
                return false;
            }
            if (player == null)
            {
                return false;
            }
            if (!DisableToiletsCommand.toiletsEnabled.ContainsKey(player))
            {
                DisableToiletsCommand.toiletsEnabled[player] = true;
            }
            if (!DisableToiletsCommand.toiletsEnabled[player])
            {
                Chat.Send(player, "<color=blue>Toilets are disabled</color>");
                return false;
            }
            var ps = PlayerState.GetPlayerState(player);
            var toiletCount = 0;
            //ServerLog.LogAsyncMessage(new LogMessage("<color=blue>Number of toilets: " + toiletCount + "    (before foreach)</color>", UnityEngine.LogType.Log));
            var toilets = RoamingJobManager.Objectives[player.ActiveColony]["toilet"].Values;
            foreach (var toilet in toilets)
            {
                //ServerLog.LogAsyncMessage(new LogMessage("<color=purple>The foreach runs</color>", UnityEngine.LogType.Log));
                var levelOfClean = toilet.ActionEnergy[NACH0.Toilets.ToiletConstants.CLEAN];
                if (levelOfClean > 0.60f)
                {
                    toiletCount++;
                    //ServerLog.LogAsyncMessage(new LogMessage("<color=blue>Number of toilets: " + toiletCount + "</color>", UnityEngine.LogType.Log));
                }
            }
            Chat.Send(player, "<color=blue>Number of toilets: " + toiletCount + "</color>");
            return true;
        }
    }
    [ChatCommandAutoLoader]
    public class DisableToiletsCommand : IChatCommand
    {
        public static Dictionary<Players.Player, bool> toiletsEnabled = new Dictionary<Players.Player, bool>();
        public bool TryDoCommand(Players.Player player, string chat, List<string> splits)
        {
            if (player == null)
            {
                return false;
            }

            if (!chat.Equals("?nach0toiletsoff"))
            {
                return false;
            }
            if (!toiletsEnabled.ContainsKey(player))
            {
                toiletsEnabled[player] = true;
            }
            if (toiletsEnabled[player])
            {
                toiletsEnabled[player] = false;
            }
            else
            {
                toiletsEnabled[player] = true;

            }


            Chat.Send(player, "<color=blue>Toilets are now disabled you will need to redo this command after every restart</color>");
            return true;
        }
    }
}
