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

    public void PlayMusic(string clipName) {
        SoundClip soundClip = clips.FirstOrDefault((soundClip) => soundClip.name == clipName);
        if (soundClip == null) {
            Debug.LogError($"'{clipName}' was not found among the audio files;");
        } else {
            musicSource.clip = soundClip.clip;
            musicSource.Play();
        }
    }

    public void PlayMusicWithPreamble(string introName, string loopName) {
        SoundClip introClip = clips.FirstOrDefault((soundClip) => soundClip.name == introName);
        SoundClip loopClip = clips.FirstOrDefault((soundClip) => soundClip.name == loopName);
    }
    
    public float PlaySFX(string clipName, float pitchVarAmp = 0) {
        SoundClip soundClip = clips.FirstOrDefault((soundClip) => soundClip.name == clipName);
        if (soundClip == null) {
            Debug.LogError($"'{clipName}' was not found among the audio files;");
            return 0;
        } else {
            sfxSource.pitch = 1 + Random.Range(0, pitchVarAmp);
            sfxSource.PlayOneShot(soundClip.clip);
            return soundClip.clip.length;
        }
    }
}