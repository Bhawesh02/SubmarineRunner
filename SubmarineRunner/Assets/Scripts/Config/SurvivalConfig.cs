using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class SurvivalConfig : GenericConfig<SurvivalConfig>
{
    [Serializable]
    public struct PlayerSubmarineData
    {
        public float MoveSpeed;
        [FoldoutGroup("Player Rotation Data", expanded: true), HideLabel]
        public SubmarineRotationData SubmarineRotationData;
        public float PlayerSpeedIncreaseOnPickupCollected;
        [Range(0,100)]
        public float PlayerSpeedPercentageOnPickupUse;
    }
    [Serializable]
    public struct AISubmarineData
    {
        public float MinMoveSpeed;
        public float MoveSpeedIncrement;
        [FoldoutGroup("AI Rotation Data", expanded: true), HideLabel]
        public SubmarineRotationData SubmarineRotationData;
        public LayerMask ObstacleLayer;
        [HorizontalGroup("Movement Switch", Title = "Movement Switch Delay")]
        public float MinMovementSwitchDelay;
        [HorizontalGroup("Movement Switch"), LabelText("Max")]
        public float MaxMovementSwitchDelay;
        [Range(0,100)]
        public float AISpeedPercentangeOnPickupUse;
    }
    [Serializable]
    public struct SubmarineRotationData
    {
        public float RotationSpeed;
        public float RotationAngle;
    }
    
    [TitleGroup("Submarine Data", alignment: TitleAlignments.Centered)]
    
    [FoldoutGroup("Submarine Data/Player Submarine"), HideLabel]
    public PlayerSubmarineData PlayerSubmarine;
    
    [Space,FoldoutGroup("Submarine Data/AI Submarine"), HideLabel]
    public AISubmarineData AISubmarine;
    
    [Space,FoldoutGroup("Submarine Data/Submarine Broken Part")]
    public float GravityForSubmarineBrokenPart = 0.75f;
    [FoldoutGroup("Submarine Data/Submarine Broken Part")]
    public Vector3 RotationForSubmarineBrokenPart;
    [FoldoutGroup("Submarine Data/Submarine Broken Part")]
    public float RotationDelayForSubmarineBrokenPart = 1f;
    
    
    [Space]
    [TitleGroup("Power Ups", alignment: TitleAlignments.Centered)]
    
    [TabGroup("Power Ups/Data","EMP")]
    public float SpeedAfterEMPHit;
    [TabGroup("Power Ups/Data","EMP")]
    public float EMPDuration;
    [TabGroup("Power Ups/Data","EMP")]
    public float EMPPfxStartDelay;
    [TabGroup("Power Ups/Data","EMP")]
    public float EMPRadius = 7f;
    
    [TabGroup("Power Ups/Data","Fishnet")]
    [FoldoutGroup("Power Ups/Data/Fishnet/Fishnet Projectile", expanded: true), HideLabel]
    public ProjectileData FishnetData;
    
    [TabGroup("Power Ups/Data","Teleport")]
    public float TeleportDistance;
    [TabGroup("Power Ups/Data","Teleport")]
    public float TeleportScaleDuration;
    [TabGroup("Power Ups/Data","Teleport")]
    public float TeleportCameraTransitionDelay;
    [TabGroup("Power Ups/Data","Teleport")]
    public float TeleportEndXIncement;
    [TabGroup("Power Ups/Data","Teleport")]
    public float TeleportEndXIncementDelay;
    
    [TabGroup("Power Ups/Data","Turbo")]
    public float TurboSpeed;
    [TabGroup("Power Ups/Data","Turbo")]
    public float TurboActiveDuration;
    
    [TabGroup("Power Ups/Data","Reverse Control")]
    [FoldoutGroup("Power Ups/Data/Reverse Control/Reverse Control Projectile", expanded: true), HideLabel]
    public ProjectileData ReverseControlProjectileData;
    [TabGroup("Power Ups/Data","Reverse Control")]
    public float SubmarineFlipSpeed = 20f;
    [TabGroup("Power Ups/Data","Reverse Control")]
    public Vector3 SubmarineFlipRotation;
    [TabGroup("Power Ups/Data","Reverse Control")]
    public float SubmarineSpriteBlinkDelay = 0.05f;
    
    [Space] 
    [Space] 
    [TitleGroup("Shark", alignment: TitleAlignments.Centered)] 
    
    [BoxGroup("Shark/Movement Data", centerLabel: true)]
    public float SharkSpeed;
    [BoxGroup("Shark/Movement Data")]
    public float SharkRotationSpeed;
    [BoxGroup("Shark/Movement Data"), Range(0,100)] 
    public float SharkSpeedPercentageOnPickupUsed = 10f;
    
    [BoxGroup("Shark/Spike Collision", centerLabel: true)]
    public float SharkPushbackForceOnSpikeCollision;
    [BoxGroup("Shark/Spike Collision")]
    public float SharkSpeedRecoveryAfterSpikeCollision = 9f;
    
    [BoxGroup("Shark/Submarine Eaten", centerLabel: true)]
    public float SharkSpeedAfterSubEaten = 10f;
    [BoxGroup("Shark/Submarine Eaten")]
    public float SharkSpeedDecreaseDelay = 2f;
    
    [BoxGroup("Shark/General", centerLabel: true)]
    public float SharkDistanceToPlayBiteAnimation;
    [BoxGroup("Shark/General")]
    public float SharkSpeedIncreaseOnPickupCollected = 1.5f;
    
}
