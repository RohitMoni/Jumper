using System.Net.Mime;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    private const float updateInterval = 0.5f;
    private float _accum = 0.0f; // FPS accumulated over the interval
    private float _frames = 0f; // Frames drawn over the interval
    private float _timeleft; // Left time for current interval

    private Text _text;

	// Use this for initialization
	void Start ()
	{
	    _text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        _timeleft -= Time.deltaTime;
        _accum += Time.timeScale / Time.deltaTime;
        ++_frames;

        // Interval ended - update GUI text and start new interval
        if (_timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            _text.text = "" + (_accum / _frames).ToString("f2");
            _timeleft = updateInterval;
            _accum = 0.0f;
            _frames = 0;
        }
	}
}
