using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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