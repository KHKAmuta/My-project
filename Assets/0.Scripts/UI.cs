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

        public int itemLevel
        {
            get { return itemlevel; }
            set
            {
                itemlevel = value;
                level.text = $"Lv.{itemlevel}";

            }
        }
        private int itemlevel;

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

            ui.icon.sprite = data.Icon;
            ui.title.text = data.Title;
            ui.desc.text = data.Desc;
        }
    }

    public void OnLevelUP(int index)
    {
        LevelUP ui = levelUpUIs[index];
        ui.itemLevel++;
    }

    public void OnItemSelect(int index)
    {
        ItemData data = levelUpItemData[index];
        LevelUP ui = levelUpUIs[index];

        switch (data.Type)
        {
            case ItemType.Bullet_Att:
                GameManager.instance.P.data.Power += 10;
                break;
            case ItemType.Bullet_Spd:
                GameManager.instance.P.data.FireDelay -= GameManager.instance.P.data.FireDelay * 0.1f;
                break;
            case ItemType.Bible:
                GameManager.instance.P.BibleAdd();
                break;
            case ItemType.Boots:
                GameManager.instance.P.data.Speed += GameManager.instance.P.data.Speed * 0.1f;
                break;
            case ItemType.Heal:
                GameManager.instance.P.data.HP = 50;
                break;
        }
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
