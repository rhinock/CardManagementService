using System.Collections.Generic;

namespace DataServices.Objects
{
    internal class NetClientResult
    {
        public NetClientResult(string data, Dictionary<string, string> metadata)
        {
            Data = data;
            Metadata = metadata;
        }

        public string Data { get; }

        public Dictionary<string, string> Metadata { get; }
    }
}