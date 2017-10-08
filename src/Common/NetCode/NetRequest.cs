using System;
using System.Runtime.Serialization;
using DispatchSystem.Common.DataHolders;

namespace DispatchSystem.Common.NetCode
{
    [Serializable]
    public enum NetRequestMetadata { InvocationRequest, InvocationReturn, ValueRequest, ValueReturn, FunctionRequest, FunctionReturn }

    [Serializable]
    public enum NetRequestResult { Invalid, Incompleted, Completed }

    [Serializable]
    public class NetRequest
    {
        public StorableValue<object[]> Data { get; }
        public NetRequestMetadata Metadata { get; set; }

        public NetRequest(NetRequestMetadata metadata, params object[] data)
        {
            Metadata = metadata;
            Data = new StorableValue<object[]>(data);
        }
    }
}
