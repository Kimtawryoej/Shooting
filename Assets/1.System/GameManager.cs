using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : SingleTone<GameManager>
{
    #region Monster변수
    [SerializeField] private List<GameObject> monsterType = new List<GameObject>();
    public List<GameObject> MonsterType { get => monsterType; } //**
    private int index = 0;

    [SerializeField] private GameObject monsterAppearPos;
    public GameObject MonsterAppearPos { get { return monsterAppearPos; } }
    #endregion

    #region Player변수
    public Vector3 PlayerPos { get => Player.Instance.transform.position; }
    #endregion

    #region Rotation변수
    private Vector3 distance;
    private Quaternion rotation;
    #endregion

    #region 레벨
    public int Leavel { get; set; }
    #endregion

    #region commonRange
    Quaternion Range = new Quaternion(-13.68f, 13.68f, -7.83f, 64f);
    float x;
    float z;
    #endregion

    

    private void Start()
    {

        StartCoroutine(MonsterAppear());
        StartCoroutine(MonsterAppearPattern());
    }

    private void Update()
    {
    }

    #region 몬스터 생성
    private IEnumerator MonsterAppear()
    {
        WaitForSeconds wait0 = new WaitForSeconds(3);
        WaitForSeconds wait1 = new WaitForSeconds(4);

        yield return new WaitForSeconds(2);
        while (true)
        {

            switch (index)
            {
                case 0:
                    MonsterType[index].TryGetComponent(out Monster Movemonster);
                    Instantiate(MonsterType[index], monsterAppearPos.transform.position, RotationCheck(Movemonster.gameObject.layer)); yield return wait0;
                    break;
                case 1:
                    MonsterType[index].TryGetComponent(out Monster Stopmonster);
                    Instantiate(MonsterType[index], monsterAppearPos.transform.position, Quaternion.identity); yield return wait1; break;
            }
        }
    }

    private IEnumerator MonsterAppearPattern()
    {
        WaitForSeconds wait = new WaitForSeconds(10);
        while (true)
        {
            yield return wait;
            if (!index.Equals(2))
            {
                index++;
            }
            if (index.Equals(2))
            {
                index = 0;
            }
        }
    }
    #endregion
    #region 회전
    public Quaternion RotationCheck(LayerMask layer)
    {
        if (layer == 6)
        {
            Debug.Log("몬스터 회전");
            distance = PlayerPos - monsterAppearPos.transform.position;
        }
        //else
        //{
        //    distance = monster1.transform.position - PlayerPos;
        //}
        rotation = Quaternion.LookRotation(distance.normalized); //Quaternion.LookRotation작동방식 알아보기
        return rotation;
    }
    #endregion
    public bool RangeCheack(GameObject gameObject)
    {
        x = Mathf.Clamp(gameObject.transform.position.x, -13.68f, 13.68f);
        z = Mathf.Clamp(gameObject.transform.position.z, -7.83f, 64f);
        if (x.Equals(Range.x) || x.Equals(Range.y))
        {
            Debug.Log("x");
            return true;
        }
        else if (z.Equals(Range.z) || z.Equals(Range.w))
        {
            Debug.Log("z");

            return true;
        }
        return false;
    }

}
