using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class ElevatorManager : MonoBehaviour
{
    [SerializeField] private Animator m_animator;

    [SerializeField] private bool m_bisCanRide = false;

    [SerializeField] private bool m_bisCanMoveTop;
    [SerializeField] private bool m_bisCanMoveBottom;

    [SerializeField] private int m_curFloor;
    [SerializeField] private List<Transform> m_elevatorPoses;

    [SerializeField] private bool m_bisCanUse = false;


    public void Update()
    {
        if (ReportManager.Instance.BIsReportActive)
        {
            return;
        }

        Debug.Log(ChiefNPCManager.Instance.CheckAllNPCStoped());
        if (m_bisCanUse && m_bisCanRide && m_bisCanMoveTop && Input.GetKeyDown(KeyCode.UpArrow) && ChiefNPCManager.Instance.CheckAllNPCStoped())
        {
            m_animator.Play("Elevator_Open");

            ChiefPlayerManager.Instance.PlayerMovement.SetPlayerMove(true);
            ChiefNPCManager.Instance.SetAllNPCWalk(true);
            Invoke("MoveTopPlayers", 0.75f);
            //MoveTopPlayers();
            Invoke("ActivePlayers", 1.5f);
            DisactivePlayers();
            Invoke("CloseElevator", 1.5f);
        }
        if (m_bisCanUse && m_bisCanRide && m_bisCanMoveBottom && Input.GetKeyDown(KeyCode.DownArrow) && ChiefNPCManager.Instance.CheckAllNPCStoped())
        {
            m_animator.Play("Elevator_Open");

            ChiefPlayerManager.Instance.PlayerMovement.SetPlayerMove(true);
            ChiefNPCManager.Instance.SetAllNPCWalk(true);
            Invoke("MoveBottomPlayers", 0.75f);
            //MoveBottomPlayers();
            Invoke("ActivePlayers", 1.5f);
            DisactivePlayers();
            Invoke("CloseElevator", 1.5f);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_bisCanRide = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_bisCanRide = false;
        }
    }

    internal bool BIsCanUse
    {
        get { return m_bisCanUse; }
        set { m_bisCanUse = value; }
    }

    private void DisactivePlayers()
    {
        ChiefPlayerManager.Instance.SetPlayerActive(false);
        ChiefNPCManager.Instance.SetAllNPCActive(false);
    }
    private void ActivePlayers()
    {
        ChiefPlayerManager.Instance.SetPlayerActive(true);
        ChiefNPCManager.Instance.SetAllNPCActive(true);
    }
    private void CloseElevator()
    {
        m_animator.Play("Elevator_Clese");
    }
    private void OpenElevator()
    {
        m_animator.Play("Elevator_Open");
    }
    private void OpenAndClose()
    {
        OpenElevator();

        Invoke("CloseElevator", 1.5f);
    }

    private void MoveTopPlayers()
    {
        if (m_curFloor == 1)
        {
            ChiefPlayerManager.Instance.SetPlayerPos(m_elevatorPoses[1].transform.position);
            ChiefNPCManager.Instance.SetAllNPCPoses(m_elevatorPoses[1].transform.position);

            m_elevatorPoses[1].GetComponent<ElevatorManager>().OpenAndClose();
        }
        else if (m_curFloor == 2)
        {
            ChiefPlayerManager.Instance.SetPlayerPos(m_elevatorPoses[2].transform.position);
            ChiefNPCManager.Instance.SetAllNPCPoses(m_elevatorPoses[2].transform.position);

            m_elevatorPoses[2].GetComponent<ElevatorManager>().OpenAndClose();
        }
    }
    private void MoveBottomPlayers()
    {
        if (m_curFloor == 2)
        {
            ChiefPlayerManager.Instance.SetPlayerPos(m_elevatorPoses[0].transform.position);
            ChiefNPCManager.Instance.SetAllNPCPoses(m_elevatorPoses[0].transform.position);

            m_elevatorPoses[0].GetComponent<ElevatorManager>().OpenAndClose();
        }
        else if (m_curFloor == 3)
        {
            ChiefPlayerManager.Instance.SetPlayerPos(m_elevatorPoses[1].transform.position);
            ChiefNPCManager.Instance.SetAllNPCPoses(m_elevatorPoses[1].transform.position);

            m_elevatorPoses[1].GetComponent<ElevatorManager>().OpenAndClose();
        }
    }
}