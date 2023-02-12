using System.Runtime.InteropServices;

namespace TempoStudio
{
    public interface ITSLib
    {
        bool InitAudio(TSSettings settings);

        bool ReinitAudio(TSSettings settings);

        uint getversion();

        bool enumaudiodevices(ITSLib.DeviceCallback deviceCallback);

        void initinput(bool hasFocus, TempoStudio.ButtonCallbackDelegate onButtonDown, TempoStudio.ButtonCallbackDelegate onButtonUp);

        bool initaudio(uint frameRate, bool exclusive, float bufferMs, float extraMs, bool useSleep, bool useMmcss, ref float exclusiveMs, ref float sharedMs, string deviceId);

        void onfocus(bool hasFocus);

        void freeinput();

        void freeaudio();

        float getglobalvolume();

        void setglobalvolume(float volume);

        bool pauseaudio();

        void unpauseaudio();

        ulong getmaxsample();

        float getlimiterscale();

        float getlimiterthreshold();

        void setlimiterthreshold(float limiterThresholdDbfs);

        float getlimiterrelease();

        void setlimiterrelease(float limiterReleaseMs);

        void tick();

        void seterrorfunc(TempoStudio.LoggerFunc loggerFunc, bool duplicate);

        void setacceptmouseinput(bool acceptMouseInput);

        void startclock();

        bool clockrunning();

        double getclock();

        void stopclock();

        float getduckvolume();

        void setduckvolume(float duckVolumeDb);

        float getduckhold();

        void setduckhold(float duckHoldMs);

        float getduckrelease();

        void setduckrelease(float duckReleaseMs);

        void play(uint id);

        void playfrom(uint id, double seconds);

        void stop(uint id);

        double getposition(uint id);

        void setposition(uint id, double seconds);

        bool isplaying(uint id);

        uint loadfile(string filename, uint bus);

        uint loaddata(byte[] data, uint size, uint frequency, uint bus);

        void unload(uint id);

        void schedulesound(uint soundId, uint referenceId, double seconds);

        void scheduleclock(uint soundId, double seconds);

        void unschedule(uint soundId);

        bool isscheduled(uint id);

        float getgain(uint id);

        void setgain(uint id, float gainDb);

        float getvolume(uint id);

        void setvolume(uint id, float volume);

        float getfade(uint id);

        void setfade(uint id, float fade);

        float getpan(uint id);

        void setpan(uint id, float pan);

        float getspeed(uint id);

        void setspeed(uint id, float speed);

        bool getloop(uint id);

        void setloop(uint id, bool loop);

        bool getduckmusic(uint id);

        void setduckmusic(uint id, bool duckMusic);

        public delegate void DeviceCallback([MarshalAs(UnmanagedType.LPWStr)] string deviceId, [MarshalAs(UnmanagedType.LPWStr)] string deviceName);
    }
}