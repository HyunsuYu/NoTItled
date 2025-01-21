using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;


public class Lighting : MonoBehaviour
{
    private Light2D m_light;

    private float m_maintenanceTime = 3.0f;
    private float m_maintenanceTimer = 0.0f;

    private float m_turnOffTimer = 0.0f;

    private Animator m_animator;

    [SerializeField] private bool m_bisBlikingEnd = false;


    public void Awake()
    {
        m_light = GetComponent<Light2D>();

        m_animator = GetComponent<Animator>();
    }
    public void FixedUpdate()
    {
        if (m_maintenanceTimer > 0.0f)
        {
            m_maintenanceTimer -= Time.deltaTime;
        }
        else
        {
            //m_animator.Play("BlinkingLight");
            m_animator.SetTrigger("Blinking");

            //m_maintenanceTime = UnityEngine.Random.Range(1.0f, 3.0f);
            m_maintenanceTimer = UnityEngine.Random.Range(3.0f, 5.0f);
        }
    }
}