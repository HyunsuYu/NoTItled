using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;


public class GunManager : MonoBehaviour
{
    private static GunManager m_instance;

    [Header("Prefabs")]
    [SerializeField] private GameObject m_prefab_bullet;

    [Header("UI Components")]
    [SerializeField] private TMP_Text m_text_remainBulletCount;

    [Header("Config")]
    [SerializeField] private GunConfig m_gunConfig;

    [SerializeField] private GameObject m_gunHlodArm;

    [SerializeField] private AudioSource m_audioSource;

    [SerializeField] private TMP_Text m_text_Reload;

    private IObjectPool<GameObject> m_bulletPool;

    private float m_bulletShootTimer;

    private int m_curShootBulletCount = 0;
    private float m_reloadDelayTimer = 0.0f;
    private bool m_bisReloading = false;

    //private bool m_bisNowShooting = false;


    public void Awake()
    {
        m_instance = this;

        //m_text_remainBulletCount.text = (m_gunConfig.BulletCount - m_curShootBulletCount).ToString() + "/" + m_gunConfig.BulletCount.ToString();
        ChiefPlayerManager.Instance.MainWeaponSlot.m_text_RemainBulletCount.text = (m_gunConfig.BulletCount - m_curShootBulletCount).ToString();
        ChiefPlayerManager.Instance.SubWeaponSlot.m_text_RemainBulletCount.text = (m_gunConfig.BulletCount - m_curShootBulletCount).ToString();
    }
    public void Update()
    {
        if (ReportManager.Instance.BIsReportActive)
        {
            return;
        }

        if (ChiefPlayerManager.Instance.CurWeaponType != ChiefPlayerManager.WeaponType.Gun)
        {
            return;
        }

        if(m_curShootBulletCount >= m_gunConfig.BulletCount)
        {
            m_reloadDelayTimer += Time.deltaTime;

            if(m_reloadDelayTimer <= m_gunConfig.BulletReloadTime)
            {
                m_gunHlodArm.SetActive(false);
                m_text_Reload.enabled = true;
                ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsReload", true);
                m_bisReloading = true;

                ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsNowShooting", false);
                ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsWalk", false);

                return;
            }
            else
            {
                m_text_Reload.enabled = false;
                ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsReload", false);
                m_bisReloading = false;

                m_reloadDelayTimer = 0.0f;

                m_curShootBulletCount = 0;

                //m_text_remainBulletCount.text = (m_gunConfig.BulletCount - m_curShootBulletCount).ToString() + "/" + m_gunConfig.BulletCount.ToString();
                ChiefPlayerManager.Instance.MainWeaponSlot.m_text_RemainBulletCount.text = (m_gunConfig.BulletCount - m_curShootBulletCount).ToString();
                ChiefPlayerManager.Instance.SubWeaponSlot.m_text_RemainBulletCount.text = (m_gunConfig.BulletCount - m_curShootBulletCount).ToString();
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (!m_audioSource.isPlaying)
            {
                m_audioSource.loop = true;
                m_audioSource.mute = false;
                m_audioSource.Play();
            }

            m_gunHlodArm.SetActive(true);
            ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsNowShooting", true);
            ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsWalk", true);

            if (m_bulletShootTimer <= 0.0f)
            {
                //m_bisNowShooting = true;

                GameObject bulletObject = Pool.Get();

                ShootBullet(bulletObject.GetComponent<Rigidbody2D>());

                bulletObject.GetComponent<Bullet>().StartShoot(new Bullet.StartData()
                {
                    LifeTime = m_gunConfig.BulletLifeTime
                });

                m_bulletShootTimer = m_gunConfig.BulletShootTerm;

                m_curShootBulletCount++;

                //m_text_remainBulletCount.text = (m_gunConfig.BulletCount - m_curShootBulletCount).ToString() + "/" + m_gunConfig.BulletCount.ToString();
                ChiefPlayerManager.Instance.MainWeaponSlot.m_text_RemainBulletCount.text = (m_gunConfig.BulletCount - m_curShootBulletCount).ToString();
                ChiefPlayerManager.Instance.SubWeaponSlot.m_text_RemainBulletCount.text = (m_gunConfig.BulletCount - m_curShootBulletCount).ToString();
            }
            else
            {
                m_bulletShootTimer -= Time.deltaTime;

                //m_bisNowShooting = false;
                //m_gunHlodArm.SetActive(false);
                //ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsNowShooting", false);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_gunHlodArm.SetActive(false);
            ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsNowShooting", false);
            ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsWalk", false);

            m_audioSource.Stop();
        }
    }

    internal static GunManager Instance
    {
        get
        {
            return m_instance;
        }
    }

    public IObjectPool<GameObject> Pool
    {
        get
        {
            if(m_bulletPool == null)
            {
                m_bulletPool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, m_gunConfig.CollectionChecks, 10, m_gunConfig.MaxPoolSize);
            }

            return m_bulletPool;
        }
    }

    internal GameObject GunHoldArm
    {
        get
        {
            return m_gunHlodArm;
        }
    }
    internal bool BIsReloading
    {
        get
        {
            return m_bisReloading;
        }
    }

    internal void SetWeaponActive(in bool bisActive)
    {

    }

    private GameObject CreatePooledItem()
    {
        GameObject bullet = null;

        switch(ChiefPlayerManager.Instance.PlayerMovement.MoveDirection)
        {
            case PlayerMovement.MovementDirection.Left:
                bullet = Instantiate(m_prefab_bullet, ChiefPlayerManager.Instance.LeftShootPos.position, Quaternion.identity);
                break;

            case PlayerMovement.MovementDirection.Right:
                bullet = Instantiate(m_prefab_bullet, ChiefPlayerManager.Instance.RightShootPos.position, Quaternion.identity);
                break;
        }

        bullet.AddComponent<Rigidbody2D>();
        bullet.AddComponent<BoxCollider2D>();

        return bullet;
    }

    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
    private void OnTakeFromPool(GameObject obj)
    {
        ShootBullet(obj.GetComponent<Rigidbody2D>());

        switch(ChiefPlayerManager.Instance.PlayerMovement.MoveDirection)
        {
            case PlayerMovement.MovementDirection.Left:
                obj.transform.position = ChiefPlayerManager.Instance.LeftShootPos.position;
                break;

            case PlayerMovement.MovementDirection.Right:
                obj.transform.position = ChiefPlayerManager.Instance.RightShootPos.position;
                break;
        }

        obj.SetActive(true);
    }
    private void OnDestroyPoolObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void ShootBullet(in Rigidbody2D bullet)
    {
        Vector2 direction = Vector2.zero;

        switch(ChiefPlayerManager.Instance.PlayerMovement.MoveDirection)
        {
            case PlayerMovement.MovementDirection.Left:
                direction = (ChiefPlayerManager.Instance.ShootTarget.position - ChiefPlayerManager.Instance.LeftShootPos.position).normalized;
                break;

            case PlayerMovement.MovementDirection.Right:
                direction = (ChiefPlayerManager.Instance.ShootTarget.position - ChiefPlayerManager.Instance.RightShootPos.position).normalized;
                break;
        }

        bullet.velocity = direction * m_gunConfig.BulletSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.rotation = angle;
    }
}