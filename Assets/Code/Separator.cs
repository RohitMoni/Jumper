using UnityEngine;
using System.Collections;

public class Separator : MonoBehaviour {

    private Transform _parentT;
    private const float SeparateAccel = 1.0f;
    private Vector3 movement;

	// Use this for initialization
	void Start () {
        _parentT = transform.parent;
	    movement = Vector3.zero;
	}

    void Update()
    {
        _parentT.transform.position += movement * Time.smoothDeltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            movement -= (other.transform.position - transform.position).normalized * SeparateAccel;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Coin")
        {
            movement += (other.transform.position - transform.position).normalized * SeparateAccel;
        }
    }

    //void OnTriggerStay (Collider other)
    //{
    //    if (other.tag == "Coin")
    //    {
    //        var movement = (other.transform.position - transform.position).normalized * SeparateAccel * Time.smoothDeltaTime;

    //        _parentT.transform.position -= movement;
    //    }
    //}
}
