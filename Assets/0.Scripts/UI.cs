using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class UI : MonoBehaviour
{
    [System.Serializable]
    public class Top
    {
        public Image expImg;
        public Text killTxt;
        public Text timeTxt;
        public Text lvText;

        public int KillCount
        {
            get { return killCount; }
            set
            {
                killCount = value;
                killTxt.text = killCount.ToString();
            }
        }
        private int killCount;

        public int Level
        {
            get { return level; }
            set
            {
                level = value;
                lvText.text = $"Lv.{level}";
            }
        }
        private int level;

        public float Exp
        {
            get { return exp; }
            set
            {
                exp = value;

                if (exp >= maxExp)
                {
                    exp = 0;
                    maxExp += 5;
                    Level++;
                    GameManager.instance.UI.OnShowLevelUPPopup(true);
                }

                expImg.rectTransform.sizeDelta = new Vector2((exp / maxExp * 1270) + 10, 32);
            }
        }
        private float exp;
        [HideInInspector]
        public float maxExp;
    }
    public Top topUI;

    [System.Serializable]
    public class LevelUP
    {
        public Image icon;
        public Text level;
        public Text title;
        public Text desc;

        public ItemType ItemType { get; private set; } // 아이템 타입 추가

        public void Initialize(ItemType type, int lv)
        {
            ItemType = type;
            itemLevel = lv;
            level.text = $"Lv.{itemLevel}";
        }

        public void UpdateLevel(int lv)
        {
            itemLevel = lv;
            level.text = $"Lv.{itemLevel}";
        }

        private int itemLevel;
    }
    [SerializeField] private List<LevelUP> levelUpUIs;
    [SerializeField] private GameObject levelUpPopup;

    [System.Serializable]
    public class Result
    {
        public GameObject obj;
        public GameObject backObj;
        public Image title;

        [HideInInspector]
        public float[] deadTitleVaule = new float[5] { 0.214f, 0.406f, 0.593f, 0.785f, 1f };
    }
    [SerializeField] private Result resultUI;

    // 아이템 데이터
    [SerializeField] private ItemData[] itemDatas;

    private List<ItemData> levelUpItemData = new List<ItemData>();

    [SerializeField] private Text[] itemLevel;

    void Start()
    {
        topUI.maxExp = 10;
        topUI.Exp = 0;
        topUI.Level = 1;
        topUI.KillCount = 0;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            //DeadTitleStart();
        }
    }

    public void OnShowLevelUPPopup(bool isShow)
    {
        GameManager.instance.state = GameState.Stop;
        levelUpPopup.SetActive(isShow);

        if (isShow == true)
        {
            ItemSuffle();

            Transform bg = levelUpPopup.transform.GetChild(0).GetChild(0);
            bg.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            bg.DOScale(Vector3.one, 0.2f)
                .SetEase(Ease.OutBounce);
        }
        else
        {
            GameManager.instance.state = GameState.Play;
        }
    }

    public void ItemSuffle()
    {
        levelUpItemData.Clear();
        List<ItemData> itemData = itemDatas.ToList();

        for (int i = 0; i < levelUpUIs.Count; i++)
        {
            int rand = Random.Range(0, itemData.Count);
            levelUpItemData.Add(itemData[rand]);
            itemData.RemoveAt(rand);
        }

        for (int i = 0; i < levelUpUIs.Count; i++)
        {
            LevelUP ui = levelUpUIs[i];
            ItemData data = levelUpItemData[i];

            ui.Initialize(data.Type, GameManager.instance.GetItem(data.Type).Level); // 아이템 레벨 초기화
            ui.icon.sprite = data.Icon;
            ui.title.text = data.Title;
            ui.desc.text = data.Desc;
        }
    }

    public void OnLevelUP(int index)
    {
        LevelUP ui = levelUpUIs[index];
        ui.UpdateLevel(GameManager.instance.GetItem(ui.ItemType).Level); // UI에서 아이템 레벨 업데이트
    }

    public void OnItemSelect(int index)
    {
        ItemData data = levelUpItemData[index];
        ItemType type = data.Type;

        GameManager.instance.LevelUpItem(type); // 해당 아이템의 레벨 증가
    }

    public void DeadTitleStart()
    {
        GameManager.instance.state = GameState.Stop;
        resultUI.obj.SetActive(true);
        resultUI.backObj.SetActive(false);
        resultUI.title.fillAmount = 0f;

        StopCoroutine("CDeadTitle");
        StartCoroutine("CDeadTitle");
    }

    IEnumerator CDeadTitle()
    {
        yield return new WaitForSeconds(0.2f);

        foreach (var item in resultUI.deadTitleVaule)
        {
            resultUI.title.fillAmount = item;

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.2f);
        resultUI.backObj.SetActive(true);

    }
}