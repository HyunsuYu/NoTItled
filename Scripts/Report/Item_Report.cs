using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Item_Report : MonoBehaviour
{
    private bool m_bisItemInteractable = false;

    [SerializeField] private string m_text;


    //public void Awake()
    //{
    //    m_text_Content.text = m_text;
    //}

    public void Update()
    {
        if (m_bisItemInteractable && Input.GetMouseButton(1))
        {
            gameObject.SetActive(false);

            //m_UI_Blur.SetActive(true);
            //m_UI_Blur.GetComponent<Animator>().Play("GameOverBlur");

            //m_scrollView_Report.SetActive(true);
            //m_button_CloseReport.SetActive(true);

            ReportManager.Instance.CurReport = this;
            //ReportManager.Instance.SetContent(m_text);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_bisItemInteractable = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            m_bisItemInteractable = false;
        }
    }
}