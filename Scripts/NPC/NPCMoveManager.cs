using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class NPCMoveManager : MonoBehaviour
{
    private Rigidbody2D m_rigidbody2D;
    private SpriteRenderer m_spriteRenderer;

    private Animator m_animator;

    [SerializeField] private int m_npcOrder;
    [SerializeField] private Animator m_rushSmokeAnimator;

    private float m_npcWaitTime = 1.0f;
    private float m_npcWaitTimer = 0.0f;

    [SerializeField] private bool m_bisMoving = false;
    private bool m_bisNPCMoveing = false;

    private bool m_bisNPCDead = false;

    public bool m_bisPHD = false;


    public void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_animator = GetComponent<Animator>();
    }
    public void FixedUpdate()
    {
        if(m_bisPHD)
        {
            return;
        }

        if(m_bisNPCDead)
        {
            //transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            m_animator.Play("Player_Died");

            return;
        }

        if(ChiefPlayerManager.Instance.PlayerMovement.BIsMoved)
        {
            m_bisMoving = true;
        }

        if(m_bisMoving)
        {
            m_npcWaitTimer += Time.deltaTime;
            if (m_bisMoving && m_npcWaitTimer >= m_npcWaitTime * (m_npcOrder + 1))
            {
                m_npcWaitTimer = 0.0f;

                m_bisNPCMoveing = true;
            }
        }

        if(m_bisNPCMoveing)
        {
            Vector3 direction = (ChiefPlayerManager.Instance.transform.position - transform.position).normalized;
            if(direction.x < 0.0f)
            {
                m_rigidbody2D.velocity = new Vector2(-ChiefPlayerManager.Instance.MovementConfig.MovementSpeed, m_rigidbody2D.velocity.y);
                m_spriteRenderer.flipX = true;

                m_animator.SetBool("BIsWalk", true);

                m_rushSmokeAnimator.gameObject.SetActive(true);
                m_rushSmokeAnimator.SetBool("BIsWalk", true);
                m_rushSmokeAnimator.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                m_rigidbody2D.velocity = new Vector2(ChiefPlayerManager.Instance.MovementConfig.MovementSpeed, m_rigidbody2D.velocity.y);
                m_spriteRenderer.flipX = false;

                m_animator.SetBool("BIsWalk", true);

                m_rushSmokeAnimator.gameObject.SetActive(true);
                m_rushSmokeAnimator.SetBool("BIsWalk", true);
                m_rushSmokeAnimator.GetComponent<SpriteRenderer>().flipX = false;
            }

            if (Vector3.Distance(ChiefPlayerManager.Instance.transform.position, transform.position) <= 0.1f)
            {
                m_bisMoving = false;
                m_bisNPCMoveing = false;

                m_rigidbody2D.velocity = Vector2.zero;

                m_animator.SetBool("BIsWalk", false);

                m_rushSmokeAnimator.gameObject.SetActive(false);
                m_rushSmokeAnimator.SetBool("BIsWalk", false);
            }
        }
    }

    internal bool BIsNPCMoveing
    {
        get
        {
            return m_bisNPCMoveing;
        }
    }
    internal bool BIsNPCDead
    {
        get
        {
            return m_bisNPCDead;
        }
        set
        {
            m_bisNPCDead = value;
        }
    }

    internal void Reorder(in int order)
    {
        m_npcOrder = order;

        //m_spriteRenderer.sortingOrder = 4 - m_npcOrder;
    }

    internal void SetWalk(in bool bisWalk)
    {
        if(bisWalk)
        {
            m_animator.SetBool("BIsWalk", true);

            m_rushSmokeAnimator.gameObject.SetActive(true);
            m_rushSmokeAnimator.SetBool("BIsWalk", true);
        }
        else
        {
            m_animator.SetBool("BIsWalk", false);

            m_rushSmokeAnimator.gameObject.SetActive(false);
            m_rushSmokeAnimator.SetBool("BIsWalk", false);
        }
    }
    internal void SetNPCActive(in bool bisActive)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = bisActive;
    }
    internal void SetNPCPos(in Vector2 pos)
    {
        transform.position = pos;
    }
}