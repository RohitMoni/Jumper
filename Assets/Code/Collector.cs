using Assets.Code;
using UnityEngine;
using System.Collections;

public class Collector : MonoBehaviour {

    /* References */

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().CollectCoin();
            transform.parent.gameObject.GetComponent<Coin>().Collect();
            Debug.Log("Coin collected!");
        }
    }
}
