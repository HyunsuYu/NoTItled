using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;


public class BossManager : MonoBehaviour
{
    internal enum BossMoveType : byte
    {
        LeftMove,
        RightMove,
        Idle
    }


    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;
    private Rigidbody2D m_rigidbody2D;
    private BoxCollider2D m_boxCollider2D;

    [SerializeField] private Animator m_tailAnimator;
    [SerializeField] private SpriteRenderer m_tailSpriteRenderer;
    [SerializeField] private Transform m_collosionChecker;

    private Queue<BossMoveType> m_nextBossMoves = new Queue<BossMoveType>();

    private float m_bossMoveTime = 1.0f;
    private float m_bossMoveTimer = 0.0f;

    private float m_health = 100.0f;

    [SerializeField] private Light2D m_light;


    public void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_boxCollider2D = GetComponent<BoxCollider2D>();

        for (int count = 0; count < 3; count++)
        {
            m_nextBossMoves.Enqueue((BossMoveType)UnityEngine.Random.Range(0, 3));
        }
    }
    public void FixedUpdate()
    {
        if(m_health <= 0.0f)
        {
            m_animator.Play("Boss_Died");
            m_rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
            m_collosionChecker.GetComponent<BossCollisionChecker>().BIsDead = true;

            Invoke("TurnOnLight", 0.6f);

            Invoke("TuenOffLight", 3.0f);

            Ending.Instance.BIsBossKilled = true;

            Invoke("Dead", 5.0f);
            return;
        }

        m_bossMoveTimer += Time.deltaTime;
        if (m_bossMoveTimer >= m_bossMoveTime)
        {
            m_bossMoveTimer = 0.0f;
            BossMoveType nextBossMove = m_nextBossMoves.Dequeue();
            switch (nextBossMove)
            {
                case BossMoveType.LeftMove:
                    m_collosionChecker.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    m_boxCollider2D.offset = new Vector2(-0.5f, -0.2f);

                    m_rigidbody2D.velocity = new Vector2(-5.0f, 0.0f);

                    m_tailSpriteRenderer.gameObject.SetActive(false);

                    m_tailSpriteRenderer.flipX = false;
                    m_spriteRenderer.flipX = false;
                    m_animator.SetBool("BIsWalk", true);
                    break;
                case BossMoveType.RightMove:
                    m_collosionChecker.transform.localPosition = new Vector3(1.0f, 0.0f, 0.0f);
                    m_boxCollider2D.offset = new Vector2(0.5f, -0.2f);

                    m_rigidbody2D.velocity = new Vector2(5.0f, 0.0f);

                    m_tailSpriteRenderer.gameObject.SetActive(false);

                    m_tailSpriteRenderer.flipX = true;
                    m_spriteRenderer.flipX = true;
                    m_animator.SetBool("BIsWalk", true);
                    break;
                case BossMoveType.Idle:
                    m_rigidbody2D.velocity = new Vector2(0.0f, 0.0f);

                    m_tailSpriteRenderer.gameObject.SetActive(true);

                    m_animator.SetBool("BIsWalk", false);
                    m_tailAnimator.Play("Boss_EndWalkEffect");
                    break;
            }
            m_nextBossMoves.Enqueue((BossMoveType)UnityEngine.Random.Range(0, 3));
        }
    }

    internal float Health
    {
        get { return m_health; }
        set { m_health = value; }
    }

    private void TurnOnLight()
    {
        m_light.enabled = true;
    }
    private void TuenOffLight()
    {
        m_light.enabled = false;
    }
    private void Dead()
    {
        Destroy(gameObject);
        m_light.enabled = false;
    }
}