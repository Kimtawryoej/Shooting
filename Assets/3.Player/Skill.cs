using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Skill : MonoBehaviour
{
    private Vector3 dis;
    private Vector3 angle;
    private Vector3 centerPoint;
    public IEnumerator Shield(GameObject shield)
    {
        WaitForSeconds wait = new WaitForSeconds(0.3f);
        while (true)
        {
           
            yield return null;
        }
    }
}
