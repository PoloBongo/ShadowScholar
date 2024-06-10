using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");

        transform.Rotate(transform.up, 2 * 10 * hor * Time.deltaTime);

        rb.AddForce(100 * vert * transform.forward);
    }
}
