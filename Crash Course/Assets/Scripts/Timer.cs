using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool isPlaying;
    Text txt;
 
    void Awake()
    {
        txt = GetComponent<Text>();
    }
 
    void Start()
    {
        isPlaying = true;
    }
 
    void Update ()
    {
        if(isPlaying)
            txt.text = Time.time.ToString("#.00");
    }
}
