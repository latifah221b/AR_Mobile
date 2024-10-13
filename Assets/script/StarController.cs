using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestory());
    }

    IEnumerator  SelfDestory()
    {
        yield return new WaitForSeconds(50);
        Destroy(this.gameObject);
    }
}
