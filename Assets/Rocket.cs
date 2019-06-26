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
        this.ProcessInput();
    }

    private void ProcessInput()
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

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }
    }
}