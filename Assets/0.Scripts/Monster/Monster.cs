using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public enum State
    {
        Run,
        Hit,
        Dead
    }

    [System.Serializable]
    public class Data
    {
        public int Level { get; set; }
        public int Power { get; set; }
        public int Defence { get; set; }
        public float HP { get; set; }
        public float Speed { get; set; }
        public float HitDelay { get; set; }
        public float AttDelay { get; set; }
    }

    [SerializeField] protected List<Sprite> run;
    [SerializeField] protected List<Sprite> hit;
    [SerializeField] protected List<Sprite> dead;

    protected Data data = new Data();


    private Exp[] exps;
    private Transform eParent;
    private SpriteRenderer sr;
    private SpriteAnimation sa;
    private Transform target;
    private State state = State.Run;

    private float attTimer = float.MaxValue;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetExp(Exp[] exps)
    {
        this.exps = exps;
    }

    public void SetExpParent(Transform eParent)
    {
        this.eParent = eParent;
    }

    public virtual void Init()
    {
        sr = GetComponent<SpriteRenderer>();
        sa = GetComponent<SpriteAnimation>();

        sa.SetSprite(run, 0.5f / data.Speed);
    }

    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.state != GameState.Play)
            return;

        if (target == null || data.HP <= 0)
            return;

        if (data.HitDelay >= 0)
        {
            data.HitDelay -= Time.deltaTime;
            return;
        }
        else if (state == State.Hit)
        {
            state = State.Run;
            sa.SetSprite(run, 0.5f / data.Speed);
        }

        Direction();
    }

    void Direction()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance > 0.8f)
        {
            Vector2 dis = target.position - transform.position;
            Vector2 dir = dis.normalized * Time.deltaTime * data.Speed;
            transform.Translate(dir);

            if (dir.normalized.x != 0)
            {
                sr.flipX = dir.normalized.x > 0 ? false : true;
            }
        }
        else
        {
            attTimer += Time.deltaTime;

            if (attTimer >= data.AttDelay)
            {
                attTimer = 0;
                target.GetComponent<Player>().Hit(data.Power);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (data.HP <= 0)
            return;

        if (collision.CompareTag("pBullet"))
        {
            Bullet b = collision.GetComponent<Bullet>();
            data.HP -= b.Power;

            state = State.Hit;

            data.HitDelay = 0.5f;
            sa.SetSprite(hit, 0.1f);

            if (data.HP <= 0)
            {
                GetComponent<Collider2D>().enabled = false;
                tag = "Untagged";
                sa.SetSprite(dead, 0.1f, 1f, End);
            }

            Destroy(collision.gameObject);
        }
    }

    void End()
    {
        int rand = Random.Range(0, 100);
        if (rand <= 90)
        {
            int expIndex = data.Level <= 2 ? 0 : data.Level <= 5 ? Random.Range(0, 2) : Random.Range(1, 3);
            Instantiate(exps[expIndex], transform.position, Quaternion.identity, eParent);
        }
        else
        {
            // »óÀÚ
        }
        
        Destroy(gameObject);
    }
}
