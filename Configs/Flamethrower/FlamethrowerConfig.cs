using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[CreateAssetMenu(fileName = "FlamethrowerConfig", menuName = "Flamethrower/FlamethrowerConfig", order = 1)]
public class FlamethrowerConfig : ScriptableObject
{
    [Header("최적화 관련")]
    public bool CollectionChecks = true;
    public int MaxPoolSize = 10;

    [Header("불 유지 시간")]
    public float FireLifeTime = 1.0f;

    [Header("불 속도(화염방사기 길이)")]
    public float FireSpeed = 15.0f;

    [Header("불 풍성함 정도")]
    public float FireSpawnTimeGap = 0.01f;

    [Header("화염방사기 장탄 크기")]
    public float FlamethrowerDurability = 50.0f;

    [Header("화염방사기 퍼짐 정도")]
    public float FireShootRadius = 5.0f;

    [Header("화염방사기 내구도 감소 속도")]
    public float FireDurabilityDecreaseSpeed = 0.1f;
}