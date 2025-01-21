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

    private bool m_bisHorizontalMove;
    private MovementDirection m_movementDirection = MovementDirection.Left;


    public void Update()
    {
        if(ChiefPlayerManager.Instance.GunManager.BIsReloading)
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
        }   
        if(Input.GetKey(KeyCode.D))
        {
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
        }

        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            ChiefPlayerManager.Instance.PlayerAnimator.SetBool("BIsWalk", false);
            ChiefPlayerManager.Instance.GunHlodArmAnimator.SetBool("BIsWalk", false);
            ChiefPlayerManager.Instance.FireHoldArmAnimator.SetBool("BIsWalk", false);
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

    public void OnMove(InputValue value)
    {

    }
}