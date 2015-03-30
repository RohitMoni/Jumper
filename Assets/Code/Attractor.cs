using UnityEngine;
using System.Collections;

public class Attractor : MonoBehaviour {

    private Vector3 _velocity;

    private Transform _parentT;

    private const float AttractAccel = 1.0f;

	// Use this for initialization
	void Start () {
        _velocity = Vector3.zero;
        _parentT = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
        //_parentT.Translate(_velocity);
	}

    void OnTriggerStay (Collider other)
    {
        if (other.tag != "Player")
            return;

        _parentT.GetComponent<Rigidbody>().AddForce((other.transform.position - transform.position).normalized * AttractAccel);
        //_velocity += ;
    }

    void OnTriggerExit (Collider other)
    {
        if (other.tag != "Player")
            return;

        _velocity = Vector3.zero;
    }
}
