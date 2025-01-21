using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using TMPro;


public class ReportManager : MonoBehaviour
{
    private static ReportManager m_instance;

    private Item_Report m_curReport;

    [SerializeField] private GameObject m_UI_Blur;
    [SerializeField] private TMP_Text m_text_Content;

    [SerializeField] private GameObject m_scrollView_Report;
    [SerializeField] private GameObject m_button_CloseReport;

    [SerializeField] private Animator m_animator_ScrollView;
    [SerializeField] private Animator m_animator_Test;
    [SerializeField] private Animator m_animator_Button;
    [SerializeField] private Animator m_animator_VerticalBar;
    [SerializeField] private Animator m_animator_Handle;

    private bool m_bisReportActive = false;


    public void Awake()
    {
        m_instance = this;
    }

    internal static ReportManager Instance
    {
        get
        {
            return m_instance;
        }
    }

    internal Item_Report CurReport
    {
        get
        {
            return m_curReport;
        }
        set
        {
            if(value != null)
            {
                m_curReport = value;

                SetUIActive(true);
            }
        }
    }
    internal bool BIsReportActive
    {
        get
        {
            return m_bisReportActive;
        }
    }

    public void CloseReport()
    {
        SetUIActive(false);
    }

    internal void SetContent(in string content)
    {
        m_text_Content.text = content;
    }

    private void SetUIActive(in bool bisActive)
    {
        m_bisReportActive = bisActive;

        if (bisActive)
        {
            m_scrollView_Report.SetActive(true);
            m_button_CloseReport.SetActive(true);

            m_UI_Blur.SetActive(true);
            m_UI_Blur.GetComponent<Animator>().Play("GameOverBlur");

            m_animator_ScrollView.Play("Report_Action");
            m_animator_Test.Play("Report_Text");
            m_animator_Button.Play("Report_Button");
            m_animator_VerticalBar.Play("Report_VerticalBar");
            m_animator_Handle.Play("Report_Handle");

        }
        else
        {
            m_UI_Blur.SetActive(false);

            m_scrollView_Report.SetActive(false);
            m_button_CloseReport.SetActive(false);

            m_curReport.gameObject.SetActive(false);
        }
    }
}