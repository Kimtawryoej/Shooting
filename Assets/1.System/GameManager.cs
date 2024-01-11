using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


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

    private void Start()
    {
        StartCoroutine(MonsterAppear());
    }

    #region 몬스터 생성
    private IEnumerator MonsterAppear()
    {
        WaitForSeconds wait = new WaitForSeconds(2);

        while (true)
        {
            yield return wait;
            switch (index)
            {
                case 0:
                    MonsterType[index].TryGetComponent(out Monster Movemonster);
                    ObjectPool.Instance.OutObject(Movemonster.ObjectLayer, MonsterType[0], monsterAppearPos.transform.position, RotationCheck(Movemonster.gameObject.layer)); break;
                case 1:
                    MonsterType[index].TryGetComponent(out Monster Stopmonster);
                    ObjectPool.Instance.OutObject(Stopmonster.ObjectLayer, MonsterType[0], monsterAppearPos.transform.position, Quaternion.identity); break;
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
            else if (index.Equals(2))
            {
                index = 0;
            }
        }
    }
    #endregion
    public Quaternion RotationCheck(LayerMask layer)
    {
        Debug.Log(layer == 6);
        if (layer == 6)
        {
            Debug.Log("회전");
            distance = PlayerPos - monsterAppearPos.transform.position;
        }
        //else
        //{
        //    distance = monster1.transform.position - PlayerPos;
        //}
        rotation = Quaternion.LookRotation(distance.normalized); //Quaternion.LookRotation작동방식 알아보기
        return rotation;
    }

  
}
