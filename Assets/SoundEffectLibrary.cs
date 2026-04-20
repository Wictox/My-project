using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] private SoundEffectGroup[] soundEffectGroups;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();

        foreach (SoundEffectGroup soundEffectGroup in soundEffectGroups)
        {
            soundDictionary[soundEffectGroup.name] = soundEffectGroup.audioClips;
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if (soundDictionary.ContainsKey(name))
        {
            List<AudioClip> clips = soundDictionary[name];
            if (clips.Count > 0)
            {
                // THE FIX: Change audioClips to clips!
                return clips[Random.Range(0, clips.Count)];
            }
        }
        else
        {
            Debug.LogWarning($"Sound effect group '{name}' not found or empty.");
            return null;
        }
        return null;
    }
    private Dictionary<string, List<AudioClip>> soundDictionary;
}

[System.Serializable]
public struct SoundEffectGroup
{
    public string name;
    public List<AudioClip> audioClips;
}