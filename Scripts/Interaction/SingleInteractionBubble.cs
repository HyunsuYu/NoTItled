using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SingleInteractionBubble : MonoBehaviour
{
    public enum BubbleType
    {
        Main,
        Sub
    }


    [SerializeField] private TMP_Text m_text_Name;
    [SerializeField] private TMP_Text m_text_Content;

    [SerializeField] private BubbleType m_bubbleType;


    internal BubbleType BubbleKind
    {
        get
        {
            return m_bubbleType;
        }
    }

    internal void SetData(string name, string content, in int fontSize)
    {
        m_text_Name.text = (name == "") ? "플레이어" : name;
        m_text_Content.text = content;

        m_text_Content.fontSize = fontSize;
    }
}