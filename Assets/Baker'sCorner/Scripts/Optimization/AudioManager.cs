using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class NamedAudio
    {
        public string name;
        public AudioClip clip;
    }

    [Header("Audio Library")]
    public List<NamedAudio> audioLibrary = new List<NamedAudio>();

    private Dictionary<string, AudioClip> clipDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist between scenes
            BuildClipDictionary();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void BuildClipDictionary()
    {
        clipDictionary = new Dictionary<string, AudioClip>();
        foreach (var entry in audioLibrary)
        {
            if (!clipDictionary.ContainsKey(entry.name))
                clipDictionary.Add(entry.name, entry.clip);
        }
    }

    // place this inside your AudioManager class (which builds clipDictionary in Awake)
    public void Play(string clipName)
    {
        if (clipDictionary == null)
        {
            Debug.LogWarning("AudioManager: clipDictionary not built.");
            return;
        }

        if (!clipDictionary.TryGetValue(clipName, out AudioClip clip) || clip == null)
        {
            Debug.LogWarning($"AudioManager: No clip found with name '{clipName}'.");
            return;
        }

        // choose a safe position to play from
        Vector3 playPos;
        if (Camera.main != null)
        {
            playPos = Camera.main.transform.position;
        }
        else if (Camera.current != null)
        {
            playPos = Camera.current.transform.position;
        }
        else
        {
            // fallback: use AudioManager's position (make sure AudioManager is placed in scene)
            playPos = transform.position;
        }

        AudioSource.PlayClipAtPoint(clip, playPos, 0.8f);
    }


    // Optional one-shots for gameplay sounds
    public static void PlayClip(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, position, 0.8f);
    }
}
