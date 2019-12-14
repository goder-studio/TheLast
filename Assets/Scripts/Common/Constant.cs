using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{
    public static readonly Vector3 playerSpawnPosition = new Vector3(5.0f, 0.3f, 11.0f);

    public const float EnemyWalkSpeed = 2;
    public const float EnemyRunSpeed = 5;
    public const float smoothSpeed = 5;

    public const float BlendIdle = 0;
    public const float BlendWalk = 0.5f;
    public const float BlendRun = 1.0f;

    public const int ActionDefault = -1;
    public const int ActionBorn = 0;
    public const int ActionDie = 100;
    public const int ActionHit = 50;
    public const int ActionAttack = 1;

    public const string AniBornName = "standUp";
    public const string AniDieName = "fallBack";
    public const string AniHitName = "hit2";
    public const string AniAttackName = "attack2";
    public const string AniIdleName = "idle";
    public const string AniWalkName = "walk";

    public const float EnemyAttackDistance = 2.0f;
    public const float EnemyAttackAngle = 180.0f;

    public const float colorSmoothSpeed = 0.5f;
    public const float fillAmountSmoothSpeed = 0.1f;

    public static Color colorGreen = new Color(0, 1.0f, 0, 0.9f);
    public static Color colorYellow = new Color(1.0f, 1.0f, 0, 0.9f);
    public static Color colorRed = new Color(1.0f, 0, 0, 0.9f);

    public static Color colorEnemyCrazy = new Color(0.9f, 0, 0, 1.0f);

    public const int SceneStartID = 0;
    public const int SceneMainID = 1;
    public const int SceneBattleID = 2;

}
