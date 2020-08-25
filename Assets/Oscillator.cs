using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attribute tells that we only want one on an object
[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 MovementVector = new Vector3(10f, 10f, 10f);

    // Determines how much object has moved
    [SerializeField] float Period = 2f;

    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        Period = (Period < 0.5) ? 0.5f : Period;
    }

    // Update is called once per frame
    void Update()
    {
        // Logic to control oscillating of an object position
        float rawSin = Mathf.Sin(Time.time / Period * 2 * Mathf.PI);
        float movementFactor = rawSin / 2 + 0.5f;
        Vector3 offset = movementFactor * MovementVector;
        transform.position = startingPos + offset;
    }
}
