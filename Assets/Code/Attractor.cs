using UnityEngine;
using System.Collections;

public class Attractor : MonoBehaviour {

    private Transform _parentT;

    private const float AttractAccel = 1.0f;

	// Use this for initialization
	void Start () {
        _parentT = transform.parent;
	}

    void OnTriggerStay (Collider other)
    {
        if (other.tag == "Player")
        {
            var movement = (other.transform.position - transform.position).normalized*AttractAccel*Time.smoothDeltaTime;

            _parentT.transform.position += movement;
        }
    }
}
