using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    public class AudioEntry
    {
        public string id;
        public AudioClip clip;
    }

    public List<AudioEntry> audioClips;
    Dictionary<string, AudioClip> audioMap = new();

    void Awake()
    {
        foreach (var entry in audioClips)
        {
            audioMap.Add(entry.id, entry.clip);
        }
    }

    public void PlayAudio(string id)
    {
        // Play audio with id
        if (audioMap.ContainsKey(id))
        {
            AudioSource.PlayClipAtPoint(audioMap[id], Camera.main.transform.position);
        }
        else
        {
            Debug.LogWarning($"Audio clip with id {id} not found");
        }
    }
}
