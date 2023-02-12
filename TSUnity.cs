using System;
using System.Collections.Generic;
using UnityEngine;

namespace TempoStudio
{
    [DefaultExecutionOrder(-997)]
    public class TSUnity : ITSLib
    {
        public bool InitAudio(TSSettings tsSettings)
        {
            TSUnity.Settings unitySettings = tsSettings.unitySettings;
            if (this.isInit)
            {
                Debug.LogError("Audio is already initialized!");
                return false;
            }
            if (TSUnity.rootObject == null)
            {
                TSUnity.rootObject = new GameObject("TSUnity");
                UnityEngine.Object.DontDestroyOnLoad(TSUnity.rootObject);
            }
            AudioConfiguration configuration = AudioSettings.GetConfiguration();
            if (TSUnity.defaultBufferSize == -1)
            {
                TSUnity.defaultBufferSize = configuration.dspBufferSize;
            }
            int dspBufferSize;
            if (!new Dictionary<TSUnity.DSPBufferSize, int>
            {
                {
                    TSUnity.DSPBufferSize.SystemDefault,
                    TSUnity.defaultBufferSize
                },
                {
                    TSUnity.DSPBufferSize.BestPerformance,
                    1024
                },
                {
                    TSUnity.DSPBufferSize.GoodLatency,
                    512
                },
                {
                    TSUnity.DSPBufferSize.BestLatency,
                    256
                }
            }.TryGetValue(unitySettings.dspBufferSize, out dspBufferSize))
            {
                Debug.LogError(string.Format("Invalid dspBufferSizeSetting: {0}", unitySettings.dspBufferSize));
                dspBufferSize = TSUnity.defaultBufferSize;
            }
            configuration.dspBufferSize = dspBufferSize;
            AudioSettings.Reset(configuration);
            TSUnity.clockStart = 0.0;
            TSUnity.clockStop = 0.0;
            TSUnity.clockRunning = false;
            this.isInit = true;
            return true;
        }

        public bool ReinitAudio(TSSettings tsSettings)
        {
            Dictionary<uint, ValueTuple<float[], bool, double>> dictionary = new Dictionary<uint, ValueTuple<float[], bool, double>>();
            foreach (KeyValuePair<uint, TSUnity.Sound> keyValuePair in TSUnity.sounds)
            {
                uint num;
                TSUnity.Sound sound;

                num = keyValuePair.Key;
                sound = keyValuePair.Value;

                uint num2 = num;
                AudioClip clip = sound.Clip;
                float[] array = new float[clip.samples * clip.channels];
                clip.GetData(array, 0);
                dictionary[num2] = new ValueTuple<float[], bool, double>(array, this.isplaying(num2), this.getposition(num2));
            }
            this.freeaudio();
            if (!this.InitAudio(tsSettings))
            {
                return false;
            }
            foreach (KeyValuePair<uint, TSUnity.Sound> keyValuePair in TSUnity.sounds)
            {
                uint num;
                TSUnity.Sound sound;

                num = keyValuePair.Key;
                sound = keyValuePair.Value;

                uint num3 = num;
                TSUnity.Sound sound2 = sound;
                ValueTuple<float[], bool, double> valueTuple = dictionary[num3];
                float[] item = valueTuple.Item1;
                bool item2 = valueTuple.Item2;
                double item3 = valueTuple.Item3;
                int num4 = 2;
                sound2.Clip = AudioClip.Create(string.Format("TempoSound {0}", num3), item.Length / num4, num4, 48000, false);
                sound2.Clip.SetData(item, 0);
                if (item2)
                {
                    this.playfrom(num3, item3);
                }
            }
            return true;
        }

        public uint getversion()
        {
            return 0U;
        }

        public void initinput(bool hasFocus, TempoStudio.ButtonCallbackDelegate onButtonDown, TempoStudio.ButtonCallbackDelegate onButtonUp)
        {
            TSUnity.vkeysDown = new Dictionary<uint, bool>();
            this.onButtonDown = onButtonDown;
            this.onButtonUp = onButtonUp;
        }

        public bool enumaudiodevices(ITSLib.DeviceCallback deviceCallback)
        {
            return false;
        }

        public bool initaudio(uint frameRate, bool exclusive, float bufferMs, float extraMs, bool useSleep, bool useMmcss, ref float exclusiveMs, ref float sharedMs, string deviceId)
        {
            return false;
        }

        public void onfocus(bool hasFocus)
        {
        }

        public void freeinput()
        {
            TSUnity.vkeysDown = null;
        }

        public void freeaudio()
        {
            this.isInit = false;
        }

        public float getglobalvolume()
        {
            return AudioListener.volume;
        }

        public void setglobalvolume(float volume)
        {
            AudioListener.volume = Mathf.Clamp(volume, 0f, 1f);
        }

        public bool pauseaudio()
        {
            return true;
        }

        public void unpauseaudio()
        {
        }

        public ulong getmaxsample()
        {
            return 0UL;
        }

        public float getlimiterscale()
        {
            return 0f;
        }

        public float getlimiterthreshold()
        {
            return this.limiterThreshold;
        }

        public void setlimiterthreshold(float limiterThresholdDbfs)
        {
            this.limiterThreshold = limiterThresholdDbfs;
        }

        public float getlimiterrelease()
        {
            return this.limiterRelease;
        }

        public void setlimiterrelease(float limiterReleaseMs)
        {
            this.limiterRelease = limiterReleaseMs;
        }

        public void tick()
        {
            this.Update();
        }

        public void seterrorfunc(TempoStudio.LoggerFunc loggerFunc, bool duplicate)
        {
        }

        public void setacceptmouseinput(bool acceptMouseInput)
        {
        }

        public void startclock()
        {
            TSUnity.clockStart = AudioSettings.dspTime;
            TSUnity.clockRunning = true;
        }

        public bool clockrunning()
        {
            return TSUnity.clockRunning;
        }

        public double getclock()
        {
            if (!TSUnity.clockRunning)
            {
                return TSUnity.clockStop;
            }
            return AudioSettings.dspTime - TSUnity.clockStart;
        }

        public void stopclock()
        {
            TSUnity.clockStop = this.getclock();
            TSUnity.clockRunning = false;
        }

        public float getduckvolume()
        {
            return this.duckVolume;
        }

        public void setduckvolume(float duckVolumeDb)
        {
            this.duckVolume = duckVolumeDb;
        }

        public float getduckhold()
        {
            return this.duckHold;
        }

        public void setduckhold(float duckHoldMs)
        {
            this.duckHold = duckHoldMs;
        }

        public float getduckrelease()
        {
            return this.duckRelease;
        }

        public void setduckrelease(float duckReleaseMs)
        {
            this.duckRelease = duckReleaseMs;
        }

        public void play(uint id)
        {
            this.playfrom(id, 0.0);
        }

        public void playfrom(uint id, double seconds)
        {
            TSUnity.sounds[id].audioSource.time = (float)seconds;
            TSUnity.sounds[id].audioSource.Play();
        }

        public void stop(uint id)
        {
            TSUnity.sounds[id].audioSource.Stop();
        }

        public double getposition(uint id)
        {
            TSUnity.Sound sound = TSUnity.sounds[id];
            return (double)(TSUnity.sounds[id].audioSource.time / sound.Speed);
        }

        public void setposition(uint id, double seconds)
        {
            TSUnity.Sound sound = TSUnity.sounds[id];
            sound.audioSource.time = (float)(seconds * (double)sound.Speed);
        }

        public bool isplaying(uint id)
        {
            return TSUnity.sounds[id].audioSource.isPlaying;
        }

        public uint loadfile(string filename, uint bus)
        {
            return uint.MaxValue;
        }

        public uint loaddata(byte[] data, uint size, uint frequency, uint bus)
        {
            int num = 2;
            int num2 = data.Length / 2;
            AudioClip audioClip = AudioClip.Create(string.Format("TempoSound {0}", TSUnity.nextId), num2 / num, num, (int)frequency, false);
            short[] array = new short[num2];
            Buffer.BlockCopy(data, 0, array, 0, data.Length);
            float[] array2 = new float[num2];
            for (int i = 0; i < num2; i++)
            {
                array2[i] = (float)array[i] / 32767f;
            }
            audioClip.SetData(array2, 0);
            GameObject gameObject = new GameObject(string.Format("TempoSound {0}", TSUnity.nextId));
            gameObject.transform.parent = TSUnity.rootObject.transform;
            TSUnity.sounds[TSUnity.nextId] = new TSUnity.Sound(gameObject, audioClip);
            return TSUnity.nextId++;
        }

        public void unload(uint id)
        {
            UnityEngine.Object.Destroy(TSUnity.sounds[id].gameObject);
            TSUnity.sounds.Remove(id);
        }

        public void schedulesound(uint soundId, uint referenceId, double seconds)
        {
            TSUnity.Sound sound = TSUnity.sounds[soundId];
            if (sound.referenceId != referenceId)
            {
                sound.referenceId = referenceId;
                sound.scheduled.Clear();
            }
            sound.scheduled.Add(seconds, null);
        }

        public void scheduleclock(uint soundId, double seconds)
        {
            this.schedulesound(soundId, uint.MaxValue, seconds);
        }

        public void unschedule(uint soundId)
        {
            TSUnity.Sound sound = TSUnity.sounds[soundId];
            sound.scheduled.Clear();
            sound.referenceId = uint.MaxValue;
            sound.currentSchedule = null;
            sound.scheduledAudioSource.Stop();
        }

        public bool isscheduled(uint id)
        {
            return TSUnity.sounds[id].currentSchedule != null;
        }

        public float getgain(uint id)
        {
            return TSUnity.Sound.LinearToDecibels(TSUnity.sounds[id].gain);
        }

        public void setgain(uint id, float gainDb)
        {
            TSUnity.sounds[id].gain = TSUnity.Sound.DecibelsToLinear((double)gainDb);
            TSUnity.sounds[id].SetVolume();
        }

        public float getvolume(uint id)
        {
            return TSUnity.sounds[id].volume;
        }

        public void setvolume(uint id, float volume)
        {
            TSUnity.sounds[id].volume = Mathf.Clamp01(volume);
            TSUnity.sounds[id].SetVolume();
        }

        public float getfade(uint id)
        {
            return TSUnity.sounds[id].fade;
        }

        public void setfade(uint id, float fade)
        {
            TSUnity.sounds[id].fade = Mathf.Clamp01(fade);
            TSUnity.sounds[id].SetVolume();
        }

        public float getpan(uint id)
        {
            return TSUnity.sounds[id].Pan;
        }

        public void setpan(uint id, float pan)
        {
            TSUnity.sounds[id].Pan = pan;
        }

        public float getspeed(uint id)
        {
            return TSUnity.sounds[id].Speed;
        }

        public void setspeed(uint id, float speed)
        {
            TSUnity.sounds[id].Speed = speed;
        }

        public bool getloop(uint id)
        {
            return TSUnity.sounds[id].Loop;
        }

        public void setloop(uint id, bool loop)
        {
            TSUnity.sounds[id].Loop = loop;
        }

        public bool getduckmusic(uint id)
        {
            return TSUnity.sounds[id].duckMusic;
        }

        public void setduckmusic(uint id, bool duckMusic)
        {
            TSUnity.sounds[id].duckMusic = duckMusic;
        }

        private void Update()
        {
            foreach (TSUnity.Sound sound in TSUnity.sounds.Values)
            {
                if (sound.currentSchedule != null || sound.scheduled.Count != 0)
                {
                    double num;
                    double num2;
                    if (sound.referenceId != 4294967295U)
                    {
                        if (!this.isplaying(sound.referenceId))
                        {
                            continue;
                        }
                        num = this.getposition(sound.referenceId);
                        num2 = AudioSettings.dspTime - num;
                    }
                    else
                    {
                        if (!TSUnity.clockRunning)
                        {
                            continue;
                        }
                        num = this.getclock();
                        num2 = TSUnity.clockStart;
                    }
                    if (sound.currentSchedule != null)
                    {
                        double num3 = num;
                        double? currentSchedule = sound.currentSchedule;
                        if (!(num3 >= currentSchedule.GetValueOrDefault() & currentSchedule != null))
                        {
                            continue;
                        }
                    }
                    this.Swap<AudioSource>(ref sound.audioSource, ref sound.scheduledAudioSource);
                    if (sound.scheduled.Count == 0)
                    {
                        sound.currentSchedule = null;
                    }
                    else
                    {
                        double num4 = sound.scheduled.Keys[0];
                        sound.currentSchedule = new double?(num4);
                        sound.scheduled.RemoveAt(0);
                        sound.scheduledAudioSource.PlayScheduled(num2 + num4);
                        if (sound.scheduled.Count != 0)
                        {
                            sound.scheduledAudioSource.SetScheduledEndTime(num2 + sound.scheduled.Keys[0]);
                        }
                    }
                }
            }
            foreach (KeyValuePair<uint, KeyCode> keyValuePair in TSUnity.vkeyToKeyCode)
            {
                uint num5;
                KeyCode keyCode;

                num5 = keyValuePair.Key;
                keyCode = keyValuePair.Value;

                uint vkCode = num5;
                KeyCode key = keyCode;
                if (Input.GetKeyDown(key))
                {
                    this.onButtonDown(vkCode);
                }
                if (Input.GetKeyUp(key))
                {
                    this.onButtonUp(vkCode);
                }
            }
            foreach (KeyValuePair<string, ValueTuple<uint, uint>> keyValuePair2 in TSUnity.axisToVkey)
            {
                string text;
                ValueTuple<uint, uint> valueTuple;

                text = keyValuePair2.Key;
                valueTuple = keyValuePair2.Value;

                ValueTuple<uint, uint> valueTuple2 = valueTuple;
                string axisName = text;
                uint item = valueTuple2.Item1;
                uint item2 = valueTuple2.Item2;
                float axisRaw = Input.GetAxisRaw(axisName);
                if (axisRaw > 0.5f && !TSUnity.vkeysDown.GetValueOrDefault(item))
                {
                    this.onButtonDown(item);
                    TSUnity.vkeysDown[item] = true;
                }
                if (axisRaw < 0.25f)
                {
                    TSUnity.vkeysDown[item] = false;
                }
                if (axisRaw < -0.5f && !TSUnity.vkeysDown.GetValueOrDefault(item2))
                {
                    this.onButtonDown(item2);
                    TSUnity.vkeysDown[item2] = true;
                }
                if (axisRaw > -0.25f)
                {
                    TSUnity.vkeysDown[item2] = false;
                }
            }
        }

        private void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        public TSUnity()
        {
        }

        // Note: this type is marked as 'beforefieldinit'.
        static TSUnity()
        {
        }

        private static uint nextId = 0U;

        private static Dictionary<uint, TSUnity.Sound> sounds = new Dictionary<uint, TSUnity.Sound>();

        private static double clockStart;

        private static double clockStop;

        private static bool clockRunning;

        private static GameObject rootObject;

        private bool isInit;

        private float limiterThreshold;

        private float limiterRelease;

        private float duckVolume;

        private float duckHold;

        private float duckRelease;

        private TempoStudio.ButtonCallbackDelegate onButtonDown;

        private TempoStudio.ButtonCallbackDelegate onButtonUp;

        private static Dictionary<uint, KeyCode> vkeyToKeyCode = new Dictionary<uint, KeyCode>
        {
            {
                13U,
                KeyCode.Return
            },
            {
                27U,
                KeyCode.Escape
            },
            {
                32U,
                KeyCode.Space
            },
            {
                37U,
                KeyCode.LeftArrow
            },
            {
                38U,
                KeyCode.UpArrow
            },
            {
                39U,
                KeyCode.RightArrow
            },
            {
                40U,
                KeyCode.DownArrow
            },
            {
                160U,
                KeyCode.LeftShift
            },
            {
                161U,
                KeyCode.RightShift
            },
            {
                190U,
                KeyCode.Period
            },
            {
                191U,
                KeyCode.Slash
            },
            {
                1U,
                KeyCode.Mouse0
            },
            {
                2U,
                KeyCode.Mouse1
            },
            {
                195U,
                KeyCode.JoystickButton0
            },
            {
                196U,
                KeyCode.JoystickButton1
            },
            {
                197U,
                KeyCode.JoystickButton2
            },
            {
                198U,
                KeyCode.JoystickButton3
            },
            {
                200U,
                KeyCode.JoystickButton4
            },
            {
                199U,
                KeyCode.JoystickButton5
            },
            {
                208U,
                KeyCode.JoystickButton6
            },
            {
                207U,
                KeyCode.JoystickButton7
            },
            {
                209U,
                KeyCode.JoystickButton8
            },
            {
                210U,
                KeyCode.JoystickButton9
            }
        };

        private static Dictionary<string, ValueTuple<uint, uint>> axisToVkey = new Dictionary<string, ValueTuple<uint, uint>>
        {
            {
                "JoyAxis6",
                new ValueTuple<uint, uint>(206U, 205U)
            },
            {
                "JoyAxis7",
                new ValueTuple<uint, uint>(203U, 204U)
            },
            {
                "JoyAxis1",
                new ValueTuple<uint, uint>(213U, 214U)
            },
            {
                "JoyAxis2",
                new ValueTuple<uint, uint>(212U, 211U)
            },
            {
                "JoyAxis4",
                new ValueTuple<uint, uint>(217U, 218U)
            },
            {
                "JoyAxis5",
                new ValueTuple<uint, uint>(216U, 215U)
            }
        };

        private const float activate = 0.5f;

        private const float deactivate = 0.25f;

        private static Dictionary<uint, bool> vkeysDown;

        private static int defaultBufferSize = -1;

        private class Sound
        {
            public Sound(GameObject gameObject, AudioClip audioClip)
            {
                this.gameObject = gameObject;
                this.audioSource = gameObject.AddComponent<AudioSource>();
                this.scheduledAudioSource = gameObject.AddComponent<AudioSource>();
                this.Clip = audioClip;
                this.scheduled = new SortedList<double, TempoSound>();
                this.referenceId = uint.MaxValue;
                this.gain = 1f;
                this.volume = 1f;
                this.fade = 1f;
                this.duckMusic = false;
                this.currentSchedule = null;
            }

            public AudioClip Clip
            {
                get
                {
                    return this.audioSource.clip;
                }
                set
                {
                    AudioSource audioSource = this.audioSource;
                    this.scheduledAudioSource.clip = value;
                    audioSource.clip = value;
                }
            }

            public float Volume
            {
                get
                {
                    return this.volume;
                }
                set
                {
                    this.volume = Mathf.Clamp01(value);
                    this.SetVolume();
                }
            }

            public float Fade
            {
                get
                {
                    return this.fade;
                }
                set
                {
                    this.fade = Mathf.Clamp01(value);
                    this.SetVolume();
                }
            }

            public float Pan
            {
                get
                {
                    return this.audioSource.panStereo;
                }
                set
                {
                    AudioSource audioSource = this.audioSource;
                    this.scheduledAudioSource.panStereo = value;
                    audioSource.panStereo = value;
                }
            }

            public float Speed
            {
                get
                {
                    return this.audioSource.pitch;
                }
                set
                {
                    AudioSource audioSource = this.audioSource;
                    this.scheduledAudioSource.pitch = value;
                    audioSource.pitch = value;
                }
            }

            public bool Loop
            {
                get
                {
                    return this.audioSource.loop;
                }
                set
                {
                    AudioSource audioSource = this.audioSource;
                    this.scheduledAudioSource.loop = value;
                    audioSource.loop = value;
                }
            }

            public void SetVolume()
            {
                this.audioSource.volume = (this.scheduledAudioSource.volume = this.gain * this.volume * this.fade);
            }

            public static float LinearToDecibels(float linear)
            {
                return (float)((double)Mathf.Log(linear) * 8.685889638065037);
            }

            public static float DecibelsToLinear(double db)
            {
                return Mathf.Exp((float)(db * 0.11512925464970228));
            }

            public GameObject gameObject;

            public AudioSource audioSource;

            public AudioSource scheduledAudioSource;

            public SortedList<double, TempoSound> scheduled;

            public uint referenceId;

            public float gain;

            public float volume;

            public float fade;

            public bool duckMusic;

            public double? currentSchedule;

            private const double LOG_2_DB = 8.685889638065037;

            private const double DB_2_LOG = 0.11512925464970228;
        }

        public enum DSPBufferSize
        {
            SystemDefault,
            BestPerformance,
            GoodLatency,
            BestLatency,
            Count
        }

        public class Settings
        {
            public TSUnity.DSPBufferSize dspBufferSize;
        }
    }

}
