using UnityEngine;
using System.Collections;

public class Attractor : MonoBehaviour {

    private Vector3 _velocity;
    private float _attractAccel;

    private Transform _parentT;

	// Use this for initialization
	void Start () {
        _velocity = Vector3.zero;
        _attractAccel = 0.01f;
        _parentT = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
        _parentT.Translate(_velocity);
	}

    void OnTriggerStay (Collider other)
    {
        if (other.tag != "Player")
            return;

        _velocity += (other.transform.position - transform.position).normalized * _attractAccel;
    }

    void OnTriggerExit (Collider other)
    {
        if (other.tag != "Player")
            return;

        _velocity = Vector3.zero;
    }
}
