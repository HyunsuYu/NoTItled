using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;


public class PlayerMovement : MonoBehaviour
{
    internal enum MovementDirection
    {
        Left,
        Right
    }


    [SerializeField] private Animator m_animator;
    [SerializeField] private Animator m_rushSmokeAnimator;

    [SerializeField] private AudioSource m_walk;

    //private bool m_bisHorizontalMove;
    private MovementDirection m_movementDirection = MovementDirection.Left;
    private bool m_bisMoved = false;


    public void Update()
    {
        if (ReportManager.Instance.BIsReportActive)
        {
            return;
        }

        if (ChiefPlayerManager.Instance.GunManager.BIsReloading)
        {
            ChiefPlayerManager.Instance.Rigidbody2D.velocity = Vector2.zero;
            return;
        }

        switch (m_movementDirection)
        {
            case MovementDirection.Left:
                ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsRight", false);
                ChiefPlayerManager.Instance.FireHoldArmAnimator.SetBool("BisRight", false);
                break;

            case MovementDirection.Right:
                ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsRight", true);
                ChiefPlayerManager.Instance.FireHoldArmAnimator.SetBool("BisRight", true);
                break;
        }

        Vector2 movementSpeed = Vector2.zero;

        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsRight", false);
        //}
        //if(Input.GetKeyDown(KeyCode.D))
        //{
        //    ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsRight", true);
        //}

        if (Input.GetKey(KeyCode.A))
        {
            if (!m_walk.isPlaying)
            {
                m_walk.loop = true;
                m_walk.mute = false;
                m_walk.Play();
            }

            m_bisMoved = true;

            movementSpeed.x = -ChiefPlayerManager.Instance.MovementConfig.MovementSpeed;

            m_movementDirection = MovementDirection.Left;

            ChiefPlayerManager.Instance.SpriteRenderer.flipX = true;
            ChiefPlayerManager.Instance.GunManager.GunHoldArm.GetComponent<SpriteRenderer>().flipX = true;
            ChiefPlayerManager.Instance.FlamethrowerManager.FireHlodArm.GetComponent<SpriteRenderer>().flipX = true;
            //ChiefPlayerManager.Instance.GunManager.GunHoldArm.transform.position = new Vector3()
            //{
            //    x = 0.158f,
            //    y = 0.218f,
            //    z = 0.0f
            //};

            ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsWalk", true);
            ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsWalk", true);
            ChiefPlayerManager.Instance.FireHoldArmAnimator.SetBool("BIsWalk", true);
            //ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsRight", false);

            m_rushSmokeAnimator.gameObject.SetActive(true);
            m_rushSmokeAnimator.SetBool("BIsWalk", true);
            m_rushSmokeAnimator.GetComponent<SpriteRenderer>().flipX = true;
        }   
        if(Input.GetKey(KeyCode.D))
        {
            if(!m_walk.isPlaying)
            {
                m_walk.loop = true;
                m_walk.mute = false;
                m_walk.Play();
            }

            m_bisMoved = true;

            movementSpeed.x = ChiefPlayerManager.Instance.MovementConfig.MovementSpeed;

            m_movementDirection = MovementDirection.Right;

            ChiefPlayerManager.Instance.SpriteRenderer.flipX = false;
            ChiefPlayerManager.Instance.GunManager.GunHoldArm.GetComponent<SpriteRenderer>().flipX = false;
            ChiefPlayerManager.Instance.FlamethrowerManager.FireHlodArm.GetComponent<SpriteRenderer>().flipX = false;
            //ChiefPlayerManager.Instance.GunManager.GunHoldArm.transform.position = new Vector3()
            //{
            //    x = -0.158f,
            //    y = 0.218f,
            //    z = 0.0f
            //};

            ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsWalk", true);
            ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsWalk", true);
            ChiefPlayerManager.Instance.FireHoldArmAnimator.SetBool("BIsWalk", true);
            //ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsRight", true);

            m_rushSmokeAnimator.gameObject.SetActive(true);
            m_rushSmokeAnimator.SetBool("BIsWalk", true);
            m_rushSmokeAnimator.GetComponent<SpriteRenderer>().flipX = false;
        }

        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            m_bisMoved = false;

            ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsWalk", false);
            ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsWalk", false);
            ChiefPlayerManager.Instance.FireHoldArmAnimator.SetBool("BIsWalk", false);

            m_rushSmokeAnimator.gameObject.SetActive(false);
            m_rushSmokeAnimator.SetBool("BIsWalk", false);

            m_walk.Stop();
        }

        ChiefPlayerManager.Instance.Rigidbody2D.velocity = new Vector2(movementSpeed.x, ChiefPlayerManager.Instance.Rigidbody2D.velocity.y);
    }

    internal MovementDirection MoveDirection
    {
        get
        {
            return m_movementDirection;
        }
    }
    internal bool BIsMoved
    {
        get
        {
            return m_bisMoved;
        }
    }

    internal void SetPlayerMove(in bool bisMove)
    {
        if(bisMove)
        {
            ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsWalk", true);
            ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsWalk", true);
            ChiefPlayerManager.Instance.FireHoldArmAnimator.SetBool("BIsWalk", true);

            m_rushSmokeAnimator.gameObject.SetActive(true);
            m_rushSmokeAnimator.SetBool("BIsWalk", true);
        }
        else
        {
            ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsWalk", false);
            ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsWalk", false);
            ChiefPlayerManager.Instance.FireHoldArmAnimator.SetBool("BIsWalk", false);

            m_rushSmokeAnimator.gameObject.SetActive(false);
            m_rushSmokeAnimator.SetBool("BIsWalk", false);
        }
    }

    public void OnMove(InputValue value)
    {

    }
}