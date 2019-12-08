using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public float turnSpeed = 1;
    public float speed = 2;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float turnAmount = Input.GetAxis("Horizontal")* turnSpeed;
        Debug.Log(turnAmount);
        rb.velocity = gameObject.transform.forward * speed;
        gameObject.transform.Rotate(new Vector3(0, turnAmount, 0));
    }

}
