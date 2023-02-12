using System;
using TempoStudio;
using UnityEngine;

namespace TempoStudio
{
    [DefaultExecutionOrder(-998)]
    public class TempoSound : MonoBehaviour
    {
        private ITSLib tslib
        {
            get
            {
                return TempoStudio.tslib;
            }
        }

        private void Awake()
        {
            float[] array = new float[this.audioClip.samples * this.audioClip.channels];
            this.audioClip.GetData(array, 0);
            short[] array2 = new short[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array2[i] = (short)(array[i] * 32767f);
            }
            short[] array3;
            if (this.audioClip.channels == 1)
            {
                array3 = new short[array.Length * 2];
                for (int j = 0; j < array.Length; j++)
                {
                    array3[j * 2] = array2[j];
                    array3[j * 2 + 1] = array2[j];
                }
            }
            else
            {
                array3 = array2;
            }
            byte[] array4 = new byte[array3.Length * 2];
            Buffer.BlockCopy(array3, 0, array4, 0, array4.Length);
            this.id = this.tslib.loaddata(array4, (uint)array4.Length, (uint)this.audioClip.frequency, (uint)this.bus);
            if (this.id == 4294967295U)
            {
                Debug.LogError("Failed to load sound; TempoSound component of " + base.name + " will be disabled");
                base.enabled = false;
            }
            this.UpdateEditorValues();
        }

        private void OnDestroy()
        {
            this.tslib.unload(this.id);
        }

        public bool IsPlaying
        {
            get
            {
                return this.tslib.isplaying(this.id);
            }
        }

        public double Position
        {
            get
            {
                return this.tslib.getposition(this.id);
            }
            set
            {
                this.tslib.setposition(this.id, value);
            }
        }

        public bool IsScheduled
        {
            get
            {
                return this.tslib.isscheduled(this.id);
            }
        }

        public float Gain
        {
            get
            {
                return this.tslib.getgain(this.id);
            }
            set
            {
                this.tslib.setgain(this.id, value);
            }
        }

        public float Volume
        {
            get
            {
                return this.tslib.getvolume(this.id);
            }
            set
            {
                this.tslib.setvolume(this.id, value);
            }
        }

        public float Fade
        {
            get
            {
                return this.tslib.getfade(this.id);
            }
            set
            {
                this.tslib.setfade(this.id, value);
            }
        }

        public float Pan
        {
            get
            {
                return this.tslib.getpan(this.id);
            }
            set
            {
                this.tslib.setpan(this.id, value);
            }
        }

        public float Speed
        {
            get
            {
                return this.tslib.getspeed(this.id);
            }
            set
            {
                this.tslib.setspeed(this.id, value);
            }
        }

        public bool Loop
        {
            get
            {
                return this.tslib.getloop(this.id);
            }
            set
            {
                this.tslib.setloop(this.id, value);
            }
        }

        public bool DuckMusic
        {
            get
            {
                return this.tslib.getduckmusic(this.id);
            }
            set
            {
                this.tslib.setduckmusic(this.id, value);
            }
        }

        public void Play()
        {
            this.tslib.play(this.id);
        }

        public void PlayFrom(float seconds)
        {
            this.tslib.playfrom(this.id, (double)seconds);
        }

        public void Stop()
        {
            this.tslib.stop(this.id);
        }

        public void Schedule(TempoSound sound, double seconds)
        {
            this.tslib.schedulesound(this.id, sound.id, seconds);
        }

        public void Schedule(double seconds)
        {
            this.tslib.scheduleclock(this.id, seconds);
        }

        public void Unschedule()
        {
            this.tslib.unschedule(this.id);
        }

        private void UpdateEditorValues()
        {
            if (this.gain != this.currentGain)
            {
                this.Gain = this.gain;
            }
            if (this.volume != this.currentVolume)
            {
                this.Volume = this.volume;
            }
            if (this.fade != this.currentFade)
            {
                this.Fade = this.fade;
            }
            if (this.pan != this.currentPan)
            {
                this.Pan = this.pan;
            }
            if (this.speed != this.currentSpeed)
            {
                this.Speed = this.speed;
            }
            if (this.playing != this.currentPlaying)
            {
                if (this.playing)
                {
                    this.Play();
                }
                else
                {
                    this.Stop();
                }
            }
            if (this.loop != this.currentLoop)
            {
                this.Loop = this.loop;
            }
            if (this.duckMusic != this.currentDuckMusic)
            {
                this.DuckMusic = this.duckMusic;
            }
            this.gain = (this.currentGain = this.Gain);
            this.volume = (this.currentVolume = this.Volume);
            this.fade = (this.currentFade = this.Fade);
            this.pan = (this.currentPan = this.Pan);
            this.speed = (this.currentSpeed = this.Speed);
            this.playing = (this.currentPlaying = this.IsPlaying);
            this.loop = (this.currentLoop = this.Loop);
            this.duckMusic = (this.currentDuckMusic = this.DuckMusic);
        }

        public AudioClip audioClip;

        [Tooltip("Bus the sound will play on (changes ignored after init)")]
        public Bus bus;

        [SerializeField]
        [Tooltip("Gain in dB (unrestricted; linear volume scale [0, 1] corresponds to [-Infinity, 0])")]
        private float gain;

        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("Linear volume scale (deprecated; range corresponds to gain vaules [-Infinity, 0])")]
        private float volume = 1f;

        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("Linear fade")]
        private float fade = 1f;

        [SerializeField]
        [Range(-1f, 1f)]
        [Tooltip("Left/right panning (-1 for 100% left, 1 for 100% right)")]
        private float pan;

        [SerializeField]
        [Tooltip("Playback speed (altering both tempo and pitch)")]
        private float speed = 1f;

        [SerializeField]
        [Tooltip("Whether the sound is currently playing (toggle to force play/stop)")]
        private bool playing;

        [SerializeField]
        [Tooltip("Whether the sound should loop immediately after playing to the end")]
        private bool loop;

        [SerializeField]
        [Tooltip("Whether to duck the music bus when this sound plays")]
        private bool duckMusic;

        public const uint INVALID_ID = 4294967295U;

        private uint id = uint.MaxValue;

        private float currentGain;

        private float currentVolume = 1f;

        private float currentFade = 1f;

        private float currentPan;

        private float currentSpeed = 1f;

        private bool currentPlaying;

        private bool currentLoop;

        private bool currentDuckMusic;
    }
}
