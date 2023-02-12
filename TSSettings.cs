using System;

namespace TempoStudio
{
    public class TSSettings
    {
        public float volume = 1f;

        public TSUnity.Settings unitySettings = new TSUnity.Settings();

        public TSWindows.Settings windowsSettings = new TSWindows.Settings();
    }
}
