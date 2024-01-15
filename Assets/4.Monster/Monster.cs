using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    TimeAgent ActiveFalse;

    //왜 함수 밖에서는 매개변수를 불러올수 없음?
    protected void Awake()
    {
        #region ActionsSet
        MonsterTypeBit = new BitArray(new int[] { MonsterType.value.ConvertTo<int>() });
        monsterAction = new TypesAction(MonsterTypeBit, unitStat, transform.rotation, this);
        #endregion
        Instance = this;
    }
    override protected void OnEnable()
    {
        ReSetStat();
        //TimerSystem.Instance.AddTimer(ActiveFalse);
        StartCoroutine(MonsterAction.GetValueMove());
        StartCoroutine(MonsterAction.GetValueAttack());
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
    public TypesAction(BitArray MonsterType, UnitStatInfo unitStat, Quaternion rotation, Monster myObject)
    {
        this.MonsterType = MonsterType;
        this.unitStat = unitStat;
        this.rotation = rotation;
        this.myObject = myObject;
    }

    public IEnumerator GetValueMove()
    {
        for (int i = 10; i < 12; i++)
        {
            if (MonsterType[i].Equals(true))
            {
                switch (i)
                {
                    case 10:
                        yield return StopMove();
                        break;
                    case 11:
                        yield return MoveMonster();
                        break;
                }
            }
        }
    }

    public IEnumerator GetValueAttack()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        for (int i = 12; i < 14; i++)
        {
            if (MonsterType[i].Equals(true))
            {
                switch (i)
                {
                    case 12:
                        yield return Shoot1(wait);
                        break;
                    case 13:
                        yield return Shoot3(wait);
                        break;
                }
            }
        }
    }

    #region ActionMethods
    IEnumerator MoveMonster()
    {
        while (true)
        {
            myObject.transform.Translate(-Vector3.forward * unitStat.MoveSpeed * Time.fixedDeltaTime);
            if (GameManager.Instance.RangeCheack(myObject.gameObject))
            {
                Debug.Log("감지");
                Destroy(myObject.gameObject);
            }
            yield return null;
        }
    }

    IEnumerator StopMove()
    {
        foreach (Vector3 item in SaveEndPos)
        {
            if (!EndPos.Contains(item))
            {
                EndPos.Add(item);
                break;
            }
            if (SaveEndPos.Last().Equals(EndPos.Last()))
            {
                EndPos.Clear();
                EndPos.Add(item);
                break;
            }
        }
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            myObject.transform.position = Vector3.Lerp(GameManager.Instance.MonsterAppearPos.transform.position, EndPos.Last(), i);
            yield return null;
        }

        yield return new WaitForSeconds(3);
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            myObject.transform.position = Vector3.Lerp(EndPos.Last(), GameManager.Instance.MonsterAppearPos.transform.position, i);
            yield return null;
        }
        Destroy(myObject.gameObject);
    }

    IEnumerator Shoot1(WaitForSeconds wait)
    {
        while (true)
        {
            myObject.Shoot(myObject.Bullet, unitStat.AttackPower, 0.4f, myObject.transform.rotation, myObject.transform.position);
            Debug.Log(unitStat.AttackPower);
            yield return wait;
        }
    }

    IEnumerator Shoot3(WaitForSeconds wait)
    {
        while (true)
        {
            myObject.BulletRotationShoot(60, 3, myObject.Bullet, unitStat.AttackPower, 0.4f);
            Debug.Log("생성");
            yield return wait;
        }
    }

    #endregion
}
