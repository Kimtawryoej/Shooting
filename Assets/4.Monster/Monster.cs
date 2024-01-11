using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

public class Monster : Unit, I_ObseverManager
{
    [SerializeField] private GameObject bullet;
    public GameObject Bullet { get => bullet; }
    public static Monster Instance;
    private List<I_Obsever> Obsevers = new List<I_Obsever>();
    #region Layer변수
    [SerializeField] private LayerMask MonsterType;
    private BitArray MonsterTypeBit;
    #endregion
    private TypesAction monsterAction;
    public TypesAction MonsterAction { get => monsterAction; }
    private TimeAgent timeNull;

    //왜 함수 밖에서는 매개변수를 불러올수 없음?
    override protected void Awake()
    {
        #region ActionsSet
        MonsterTypeBit = new BitArray(new int[] { MonsterType.value.ConvertTo<int>() });
        monsterAction = new TypesAction(MonsterTypeBit, unitStat, transform.rotation, this);
        monsterAction.Set();
        #endregion
        Instance = this;
    }
    override protected void OnEnable()
    {
        ReSetStat();
        StartCoroutine(MonsterAction.GetValue());
    }

    private void Start()
    {

    }

    private void FixedUpdate()
    {

    }
    override protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            if (bullet.Layer == LayerMask.GetMask("PlayerBullet"))
            {
                ChangeHp(-bullet.Power);
                bullet.InObj();
            }
        }
    }


    #region ObseverManager
    public void Add(I_Obsever obsever)
    {
        Obsevers.Add(obsever);
    }
    public void Delete(I_Obsever obsever)
    {
        Obsevers.Clear();
    }
    public void NotifyObserver<T>(List<I_Obsever> obsevers, T value)
    {
        foreach (I_Obsever Obsevers in Obsevers)
        {
            Obsevers.Refresh<T>(value);
        }
    }
    #endregion
}

public class TypesAction : MonoBehaviour
{
    private BitArray MonsterType;
    private UnitStatInfo unitStat;
    private Quaternion rotation;
    private Monster myObject;
    private Vector3[] SaveEndPos = { new Vector3(-11, 0, 3.115f), new Vector3(1, 0, 3.115f), new Vector3(11, 0, 3.115f) };
    private static HashSet<Vector3> EndPos = new HashSet<Vector3>();
    #region 타이머
    private TimeAgent WaitShoot;
    #endregion
    public TypesAction(BitArray MonsterType, UnitStatInfo unitStat, Quaternion rotation, Monster myObject)
    {
        this.MonsterType = MonsterType;
        this.unitStat = unitStat;
        this.rotation = rotation;
        this.myObject = myObject;
    }

    public void Set()
    {
        WaitShoot = new TimeAgent(1, (TimeAgent) => { }, (TimeAgent) => { myObject.Shoot(myObject.Bullet, unitStat.AttackPower, 0.5f, myObject.transform.rotation); });
    }
    public IEnumerator GetValue() //인스펙터 창에서 체크한 레이어에 따른 코드를 실행시키고 싶으면 움직이는 코루틴 공격하는 코루틴 두개 만들어서 공격이랑 움직임 안 겹치게 해야함
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        while (true)
        {
            for (int i = 0; i < MonsterType.Count; i++)
            {
                if (MonsterType[i].Equals(true)) 
                {
                    switch (i)
                    {
                        case 10:
                            yield return StopMove(wait);
                            break;
                        case 11:
                            yield return MoveMonster();
                            break;
                    }
                }
            }

        }
    }

    #region ActionMethods
    IEnumerator MoveMonster()
    {
        myObject.transform.Translate(-Vector3.forward * unitStat.MoveSpeed * Time.fixedDeltaTime);
        yield return null;
        TimerSystem.Instance.AddTimer(WaitShoot);
    }

    IEnumerator StopMove(WaitForSeconds wait)
    {
        foreach (Vector3 item in SaveEndPos)
        {
            if (!EndPos.Contains(item))
            {
                EndPos.Add(item);
                break;
            }
            else if (SaveEndPos.Last().Equals(EndPos.Last()))
            {
                EndPos.Clear();
            }
        }
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            myObject.transform.position = Vector3.Lerp(GameManager.Instance.MonsterAppearPos.transform.position, EndPos.Last(), i);
            yield return null;
        }
        for (float i = 0; i <= 10; i ++)
        {
            myObject.Shoot(myObject.Bullet, unitStat.AttackPower, 0.5f, Quaternion.identity);//**
            yield return wait;
        }
    }
    #endregion
}
