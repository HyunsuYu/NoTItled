using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;


[CreateAssetMenu(fileName = "GunConfig", menuName = "Gun/GunConfig", order = 1)]
public class GunConfig : ScriptableObject
{
    [Header("최적화 관련")]
    public bool CollectionChecks = true;
    public int MaxPoolSize = 10;

    [Header("공격 속도 & 총알 속도")]
    public float BulletShootTerm = 0.5f;
    public float BulletSpeed = 50.0f;

    [Header("총알 Active 시간")]
    public float BulletLifeTime = 3.0f;

    [Header("한 발 당 데미지")]
    public float BulletDamage = 1.0f;

    [Header("총 탄창 갯수 및 재장전 소요 시간")]
    public int BulletCount = 30;
    public float BulletReloadTime = 2.0f;
}