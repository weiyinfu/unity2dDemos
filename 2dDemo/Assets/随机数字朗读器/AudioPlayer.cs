using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource[] a;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var i in a)
        {
            i.playOnAwake = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 5 == 0)
        {

            var playing = false;
            foreach (var i in a)
            {
                if (i.isPlaying)
                {
                    playing = true;
                }
            }
            if (playing)
            {
                return;
            }
            var ind = (int)(Random.value * 10);
            a[ind].Play();
        }

    }
}
