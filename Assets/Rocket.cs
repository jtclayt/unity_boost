using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float RCSThrust = 175f;
    [SerializeField] float VerticalThrust = 3f;
    Rigidbody rigidBody;
    AudioSource boosterAudio;

    const string FRIENDLY_TAG = "Friendly";
    const string FINISH_TAG = "Finish";

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        boosterAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * VerticalThrust);
            if (!boosterAudio.isPlaying)
            {
                boosterAudio.Play();
            }
        }
        else
        {
            boosterAudio.Stop();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag(FRIENDLY_TAG))
        {
            print("alive");
        }
        else if (collision.gameObject.CompareTag(FINISH_TAG))
        {
            print("win");
        }
        else
        {
            print("dead");
        }
    }

    private void Rotate()
    {
        Vector3 rotationThisFrame = RCSThrust * Time.deltaTime * Vector3.forward;

        // Take control of rotation
        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-rotationThisFrame);
        }

        // Release control of rotation to physics engine
        rigidBody.freezeRotation = false;
    }
}
