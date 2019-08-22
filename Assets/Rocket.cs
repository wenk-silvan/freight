using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private int currentLevelIndex = 0;

    [SerializeField] private float rcsThrust = 100f;
    [SerializeField] private float mainThrust = 100f;

    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip victory;

    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private ParticleSystem victoryParticles;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

    bool collisionsDisabled = false;
    bool isTransitioning = false;

    private void Start()
    {
        this.rigidBody = GetComponent<Rigidbody>();
        this.audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Debug.isDebugBuild)
        {
            this.RespondToDebugKey();
        }

        if (this.isTransitioning)
        {
            return;
        }

        this.RespondToThrustInput();
        this.RespondToRotateInput();
    }

    private void RespondToDebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            this.LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            this.collisionsDisabled = !this.collisionsDisabled;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (this.isTransitioning || this.collisionsDisabled) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                OnFinishLevel();
                break;

            case "Fuel":
                break;

            default:
                OnDeadlyCollision();
                break;
        }
    }

    private void OnDeadlyCollision()
    {
        this.isTransitioning = true;
        this.audioSource.Stop();
        this.audioSource.PlayOneShot(this.explosion);
        this.mainEngineParticles.Stop();
        this.explosionParticles.Play();
        Invoke("LoadSameLevel", 2f);
    }

    private void OnFinishLevel()
    {
        this.isTransitioning = true;
        this.audioSource.Stop();
        this.audioSource.PlayOneShot(this.victory);
        this.mainEngineParticles.Stop();
        this.victoryParticles.Play();
        Invoke("LoadNextLevel", 1f);
    }

    private void LoadSameLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextLevel()
    {
        var nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
            return;
        }
        SceneManager.LoadScene(nextLevelIndex);
    }

    private void RespondToThrustInput()
    {
        var thrustThisFrame = this.mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust(thrustThisFrame);
        }
        else
        {
            this.audioSource.Stop();
            this.mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust(float thrustThisFrame)
    {
        this.rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!this.audioSource.isPlaying)
        {
            this.audioSource.PlayOneShot(this.mainEngine);
        }
        this.mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        var rotationThisFrame = this.rcsThrust * Time.deltaTime;
        ApplyRotation(rotationThisFrame);

        rigidBody.freezeRotation = false; // resume physics control of rotation
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
    }
}