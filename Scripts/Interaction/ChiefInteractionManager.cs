using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class ChiefInteractionManager : MonoBehaviour
{
    internal struct SpeakData
    {
        public int SituationIndex;
        public string SpeakerName;
        public string SpeakContent;
        public int FontSize;
    }
    //internal struct BranckData
    //{
    //    public int SituationIndex;
    //    public List<int> PreRequsitSitIndexes;
    //}
    [Serializable] public struct BranchData
    {
        public List<int> relatedSitNums;
    }


    private static ChiefInteractionManager m_instance;

    [SerializeField] private TextAsset m_speakDataTextAsset;
    [SerializeField] private TextAsset m_branckDataTextAsset;

    [SerializeField] private Transform m_transform_InteractionBubbleParent;

    [SerializeField] private GameObject m_prefab_MainInteractionBubble;
    [SerializeField] private GameObject m_prefab_SubInteractionBubble;

    private IObjectPool<GameObject> m_mainInteractionBubblePool;
    private IObjectPool<GameObject> m_subInteractionBubblePool;

    [SerializeField] private int m_targetSituationIndex = 0;

    private int m_curSituationOffset = 0;
    private int m_curSituationSpeakIndex = 0;

    private bool m_bisStartSpeak = false;
    private bool m_bisCurSpeakCompleted = false;

    private List<SpeakData> m_speakDatas = new List<SpeakData>();
    //private List<BranckData> m_branckDatas = new List<BranckData>();
    [SerializeField] private List<BranchData> m_branchDatas;
    private List<int> m_completedSituationIndexes = new List<int>();

    //[SerializeField] private NPC2NameTable m_nPC2NameTable;

    private bool m_bisCanKeepSpeak = false;

    private List<GameObject> m_curSpawnedInteractionBubbles = new List<GameObject>();

    private List<int> m_completedSitIndexes = new List<int>();


    public void Awake()
    {
        m_instance = this;

        // SpeakData
        string[] lines = m_speakDataTextAsset.text.Split('\n');
        for(int index = 1; index < lines.Length; index++)
        {
            string[] datas = lines[index].Split(',');
            //for(int innerIndex = 0; innerIndex < datas.Length; innerIndex++)
            //{
            //    Debug.Log(index.ToString() + " - " + innerIndex.ToString() + " - " + datas[innerIndex]);
            //}

            int fontSize = 0;
            if (!int.TryParse(datas[2], out fontSize))
            {
                fontSize = 25;
            }
            SpeakData speakData = new SpeakData()
            {
                SituationIndex = int.Parse(datas[0].Replace(" ", "")),
                SpeakerName = datas[1],
                SpeakContent = datas[3],
                FontSize = fontSize
            };
            m_speakDatas.Add(speakData);
        }

        // BranckData

    }
    public void Update()
    {
        if(m_bisStartSpeak)
        {
            if(m_bisCurSpeakCompleted && Input.GetMouseButtonDown(1))
            {
                m_bisCanKeepSpeak = true;
            }

            if(!m_bisCurSpeakCompleted)
            {
                if((m_curSituationOffset + m_curSituationSpeakIndex + 1 < m_speakDatas.Count &&
                    m_speakDatas[m_curSituationOffset + m_curSituationSpeakIndex + 1].SituationIndex != m_targetSituationIndex) ||
                    m_curSituationOffset + m_curSituationSpeakIndex + 1 >= m_speakDatas.Count)
                {
                    foreach (GameObject curSpawnedItem in m_curSpawnedInteractionBubbles)
                    {
                        switch (curSpawnedItem.GetComponent<SingleInteractionBubble>().BubbleKind)
                        {
                            case SingleInteractionBubble.BubbleType.Main:
                                MainInteractionBubblePool.Release(curSpawnedItem);
                                break;

                            case SingleInteractionBubble.BubbleType.Sub:
                                SubInteractionBubblePool.Release(curSpawnedItem);
                                break;
                        }
                    }
                    m_curSpawnedInteractionBubbles.Clear();

                    m_bisStartSpeak = false;
                    m_completedSitIndexes.Add(m_targetSituationIndex);
                    return;
                }

                SpawnInteractionBubble(m_speakDatas[m_curSituationOffset + m_curSituationSpeakIndex].SpeakerName,
                                        m_speakDatas[m_curSituationOffset + m_curSituationSpeakIndex].SpeakContent,
                                        m_speakDatas[m_curSituationOffset + m_curSituationSpeakIndex].FontSize);

                m_curSituationSpeakIndex++;
                m_bisCurSpeakCompleted = true;
            }

            if(m_bisCanKeepSpeak && Input.GetMouseButtonDown(1))
            {
                m_bisCurSpeakCompleted = false;
            }
        }

        if(m_curSpawnedInteractionBubbles.Count > 3)
        {
            switch(m_curSpawnedInteractionBubbles[0].GetComponent<SingleInteractionBubble>().BubbleKind)
            {
                case SingleInteractionBubble.BubbleType.Main:
                    MainInteractionBubblePool.Release(m_curSpawnedInteractionBubbles[0]);
                    m_curSpawnedInteractionBubbles.RemoveAt(0);
                    break;

                case SingleInteractionBubble.BubbleType.Sub:
                    SubInteractionBubblePool.Release(m_curSpawnedInteractionBubbles[0]);
                    m_curSpawnedInteractionBubbles.RemoveAt(0);
                    break;
            }
        }
    }

    internal static ChiefInteractionManager Instance
    {
        get
        {
            return m_instance;
        }
    }

    internal List<SpeakData> speakDatas
    {
        get
        {
            return m_speakDatas;
        }
    }
    internal int TargetSituationIndex
    {
        get
        {
            return m_targetSituationIndex;
        }
        set
        {
            m_targetSituationIndex = value;
        }
    }
    internal List<int> CompletedSitIndexes
    {
        get
        {
            return m_completedSitIndexes;
        }
    }
    internal List<BranchData> BranchDatas
    {
        get
        {
            return m_branchDatas;
        }
    }

    #region Main Interaction Bubble Pool
    internal IObjectPool<GameObject> MainInteractionBubblePool
    {
        get
        {
            if (m_mainInteractionBubblePool == null)
            {
                m_mainInteractionBubblePool = new ObjectPool<GameObject>(CreateMainPooledItem, OnTakeFromMainPool, OnReturnedToMainPool, OnDestroyMainPoolObject, true, 10, 10);
            }

            return m_mainInteractionBubblePool;
        }
    }

    private GameObject CreateMainPooledItem()
    {
        return Instantiate(m_prefab_MainInteractionBubble, m_transform_InteractionBubbleParent);
    }
    private void OnTakeFromMainPool(GameObject item)
    {
        item.SetActive(true);
    }
    private void OnReturnedToMainPool(GameObject item)
    {
        item.SetActive(false);
    }
    private void OnDestroyMainPoolObject(GameObject item)
    {
        item.SetActive(false);
    }
    #endregion

    #region Sub Interaction Bubble Pool
    internal IObjectPool<GameObject> SubInteractionBubblePool
    {
        get
        {
            if (m_subInteractionBubblePool == null)
            {
                m_subInteractionBubblePool = new ObjectPool<GameObject>(CreateSubPooledItem, OnTakeFromSubPool, OnReturnedToSubPool, OnDestroySubPoolObject, true, 10, 10);
            }
            return m_subInteractionBubblePool;
        }
    }

    private GameObject CreateSubPooledItem()
    {
        return Instantiate(m_prefab_SubInteractionBubble, m_transform_InteractionBubbleParent);
    }
    private void OnTakeFromSubPool(GameObject item)
    {
        item.SetActive(true);
    }
    private void OnReturnedToSubPool(GameObject item)
    {
        item.SetActive(false);
    }
    private void OnDestroySubPoolObject(GameObject item)
    {
        item.SetActive(false);
    }
    #endregion

    public void StartInteraction()
    {
        for (int index = 0; index < m_speakDatas.Count; index++)
        {
            if (m_speakDatas[index].SituationIndex == m_targetSituationIndex)
            {
                m_curSituationOffset = index;
                m_curSituationSpeakIndex = 0;
                break;
            }
        }

        m_bisStartSpeak = true;
    }

    internal int FindSituationStartPos(in int targetSituationIndex)
    {
        for (int index = 0; index < m_speakDatas.Count; index++)
        {
            if (m_speakDatas[index].SituationIndex == targetSituationIndex)
            {
                return index;
            }
        }
        return -1;
    }

    private void SpawnInteractionBubble(in string name, in string content, in int fontSize)
    {
        if(name == string.Empty)
        {
            GameObject mainInteractionBubble = MainInteractionBubblePool.Get();
            mainInteractionBubble.GetComponent<SingleInteractionBubble>().SetData(name, content, fontSize);
            mainInteractionBubble.transform.SetAsFirstSibling();

            m_curSpawnedInteractionBubbles.Add(mainInteractionBubble);
        }
        else
        {
            GameObject subInteractionBubble = SubInteractionBubblePool.Get();
            subInteractionBubble.GetComponent<SingleInteractionBubble>().SetData(name, content, fontSize);
            subInteractionBubble.transform.SetAsFirstSibling();

            m_curSpawnedInteractionBubbles.Add(subInteractionBubble);
        }
    }
}