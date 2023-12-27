using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    private void Start()
    {
        StartCoroutine(Disappear());
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward*0.5f);
    }

    private IEnumerator Disappear()
    {
        WaitForSeconds wait = new WaitForSeconds(3);
        WaitUntil condition = new WaitUntil(() => gameObject.activeSelf);
        while (true)
        {
            yield return condition;
            yield return wait;
            ObjectPool.Instance.InObject(layer, gameObject);
        }
    }
}
