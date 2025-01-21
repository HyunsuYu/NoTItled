using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ChiefPlayerManager : MonoBehaviour
{
    internal enum WeaponType
    {
        Gun,
        Flamethrower
    }

    [Serializable] public struct WeaponSlot
    {
        public Image m_image_MainImage;

        public Image m_image_RemainBulletCount;
        public TMP_Text m_text_RemainBulletCount;

        public Image m_image_FlamethrowerBackground;
        public RectTransform m_rectTransform_RemainFlamethrowerDurability;
    }
    [Serializable] public struct WeaponProfile
    {
        public Sprite m_sprite_ActiveState;
        public Sprite m_sprite_InactiveState;
    }


    private static ChiefPlayerManager m_instance;

    [SerializeField] private MovementConfig m_movementConfig;

    [SerializeField] private Transform m_shootTarget;

    [Header("Bullet Shoot Pos")]
    [SerializeField] private Transform m_leftShootPos;
    [SerializeField] private Transform m_rightShootPos;

    [SerializeField] private WeaponSlot m_mainWeaponSlot;
    [SerializeField] private WeaponSlot m_subWeaponSlot;

    [SerializeField] private WeaponProfile m_gunProfile;
    [SerializeField] private WeaponProfile m_flamethrowerProfile;

    private PlayerMovement m_playerMovement;
    private GunManager m_gunManager;
    private FlamethrowerManager m_flamethrowerManager;

    private Animator m_playerAnimator;
    [SerializeField] private Animator m_gunHlodArmAnimator;
    [SerializeField] private Animator m_fireHoldArmAnimator;

    private Rigidbody2D m_rigidbody2D;
    private SpriteRenderer m_spriteRenderer;

    private WeaponType m_curWeaponType = WeaponType.Gun;

    private bool m_bisFlamethrowerUsable = false;
    private bool m_bisFirstHint = true;

    [SerializeField] private TMP_Text m_text_Flame;


    public void Awake()
    {
        m_instance = this;

        m_playerMovement = GetComponent<PlayerMovement>();
        m_gunManager = GetComponent<GunManager>();
        m_flamethrowerManager = GetComponent<FlamethrowerManager>();

        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_playerAnimator = GetComponent<Animator>();
    }
    public void Update()
    {
        if(ReportManager.Instance.BIsReportActive)
        {
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        switch (ChiefPlayerManager.Instance.PlayerMovement.MoveDirection)
        {
            case PlayerMovement.MovementDirection.Left:
                {
                    float direction = m_leftShootPos.position.x - mousePos.x;

                    if (direction >= 0.0f)
                    {
                        m_shootTarget.position = new Vector3()
                        {
                            x = mousePos.x,
                            y = mousePos.y,
                            z = 0.0f
                        };
                    }
                    else
                    {
                        m_shootTarget.position = new Vector3()
                        {
                            x = m_leftShootPos.position.x,
                            y = mousePos.y,
                            z = 0.0f
                        };
                    }
                }
                break;

            case PlayerMovement.MovementDirection.Right:
                {
                    float direction = m_rightShootPos.position.x - mousePos.x;

                    if (direction <= 0.0f)
                    {
                        m_shootTarget.position = new Vector3()
                        {
                            x = mousePos.x,
                            y = mousePos.y,
                            z = 0.0f
                        };
                    }
                    else
                    {
                        m_shootTarget.position = new Vector3()
                        {
                            x = m_rightShootPos.position.x,
                            y = mousePos.y,
                            z = 0.0f
                        };
                    }
                }
                break;
        }

        if (!m_bisFlamethrowerUsable)
        {
            return;
        }
        else if(m_bisFirstHint)
        {
            m_text_Flame.enabled = true;

            m_bisFirstHint = false;

            Invoke("HideFlameHint", 3.0f);
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(m_curWeaponType == WeaponType.Gun)
            {
                m_curWeaponType = WeaponType.Flamethrower;

                m_gunManager.SetWeaponActive(false);
                m_flamethrowerManager.SetWeaponActive(true);

                SetFlamethrower2MainWeapon();

                PlayerAnimator.SetBool("BIsGun", false);
            }
            else
            {
                m_curWeaponType = WeaponType.Gun;

                m_gunManager.SetWeaponActive(true);
                m_flamethrowerManager?.SetWeaponActive(false);

                SetGun2MainWeapon();

                PlayerAnimator.SetBool("BIsGun", true);
            }
        }
    }

    internal static ChiefPlayerManager Instance
    {
        get
        {
            return m_instance;
        }

    }

    internal bool BIsFlamethrowerUsable
    {
        get
        {
            return m_bisFlamethrowerUsable;
        }
        set
        {
            m_bisFlamethrowerUsable = value;
        }
    }

    internal Transform ShootTarget
    {
        get
        {
            return m_shootTarget;
        }
    }
    internal Transform LeftShootPos
    {
        get
        {
            return m_leftShootPos;
        }
    }
    internal Transform RightShootPos
    {
        get
        {
            return m_rightShootPos;
        }
    }

    internal PlayerMovement PlayerMovement
    {
        get
        {
            return m_playerMovement;
        }
    }
    internal GunManager GunManager
    {
        get
        {
            return m_gunManager;
        }
    }
    internal FlamethrowerManager FlamethrowerManager
    {
        get
        {
            return m_flamethrowerManager;
        }
    }

    internal MovementConfig MovementConfig
    {
        get
        {
            return m_movementConfig;
        }
    }

    internal Rigidbody2D Rigidbody2D
    {
        get
        {
            return m_rigidbody2D;
        }
    }
    internal SpriteRenderer SpriteRenderer
    {
        get
        {
            return m_spriteRenderer;
        }
    }

    internal WeaponType CurWeaponType
    {
        get
        {
            return m_curWeaponType;
        }
        set
        {
            m_curWeaponType = value;
        }
    }

    internal WeaponSlot MainWeaponSlot
    {
        get
        {
            return m_mainWeaponSlot;
        }
    }
    internal WeaponSlot SubWeaponSlot
    {
        get
        {
            return m_subWeaponSlot;
        }
    }

    internal Animator PlayerAnimator
    {
        get
        {
            return m_playerAnimator;
        }
    }
    internal Animator GunHlodArmAnimator
    {
        get
        {
            return m_gunHlodArmAnimator;
        }
    }
    internal Animator FireHoldArmAnimator
    {
        get
        {
            return m_fireHoldArmAnimator;
        }
    }

    private void SetGun2MainWeapon()
    {
        m_mainWeaponSlot.m_image_MainImage.sprite = m_gunProfile.m_sprite_ActiveState;
        m_subWeaponSlot.m_image_MainImage.sprite = m_flamethrowerProfile.m_sprite_InactiveState;

        // Main Slot
        m_mainWeaponSlot.m_image_RemainBulletCount.gameObject.SetActive(true);
        m_mainWeaponSlot.m_text_RemainBulletCount.gameObject.SetActive(true);

        m_mainWeaponSlot.m_image_FlamethrowerBackground.gameObject.SetActive(false);
        m_mainWeaponSlot.m_rectTransform_RemainFlamethrowerDurability.gameObject.SetActive(false);

        // Sub Slot
        m_subWeaponSlot.m_image_RemainBulletCount.gameObject.SetActive(false);
        m_subWeaponSlot.m_text_RemainBulletCount.gameObject.SetActive(false);

        m_subWeaponSlot.m_image_FlamethrowerBackground.gameObject.SetActive(true);
        m_subWeaponSlot.m_rectTransform_RemainFlamethrowerDurability.gameObject.SetActive(true);
    }
    private void SetFlamethrower2MainWeapon()
    {
        m_mainWeaponSlot.m_image_MainImage.sprite = m_flamethrowerProfile.m_sprite_ActiveState;
        m_subWeaponSlot.m_image_MainImage.sprite = m_gunProfile.m_sprite_InactiveState;

        // Main Slow
        m_mainWeaponSlot.m_image_FlamethrowerBackground.gameObject.SetActive(true);
        m_mainWeaponSlot.m_rectTransform_RemainFlamethrowerDurability.gameObject.SetActive(true);

        m_mainWeaponSlot.m_image_RemainBulletCount.gameObject.SetActive(false);
        m_mainWeaponSlot.m_text_RemainBulletCount.gameObject.SetActive(false);

        // Sub Slot
        m_subWeaponSlot.m_image_RemainBulletCount.gameObject.SetActive(true);
        m_subWeaponSlot.m_text_RemainBulletCount.gameObject.SetActive(true);

        m_subWeaponSlot.m_image_FlamethrowerBackground.gameObject.SetActive(false);
        m_subWeaponSlot.m_rectTransform_RemainFlamethrowerDurability.gameObject.SetActive(false);
    }

    internal void SetPlayerActive(in bool bisActive)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = bisActive;
    }
    internal void SetPlayerPos(in Vector2 pos)
    {
        transform.position = pos;
    }

    private void HideFlameHint()
    {
        m_text_Flame.enabled = false;
    }
}