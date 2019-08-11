using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource thrustingSound;
    int levelCounter;

    private void Start()
    {
        this.rigidBody = GetComponent<Rigidbody>();
        this.thrustingSound = GetComponent<AudioSource>();
        this.levelCounter = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        this.Thrust();
        this.Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Finish":
                print("Hit finish");
                //this.levelCounter = this.levelCounter + 1;
                //if (this.levelCounter > SceneManager.sceneCount - 1)
                //{
                //    this.levelCounter = 0;
                //}
                SceneManager.LoadScene(1);
                break;
            case "Fuel":
                print("Refilled fuel");
                break;
            default:
                print("Dead");
                SceneManager.LoadScene(0);
                break;
        }
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