using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float rcsThrust = 100f;
    [SerializeField] private float mainThrust = 100f;

    private Rigidbody rigidBody;
    private AudioSource thrustingSound;

    private enum State
    {
        Alive,
        Dying,
        Transcending,
    }

    [SerializeField] private State state = State.Alive;

    private void Start()
    {
        this.rigidBody = GetComponent<Rigidbody>();
        this.thrustingSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (this.state == State.Dying || this.state == State.Transcending)
        {
            this.thrustingSound.Stop();
            return;
        }

        this.Thrust();
        this.Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;

            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f);
                break;

            case "Fuel":
                print("Refilled fuel");
                break;

            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 2f);
                break;
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void Thrust()
    {
        var thrustThisFrame = this.mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            this.rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!this.thrustingSound.isPlaying) this.thrustingSound.Play();
        }
        else
        {
            this.thrustingSound.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        var rotationThisFrame = this.rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation
    }
}