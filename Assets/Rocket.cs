using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(3 * Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rigidBody.AddRelativeTorque(Vector3.back);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rigidBody.AddRelativeTorque(Vector3.forward);
        }
    }
}
