using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float RCSThrust = 175f;
    [SerializeField] float VerticalThrust = 3f;
    [SerializeField] float RespawnTimer = 1f;
    [SerializeField] AudioClip MainEngineAudio;
    [SerializeField] AudioClip ExplosionAudio;
    [SerializeField] AudioClip WinAudio;
    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    const string FRIENDLY_TAG = "Friendly";
    const string FINISH_TAG = "Finish";

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            ThrustInput();
            RotateInput();
        }
    }

    private void ThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Thrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Thrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * VerticalThrust);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(MainEngineAudio);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (state != State.Alive)
        {
            return;
        }

        audioSource.Stop();
        if (collision.gameObject.CompareTag(FINISH_TAG))
        {
            audioSource.PlayOneShot(WinAudio);
            state = State.Transcending;
            // Invoke will call method after desired wait time (1f)
            Invoke("LoadNextLevel", RespawnTimer);
        }
        else if (!collision.gameObject.CompareTag(FRIENDLY_TAG))
        {
            audioSource.PlayOneShot(ExplosionAudio);
            state = State.Dying;
            Invoke("LoadFirstLevel", RespawnTimer);
        }
    }

    private void RotateInput()
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

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}
