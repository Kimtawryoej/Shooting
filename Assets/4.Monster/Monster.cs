using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Unit, I_ObseverManager
{
    [SerializeField] private GameObject bullet;
    public static Monster Instance;
    public List<I_Obsever> Obsevers = new List<I_Obsever>();

    override protected void OnEnable()
    {
        ReSetStat();
        
        //StartCoroutine(Shoot1(bullet, unitStat.AttackPower, -0.5f));
        //StartCoroutine(Shoot(() => , bullet, unitStat.AttackPower, -0.5f));
    }

    override protected void Awake()
    {
        Instance = this;
    }
     
    private void FixedUpdate()
    {
        transform.Translate(-Vector3.forward * unitStat.MoveSpeed);
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
    #region 지워고 고쳐야할코드
    protected IEnumerator Shoot1(GameObject bullet, int bulletPower, float bulletSpeed)
    {
        WaitForSeconds wait = new WaitForSeconds(1);
        while (true)
        {
            yield return wait;
            ObjectPool.Instance.OutObject(layer, bullet, transform.position, transform.rotation).gameObject.TryGetComponent(out Bullet bulletor);
            bulletor.Power = bulletPower;
            bulletor.Speed = bulletSpeed;
        }
    }
    #endregion
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
