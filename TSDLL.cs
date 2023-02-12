using System;
using System.Runtime.InteropServices;
using TempoStudio;

namespace TempoStudio
{
    public static class TSDLL
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr LoadLibrary(string libname);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern uint getversion();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern bool enumaudiodevices(ITSLib.DeviceCallback deviceCallback);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void initinput(bool hasFocus, TempoStudio.ButtonCallbackDelegate onButtonDown, TempoStudio.ButtonCallbackDelegate onButtonUp);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern bool initaudio(uint frameRate, bool exclusive, float bufferMs, float extraMs, bool useSleep, bool useMmcss, ref float exclusiveMs, ref float sharedMs, string deviceId);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void onfocus(bool hasFocus);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void freeinput();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void freeaudio();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern float getglobalvolume();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setglobalvolume(float volume);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern bool pauseaudio();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void unpauseaudio();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern ulong getmaxsample();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern float getlimiterscale();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern float getlimiterthreshold();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setlimiterthreshold(float limiterThresholdDbfs);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern float getlimiterrelease();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setlimiterrelease(float limiterReleaseMs);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void tick();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void seterrorfunc(TempoStudio.LoggerFunc loggerFunc, bool duplicate);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setacceptmouseinput(bool acceptMouseInput);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void startclock();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern bool clockrunning();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern double getclock();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void stopclock();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern float getduckvolume();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setduckvolume(float duckVolumeDb);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern float getduckhold();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setduckhold(float duckHoldMs);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern float getduckrelease();

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setduckrelease(float duckReleaseMs);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void play(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void playfrom(uint id, double seconds);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void stop(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern double getposition(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setposition(uint id, double seconds);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern bool isplaying(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern uint loadfile(string filename, uint bus);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern uint loaddata(byte[] data, uint size, uint frequency, uint bus);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void unload(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void schedulesound(uint soundId, uint referenceId, double seconds);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void scheduleclock(uint soundId, double seconds);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void unschedule(uint soundId);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern bool isscheduled(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern float getgain(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setgain(uint id, float gainDb);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern float getvolume(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setvolume(uint id, float volume);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern float getfade(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setfade(uint id, float fade);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern float getpan(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setpan(uint id, float pan);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern float getspeed(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setspeed(uint id, float speed);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern bool getloop(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setloop(uint id, bool loop);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern bool getduckmusic(uint id);

        [DllImport("TempoStudio", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern void setduckmusic(uint id, bool duckMusic);

        private static IntPtr Handle;
    }
}
