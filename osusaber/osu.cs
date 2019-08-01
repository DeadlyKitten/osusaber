using UnityEngine;
using System.Reflection;
using System.Collections;
using System;
using System.Linq;

namespace osusaber
{
    class osu : MonoBehaviour
    {
        static osu _instance;
        static bool firstActivation = true;
        AudioClip circlesClip = null;

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
                    audiosource.PlayDelayed(0.5f);
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
                StartCoroutine(ClickTheCircles());
            }
        }

        IEnumerator ClickTheCircles()
        {
            SongPreviewPlayer previewPlayer = null;
            yield return new WaitUntil(() => previewPlayer = Resources.FindObjectsOfTypeAll<SongPreviewPlayer>().FirstOrDefault());
            yield return new WaitForSeconds(1);
            if (previewPlayer)
                previewPlayer.CrossfadeTo(circlesClip, 1, circlesClip.length);
        }

        public static void Unload()
        {
            Destroy (_instance.gameObject);
        }
    }
}
