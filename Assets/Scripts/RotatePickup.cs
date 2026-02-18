using System;
using UnityEngine;

public class RotatePickup : MonoBehaviour
{
    public float bobStrength = 1.0f;
    public float originalY;
    void Start()
    {
        this.originalY = this.transform.position.y;
    }
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

        Vector3 pos = transform.position;
        //kinda bobs up and down? not really noticable
        transform.position = new Vector3(pos.x, originalY + ((float)Math.Cos(Time.time) * bobStrength), pos.z);

        //transform.position = Vector3.up * Mathf.Cos(Time.time) * speed;

        // float newY = Mathf.Sin(Time.time * speed) * height + transform.position.y;
        // transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }


}
