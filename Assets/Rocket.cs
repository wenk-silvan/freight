using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource thrustingSound;

    private void Start()
    {
        this.rigidBody = GetComponent<Rigidbody>();
        this.thrustingSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        this.Thrust();
        this.Rotate();
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