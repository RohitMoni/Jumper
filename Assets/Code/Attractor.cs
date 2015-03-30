using UnityEngine;
using System.Collections;

public class Attractor : MonoBehaviour {


    private Transform _parentT;

    private const float AttractAccel = 50.0f;

	// Use this for initialization
	void Start () {
        _parentT = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerStay (Collider other)
    {
        if (other.tag != "Player")
            return;

        _parentT.GetComponent<Rigidbody>().AddForce((other.transform.position - transform.position).normalized * AttractAccel * Time.smoothDeltaTime);
    }
}
