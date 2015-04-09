using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConnectToServerButton : MonoBehaviour
{
    public HostData HostData;

    public void Initialise(HostData hostData)
    {
        HostData = hostData;
        transform.FindChild("Text").GetComponent<Text>().text = hostData.gameName;
    }

}
