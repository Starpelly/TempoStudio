using System;
using System.Runtime.CompilerServices;

namespace TempoStudio
{
    public class TSWindows : ITSLib
    {
        public bool InitAudio(TSSettings tsSettings)
        {
            uint num = TempoStudio.tslib.getversion();
            uint num2 = num >> 16;
            uint num3 = num & 65535U;
            uint num4 = num2;
            uint num5 = num3;
            ValueTuple<uint, uint> version = TSWindows.VERSION;
            if (num4 != version.Item1 || num5 != version.Item2)
            {
                throw new Exception(string.Format("Invalid DLL version {0}.{1} (expected {2}.{3})", new object[]
                {
                    num2,
                    num3,
                    TSWindows.VERSION.Item1,
                    TSWindows.VERSION.Item2
                }));
            }
            TSWindows.Settings windowsSettings = tsSettings.windowsSettings;
            float exclusiveBufferMs = 0f;
            float sharedBufferMs = 0f;
            if (!this.initaudio(windowsSettings.frequency, windowsSettings.exclusive, windowsSettings.exclusive ? windowsSettings.exclusiveBufferMs : windowsSettings.sharedBufferMs, windowsSettings.extraMs, windowsSettings.useSleep, windowsSettings.useMmcss, ref exclusiveBufferMs, ref sharedBufferMs, windowsSettings.deviceId))
            {
                return false;
            }
            if (windowsSettings.exclusiveBufferMs == 0f)
            {
                windowsSettings.exclusiveBufferMs = exclusiveBufferMs;
            }
            if (windowsSettings.sharedBufferMs == 0f)
            {
                windowsSettings.sharedBufferMs = sharedBufferMs;
            }
            return true;
        }

        public bool ReinitAudio(TSSettings settings)
        {
            this.freeaudio();
            return this.InitAudio(settings);
        }

        public uint getversion()
        {
            return TSDLL.getversion();
        }

        public bool enumaudiodevices(ITSLib.DeviceCallback deviceCallback)
        {
            return TSDLL.enumaudiodevices(deviceCallback);
        }

        public void initinput(bool hasFocus, TempoStudio.ButtonCallbackDelegate onButtonDown, TempoStudio.ButtonCallbackDelegate onButtonUp)
        {
            TSDLL.initinput(hasFocus, onButtonDown, onButtonUp);
        }

        public bool initaudio(uint frameRate, bool exclusive, float bufferMs, float extraMs, bool useSleep, bool useMmcss, ref float exclusiveMs, ref float sharedMs, string deviceId)
        {
            return TSDLL.initaudio(frameRate, exclusive, bufferMs, extraMs, useSleep, useMmcss, ref exclusiveMs, ref sharedMs, deviceId);
        }

        public void onfocus(bool hasFocus)
        {
            TSDLL.onfocus(hasFocus);
        }

        public void freeinput()
        {
            TSDLL.freeinput();
        }

        public void freeaudio()
        {
            TSDLL.freeaudio();
        }

        public float getglobalvolume()
        {
            return TSDLL.getglobalvolume();
        }

        public void setglobalvolume(float volume)
        {
            TSDLL.setglobalvolume(volume);
        }

        public bool pauseaudio()
        {
            return TSDLL.pauseaudio();
        }

        public void unpauseaudio()
        {
            TSDLL.unpauseaudio();
        }

        public ulong getmaxsample()
        {
            return TSDLL.getmaxsample();
        }

        public float getlimiterscale()
        {
            return TSDLL.getlimiterscale();
        }

        public float getlimiterthreshold()
        {
            return TSDLL.getlimiterthreshold();
        }

        public void setlimiterthreshold(float limiterThresholdDbfs)
        {
            TSDLL.setlimiterthreshold(limiterThresholdDbfs);
        }

        public float getlimiterrelease()
        {
            return TSDLL.getlimiterrelease();
        }

        public void setlimiterrelease(float limiterReleaseMs)
        {
            TSDLL.setlimiterrelease(limiterReleaseMs);
        }

        public void tick()
        {
            TSDLL.tick();
        }

        public void seterrorfunc(TempoStudio.LoggerFunc loggerFunc, bool duplicate)
        {
            TSDLL.seterrorfunc(loggerFunc, duplicate);
        }

        public void setacceptmouseinput(bool acceptMouseInput)
        {
            TSDLL.setacceptmouseinput(acceptMouseInput);
        }

        public void startclock()
        {
            TSDLL.startclock();
        }

        public bool clockrunning()
        {
            return TSDLL.clockrunning();
        }

        public double getclock()
        {
            return TSDLL.getclock();
        }

        public void stopclock()
        {
            TSDLL.stopclock();
        }

        public float getduckvolume()
        {
            return TSDLL.getduckvolume();
        }

        public void setduckvolume(float duckVolumeDb)
        {
            TSDLL.setduckvolume(duckVolumeDb);
        }

        public float getduckhold()
        {
            return TSDLL.getduckhold();
        }

        public void setduckhold(float duckHoldMs)
        {
            TSDLL.setduckhold(duckHoldMs);
        }

        public float getduckrelease()
        {
            return TSDLL.getduckrelease();
        }

        public void setduckrelease(float duckReleaseMs)
        {
            TSDLL.setduckrelease(duckReleaseMs);
        }

        public void play(uint id)
        {
            TSDLL.play(id);
        }

        public void playfrom(uint id, double seconds)
        {
            TSDLL.playfrom(id, seconds);
        }

        public void stop(uint id)
        {
            TSDLL.stop(id);
        }

        public double getposition(uint id)
        {
            return TSDLL.getposition(id);
        }

        public void setposition(uint id, double seconds)
        {
            TSDLL.setposition(id, seconds);
        }

        public bool isplaying(uint id)
        {
            return TSDLL.isplaying(id);
        }

        public uint loadfile(string filename, uint bus)
        {
            return TSDLL.loadfile(filename, bus);
        }

        public uint loaddata(byte[] data, uint size, uint frequency, uint bus)
        {
            return TSDLL.loaddata(data, size, frequency, bus);
        }

        public void unload(uint id)
        {
            TSDLL.unload(id);
        }

        public void schedulesound(uint soundId, uint referenceId, double seconds)
        {
            TSDLL.schedulesound(soundId, referenceId, seconds);
        }

        public void scheduleclock(uint soundId, double seconds)
        {
            TSDLL.scheduleclock(soundId, seconds);
        }

        public void unschedule(uint soundId)
        {
            TSDLL.unschedule(soundId);
        }

        public bool isscheduled(uint id)
        {
            return TSDLL.isscheduled(id);
        }

        public float getgain(uint id)
        {
            return TSDLL.getgain(id);
        }

        public void setgain(uint id, float gainDb)
        {
            TSDLL.setgain(id, gainDb);
        }

        public float getvolume(uint id)
        {
            return TSDLL.getvolume(id);
        }

        public void setvolume(uint id, float volume)
        {
            TSDLL.setvolume(id, volume);
        }

        public float getfade(uint id)
        {
            return TSDLL.getfade(id);
        }

        public void setfade(uint id, float fade)
        {
            TSDLL.setfade(id, fade);
        }

        public float getpan(uint id)
        {
            return TSDLL.getpan(id);
        }

        public void setpan(uint id, float pan)
        {
            TSDLL.setpan(id, pan);
        }

        public float getspeed(uint id)
        {
            return TSDLL.getspeed(id);
        }

        public void setspeed(uint id, float speed)
        {
            TSDLL.setspeed(id, speed);
        }

        public bool getloop(uint id)
        {
            return TSDLL.getloop(id);
        }

        public void setloop(uint id, bool loop)
        {
            TSDLL.setloop(id, loop);
        }

        public bool getduckmusic(uint id)
        {
            return TSDLL.getduckmusic(id);
        }

        public void setduckmusic(uint id, bool duckMusic)
        {
            TSDLL.setduckmusic(id, duckMusic);
        }

        public TSWindows()
        {
        }

        // Note: this type is marked as 'beforefieldinit'.
        static TSWindows()
        {
        }

        private static uint TEMPO_STUDIO_VERSION_MAJOR = 0U;

        private static uint TEMPO_STUDIO_VERSION_MINOR = 6U;

        /*[TupleElementNames(new string[]
        {
            "major",
            "minor"
        })]*/
        private static ValueTuple<uint, uint> VERSION = new ValueTuple<uint, uint>(TSWindows.TEMPO_STUDIO_VERSION_MAJOR, TSWindows.TEMPO_STUDIO_VERSION_MINOR);

        public class Settings
        {
            public Settings()
            {
            }

            public uint frequency;

            public bool exclusive;

            public float exclusiveBufferMs;

            public float sharedBufferMs;

            public float extraMs;

            public bool useSleep;

            public bool useMmcss;

            public string deviceId;
        }
    }
}
