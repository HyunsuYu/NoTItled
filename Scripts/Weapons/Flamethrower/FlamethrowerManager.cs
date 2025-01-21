using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Pool;


public class FlamethrowerManager : MonoBehaviour
{
    //[SerializeField] private Transform m_transform_particleSystem;
    [SerializeField] private GameObject m_prefab_Fire;

    [SerializeField] private FlamethrowerConfig m_flamethrowerConfig;

    [SerializeField] private GameObject m_fireHlodArm;

    [SerializeField] private AudioSource m_audioSource;

    private IObjectPool<GameObject> m_firePool;

    private float m_curDurability = 50.0f;

    private float m_fireShootTimer = 0.0f;
    private bool m_bisFireable = true;

    private float m_mainWeaponSlotDurationWidth = 0.0f;
    private float m_subWeaponSlotDurationWidth = 0.0f;


    public void Awake()
    {
        m_curDurability = m_flamethrowerConfig.FlamethrowerDurability;
    }
    public void Start()
    {
        m_mainWeaponSlotDurationWidth = ChiefPlayerManager.Instance.MainWeaponSlot.m_rectTransform_RemainFlamethrowerDurability.sizeDelta.x;
        m_subWeaponSlotDurationWidth = ChiefPlayerManager.Instance.SubWeaponSlot.m_rectTransform_RemainFlamethrowerDurability.sizeDelta.x;
    }
    public void Update()
    {
        if (ReportManager.Instance.BIsReportActive)
        {
            return;
        }

        ChiefPlayerManager.Instance.MainWeaponSlot.m_rectTransform_RemainFlamethrowerDurability.sizeDelta = new Vector2()
        {
            x = m_mainWeaponSlotDurationWidth * (m_curDurability / m_flamethrowerConfig.FlamethrowerDurability),
            y = ChiefPlayerManager.Instance.MainWeaponSlot.m_rectTransform_RemainFlamethrowerDurability.sizeDelta.y
        };
        ChiefPlayerManager.Instance.SubWeaponSlot.m_rectTransform_RemainFlamethrowerDurability.sizeDelta = new Vector2()
        {
            x = m_subWeaponSlotDurationWidth * (m_curDurability / m_flamethrowerConfig.FlamethrowerDurability),
            y = ChiefPlayerManager.Instance.SubWeaponSlot.m_rectTransform_RemainFlamethrowerDurability.sizeDelta.y
        };

        if (ChiefPlayerManager.Instance.CurWeaponType != ChiefPlayerManager.WeaponType.Flamethrower)
        {
            return;
        }

        if(!m_bisFireable)
        {
            m_fireShootTimer += Time.deltaTime;

            if (m_fireShootTimer <= m_flamethrowerConfig.FireSpawnTimeGap)
            {
                return;
            }
            else
            {
                m_fireShootTimer = 0.0f;

                m_bisFireable = true;
            }
        }

        if (m_bisFireable && Input.GetMouseButton(0) && m_curDurability >= 0.0f)
        {
            if(!m_audioSource.isPlaying)
            {
                m_audioSource.loop = true;
                m_audioSource.mute = false;
                m_audioSource.Play();
            }

            GameObject fireObject = Pool.Get();
            ShootFire(fireObject.GetComponent<Rigidbody2D>());

            fireObject.GetComponent<Fire>().StartShoot(new Fire.StartData()
            {
                LifeTime = m_flamethrowerConfig.FireLifeTime
            });

            m_bisFireable = false;

            m_curDurability -= Time.deltaTime * m_flamethrowerConfig.FireDurabilityDecreaseSpeed;

            m_fireHlodArm.SetActive(true);
            ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsNowShooting", true);
        }
        else if(m_bisFireable && Input.GetMouseButton(0) && m_curDurability <= 0.0f)
        {
            NotificationManager.Instance.EnqueueNotification("연료가 부족하다");
            m_audioSource.Stop();
        }
        else
        {
            m_fireHlodArm.SetActive(false);
            ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsNowShooting", false);
            m_audioSource.Stop();
        }
    }

    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (m_firePool == null)
            {
                m_firePool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, m_flamethrowerConfig.CollectionChecks, 10, m_flamethrowerConfig.MaxPoolSize);
            }

            return m_firePool;
        }
    }

    internal FlamethrowerConfig FlamethrowerConfig
    {
        get
        {
            return m_flamethrowerConfig;
        }
    }
    internal float CurDurability
    {
        get
        {
            return m_curDurability;
        }
        set
        {
            m_curDurability = value;
        }
    }
    internal GameObject FireHlodArm
    {
        get
        {
            return m_fireHlodArm;
        }
    }

    internal void SetWeaponActive(in bool bisActive)
    {
        //m_transform_particleSystem.gameObject.SetActive(bisActive);

        if(bisActive)
        {

        }
        else
        {

        }
    }

    private GameObject CreatePooledItem()
    {
        GameObject fire = null;

        switch (ChiefPlayerManager.Instance.PlayerMovement.MoveDirection)
        {
            case PlayerMovement.MovementDirection.Left:
                fire = Instantiate(m_prefab_Fire, ChiefPlayerManager.Instance.LeftShootPos.position, Quaternion.identity);
                break;

            case PlayerMovement.MovementDirection.Right:
                fire = Instantiate(m_prefab_Fire, ChiefPlayerManager.Instance.RightShootPos.position, Quaternion.identity);
                break;
        }

        //var rigidBody = fire.AddComponent<Rigidbody2D>();
        //rigidBody.bodyType = RigidbodyType2D.Kinematic;

        //fire.AddComponent<BoxCollider2D>();

        fire.SetActive(true);

        return fire;
    }
    private void OnTakeFromPool(GameObject obj)
    {
        ShootFire(obj.GetComponent<Rigidbody2D>());

        switch (ChiefPlayerManager.Instance.PlayerMovement.MoveDirection)
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
    private void OnReturnedToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
    private void OnDestroyPoolObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void ShootFire(in Rigidbody2D bullet)
    {
        Vector2 direction = Vector2.zero;

        Vector3 randomShootPos = new Vector3()
        {
            x = ChiefPlayerManager.Instance.ShootTarget.position.x + UnityEngine.Random.Range(-m_flamethrowerConfig.FireShootRadius, m_flamethrowerConfig.FireShootRadius),
            y = ChiefPlayerManager.Instance.ShootTarget.position.y + UnityEngine.Random.Range(-m_flamethrowerConfig.FireShootRadius, m_flamethrowerConfig.FireShootRadius),
            z = ChiefPlayerManager.Instance.ShootTarget.position.z
        };

        switch (ChiefPlayerManager.Instance.PlayerMovement.MoveDirection)
        {
            case PlayerMovement.MovementDirection.Left:
                direction = (randomShootPos - ChiefPlayerManager.Instance.LeftShootPos.position).normalized;
                break;

            case PlayerMovement.MovementDirection.Right:
                direction = (randomShootPos - ChiefPlayerManager.Instance.RightShootPos.position).normalized;
                break;
        }

        bullet.velocity = direction * m_flamethrowerConfig.FireSpeed;
    }
}