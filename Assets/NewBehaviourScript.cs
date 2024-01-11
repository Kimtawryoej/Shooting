using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public void a()
    {
        GameManager.Instance.gameObject.SetActive(false);
    }
    public void b()
    {
        GameManager.Instance.gameObject.SetActive(true);
    }
}
