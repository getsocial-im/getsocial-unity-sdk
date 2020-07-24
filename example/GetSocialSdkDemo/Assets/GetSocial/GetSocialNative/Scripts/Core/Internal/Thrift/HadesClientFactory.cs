#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
using System;
using System.Net;
using System.Threading;
using Thrift.Protocol;
using Thrift.Transport;

namespace GetSocialSdk.Core
{
    internal static class HadesClientFactory
    {
        public static void Create(Action<Hades.Client> onCreate)
        {
            new Thread(() =>
            {
                var uri = new Uri(NativeBuildConfig.HadesUrl);
                using (var transport = new THttpClient(uri))
                using (var protocol = new TBinaryProtocol(transport))
                using (var client = new Hades.Client(protocol))
                {
                    transport.Open();
                    onCreate(client);
                }
            }).Start();
        }
    }
}
#endif
