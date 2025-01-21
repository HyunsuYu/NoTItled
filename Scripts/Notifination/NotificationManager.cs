using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class NotificationManager : MonoBehaviour
{
    private static NotificationManager m_instance;

    [SerializeField] private TMP_Text m_text_NotificationContent;

    [SerializeField] private Animator m_animator_Notification;

    private Queue<string> m_notificationQueue = new Queue<string>();
    private bool m_bisNotificationShowing = false;

    private float m_fNotificationShowTime = 5.0f;
    private float m_fNotificationShowTimer = 0.0f;

    private string m_curNotificationContent = string.Empty;


    public void Awake()
    {
        m_instance = this;

        //m_text_NotificationContent = GetComponent<TMP_Text>();
        //m_animator_Notification = GetComponent<Animator>();
    }
    public void FixedUpdate()
    {
        if(m_bisNotificationShowing)
        {
            m_fNotificationShowTimer += Time.deltaTime;
            if (m_fNotificationShowTimer >= m_fNotificationShowTime)
            {
                m_animator_Notification.Play("Notification_Hide");
                m_bisNotificationShowing = false;
                m_fNotificationShowTimer = 0.0f;
            }
        }

        if (m_notificationQueue.Count > 0 && !m_bisNotificationShowing)
        {
            ShowNotification(m_notificationQueue.Dequeue());
            m_bisNotificationShowing = true;
        }
    }

    internal static NotificationManager Instance
    {
        get
        {
            return m_instance;
        }
    }

    internal void EnqueueNotification(in string content)
    {
        if (content == "연료가 부족하다" && m_curNotificationContent == content)
        {
            return;
        }

        m_notificationQueue.Enqueue(content);
    }

    private void ShowNotification(in string content)
    {
        m_curNotificationContent = content;

        m_animator_Notification.gameObject.SetActive(true);

        m_text_NotificationContent.text = content;
        m_animator_Notification.Play("Notification_Show");

        Invoke("SetDisactive", 5.0f);
    }
    private void SetDisactive()
    {
        m_animator_Notification.gameObject.SetActive(false);
    }
}