using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player : MonoBehaviour
{
    public class Data
    {
        public float HP { get; set; }
        public float MaxHP { get; set; }
        public int Level { get; set; }
        public float EXP { get; set; }
        public float Speed { get; set; }
        public float FireDelay { get; set; }
        public float Power { get; set; }

        public int itemLevel
        {
            get { return itemlevel; }
            set
            {
                itemlevel = value;
            }
        }
        private int itemlevel;

    }

    public enum State
    {
        Stand,
        Run,
        Dead
    }

    private State state = State.Stand;

    [SerializeField] private RectTransform hpRect;
    [SerializeField] private Transform firePos;
    [SerializeField] private Transform bibleTrans;
    [SerializeField] private Transform bible;
    [SerializeField] private Transform playerUI;
    [SerializeField] private Transform bParent;
    [SerializeField] private Bullet bullet;

    public Data data = new Data();

    private List<Sprite> stand;
    private List<Sprite> run;
    private List<Sprite> dead;
    private Transform target;

    private float fireTimer = 0f;

    

    void Start()
    {
        GameManager.instance.state = GameState.Play;

        int index = GameManager.instance.charSelectIndex;
        stand = GameManager.instance.charSprites[index].stand;
        run = GameManager.instance.charSprites[index].run;
        dead = GameManager.instance.charSprites[index].dead;

        GetComponent<SpriteAnimation>().SetSprite(stand, 0.2f);
        data.HP = 100;
        data.MaxHP = 100;
        data.Speed = 4f;
        data.FireDelay = 0.5f;
        data.Power = 10f;

        switch (index)
        {
            case 0:
                data.Speed += data.Speed * 0.1f;
                break;
            case 1:
                data.FireDelay -= 0.1f;
                break;
            case 2:
                data.Power += data.Power * 0.2f;
                break;
            case 3:
                break;
        }
    }

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.state != GameState.Play)
            return;

        Move();
        FindMonster();
        FindExp();

        if (target != null)
        {
            FireRotate();
            Fire();
        }

        playerUI.position = transform.position;
        bibleTrans.Rotate(Vector3.back * Time.deltaTime * 400f);
    }

    public void BibleAdd()
    {
        Instantiate(bible, bibleTrans);

        SetBible();
    }

    public void BibleDelete()
    {
        if (bibleTrans.childCount <= 0)
            return;

        Destroy(bibleTrans.GetChild(bibleTrans.childCount - 1).gameObject);

        SetBible();
    }

    void SetBible()
    {
        bibleTrans.rotation = Quaternion.identity;
        float rot = 360 / bibleTrans.childCount;
        float addRot = 0;

        for (int i = 0; i < bibleTrans.childCount; i++)
        {
            bibleTrans.GetChild(i).rotation = Quaternion.Euler(0f, 0f, addRot);
            addRot += rot;
        }
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * data.Speed;
        float y = Input.GetAxisRaw("Vertical") * Time.deltaTime * data.Speed;

        float cX = Mathf.Clamp(transform.position.x + x, -19, 19);
        float cY = Mathf.Clamp(transform.position.y + y, -19, 19);

        transform.position = new Vector2(cX, cY);

        // Scale
        if (x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (x > 0)
            transform.localScale = Vector3.one;

        // Animation
        if (state != State.Stand && x == 0 && y == 0)
        {
            state = State.Stand;
            GetComponent<SpriteAnimation>().SetSprite(stand, 0.2f);
        }
        else if (state != State.Run && (x != 0 || y != 0))
        {
            state = State.Run;
            GetComponent<SpriteAnimation>().SetSprite(run, 0.5f / data.Speed);
        }
    }

    private void FireRotate()
    {
        // 타켓을 찾아 방향 전환
        Vector2 vec = transform.position - target.position;
        float angle = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
        firePos.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
    }
    
    private void Fire()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= data.FireDelay)
        {
            fireTimer = 0f;
            Bullet b = Instantiate(bullet, firePos.GetChild(0));
            b.transform.SetParent(bParent);
            b.SetPower(data.Power);
        }
    }

    public void Hit(int dmg)
    {
        if (data.HP <= 0)
            return;

        data.HP -= dmg;

        float sizeX = 120f * (data.HP / data.MaxHP);
        hpRect.sizeDelta = new Vector2(sizeX, 30f);

        if (data.HP <= 0)
        {
            GameManager.instance.UI.DeadTitleStart();
        }
    }

    public void FindMonster()
    {
        target = null;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Monster");

        if (objs.Length > 0)
        {
            float distance = float.MaxValue;
            int findIndex = -1;

            for (int i = 0; i < objs.Length; i++)
            {
                float dis = Vector2.Distance(objs[i].transform.position, transform.position);

                if (dis <= 6)
                {
                    if (dis <= distance)
                    {
                        distance = dis;
                        findIndex = i;
                    }
                }
            }

            if (findIndex != -1)
            {
                target = objs[findIndex].transform;
            }
        }
    }

    public void FindExp()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Exp");

        if (objs.Length > 0)
        {
            foreach (var item in objs.Select((value, index) => (value, index)))
            {
                float distance = Vector2.Distance(transform.position, item.value.transform.position);

                if (distance <= 3f)
                {
                    item.value.GetComponent<Exp>().Target = transform;
                }
            }
        }
    }
}
