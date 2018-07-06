using System.IO;
using UnityEngine;

namespace Assets.GetSocialDemo.Scripts.Utils
{
    public class DemoUtils
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
    }
}