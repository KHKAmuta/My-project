using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public enum GameState
{
    Play,
    Stop
}

public enum ItemType
{
    Bullet_Att,
    Bullet_Spd,
    Bible,
    Heal,
    Boots
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Dictionary<ItemType, Item> items = new Dictionary<ItemType, Item>();

    private void InitializeItems()
    {
        // 각 아이템을 초기화하고 딕셔너리에 추가
        items.Add(ItemType.Bullet_Att, new Item(ItemType.Bullet_Att));
        items.Add(ItemType.Bullet_Spd, new Item(ItemType.Bullet_Spd));
        items.Add(ItemType.Bible, new Item(ItemType.Bible));
        items.Add(ItemType.Heal, new Item(ItemType.Heal));
        items.Add(ItemType.Boots, new Item(ItemType.Boots));
        // 나머지 아이템들도 추가
    }

    public Item GetItem(ItemType type)
    {
        // 아이템 딕셔너리에서 해당 타입의 아이템 반환
        if (items.ContainsKey(type))
        {
            return items[type];
        }
        else
        {
            Debug.LogError("Item not found: " + type);
            return null;
        }
    }

    public void LevelUpItem(ItemType type)
    {
        // 해당 아이템의 레벨 증가
        Item item = GetItem(type);
        if (item != null)
        {
            item.LevelUp();
            Debug.Log(type + " leveled up to " + item.Level); // 레벨 증가 확인을 위한 로그
        }
    }

    [System.Serializable]
    public class CharSprite
    {
        public List<Sprite> stand;
        public List<Sprite> run;
        public List<Sprite> dead;
    }
    public List<CharSprite> charSprites;

    public GameState state = GameState.Stop;

    public int charSelectIndex = 0;

    public UI UI
    {
        get
        {
            if (ui == null)
            {
                ui = GameObject.FindObjectOfType<UI>();
            }

            return ui;
        }
    }
    private UI ui;

    public Player P
    {
        get
        {
            if (p == null)
            {
                p = GameObject.FindObjectOfType<Player>();
            }

            return p;
        }
    }
    private Player p;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            InitializeItems();
            DontDestroyOnLoad(gameObject);
        }
    }

    public void OnSelect()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public void OnGame()
    {
        SceneManager.LoadScene("Game");
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    public void OnTitle()
    {
        SceneManager.LoadScene("Title");
    }
}

