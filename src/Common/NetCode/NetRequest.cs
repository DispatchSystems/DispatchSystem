using DispatchSystem.Common.DataHolders;

namespace DispatchSystem.Common.NetCode
{
    public enum NetRequestMetadata { Invocation, ValueRequest, ValueReturn, FunctionRequest, FunctionReturn }

    public class NetRequest
    {
        public StorableValue<object[]> Data { get; }
        public NetRequestMetadata Metadata { get; set; }

        public NetRequest(NetRequestMetadata Metadata, params object[] Data)
        {
            this.Metadata = Metadata;
            this.Data = new StorableValue<object[]>(Data);
        }
    }
}