﻿using BeatLeader.Components;
using BeatLeader.Models;
using BeatLeader.Replayer.Camera;
using BeatLeader.Replayer.Emulation;
using BeatLeader.UI;
using BeatLeader.Utils;
using System;
using Zenject;

namespace BeatLeader.Replayer {
    internal class SettingsLoader : IInitializable, IDisposable {
        [InjectOptional] private readonly PlayerDataModel _playerDataModel;
        [InjectOptional] private readonly ReplayWatermark _watermark;
        [InjectOptional] private readonly ReplayerCameraController _cameraController;
        [InjectOptional] private readonly ReplayLaunchData _launchData;
        [InjectOptional] private readonly VRControllersAccessor _controllersProvider;
        [InjectOptional] private readonly ReplayerUIBinder _uiBinder;

        public void Initialize() {
            if (_watermark != null) {
                _watermark.Enabled = _launchData.ActualSettings.ShowWatermark;
            }

            if (_playerDataModel != null && _cameraController != null) {
                if (!_playerDataModel.playerData.playerSpecificSettings.reduceDebris)
                    _cameraController.CullingMask |= 1 << LayerMasks.noteDebrisLayer;
                else
                    _cameraController.CullingMask &= ~(1 << LayerMasks.noteDebrisLayer);
            }

            if (_uiBinder != null)
                _uiBinder.UIVisibilityChangedEvent += HandleUIVisibilityChanged;

            if (_cameraController != null) {
                if (InputUtils.IsInFPFC) {
                    _cameraController.FieldOfView = _launchData.ActualSettings.CameraFOV;
                    _cameraController.CameraFOVChangedEvent += HandleCameraFOVChanged;
                }
                _cameraController.SetCameraPose(InputUtils.IsInFPFC ?
                    _launchData.ActualSettings.FPFCCameraPose : _launchData.ActualSettings.VRCameraPose);
                _cameraController.CameraPoseChangedEvent += HandleCameraPoseChanged;
            }

            if (_controllersProvider != null) {
                var settings = _launchData.ActualSettings;

                _controllersProvider.Head.gameObject.SetActive(settings.ShowHead);
                _controllersProvider.LeftSaber.gameObject.SetActive(settings.ShowLeftSaber);
                _controllersProvider.RightSaber.gameObject.SetActive(settings.ShowRightSaber);

                if (!InputUtils.IsInFPFC)
                _controllersProvider.ShowHands();
            }
        }
        public void Dispose() {
            if (_cameraController != null) {
                _cameraController.CameraFOVChangedEvent -= HandleCameraFOVChanged;
                _cameraController.CameraPoseChangedEvent -= HandleCameraPoseChanged;
            }

            if (_uiBinder != null) _uiBinder.UIVisibilityChangedEvent -= HandleUIVisibilityChanged;

            PluginConfig.NotifyReplayerSettingsChanged();
        }

        private void HandleUIVisibilityChanged(bool visible) {
            _launchData.ActualToWriteSettings.ShowUI = visible;
        }
        private void HandleCameraFOVChanged(int fov) {
            _launchData.ActualToWriteSettings.CameraFOV = fov;
        }
        private void HandleCameraPoseChanged(ICameraPoseProvider poseProvider) {
            var settings = _launchData.ActualToWriteSettings;
            settings.FPFCCameraPose = InputUtils.IsInFPFC ? poseProvider.Name : settings.FPFCCameraPose;
            settings.VRCameraPose = !InputUtils.IsInFPFC ? poseProvider.Name : settings.VRCameraPose;
        }
    }
}
