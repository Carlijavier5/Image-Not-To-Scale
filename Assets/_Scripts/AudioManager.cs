using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour {

    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class SoundClip {
        public AudioClip clip;
        public string name;
    }

    [SerializeField] private AudioSource musicSource, sfxSource;
    [SerializeField] private SoundClip[] clips;

    [SerializeField] private bool playPreamble;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }
    }

    private void Start() {
        if (playPreamble) PlayMusicWithPreamble(musicSource.clip.name, sfxSource.clip.name);
        else PlayMusic(musicSource.clip.name);
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
        PlaySFX(introClip.name);
        StartCoroutine(PlayLoopable(loopClip));
    }

    private IEnumerator PlayLoopable(SoundClip loopClip) {
        while (sfxSource.isPlaying && !_preambleFinished) {
            Debug.Log("hi");
            yield return null;
        }

        _preambleFinished = true;
        PlayMusic(loopClip.name);
    }

    private bool _preambleFinished = false;
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