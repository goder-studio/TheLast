using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 5;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        controller.SimpleMove(new Vector3(h, 0, v) * speed);
    }
}
