using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace MidnightCoder.Game
{
    public class AudioMgr : MonoBehaviour
    {
        //
        // Static Fields
        //
        private const float FADE_DURATION = 1;

        private const string PREFS_BGM_MUTE = "BGMMute";

        private const string PREFS_SFX_MUTE = "SFXMute";

        private static AudioMgr instance;

        //
        // Fields
        //
        [SerializeField]
        private List<AudioSource> bgmIntro = new List<AudioSource>();

        [SerializeField]
        private List<AudioSource> bgmLoop = new List<AudioSource>();

        [SerializeField]
        private List<AudioSource> sfx = new List<AudioSource>();

        [SerializeField]
        private AudioSource sting;

        [SerializeField]
        private AudioSource voice;

        private int sfxIndex;

        private int bgmIndex;

        //
        // Static Properties
        //
        public static AudioMgr Instance
        {
            get
            {
                return AudioMgr.instance;
            }
        }

        //
        // Static Methods
        //
        public static void Initialize()
        {
            GameObject gameObject = new GameObject("[AudioMgr]");
            AudioMgr.instance = gameObject.AddComponent<AudioMgr>();
            AudioMgr.instance.CreateObjects();
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
        }

        //
        // Methods
        //
        private AudioSource CreateAudioSource(string name)
        {
            GameObject gameObject = new GameObject(name);
            if (gameObject == null)
            {
                return null;
            }
            gameObject.transform.SetParent(transform);
            return gameObject.AddComponent<AudioSource>();
        }

        [ContextMenu("Create Objects")]
        public void CreateObjects()
        {
            this.bgmIntro.Clear();
            this.bgmLoop.Clear();
            for (int i = 0; i < 2; i++)
            {
                this.bgmIntro.Add(this.CreateAudioSource("Intro" + i.ToString()));
                AudioSource audioSource = this.CreateAudioSource("Loop" + i.ToString());
                audioSource.loop = true;
                this.bgmLoop.Add(audioSource);
            }
            for (int j = 0; j < 5; j++)
            {
                this.sfx.Add(this.CreateAudioSource("SFX" + j.ToString()));
            }
            this.sting = this.CreateAudioSource("Sting");
            this.voice = this.CreateAudioSource("Voice");
        }

        [DebuggerHidden]
        private IEnumerator Fade(AudioSource source, bool isFadeIn)
        {
            if (source == null)
            {
                yield break;
            }
            float time0 = 0;
            float start1 = (!isFadeIn) ? 1 : 0;
            float end2 = (!isFadeIn) ? 0 : 1;
            source.volume = start1;
            yield return null;

            GOTO:
            time0 += Time.deltaTime;
            if (time0 < 1)
            {
                source.volume = Mathf.Lerp(start1, end2, time0 / 1);
                yield return null;
                goto GOTO;
            }
            source.volume = end2;
            if (!isFadeIn)
            {
                source.Stop();
            }
        }

        public bool IsBGMusicEnabled()
        {
            return PlayerPrefs.GetInt("BGMMute", 0) == 0;
        }

        public bool IsSFXEnabled()
        {
            return PlayerPrefs.GetInt("SFXMute", 0) == 0;
        }

        private bool IsValidBGMIndex(int index)
        {
            return index < this.bgmIntro.Count && index < this.bgmLoop.Count && !(this.bgmIntro[index] == null) && !(this.bgmLoop[index] == null);
        }

        public void PlaySFX(AudioClip clip)
        {
            this.PlaySFX(clip, 1, 1);
        }

        public void PlaySFX(InfoClip infoClip, float pitch = 1)
        {
            if (infoClip != null && infoClip.clip != null)
            {
                this.PlaySFX(infoClip.clip, infoClip.volume, pitch);
            }
        }

        public void PlaySFX(AudioClip clip, float volume, float pitch = 1)
        {
            if (clip == null)
            {
                return;
            }
            if (!this.IsSFXEnabled())
            {
                return;
            }
            this.sfxIndex = (this.sfxIndex + 1) % this.sfx.Count;
            AudioSource audioSource = this.sfx[this.sfxIndex];
            if (audioSource == null)
            {
                return;
            }
            if (!audioSource.enabled)
            {
                return;
            }
            if (volume < 0)
            {
                volume = 0;
            }
            else
            {
                if (volume > 1)
                {
                    volume = 1;
                }
            }
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip, volume);
        }

        public void PlaySting(AudioClip clip)
        {
            if (this.sting == null)
            {
                return;
            }
            this.sting.loop = false;
            this.sting.clip = clip;
            this.sting.Play();
        }

        public void PlayVoice(AudioClip clip)
        {
            if (this.voice == null)
            {
                return;
            }
            this.voice.loop = false;
            this.voice.clip = clip;
            this.voice.Play();
        }

        public void SetBGMusic(BGMParts clip, bool isLoop = true)
        {
            int index = this.bgmIndex;
            this.bgmIndex = (this.bgmIndex + 1) % this.bgmIntro.Count;
            if (!this.IsValidBGMIndex(this.bgmIndex))
            {
                return;
            }
            if (this.IsValidBGMIndex(index))
            {
                if (this.bgmIntro[index].isPlaying)
                {
                    base.StartCoroutine(this.Fade(this.bgmIntro[index], false));
                }
                base.StartCoroutine(this.Fade(this.bgmLoop[index], false));
            }
            if (clip == null)
            {
                return;
            }
            if (clip.intro != null)
            {
                this.bgmIntro[this.bgmIndex].clip = clip.intro;
                this.bgmIntro[this.bgmIndex].Play();
                this.bgmLoop[this.bgmIndex].clip = clip.loop;
                double num = AudioSettings.dspTime + (double)clip.intro.length;
                this.bgmLoop[this.bgmIndex].PlayScheduled(num);
                base.StartCoroutine(this.Fade(this.bgmIntro[this.bgmIndex], true));
                base.StartCoroutine(this.Fade(this.bgmLoop[this.bgmIndex], true));
            }
            else
            {
                if (clip.loop != null)
                {
                    this.bgmLoop[this.bgmIndex].clip = clip.loop;
                    this.bgmLoop[this.bgmIndex].Play();
                    base.StartCoroutine(this.Fade(this.bgmLoop[this.bgmIndex], true));
                }
            }
        }

        public void SetBGMusicActive(bool isActive)
        {
            foreach (AudioSource current in this.bgmIntro)
            {
                current.mute = !isActive;
            }
            foreach (AudioSource current2 in this.bgmLoop)
            {
                current2.mute = !isActive;
            }
            PlayerPrefs.SetInt("BGMMute", (!isActive) ? 1 : 0);
        }

        public void SetSFXActive(bool isActive)
        {
            foreach (AudioSource current in this.sfx)
            {
                current.mute = !isActive;
            }
            if (this.sting != null)
            {
                this.sting.mute = !isActive;
            }
            if (this.voice != null)
            {
                this.voice.mute = !isActive;
            }
            PlayerPrefs.SetInt("SFXMute", (!isActive) ? 1 : 0);
        }

        public void Start()
        {
            int int1 = PlayerPrefs.GetInt("BGMMute", 0);
            int int2 = PlayerPrefs.GetInt("SFXMute", 0);
            this.SetBGMusicActive(int1 == 0);
            this.SetSFXActive(int2 == 0);
        }
    }
}
