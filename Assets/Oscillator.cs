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
    }

    // Update is called once per frame
    void Update()
    {
        float radianTraveled = Time.time / Period * 2 * Mathf.PI;
        Vector3 offset = Mathf.Sin(radianTraveled) * MovementVector;
        transform.position = startingPos + offset;
    }
}
