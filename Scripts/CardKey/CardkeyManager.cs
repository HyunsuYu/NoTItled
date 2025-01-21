using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CardkeyManager : MonoBehaviour
{
    internal enum CardKeyType : byte
    {
        Red, Yellow, Blue
    }


    [SerializeField] private CardKeyType m_cardKeyType;
    [SerializeField] private List<Sprite> m_sprites;

    [SerializeField] private List<DoorManager> m_doorManagers;
    [SerializeField] private List<ElevatorManager> m_elevatorManagers;

    private bool m_bisItemInteractable = false;

    [SerializeField] private TMP_Text m_text_ElevatorHine;


    public void Awake()
    {
        switch(m_cardKeyType)
        {
            case CardKeyType.Red:
                GetComponent<SpriteRenderer>().sprite = m_sprites[0];
                break;
            case CardKeyType.Yellow:
                GetComponent<SpriteRenderer>().sprite = m_sprites[2];
                break;
            case CardKeyType.Blue:
                GetComponent<SpriteRenderer>().sprite = m_sprites[1];
                break;
        }
    }
    public void Update()
    {
        if (m_bisItemInteractable && Input.GetMouseButton(1))
        {
            gameObject.SetActive(false);

            switch(m_cardKeyType)
            {
                case CardKeyType.Yellow:
                    foreach (var elevator in m_elevatorManagers)
                    {
                        elevator.BIsCanUse = true;
                    }
                    NotificationManager.Instance.EnqueueNotification("이 키로 엘레베이터를 이용할 수 있다");

                    m_text_ElevatorHine.enabled = true;

                    Invoke("HideElevatorHine", 3.0f);
                    break;

                case CardKeyType.Blue:
                    foreach(var door in m_doorManagers)
                    {
                        door.OpenDoor();
                    }
                    NotificationManager.Instance.EnqueueNotification("어딘가에서 열리는 소리가 났다");
                    break;
            }
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

    private void HideElevatorHine()
    {
        m_text_ElevatorHine.enabled = false;
    }
}