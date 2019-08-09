using UnityEngine;

public class Rocket : MonoBehaviour
{
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
        if (Input.GetKey(KeyCode.Space))
        {
            this.rigidBody.AddRelativeForce(Vector3.up);
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

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation
    }
}