using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    [SerializeField] AudioClip[] clips;

    public AudioClip RandomSound()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}
