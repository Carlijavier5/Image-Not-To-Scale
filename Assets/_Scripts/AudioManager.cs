using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class SoundClip {
        public AudioClip clip;
        public string name;
    }

    [SerializeField] private AudioSource musicSource, sfxSource;
    [SerializeField] private SoundClip[] clips;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }
    
    public float PlaySFX(string clipName, float pitchVarAmp = 0) {
        SoundClip soundClip = clips.FirstOrDefault((soundClip) => soundClip.name == clipName);
        if (soundClip == null) {
            Debug.LogError($"'{clipName}' was not found among the audio files;");
            return 0;
        } else {
            sfxSource.pitch = 1 + Random.Range(-pitchVarAmp, pitchVarAmp);
            sfxSource.PlayOneShot(soundClip.clip);
            return soundClip.clip.length;
        }
    }
}