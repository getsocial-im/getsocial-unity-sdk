using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.GetSocialDemo.Scripts.Utils
{
#pragma warning disable 0618
    public static class DemoUtils
    {
        public static byte[] LoadSampleVideoBytes()
        {
            var filePath = Application.streamingAssetsPath + Path.DirectorySeparatorChar + "sampleVideo.mp4";
            if (Application.platform == RuntimePlatform.Android)
            {
                var reader = new WWW(filePath);
                while (!reader.isDone) { }

                return reader.bytes;
            }
            return File.ReadAllBytes(filePath);
        }

        public static void ShowPopup(string title, string message)
        {
            var popup = new MNPopup (title, message);
            popup.AddAction("OK", () => {});
            popup.Show();               
        }
    }
#pragma warning restore 0618
}