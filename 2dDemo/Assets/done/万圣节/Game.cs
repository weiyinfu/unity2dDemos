using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject particle;
    public GameObject moon;
    public GameObject face;
    public GameObject words;
    public GameObject bat;
    float[] timeIndex = {0, 4, 6, 8, 10};
    GameObject[] a;
    int lastIndex = 0;
    bool canClickWord = false;

    void Start()
    {
        a = new GameObject[] {particle, moon, bat, face, words};
        foreach (var i in a)
        {
            i.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        while (lastIndex < timeIndex.Length && Time.realtimeSinceStartup >= timeIndex[lastIndex])
        {
            if (a[lastIndex] == words)
            {
                canClickWord = true;
            }
            else
            {
                a[lastIndex].SetActive(true);
                if (a[lastIndex] == face)
                {
                    face.GetComponent<AudioSource>().Play();
                }
            }

            lastIndex++;
        }

        if (canClickWord && Mouse.current.leftButton.wasPressedThisFrame)
        {
            words.SetActive(true);
        }
    }
}