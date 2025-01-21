using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;


public class ChiefNPCManager : MonoBehaviour
{
    private static ChiefNPCManager m_instance;

    [SerializeField] private List<NPCMoveManager> m_NPCMoveManagers;

    [SerializeField] private GameObject m_UI_GameOverText;
    [SerializeField] private GameObject m_UI_GameOverLine;
    [SerializeField] private GameObject m_UI_GameOverBlur;
    [SerializeField] private GameObject m_UI_GoToTitle;
    [SerializeField] private Animator m_UI_GoToTitle_Text_Animator;


    public void Awake()
    {
        Reorder();

        m_instance = this;
    }
    public void Start()
    {
        LifeManager.Instance.LifeisZeroCallback += SwapPC;
    }

    internal static ChiefNPCManager Instance
    {
        get
        {
            return m_instance;
        }
    }

    internal List<NPCMoveManager> NPCMoveManagers
    {
        get
        {
            return m_NPCMoveManagers;
        }
    }

    public void GoToLobby()
    {
        SceneManager.LoadScene(0);
    }

    internal void SwapPC()
    {
        Debug.Log("SwapPC");

        // Game Over
        if(m_NPCMoveManagers.Count == 0)
        {
            //ChiefPlayerManager.Instance.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            ChiefPlayerManager.Instance.PlayerAnimator.Play("Player_Died");

            m_UI_GameOverText.SetActive(true);
            m_UI_GameOverLine.SetActive(true);
            m_UI_GameOverBlur.SetActive(true);
            m_UI_GoToTitle.SetActive(true);

            m_UI_GameOverText.GetComponent<Animator>().Play("GameOver");
            m_UI_GameOverLine.GetComponent<Animator>().Play("GameOverLine");
            m_UI_GameOverBlur.GetComponent<Animator>().Play("GameOverBlur");
            m_UI_GoToTitle.GetComponent<Animator>().Play("GameOver_GoToLobby");
            m_UI_GoToTitle_Text_Animator.Play("GameOver_GoToLobby_Text");

            return;
        }

        Vector3 prevPlayerPos = ChiefPlayerManager.Instance.transform.position;

        ChiefPlayerManager.Instance.transform.position = m_NPCMoveManagers[0].transform.position;
        m_NPCMoveManagers[0].transform.position = prevPlayerPos;

        LifeManager.Instance.CurLife = 6;

        if(m_NPCMoveManagers.Count > 0)
        {
            m_NPCMoveManagers[0].BIsNPCDead = true;
            m_NPCMoveManagers.RemoveAt(0);

            Reorder();
        }
    }
    internal bool CheckAllNPCStoped()
    {
        for (int index = 0; index < m_NPCMoveManagers.Count; index++)
        {
            if (m_NPCMoveManagers[index].BIsNPCMoveing)
            {
                return false;
            }
        }
        return true;
    }
    internal void SetAllNPCWalk(in bool bisWalk)
    {
        foreach(NPCMoveManager npcMoveManager in m_NPCMoveManagers)
        {
            npcMoveManager.SetWalk(bisWalk);
        }
    }
    internal void SetPlayerActive(in bool bisActive)
    {
        ChiefPlayerManager.Instance.gameObject.SetActive(bisActive);
    }
    internal void SetAllNPCActive(in bool bisActive)
    {
        foreach (NPCMoveManager npcMoveManager in m_NPCMoveManagers)
        {
            npcMoveManager.SetNPCActive(bisActive);
        }
    }
    internal void SetAllNPCPoses(in Vector2 pos)
    {
        foreach (NPCMoveManager npcMoveManager in m_NPCMoveManagers)
        {
            npcMoveManager.transform.position = pos;
        }
        //for(int index = 0; index < m_NPCMoveManagers.Count - 1; index++)
        //{
        //    m_NPCMoveManagers[index].transform.position = pos;
        //}
    }

    private void Reorder()
    {
        for (int index = 0; index < m_NPCMoveManagers.Count; index++)
        {
            m_NPCMoveManagers[index].Reorder(index);
        }
    }
}