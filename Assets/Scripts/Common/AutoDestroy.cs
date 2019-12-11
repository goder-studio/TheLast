using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private float time = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAfter());
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
