using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class DoorManager : MonoBehaviour
{
    private Animator m_animator;


    public void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    internal void OpenDoor()
    {
        m_animator.Play("Door_Open");
    }
}