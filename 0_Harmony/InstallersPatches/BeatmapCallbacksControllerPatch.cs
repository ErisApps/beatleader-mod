using BeatLeader.Core.Managers.NoteEnhancer;
using HarmonyLib;
using IPA.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace BeatLeader
{

    [HarmonyPatch(typeof(BeatmapCallbacksController), nameof(BeatmapCallbacksController.ManualUpdate))]
    public static class BeatmapCallbacksControllerPatch
    {
        static bool Prefix(BeatmapCallbacksController __instance, float songTime, float ____prevSongTime, Dictionary<float, CallbacksInTime> ____callbacksInTimes, float ____startFilterTime, IReadonlyBeatmapData ____beatmapData)
        {
            if ((double)songTime == (double)____prevSongTime)
                return false;
            __instance.SetField("_songTime", songTime);
            __instance.SetField("_processingCallbacks", true);

            bool backInTime = (double)songTime < (double)____prevSongTime;

            foreach (KeyValuePair<float, CallbacksInTime> callbacksInTime1 in ____callbacksInTimes)
            {
                CallbacksInTime callbacksInTime2 = callbacksInTime1.Value;
                for (LinkedListNode<BeatmapDataItem> linkedListNode = callbacksInTime2.lastProcessedNode != null && !backInTime ? callbacksInTime2.lastProcessedNode.Next : ____beatmapData.allBeatmapDataItems.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
                {
                    BeatmapDataItem beatmapDataItem = linkedListNode.Value;
                    if ((double)beatmapDataItem.time - (double)callbacksInTime2.aheadTime <= (double)songTime)
                    {
                        if (beatmapDataItem.type == BeatmapDataItem.BeatmapDataItemType.BeatmapEvent || beatmapDataItem.type == BeatmapDataItem.BeatmapDataItemType.BeatmapObject && (double)beatmapDataItem.time >= (double)____startFilterTime)
                            callbacksInTime2.CallCallbacks(beatmapDataItem);
                        callbacksInTime2.lastProcessedNode = linkedListNode;
                    }
                    else
                        break;
                }
            }
            __instance.SetField("_prevSongTime", songTime);
            __instance.SetField("_processingCallbacks", false);

            return false;
        }
    }
}
