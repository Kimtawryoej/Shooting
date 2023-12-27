using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTone<GameManager>
{
    [SerializeField] private List<GameObject> monsterType = new List<GameObject>();
    public List<GameObject> MonsterType { get => monsterType; } //**

    public Vector3 PlayerPos { get => Player.Instance.transform.position; }

    private int index = 0;
    public Vector3 MonsterPos { get => monster1.transform.position; }
    private GameObject monster1;

    private Vector3 distance;
    private Quaternion rotation;

    private void Start()
    {
        StartCoroutine(MonsterAppear());
    }

    private IEnumerator MonsterAppear()
    {
        WaitForSeconds wait = new WaitForSeconds(7);

        while (true)
        {
            yield return wait;
            MonsterType[0].TryGetComponent(out Monster monster);
            monster1 = ObjectPool.Instance.OutObject(monster.layer, MonsterType[0], monster.MonsterAppearPos.transform.position, Quaternion.identity);
        }
    }

    public Quaternion RotationCheck(LayerMask layer) //layer비트연산
    {
        if (layer == LayerMask.GetMask("Monster"))
        {
            distance = PlayerPos - MonsterPos;
        }
        else
        {
            distance = MonsterPos - PlayerPos;
        }
        rotation = Quaternion.LookRotation(distance.normalized);
        return rotation;
    }
}
