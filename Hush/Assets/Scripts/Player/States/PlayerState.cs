using System;
using UnityEngine;

namespace Player.Helpers
{
    public sealed class PlayerState
    {
        private static readonly Lazy<PlayerState> LazyState = new Lazy<PlayerState>(() => new PlayerState());

        private PlayerState() { }
        
        public static PlayerState Instance => LazyState.Value;

        #region Multithreading Locks
        
        private static readonly object MoveDirectionLock = new object();
        private static readonly object DesiredForwardSpeedLock = new object();
        private static readonly object DesiredLateralSpeedLock = new object();
        private static readonly object ActualForwardSpeedLock = new object();
        private static readonly object ActualLateralSpeedLock = new object();
        
        private static readonly object SprintingLock = new object();
        private static readonly object CrouchingLock = new object();
        private static readonly object JumpingLock = new object();

        #endregion
        
        #region Movement States
        
        private Vector2 _moveDirection;
        public Vector2 MoveDirection
        {
            get
            {
                lock (MoveDirectionLock)
                {
                    return _moveDirection;
                }
            }
            set
            {
                lock (MoveDirectionLock)
                {
                    _moveDirection = value;
                }
            }
        }
        
        private float _desiredForwardSpeed;
        public float DesiredForwardSpeed
        {
            get
            {
                lock (DesiredForwardSpeedLock)
                {
                    return _desiredForwardSpeed;
                }
            }
            set
            {
                lock (DesiredForwardSpeedLock)
                {
                    _desiredForwardSpeed = value;
                }
            }
        }
        
        private float _desiredLateralSpeed;
        public float DesiredLateralSpeed
        {
            get
            {
                lock (DesiredLateralSpeedLock)
                {
                    return _desiredLateralSpeed;
                }
            }
            set
            {
                lock (DesiredLateralSpeedLock)
                {
                    _desiredLateralSpeed = value;
                }
            }
        }
        
        private float _actualForwardSpeed;
        public float ActualForwardSpeed
        {
            get
            {
                lock (ActualForwardSpeedLock)
                {
                    return _actualForwardSpeed;
                }
            }
            set
            {
                lock (ActualForwardSpeedLock)
                {
                    _actualForwardSpeed = value;
                }
            }
        }
        
        private float _actualLateralSpeed;
        public float ActualLateralSpeed
        {
            get
            {
                lock (ActualLateralSpeedLock)
                {
                    return _actualLateralSpeed;
                }
            }
            set
            {
                lock (ActualLateralSpeedLock)
                {
                    _actualLateralSpeed = value;
                }
            }
        }
    
        #endregion
        
        #region Input States
        
        private bool _sprinting;
        public bool Sprinting
        {
            get
            {
                lock (SprintingLock)
                {
                    return _sprinting;
                }
            }
            set
            {
                lock (SprintingLock)
                {
                    _sprinting = value;
                }
            }
        }
        
        private bool _crouching;
        public bool Crouching{
            get
            {
                lock (CrouchingLock)
                {
                    return _crouching;
                }
            }
            set
            {
                lock (CrouchingLock)
                {
                    _crouching = value;
                }
            }
        }
        
        private bool _jumping;
        public bool Jumping{
            get
            {
                lock (JumpingLock)
                {
                    return _jumping;
                }
            }
            set
            {
                lock (JumpingLock)
                {
                    _jumping = value;
                }
            }
        }

        #endregion
    }
}