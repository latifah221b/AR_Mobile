using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectable_logic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestory());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator  SelfDestory()
    {
        yield return new WaitForSeconds(50);
        Destroy(this.gameObject);
    }
}
