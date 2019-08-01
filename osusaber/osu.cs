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
            AudioClip welcomeClip = null;
            AudioClip circlesClip = null;
            Logger.Log("Loading AssetBundle");

            try
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("osusaber.Resources.welcome.asset");
                var assetBundle = AssetBundle.LoadFromStream(stream);

                welcomeClip = assetBundle.LoadAsset<AudioClip>("Assets/welcome.wav");
                circlesClip = assetBundle.LoadAsset<AudioClip>("Assets/circles.wav");
            }
            catch (Exception e)
            {
                Logger.Log("Error loading AssetBundle", Logger.LogLevel.Error);
                Logger.Log($"{e.Message}\n{e.StackTrace}", Logger.LogLevel.Error);
            }

            if (welcomeClip)
            {
                Logger.Log("Welcome to osu!");
                try
                {
                    var audiosource = gameObject.AddComponent<AudioSource>();
                    audiosource.priority = 10;
                    audiosource.clip = welcomeClip;
                    audiosource.PlayDelayed(0.2f);
                }
                catch (Exception e)
                {
                    Logger.Log("Error loading AudioClip [welcome.wav]", Logger.LogLevel.Error);
                    Logger.Log($"{e.Message}\n{e.StackTrace}", Logger.LogLevel.Error);
                }
            }

            if (circlesClip)
            {
                Logger.Log("Click the circles!");
                try
                {
                    var audiosource = gameObject.AddComponent<AudioSource>();
                    audiosource.priority = 10;
                    audiosource.clip = circlesClip;
                    audiosource.Play();
                }
                catch (Exception e)
                {
                    Logger.Log("Error loading AudioClip [circles.wav]", Logger.LogLevel.Error);
                    Logger.Log($"{e.Message}\n{e.StackTrace}", Logger.LogLevel.Error);
                }
            }
        }

        public static void Unload()
        {
            Destroy (_instance.gameObject);
        }
    }
}
