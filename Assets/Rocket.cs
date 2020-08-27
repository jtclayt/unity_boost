using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float RCSThrust = 175f;
    [SerializeField] float VerticalThrust = 3f;
    [SerializeField] float LevelLoadDelay = 1f;
    [SerializeField] AudioClip MainEngineAudio;
    [SerializeField] AudioClip ExplosionAudio;
    [SerializeField] AudioClip WinAudio;
    [SerializeField] ParticleSystem MainEngineParticles;
    [SerializeField] ParticleSystem ExplosionParticles;
    [SerializeField] ParticleSystem WinParticles;
    Rigidbody rigidBody;
    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionsOff = false;

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
        if (!isTransitioning)
        {
            ThrustInput();
            RotateInput();
        }
        if (Debug.isDebugBuild)
        {
            DebugInput();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (isTransitioning || collisionsOff)
        {
            return;
        }

        if (collision.gameObject.CompareTag(FINISH_TAG))
        {
            Win();
        }
        else if (!collision.gameObject.CompareTag(FRIENDLY_TAG))
        {
            GameOver();
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
            StopThrust();
        }
    }

    private void Thrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * VerticalThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(MainEngineAudio);
            MainEngineParticles.Play();
        }
    }

    private void StopThrust()
    {
        MainEngineParticles.Stop();
        audioSource.Stop();
    }

    private void RotateInput()
    {
        Vector3 rotationThisFrame = RCSThrust * Time.deltaTime * Vector3.forward;

        // Take control of rotation
        rigidBody.angularVelocity = Vector3.zero;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-rotationThisFrame);
        }
    }

    private void DebugInput()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCollisions();
        }
    }

    private void ToggleCollisions()
    {
        if (!collisionsOff)
        {
            print("Collisions off");
            collisionsOff = true;
        }
        else
        {
            print("Collisions on");
            collisionsOff = false;
        }
    }

    private void Win()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(WinAudio);
        MainEngineParticles.Stop();
        WinParticles.Play();
        isTransitioning = true;
        // Invoke will call method after desired wait time (1f)
        Invoke("LoadNextLevel", LevelLoadDelay);
    }

    private void LoadNextLevel()
    {
        int currSceneIdx = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIdx = (currSceneIdx + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextSceneIdx);
    }

    private void GameOver()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(ExplosionAudio);
        MainEngineParticles.Stop();
        ExplosionParticles.Play();
        isTransitioning = true;
        Invoke("LoadFirstLevel", LevelLoadDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
}
