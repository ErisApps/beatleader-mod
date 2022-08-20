﻿using BeatLeader.Replayer;
using BeatLeader.Replayer.Managers;
using BeatSaberMarkupLanguage.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace BeatLeader.Components.Settings
{
    [SerializeAutomatically]
    [ViewDefinition(Plugin.ResourcesPath + ".BSML.Replayer.Components.Settings.Items.CameraMenu.bsml")]
    internal class CameraMenu : MenuWithContainer
    {
        [Inject] private readonly ReplayerCameraController _cameraController;
        [Inject] private readonly InputManager _inputManager;

        [SerializeAutomatically] private static string cameraView = "PlayerView";
        [SerializeAutomatically] private static int cameraFov = 90;

        [UIObject("camera-fov-container")] private GameObject _cameraFovContainer;
        [UIValue("offsets-menu-button")] private SubMenuButton _offsetsMenuButton;

        [UIValue("camera-view-values")] private List<object> _cameraViewValues;
        [UIValue("camera-view")] private string _cameraView
        {
            get => _cameraController.CurrentPose;
            set
            {
                cameraView = value;
                _cameraController.SetCameraPose(value);
                NotifyPropertyChanged(nameof(_cameraView));
                AutomaticConfigTool.NotifyTypeChanged(GetType());
            }
        }
        [UIValue("camera-fov")] private int _cameraFov
        {
            get => _cameraController.FieldOfView;
            set
            {
                cameraFov = value;
                _cameraController.FieldOfView = value;
                NotifyPropertyChanged(nameof(_cameraFov));
                AutomaticConfigTool.NotifyTypeChanged(GetType());
            }
        }

        protected override void OnBeforeParse()
        {
            _offsetsMenuButton = CreateButtonForMenu(this, InstantiateInContainer<OffsetsMenu>(Container), "Offsets");
            _offsetsMenuButton.ButtonGameObject.AddComponent<InputDependentObject>().Init(_inputManager, InputManager.InputType.FPFC);
            _cameraViewValues = new List<object>(_cameraController.poseProviders.Select(x => x.Name));
        }
        protected override void OnAfterParse()
        {
            var obj = _cameraFovContainer.AddComponent<InputDependentObject>();
            obj.Init(_inputManager, InputManager.InputType.FPFC);
            if (obj.ShouldBeVisible) _cameraFov = cameraFov;
            _cameraView = cameraView;
        }
    }
}
