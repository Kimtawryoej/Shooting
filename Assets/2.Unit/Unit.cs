using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.FilePathAttribute;
using System.Drawing;
using System.Security.Cryptography;
using UnityEngine.UIElements;


[System.Serializable]
public struct UnitStatInfo
{
    public float MaxHp;
    public int MinHp;
    public float MoveSpeed;
    public int AttackPower;
    public float Deefense;
    public int AttackPowerUp;
}


//[RequireComponent(typeof(SpriteRenderer))]
//[RequireComponent(typeof(Rigidbody2D))]
public abstract class Unit : MonoBehaviour
{
    #region 변수
    public LayerMask ObjectLayer;
    [SerializeField] protected bool hitcheck;
    [SerializeField] protected UnitStatInfo unitStat;
    public UnitStatInfo UnitStat => unitStat;

    [SerializeField] protected float currentHp;
    public float CurrentHp => currentHp;

    protected bool isDead = false;

    protected SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    [SerializeField] protected Animator animator;
    public Animator Anim => animator;

    protected Rigidbody2D rigid;
    public Rigidbody2D Rigid => rigid;
    #endregion

    protected virtual void OnEnable()
    {
        ReSetStat();
    }


    protected abstract void OnTriggerEnter(Collider other);

    #region ReSet
    protected void ReSetStat()
    {
        ResetHp();
        //ResetSpeed();
        //ResetAttackPower();
    }

    protected virtual void ResetHp()
    {
        currentHp = unitStat.MaxHp;
    }

    //protected abstract void ResetSpeed();

    //protected abstract void ResetAttackPower();
    #endregion

    #region Hp
    public virtual void SetHp(int healValue)
    {
        ChangeHp(healValue);
    }

    public virtual void ChangeHp(int value)
    {
        if (!isDead)
        {
            ClampHp(ref value);
            if (CurrentHp <= UnitStat.MinHp)
            {
                Death();
                isDead = true;
            }
        }
    }

    protected void ClampHp(ref int value)
    {
        if (CurrentHp + value >= UnitStat.MaxHp)
        {
            currentHp = UnitStat.MaxHp;
        }

        else if (CurrentHp + value <= UnitStat.MinHp)
        {
            currentHp = UnitStat.MinHp;
        }

        else { currentHp += value; }
    }
    #endregion

    #region GetValue
    public float GetMoveSpeed()
    {
        return unitStat.MoveSpeed;
    }

    public float GetHp()
    {
        return currentHp;
    }

    public float GetMaxHp()
    {
        return unitStat.MaxHp;
    }

    public int GetAttackPower()
    {
        return unitStat.AttackPower;
    }
    #endregion

    #region Defence
    public void ChangeDeefense(float value)
    {
        unitStat.Deefense += value;
    }
    #endregion

    #region AttackPowerUp
    public void ChangeAttackPowerUp(int value)
    {
        unitStat.AttackPowerUp += value;
    }
    #endregion
    protected virtual void Death()
    {
        Debug.Log("죽음");
        Destroy(gameObject);
        //animator.SetBool("Death", true);
        //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        //{
        //    gameObject.SetActive(false);
        //}
    }

    public IEnumerator AniStop(string Aniname, string Aniset)
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        animator.SetBool(Aniset, false);
        yield return new WaitForSeconds(2.5f);
    }

    #region BulletAppear

    public void Shoot(GameObject bullet, int bulletPower, float bulletSpeed, Quaternion rotation,Vector3 position)
    {
        //ObjectPool.Instance.OutObject(ObjectLayer, bullet, transform.position, rotation).gameObject.TryGetComponent(out Bullet bulletor);
        Instantiate(bullet, position, rotation).gameObject.TryGetComponent(out Bullet bulletor);
        bulletor.Power = bulletPower;
        bulletor.Speed = bulletSpeed;
    }

    public void BulletRotationShoot(int angle, int count, GameObject bullet, int bulletPower, float bulletSpeed)
    {
        GameObject[] bulletor = new GameObject[count];
        int RotAngle = angle / (count - 1);
        bulletor[0] = Instantiate(bullet, transform.position, Quaternion.identity);
        for (int i = 1; i < count; i++)
        {
            bulletor[i] = Instantiate(bullet, transform.position, Quaternion.AngleAxis(i % 2 != 0 ? -RotAngle  : RotAngle, -Vector3.up));
            //Quaternion은 *가 더하는거
        }
        foreach (var BulletSet in bulletor)
        {
            BulletSet.gameObject.TryGetComponent(out Bullet Bulletor);
            Bulletor.Power = bulletPower;
            Bulletor.Speed = bulletSpeed*-1;
        }

    }
    #endregion
}
