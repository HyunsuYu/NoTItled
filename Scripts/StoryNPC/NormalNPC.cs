using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class NormalNPC : MonoBehaviour
{
    private bool m_bisTouching = false;

    [SerializeField] private string m_speakerName = "연구원";

    private bool m_bisFirstInteraction = true;
    private bool m_bisSecondInteraction = true;

    private float m_bulletFileTime = 0.5f;
    [SerializeField] private float m_bulletLifeTimer = 0.0f;

    [SerializeField] private NPCMoveManager m_npcMoveManager;
    [SerializeField] private Animator m_animator;

    public static int PHD_HP = 6;


    public void Update()
    {
        if (m_speakerName == "소장" && PHD_HP <= 0)
        {
            m_animator.Play("PHD_Died");
            m_animator.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            m_npcMoveManager.m_bisPHD = true;

            Ending.Instance.BIsPHDWithMe = false;
        }

        if (m_bisTouching && Input.GetMouseButtonDown(1))
        {
            switch (m_speakerName)
            {
                case "연구원":
                    if(m_bisFirstInteraction)
                    {
                        ChiefInteractionManager.Instance.TargetSituationIndex = 1;
                        ChiefInteractionManager.Instance.StartInteraction();

                        m_bisFirstInteraction = false;
                    }

                    if(m_bisSecondInteraction &&
                        ChiefInteractionManager.Instance.CompletedSitIndexes.Contains(0) && 
                        ChiefInteractionManager.Instance.CompletedSitIndexes.Contains(1))
                    {
                        ChiefInteractionManager.Instance.TargetSituationIndex = 3;
                        ChiefInteractionManager.Instance.StartInteraction();

                        m_bisSecondInteraction = false;
                    }
                    break;

                case "겁먹은 연구원":
                    if(m_bisFirstInteraction)
                    {
                        ChiefInteractionManager.Instance.TargetSituationIndex = 0;
                        ChiefInteractionManager.Instance.StartInteraction();

                        m_bisFirstInteraction = false;
                    }

                    //if (m_bisSecondInteraction &&
                    //    ChiefInteractionManager.Instance.CompletedSitIndexes.Contains(0) &&
                    //    ChiefInteractionManager.Instance.CompletedSitIndexes.Contains(1))
                    //{
                    //    ChiefInteractionManager.Instance.TargetSituationIndex = 4;
                    //    ChiefInteractionManager.Instance.StartInteraction();

                    //    m_bisSecondInteraction = false;
                    //}
                    break;

                case "소장":
                    if (m_bisFirstInteraction)
                    {
                        ChiefInteractionManager.Instance.TargetSituationIndex = 2;
                        ChiefInteractionManager.Instance.StartInteraction();

                        //ChiefNPCManager.Instance.NPCMoveManagers.Add(GetComponent<NPCMoveManager>());
                        m_npcMoveManager.m_bisPHD = false;

                        Ending.Instance.BIsPHDWithMe = true;

                        m_bisFirstInteraction = false;
                    }

                    if (m_bisSecondInteraction &&
                        ChiefInteractionManager.Instance.CompletedSitIndexes.Contains(0) &&
                        ChiefInteractionManager.Instance.CompletedSitIndexes.Contains(1))
                    {
                        ChiefInteractionManager.Instance.TargetSituationIndex = 4;
                        ChiefInteractionManager.Instance.StartInteraction();

                        m_bisSecondInteraction = false;
                    }
                    break;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_bisTouching = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_bisTouching = false;
        }
    }
}