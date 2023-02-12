using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using TempoStudio;
using UnityEngine;

namespace TempoStudio
{
    [DefaultExecutionOrder(-999)]
    public class TempoStudio : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void BeforeSceneLoad()
        {
            UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("TempoStudio")).name = "TempoStudio";
        }

        private void Awake()
        {
            if (TempoStudio.instance != null)
            {
                Debug.LogError("Only one instance of TempoStudio should be created");
                UnityEngine.Object.Destroy(this);
                return;
            }
            TempoStudio.instance = this;
            UnityEngine.Object.DontDestroyOnLoad(TempoStudio.instance);
            if (TempoStudio.implementation == TSImplementation.Default)
            {
                TempoStudio.implementation = TSImplementation.Windows;
            }
            TSImplementation tsimplementation = TempoStudio.implementation;
            if (tsimplementation != TSImplementation.Unity)
            {
                if (tsimplementation == TSImplementation.Windows)
                {
                    TempoStudio.tslib = new TSWindows();
                    goto IL_7E;
                }
                Debug.LogError(string.Format("Invalid implementation: {0}", TempoStudio.implementation));
            }
            TempoStudio.tslib = new TSUnity();
        IL_7E:
            TempoStudio.onButtonDown = new TempoStudio.ButtonCallbackDelegate(TempoStudio.OnButtonDown);
            TempoStudio.onButtonUp = new TempoStudio.ButtonCallbackDelegate(TempoStudio.OnButtonUp);
            TempoStudio.logError = new TempoStudio.LoggerFunc(TempoStudio.LogError);
            bool duplicate = true;
            TempoStudio.tslib.seterrorfunc(TempoStudio.logError, duplicate);
            TempoStudio.tslib.initinput(Application.isFocused, TempoStudio.onButtonDown, TempoStudio.onButtonUp);
            TempoStudio.InitAudio();
            this.LimiterThreshold = this.limiterThreshold;
            this.LimiterRelease = this.limiterRelease;
        }

        private static void LogError(string msg)
        {
            Debug.LogError(msg);
        }

        public float MaxSample
        {
            get
            {
                return TempoStudio.tslib.getmaxsample();
            }
        }

        public float LimiterScale
        {
            get
            {
                return TempoStudio.tslib.getlimiterscale();
            }
        }

        public float LimiterThreshold
        {
            get
            {
                return TempoStudio.tslib.getlimiterthreshold();
            }
            set
            {
                this.limiterThreshold = value;
                TempoStudio.tslib.setlimiterthreshold(value);
            }
        }

        public float LimiterThresholdLinear
        {
            get
            {
                return (float)Decibels.DecibelsToLinear((double)this.LimiterThreshold + Decibels.LinearToDecibels(32767.0));
            }
        }

        public float LimiterRelease
        {
            get
            {
                return TempoStudio.tslib.getlimiterrelease();
            }
            set
            {
                this.limiterRelease = value;
                TempoStudio.tslib.setlimiterrelease(value);
            }
        }

        public float DuckVolume
        {
            get
            {
                return TempoStudio.tslib.getduckvolume();
            }
            set
            {
                this.duckVolume = value;
                TempoStudio.tslib.setduckvolume(value);
            }
        }

        public float DuckHold
        {
            get
            {
                return TempoStudio.tslib.getduckhold();
            }
            set
            {
                this.duckHold = value;
                TempoStudio.tslib.setduckhold(value);
            }
        }

        public float DuckRelease
        {
            get
            {
                return TempoStudio.tslib.getduckrelease();
            }
            set
            {
                this.duckRelease = value;
                TempoStudio.tslib.setduckrelease(value);
            }
        }

        public static void InitAudio()
        {
            TempoStudio.audioOkay = TempoStudio.tslib.InitAudio(TempoStudio.settings);
            TempoStudio.Volume = TempoStudio.settings.volume;
            if (!TempoStudio.audioOkay)
            {
                Debug.LogError(string.Format("Failed to init audio [engine={0}]", TempoStudio.implementation));
                Debug.Log(JsonConvert.SerializeObject(TempoStudio.settings, Formatting.Indented));
            }
        }

        public static void ReinitAudio()
        {
            TempoStudio.audioOkay = TempoStudio.tslib.ReinitAudio(TempoStudio.settings);
            TempoStudio.Volume = TempoStudio.settings.volume;
            if (!TempoStudio.audioOkay)
            {
                Debug.LogError(string.Format("Failed to reinit audio [engine={0}]", TempoStudio.implementation));
                Debug.Log(JsonConvert.SerializeObject(TempoStudio.settings, Formatting.Indented));
            }
        }

        private static void OnButtonDown(uint vkCode)
        {
            TempoStudio.controller = (vkCode >= 192U && vkCode <= 223U);
            TempoStudio.mouse = (vkCode >= 1U && vkCode <= 2U);
            TempoStudio.keyEvents.Enqueue(new ValueTuple<uint, bool>(vkCode, true));
            TempoStudio.CallbackDelegate callbackDelegate;
            if (TempoStudio.onVkeyDown.TryGetValue(vkCode, out callbackDelegate))
            {
                callbackDelegate();
            }
            Action key;
            TempoStudio.CallbackDelegate callbackDelegate2;
            if (TempoStudio.vkeyToAction.TryGetValue(vkCode, out key) && TempoStudio.onActionDown.TryGetValue(key, out callbackDelegate2))
            {
                callbackDelegate2();
            }
        }

        private static void OnButtonUp(uint vkCode)
        {
            TempoStudio.keyEvents.Enqueue(new ValueTuple<uint, bool>(vkCode, false));
            TempoStudio.CallbackDelegate callbackDelegate;
            if (TempoStudio.onVkeyUp.TryGetValue(vkCode, out callbackDelegate))
            {
                callbackDelegate();
            }
            Action key;
            TempoStudio.CallbackDelegate callbackDelegate2;
            if (TempoStudio.vkeyToAction.TryGetValue(vkCode, out key) && TempoStudio.onActionUp.TryGetValue(key, out callbackDelegate2))
            {
                callbackDelegate2();
            }
        }

        public static void SetActionMap(Dictionary<uint, Action> actionMap)
        {
            TempoStudio.vkeyToAction = actionMap;
        }

        public static void SetButtonDownCallback(TempoStudio.CallbackDelegate callback, uint vkCode = 32U)
        {
            TempoStudio.onVkeyDown[vkCode] = callback;
        }

        public static void SetButtonUpCallback(TempoStudio.CallbackDelegate callback, uint vkCode = 32U)
        {
            TempoStudio.onVkeyUp[vkCode] = callback;
        }

        public static void SetActionDownCallback(TempoStudio.CallbackDelegate callback, Action action)
        {
            TempoStudio.onActionDown[action] = callback;
        }

        public static void SetActionUpCallback(TempoStudio.CallbackDelegate callback, Action action)
        {
            TempoStudio.onActionUp[action] = callback;
        }

        public static float Volume
        {
            get
            {
                return TempoStudio.tslib.getglobalvolume();
            }
            set
            {
                TempoStudio.settings.volume = value;
                if (TempoStudio.tslib != null)
                {
                    TempoStudio.tslib.setglobalvolume(value);
                }
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            TempoStudio.hasFocus = hasFocus;
            TempoStudio.tslib.onfocus(hasFocus && !TempoStudio.overlayOpen);
        }

        public static void OnOverlayChange(bool overlayOpen)
        {
            TempoStudio.overlayOpen = overlayOpen;
            TempoStudio.tslib.onfocus(TempoStudio.hasFocus && !overlayOpen);
        }

        private void OnDestroy()
        {
            if (TempoStudio.instance != this)
            {
                return;
            }
            TempoStudio.tslib.freeinput();
            TempoStudio.tslib.freeaudio();
            TempoStudio.instance = null;
        }

        private void Update()
        {
            TempoStudio.tslib.tick();
            TempoStudio.keysDown.Clear();
            TempoStudio.keysUp.Clear();
            TempoStudio.actionsDown.Clear();
            TempoStudio.actionsUp.Clear();
            ValueTuple<uint, bool> valueTuple;
            while (TempoStudio.keyEvents.TryDequeue(out valueTuple))
            {
                ValueTuple<uint, bool> valueTuple2 = valueTuple;
                uint item = valueTuple2.Item1;
                bool item2 = valueTuple2.Item2;
                (item2 ? TempoStudio.keysDown : TempoStudio.keysUp).Add(item);
                Action item3;
                if (TempoStudio.vkeyToAction.TryGetValue(item, out item3))
                {
                    (item2 ? TempoStudio.actionsDown : TempoStudio.actionsUp).Add(item3);
                }
            }
        }

        private void LateUpdate()
        {
            if (this.LimiterThreshold != this.limiterThreshold)
            {
                this.LimiterThreshold = this.limiterThreshold;
            }
            if (this.LimiterRelease != this.limiterRelease)
            {
                this.LimiterRelease = this.limiterRelease;
            }
            if (this.DuckVolume != this.duckVolume)
            {
                this.DuckVolume = this.duckVolume;
            }
            if (this.DuckHold != this.duckHold)
            {
                this.DuckHold = this.duckHold;
            }
            if (this.DuckRelease != this.duckRelease)
            {
                this.DuckRelease = this.duckRelease;
            }
        }

        public static bool GetKeyDown(uint vkey)
        {
            return TempoStudio.keysDown.Contains(vkey);
        }

        public static bool GetKeyUp(uint vkey)
        {
            return TempoStudio.keysUp.Contains(vkey);
        }

        public static bool GetActionDown(Action action)
        {
            return TempoStudio.actionsDown.Contains(action);
        }

        public static bool GetActionUp(Action action)
        {
            return TempoStudio.actionsUp.Contains(action);
        }

        public static bool AnyKeyDown()
        {
            return TempoStudio.actionsDown.Count != 0;
        }

        public static bool PauseAudio()
        {
            return TempoStudio.tslib.pauseaudio();
        }

        public static void UnpauseAudio()
        {
            TempoStudio.tslib.unpauseaudio();
        }

        public static void StartClock()
        {
            TempoStudio.tslib.startclock();
        }

        public static bool ClockRunning
        {
            get
            {
                return TempoStudio.tslib.clockrunning();
            }
        }

        public static double Clock
        {
            get
            {
                return TempoStudio.tslib.getclock();
            }
        }

        public static void StopClock()
        {
            TempoStudio.tslib.stopclock();
        }

        public TempoStudio()
        {
        }

        // Note: this type is marked as 'beforefieldinit'.
        static TempoStudio()
        {
        }

        public static bool controller = true;

        public static bool mouse = false;

        private static TempoStudio instance;

        public static TSImplementation implementation = TSImplementation.Default;

        public static TSSettings settings = new TSSettings();

        [SerializeField]
        [Range(-6f, 0f)]
        [Tooltip("Threshold below which to keep the signal (dBFS)")]
        private float limiterThreshold;

        [SerializeField]
        [Range(0f, 100f)]
        [Tooltip("Period over which to return signal to normal (ms)")]
        private float limiterRelease;

        [SerializeField]
        [Range(-12f, 0f)]
        [Tooltip("Amount to adjust music volume when ducking is active (dB)")]
        private float duckVolume;

        [SerializeField]
        [Range(0f, 1000f)]
        [Tooltip("Duration to maintain ducked volume (ms)")]
        private float duckHold;

        [SerializeField]
        [Range(0f, 1000f)]
        [Tooltip("Period over which to return music to normal (ms)")]
        private float duckRelease;

        public const uint VK_LBUTTON = 1U;

        public const uint VK_RBUTTON = 2U;

        public const uint VK_RETURN = 13U;

        public const uint VK_ESCAPE = 27U;

        public const uint VK_SPACE = 32U;

        public const uint VK_LEFT = 37U;

        public const uint VK_UP = 38U;

        public const uint VK_RIGHT = 39U;

        public const uint VK_DOWN = 40U;

        public const uint VK_LSHIFT = 160U;

        public const uint VK_RSHIFT = 161U;

        public const uint VK_OEM_2 = 191U;

        public const uint VK_OEM_PERIOD = 190U;

        public const uint VK_GAMEPAD_A = 195U;

        public const uint VK_GAMEPAD_B = 196U;

        public const uint VK_GAMEPAD_X = 197U;

        public const uint VK_GAMEPAD_Y = 198U;

        public const uint VK_GAMEPAD_RIGHT_SHOULDER = 199U;

        public const uint VK_GAMEPAD_LEFT_SHOULDER = 200U;

        public const uint VK_GAMEPAD_LEFT_TRIGGER = 201U;

        public const uint VK_GAMEPAD_RIGHT_TRIGGER = 202U;

        public const uint VK_GAMEPAD_DPAD_UP = 203U;

        public const uint VK_GAMEPAD_DPAD_DOWN = 204U;

        public const uint VK_GAMEPAD_DPAD_LEFT = 205U;

        public const uint VK_GAMEPAD_DPAD_RIGHT = 206U;

        public const uint VK_GAMEPAD_MENU = 207U;

        public const uint VK_GAMEPAD_VIEW = 208U;

        public const uint VK_GAMEPAD_LEFT_THUMBSTICK_BUTTON = 209U;

        public const uint VK_GAMEPAD_RIGHT_THUMBSTICK_BUTTON = 210U;

        public const uint VK_GAMEPAD_LEFT_THUMBSTICK_UP = 211U;

        public const uint VK_GAMEPAD_LEFT_THUMBSTICK_DOWN = 212U;

        public const uint VK_GAMEPAD_LEFT_THUMBSTICK_RIGHT = 213U;

        public const uint VK_GAMEPAD_LEFT_THUMBSTICK_LEFT = 214U;

        public const uint VK_GAMEPAD_RIGHT_THUMBSTICK_UP = 215U;

        public const uint VK_GAMEPAD_RIGHT_THUMBSTICK_DOWN = 216U;

        public const uint VK_GAMEPAD_RIGHT_THUMBSTICK_RIGHT = 217U;

        public const uint VK_GAMEPAD_RIGHT_THUMBSTICK_LEFT = 218U;

        private static TempoStudio.ButtonCallbackDelegate onButtonDown;

        private static TempoStudio.ButtonCallbackDelegate onButtonUp;

        private static TempoStudio.LoggerFunc logError;

        private static Dictionary<uint, TempoStudio.CallbackDelegate> onVkeyDown = new Dictionary<uint, TempoStudio.CallbackDelegate>();

        private static Dictionary<uint, TempoStudio.CallbackDelegate> onVkeyUp = new Dictionary<uint, TempoStudio.CallbackDelegate>();

        private static Dictionary<Action, TempoStudio.CallbackDelegate> onActionDown = new Dictionary<Action, TempoStudio.CallbackDelegate>();

        private static Dictionary<Action, TempoStudio.CallbackDelegate> onActionUp = new Dictionary<Action, TempoStudio.CallbackDelegate>();

        private static ConcurrentQueue<ValueTuple<uint, bool>> keyEvents = new ConcurrentQueue<ValueTuple<uint, bool>>();

        private static HashSet<uint> keysDown = new HashSet<uint>();

        private static HashSet<uint> keysUp = new HashSet<uint>();

        private static HashSet<Action> actionsDown = new HashSet<Action>();

        private static HashSet<Action> actionsUp = new HashSet<Action>();

        private static Dictionary<uint, Action> nullActionMap = new Dictionary<uint, Action>();

        private static Dictionary<uint, Action> vkeyToAction = TempoStudio.nullActionMap;

        public static ITSLib tslib;

        public static bool audioOkay;

        private static bool hasFocus = false;

        private static bool overlayOpen = false;

        public delegate void CallbackDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ButtonCallbackDelegate(uint vkCode);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        public delegate void LoggerFunc(string msg);
    }
}
