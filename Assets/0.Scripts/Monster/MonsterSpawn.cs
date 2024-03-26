using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] private Transform mParent;
    [SerializeField] private Transform eParent;
    [SerializeField] private Monster m;
    [SerializeField] private Player p;
    [SerializeField] private Exp[] exps;

    private float spawnTimer = 0f;
    private float spawnDelay;

    void Start()
    {
        spawnDelay = Random.Range(1, 5);
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnDelay)
        {
            spawnTimer = 0f;
            spawnDelay = Random.Range(5, 15);

            Spawn();
        }
    }

    public void Spawn()
    {
        if (GameManager.instance != null && GameManager.instance.state != GameState.Play)
            return;

        Monster mon = Instantiate(m, Return_RandomPosition(), Quaternion.identity, mParent);
        mon.SetTarget(p.transform);
        mon.SetExp(exps);
        mon.SetExpParent(eParent);
    }

    // 랜덤스폰
    public GameObject rangeObject;
    Collider2D rangeCollider;

    private void Awake()
    {
        rangeCollider = rangeObject.GetComponent<Collider2D>();
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider.bounds.size.x;
        float range_Y = rangeCollider.bounds.size.y;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Y = Random.Range((range_Y / 2) * -1, range_Y / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, 0f);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
}
