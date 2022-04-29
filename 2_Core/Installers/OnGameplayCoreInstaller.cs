using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using BeatLeader.Utils;
using BeatLeader.Models;
using BeatLeader.Replays;
using BeatLeader.Replays.MapEmitating;
using BeatLeader.Replays.Movement;
using BeatLeader.Replays.Scoring;
using BeatLeader.Core.Managers.ReplayEnhancer;
using HarmonyLib;
using IPA.Utilities;
using Zenject;
using UnityEngine;

namespace BeatLeader.Installers
{
    public class OnGameplayCoreInstaller : Installer<OnGameplayCoreInstaller>
    {
        private static readonly MethodBase ScoreSaber_playbackEnabled = AccessTools.Method("ScoreSaber.Core.ReplaySystem.HarmonyPatches.PatchHandleHMDUnmounted:Prefix");

        public override void InstallBindings()
        {
            if (ReplayMenuUI.asReplay)
            {
                ReplayManualInstaller.Install(ReplayMenuUI.replay, new ReplayManualInstaller.InitData(true, true, true, true, 130, 2, true), Container);
            }
            else InitRecorder();
        }
        private void InitRecorder()
        {
            #region Gates
            //if (ScoreSaber_playbackEnabled != null && (bool)ScoreSaber_playbackEnabled.Invoke(null, null) == false)
            //{
            //    Plugin.Log.Warn("SS replay is running, BL Replay Recorder will not be started!");
            //    return;
            //}
            //if (!(MapEnhancer.previewBeatmapLevel.levelID.StartsWith(CustomLevelLoader.kCustomLevelPrefixId)))
            //{
            //    Plugin.Log.Notice("OST level detected! Recording unavailable!");
            //    return;
            //}
            #endregion

            Plugin.Log.Debug("Starting a BL Replay Recorder.");

            Container.BindInterfacesAndSelfTo<ReplayRecorder>().AsSingle();
            Container.BindInterfacesAndSelfTo<TrackingDeviceEnhancer>().AsTransient();
        }
    }
}