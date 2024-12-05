using System;
using Game.Scripts.Behaviours;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Playable
{
    public class NumericChanger : MonoBehaviour
    {
        
        // Added for Playable Version2
        //
        [LunaPlaygroundField("Swerve speed", 0, "On Air Plane Physics settings")] [SerializeField]
        private float _onAirSwerveSpeedPL;

        [LunaPlaygroundField("Max Swerve amount", 1, "On Air Plane Physics settings")] [SerializeField]
        private float _maxOnAirSwerveAmountPL;

        [LunaPlaygroundField("Swerve speed", 0, "In Tunnel Plane Physics settings")] [SerializeField]
        private float _inTunnelSwerveSpeedPL;
        
        [LunaPlaygroundField("Max Swerve amount", 1, "In Tunnel Plane Physics settings")] [SerializeField]
        private float _inTunnelSwerveAmountPL;
        
        [LunaPlaygroundField("Plane speed", 2, "In Tunnel Plane Physics settings")] [SerializeField]
        private float _planeSpeedPL;

        [LunaPlaygroundField("Rotation Speed", 3, "In Tunnel Plane Physics settings")] [SerializeField]
        private float _rotationSpeedPL;
        
        [LunaPlaygroundField("Rotation Logic ", 4, "In Tunnel Plane Physics settings")] [SerializeField]
        private bool _rotationWayPL;

        [LunaPlaygroundField("Train speed", 0, "Train Physics")] [SerializeField]
        private float _trainSpeedPL;

        [SerializeField] private PlaneBehaviour _planeBehaviour;
        [SerializeField] private SwerveMovement _swerveMovement;
        [SerializeField] private TrainMovement _trainMovement;
        
        //

        private void Awake()
        {
            // Added for Playable Version2
            //
            _planeBehaviour.onAirSwerveSpeed = _onAirSwerveSpeedPL;
            _planeBehaviour.onAirMaxSwerveAmount = _maxOnAirSwerveAmountPL;
            _swerveMovement.swerveSpeed = _inTunnelSwerveSpeedPL;
            _swerveMovement.maxSwerveAmount = _inTunnelSwerveAmountPL;
            _swerveMovement._forwardSpeed = _planeSpeedPL;
            _swerveMovement._rotationSpeed = _rotationSpeedPL;
            _swerveMovement.inverseRotationWay = _rotationWayPL;
            _trainMovement._trainSpeed = _trainSpeedPL;
            //
        }
    }
}