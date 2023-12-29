using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TimeManager
{
    public TimeAgent timeManager05;
}
public class GameManager : SingleTone<GameManager>
{
    #region Monster변수
    [SerializeField] private List<GameObject> monsterType = new List<GameObject>();
    public List<GameObject> MonsterType { get => monsterType; } //**
    private int index = 0;
    public Vector3 MonsterPos { get => MonsterPos; set => MonsterPos = value; }
    [SerializeField] private GameObject monsterAppearPos;
    #endregion

    #region Player변수
    public Vector3 PlayerPos { get => Player.Instance.transform.position; }
    #endregion

    #region Rotation변수
    private Vector3 distance;
    private Quaternion rotation;
    #endregion

    #region Timer변수
    public TimeManager Time;
    #endregion

    private void Start()
    {
        TimeSet();
        StartCoroutine(MonsterAppear());
    }

    private IEnumerator MonsterAppear()
    {
        WaitForSeconds wait = new WaitForSeconds(7);

        while (true)
        {
            yield return wait;
            MonsterType[0].TryGetComponent(out Monster monster);
            ObjectPool.Instance.OutObject(monster.layer, MonsterType[0], monsterAppearPos.transform.position, RotationCheck(monster.layer));
        }
    }

    public Quaternion RotationCheck(LayerMask layer) //layer비트연산
    {
        if (layer == LayerMask.GetMask("Monster"))
        {
            distance = PlayerPos - monsterAppearPos.transform.position;

        }
        //else
        //{
        //    distance = monster1.transform.position - PlayerPos;
        //}
        rotation = Quaternion.LookRotation(distance.normalized); //Quaternion.LookRotation작동방식 알아보기
        return rotation;
    }

    private void TimeSet()
    {
        Time.timeManager05 = new TimeAgent(0.5f, (TimeAgent) => Debug.Log("끝"), (TimeAgent) => Debug.Log("끝"));
    }
}
