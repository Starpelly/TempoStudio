using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace TempoStudio
{
    public class TempoSoundLoader : MonoBehaviour
    {
        private void Start()
        {
            if (!File.Exists(this.path))
            {
                Debug.LogError("Custom audio file does not exist: " + this.path);
                return;
            }
            base.StartCoroutine(this.GetAudioClip());
        }

        private IEnumerator GetAudioClip()
        {
            string url = "file:///" + this.path;
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
            {
                yield return www.SendWebRequest();
                AudioClip content = DownloadHandlerAudioClip.GetContent(www);
                content.name = url;
                base.gameObject.SetActive(false);
                base.gameObject.AddComponent<TempoSound>().audioClip = content;
                base.gameObject.SetActive(true);
            }
            yield break;
        }

        public string path;
    }

}
