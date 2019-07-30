using UnityEngine;
using System.Reflection;
using System;

namespace osusaber
{
    class osu : MonoBehaviour
    {
        static osu _instance;
        static bool firstActivation = true;

        public static void Load()
        {
            if (!_instance && firstActivation)
                _instance = new GameObject().AddComponent<osu>();
        }

        void Awake()
        {
            firstActivation = false;
            DontDestroyOnLoad(this);
            AudioClip clip = null;
            Logger.Log("Loading AssetBundle");

            try
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("osusaber.Resources.welcome.asset");
                var assetBundle = AssetBundle.LoadFromStream(stream);

                clip = assetBundle.LoadAsset<AudioClip>("Assets/welcome.wav");
            }
            catch (Exception e)
            {
                Logger.Log("Error loading AssetBundle", Logger.LogLevel.Error);
                Logger.Log($"{e.Message}\n{e.StackTrace}", Logger.LogLevel.Error);
            }

            if (clip)
            {
                Logger.Log("Welcome to osu!");
                try
                {
                    var audiosource = gameObject.AddComponent<AudioSource>();
                    audiosource.priority = 10;
                    audiosource.clip = clip;
                    audiosource.Play();
                }
                catch (Exception e)
                {
                    Logger.Log("Error loading AudioClip", Logger.LogLevel.Error);
                    Logger.Log($"{e.Message}\n{e.StackTrace}", Logger.LogLevel.Error);
                }
            }
        }
    }
}
