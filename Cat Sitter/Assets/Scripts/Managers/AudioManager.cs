using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Dictionary<string, AudioClip> audioClips = new();

    public void PlayAudio(String id)
    {
        // Play audio with id
    }
}
