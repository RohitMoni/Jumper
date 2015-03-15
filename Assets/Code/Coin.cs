using UnityEngine;

public class Coin : MonoBehaviour {

    /* References */
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().CollectCoin();
            Debug.Log("Coin collected!");
        }
    }
}
