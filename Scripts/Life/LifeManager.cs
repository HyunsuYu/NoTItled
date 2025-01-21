using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


public class LifeManager : MonoBehaviour
{
    internal delegate void LifeisZero();


    private static LifeManager m_instance;

    [Header("Life Icon Prefab & Parent")]
    [SerializeField] private GameObject m_prefab_LifeIcon;
    [SerializeField] private Transform m_Transform_LifeIconParent;

    [Header("Life Icon Sprites")]
    [SerializeField] private List<Sprite> m_lifeIcons;

    [Header("Life Count - 2의 배수로만")]
    [SerializeField] private int m_maxLife;

    private int m_prevLife;
    [SerializeField] private int m_curLife;

    [SerializeField] private float m_lifeChangeAnimationDuration = 0.5f;
    [SerializeField] private float m_lifeChangeAnimationTimer = 0.0f;

    private List<SingleLifeIcon> m_singleLifeIcons = new List<SingleLifeIcon>();

    private LifeisZero m_lifeisZero;
    private bool m_bisDead = false;


    public void Awake()
    {
        m_instance = this;

        m_curLife = m_maxLife;
        m_prevLife = m_curLife;
        int spawnedLifeIconCount = m_maxLife / 2;

        for(int count = 0; count < spawnedLifeIconCount; count++)
        {
            GameObject newLifeIconObject = Instantiate(m_prefab_LifeIcon, m_Transform_LifeIconParent);
            SingleLifeIcon newSingleLifeIcon = newLifeIconObject.GetComponent<SingleLifeIcon>();

            m_singleLifeIcons.Add(newSingleLifeIcon);
        }

        m_lifeisZero += TestDeadCallback;
    }
    public void FixedUpdate()
    {
        if(m_curLife == 0 && !m_bisDead)
        {
            m_lifeisZero?.Invoke();

            m_bisDead = true;
        }

        if(m_prevLife != m_curLife)
        {
            ApplyLifeIconChangingState();

            m_lifeChangeAnimationTimer += Time.fixedDeltaTime;

            if (m_lifeChangeAnimationTimer >= m_lifeChangeAnimationDuration)
            {
                m_lifeChangeAnimationTimer = 0.0f;
                m_prevLife = m_curLife;
            }
        }

        if(m_prevLife == m_curLife)
        {
            ApplyLifeIconState();
        }
    }

    internal static LifeManager Instance
    {
        get
        {
            return m_instance;
        }
    }

    internal List<Sprite> LifeIcons
    {
        get
        {
            return m_lifeIcons;
        }
    }
    internal int CurLife
    {
        get
        {
            return m_curLife;
        }
        set
        {
            if (0 <= value && value <= m_maxLife)
            {
                m_curLife = value;
            }
        }
    }

    internal LifeisZero LifeisZeroCallback
    {
        get
        {
            return m_lifeisZero;
        }
        set
        {
            m_lifeisZero = value;
        }
    }

    internal int MaxLife
    {
        get
        {
            return m_maxLife;
        }
    }

    private void ApplyLifeIconChangingState()
    {
        foreach(SingleLifeIcon singleLifeIcon in m_singleLifeIcons)
        {
            singleLifeIcon.SetIconState(SingleLifeIcon.LifeIconType.Changing);
        }
    }
    private void ApplyLifeIconState()
    {
        int curLife = m_curLife;

        for (int index = 0; index < m_singleLifeIcons.Count; index++)
        {
            if(curLife - 2 >= 0)
            {
                m_singleLifeIcons[index].SetIconState(SingleLifeIcon.LifeIconType.Full);
                curLife -= 2;
            }
            else if(curLife - 1 >= 0)
            {
                m_singleLifeIcons[index].SetIconState(SingleLifeIcon.LifeIconType.Half);
                curLife -= 1;
            }
            else
            {
                m_singleLifeIcons[index].SetIconState(SingleLifeIcon.LifeIconType.None);
            }
        }
    }

    private void TestDeadCallback()
    {
        Debug.Log("Dead");
    }
}