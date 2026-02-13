using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public static class AudioIds
{
    public const string GunShot = "Gun_Shot";
    public const string RailCannonFire = "Rail_Cannon_Fire";
    public const string GrenadeLauncherFire = "Grenade_Launcher_Fire";
    public const string GrenadeExplosion = "Grenade_Explosion";
    public const string GroundPoundLand = "Ground_Pound_Land";
    public const string GroundPoundStart = "Ground_Pound_Start";
    public const string Dash = "Dash";
    public const string Jump = "Jump";
    public const string Slide = "Slide";
    public const string Hurt = "hurt";
    public const string MovementFirst = "Movement first";
    public const string AirlockDoorOpenAndClose = "airlock door open and close";
    public const string BarrelExplosion = "barrel explosion";
    public const string VicinityRobotMoving = "vicinity robot moving";
    public const string BlackHoleGunFire = "BlackHole_Gun_Fire";
    public const string SFXDistantScreams = "SFX_Distant Screams";
    public const string SFXEmergencySirenStarting = "SFX_Emergency Siren starting";
    public const string SFXFleshHeartbeat = "SFX_Flesh Heartbeat";
    public const string SFXGasLeak = "SFX_Gas Leak";
    public const string SFXOrganicSquish = "SFX_Organic Squish";
    public const string SFXTrainSeparationEnding = "SFX_train separation ending";
    public const string ObjectItemPickup = "object_item pickup";
    public const string OmissionDeadBlow = "omission_dead blow";
    public const string OmissionDeadHurt = "omission_dead hurt";
    public const string OmissionGroundPoundBarrel = "omission_ground pound barrel";
    public const string OmissionGroundPoundBox = "omission_ground pound box";
    public const string PlayerMovementSecond = "player_Movement second";
    public const string PlayerFleshImpact = "player_flesh impact";
    public const string PlayerLanding = "player_landing";
    public const string PlayerRecovery = "player_recovery";
    public const string RobotTargetAcquiredBleep = "robot_Target Acquired Bleep";
    public const string RobotBombRobotCrashAttack = "robot_bomb robot crash attack";
    public const string RobotBombRobotExplosionAttack = "robot_bomb robot explosion attack";
    public const string RobotBombRobotHatchOpen = "robot_bomb robot hatch open";
    public const string RobotBombRobotMoving = "robot_bomb robot moving";
    public const string RobotBombRobotWalking = "robot_bomb robot walking";
    public const string RobotRobotCrunch1 = "robot_robot crunch1";
    public const string RobotRobotCrunch2 = "robot_robot crunch2";
    public const string RobotRobotCrunch3 = "robot_robot crunch3";
    public const string RobotRobotHeat = "robot_robot heat";
    public const string RobotTurretRobotMoving = "robot_turret robot moving";
    public const string RobotTurretRobotRotate = "robot_turret robot rotate";
    public const string RobotTurretRobotShot = "robot_turret robot shot";
    public const string RobotTurretRobotTransform = "robot_turret robot transform";
    public const string RobotTurretRobotWalking = "robot_turret robot walking";
    public const string RobotVicinityRobotScratch1 = "robot_vicinity robot scratch1";
    public const string RobotVicinityRobotScreech = "robot_vicinity robot screech";
    public const string RobotVicinityRobotSignal = "robot_vicinity robot signal";
    public const string RobotVicinityRobotWaliking = "robot_vicinity robot waliking";
    public const string UiSystemFirstButtonClick = "ui system_first button click";
    public const string UiSystemGlitchNoise = "ui system_glitch noise";
    public const string UiSystemMenuSwipe = "ui system_menu swipe";
    public const string UiSystemSecondButtonClick = "ui system_second button click";
    public const string UiSystemSecondWarningHeart = "ui system_second warning heart";
    public const string WeaponRailgunBeamChargingPower = "weapon_Railgun Beam charging power";
    public const string WeaponBulletShellFalling = "weapon_bullet shell falling";
    public const string WeaponDryFire = "weapon_dry fire";
    public const string WeaponPistolReload = "weapon_pistol reload";
    public const string WeaponPistolWallshot = "weapon_pistol wallshot";
    public const string WeaponRifleReload = "weapon_rifle reload";
    public const string WeaponRifleShot = "weapon_rifle shot";
    public const string BossPhaseChange = "boss phase change";
    public const string BossTurretActivate = "boss turret activate";
    public const string BossTurretLaser = "boss turret laser";
    public const string BossWaring = "boss waring";
    public const string ContainerClose = "container close";
    public const string ContainerOpen = "container open";
    public const string ScanDenied = "scan denied";
    public const string ScanSuccess = "scan success";
    public const string Scaning = "scaning";

    public static readonly string[] All =
    {
        GunShot,
        RailCannonFire,
        GrenadeLauncherFire,
        GrenadeExplosion,
        GroundPoundLand,
        GroundPoundStart,
        Dash,
        Jump,
        Slide,
        Hurt,
        MovementFirst,
        AirlockDoorOpenAndClose,
        BarrelExplosion,
        VicinityRobotMoving,
        BlackHoleGunFire,
        SFXDistantScreams,
        SFXEmergencySirenStarting,
        SFXFleshHeartbeat,
        SFXGasLeak,
        SFXOrganicSquish,
        SFXTrainSeparationEnding,
        ObjectItemPickup,
        OmissionDeadBlow,
        OmissionDeadHurt,
        OmissionGroundPoundBarrel,
        OmissionGroundPoundBox,
        PlayerMovementSecond,
        PlayerFleshImpact,
        PlayerLanding,
        PlayerRecovery,
        RobotTargetAcquiredBleep,
        RobotBombRobotCrashAttack,
        RobotBombRobotExplosionAttack,
        RobotBombRobotHatchOpen,
        RobotBombRobotMoving,
        RobotBombRobotWalking,
        RobotRobotCrunch1,
        RobotRobotCrunch2,
        RobotRobotCrunch3,
        RobotRobotHeat,
        RobotTurretRobotMoving,
        RobotTurretRobotRotate,
        RobotTurretRobotShot,
        RobotTurretRobotTransform,
        RobotTurretRobotWalking,
        RobotVicinityRobotScratch1,
        RobotVicinityRobotScreech,
        RobotVicinityRobotSignal,
        RobotVicinityRobotWaliking,
        UiSystemFirstButtonClick,
        UiSystemGlitchNoise,
        UiSystemMenuSwipe,
        UiSystemSecondButtonClick,
        UiSystemSecondWarningHeart,
        WeaponRailgunBeamChargingPower,
        WeaponBulletShellFalling,
        WeaponDryFire,
        WeaponPistolReload,
        WeaponPistolWallshot,
        WeaponRifleReload,
        WeaponRifleShot,
        BossPhaseChange,
        BossTurretActivate,
        BossTurretLaser,
        BossWaring,
        ContainerClose,
        ContainerOpen,
        ScanDenied,
        ScanSuccess,
        Scaning,
    };
}
