using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeshCollider : MonoBehaviour
{
    public Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}
