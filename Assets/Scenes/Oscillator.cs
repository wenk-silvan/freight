using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] private Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    float movementFactor; //0 for not moved, 1 for fully moved.

    private Vector3 startingPosition;

    // Start is called before the first frame update
    private void Start()
    {
        this.startingPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        // todo: protect against period is zero
        var cycles = Time.time / period;
        const float tau = Mathf.PI * 2f;
        var rawSinWave = Mathf.Sin(cycles * tau);

        this.movementFactor = rawSinWave / 2f + 0.5f;
        var offset = this.movementVector * this.movementFactor;
        transform.position = this.startingPosition + offset;
    }
}