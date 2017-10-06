using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DispatchSystem.Common.DataHolders;

namespace DispatchSystem.Common.NetCode
{
    public class NetRequestHandler
    {
        private readonly Socket s;
        private readonly Thread listenThread;
        private readonly Dictionary<string, object> cachedNetFunctions;
        private readonly Dictionary<string, object> cachedNetValues;

        public Dictionary<string, NetEvent> NetEvents { get; }
        public Dictionary<string, NetFunction<object>> NetFunctions { get; }
        public Dictionary<string, object> NetValues { get; }

        public NetRequestHandler(Socket Socket)
        {
            NetEvents = new Dictionary<string, NetEvent>();
            NetFunctions = new Dictionary<string, NetFunction<object>>();
            NetValues = new Dictionary<string, object>();

            cachedNetValues = new Dictionary<string, object>();
            cachedNetFunctions = new Dictionary<string, object>();

            s = Socket;
            listenThread = new Thread(Listener);
            listenThread.Start();
        }

        private void Listener()
        {
            while (s.Connected)
            {
                byte[] buffer = new byte[5000];
                int end;
                try
                {
                    end = s.Receive(buffer);
                }
                catch (SocketException)
                {
                    break;
                }
                buffer = buffer.Take(end).ToArray();
                NetRequest netRequest = new StorableValue<NetRequest>(buffer).Value;

                switch (netRequest.Metadata)
                {
                    case NetRequestMetadata.Invocation:
                        HandleInvocation(netRequest).Wait();
                        break;

                    case NetRequestMetadata.ValueRequest:
                        HandleValueRequest(netRequest).Wait();
                        break;

                    case NetRequestMetadata.ValueReturn:
                        HandleValueReturned(netRequest).Wait();
                        break;

                    case NetRequestMetadata.FunctionRequest:
                        HandleFunctionRequest(netRequest).Wait();
                        break;

                    case NetRequestMetadata.FunctionReturn:
                        HandleFunctionReturn(netRequest).Wait();
                        break;
                }
            }
        }

        #region handlers
        private async Task<bool> HandleInvocation(NetRequest Request)
        {
            if (Request.Metadata != NetRequestMetadata.Invocation)
                return false;

            string netEvent = (string)Request.Data.Value[0];
            object[] parameters = Request.Data.Value.Skip(1).ToArray();

            if (!NetEvents.ContainsKey(netEvent))
                return false;

            await NetEvents[netEvent].Invoke(parameters);

            return true;
        }

        private async Task<bool> HandleValueRequest(NetRequest Request)
        {
            if (Request.Metadata != NetRequestMetadata.ValueRequest)
                return false;

            string valueName = (string)Request.Data.Value[0];

            if (!NetValues.ContainsKey(valueName))
            {
                try
                {
                    s.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.ValueReturn, valueName, false)).Bytes);
                }
                catch (SocketException)
                {
                    return false;
                }

                return true;
            }

            try
            {
                s.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.ValueReturn, valueName, true,
                    NetValues[valueName])).Bytes);
            }
            catch (SocketException)
            {
                return false;
            }

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleValueReturned(NetRequest Request)
        {
            if (Request.Metadata != NetRequestMetadata.ValueReturn)
                return false;

            string valueName = (string)Request.Data.Value[0];
            bool existed = (bool)Request.Data.Value[1];

            if (!existed)
                return true;

            cachedNetValues.Add(valueName, Request.Data.Value[2]);

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleFunctionRequest(NetRequest Request)
        {
            if (Request.Metadata != NetRequestMetadata.FunctionRequest)
                return false;

            string valueName = (string)Request.Data.Value[0];
            object[] parameters = Request.Data.Value.Skip(1).ToArray();

            if (!NetFunctions.ContainsKey(valueName))
            {
                try
                {
                    s.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.FunctionReturn, valueName, false
                        )).Bytes);
                }
                catch (SocketException)
                {
                    return false;
                }

                return true;
            }

            try
            {
                s.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.FunctionReturn, valueName, true,
                    NetFunctions[valueName].Invoke(parameters))).Bytes);
            }
            catch (SocketException)
            {
                return false;
            }

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleFunctionReturn(NetRequest Request)
        {
            if (Request.Metadata != NetRequestMetadata.FunctionReturn)
                return false;

            string valueName = (string)Request.Data.Value[0];
            bool existed = (bool)Request.Data.Value[1];

            if (!existed)
                return true;

            cachedNetFunctions.Add(valueName, Request.Data.Value[2]);

            await Task.FromResult(0);
            return true;
        }
        #endregion

        public async Task<bool> TriggetNetEvent(string NetEventName, params object[] Parameters)
        {
            try
            {
                s.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.Invocation, NetEventName, Parameters)).Bytes);
            }
            catch (SocketException)
            {
                return false;
            }

            await Task.FromResult(0);
            return true;
        }

        public async Task<T> GetNetValue<T>(string NetValueName)
        {
            try
            {
                s.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.ValueRequest, NetValueName)).Bytes);
            }
            catch (SocketException)
            {
                throw new InvalidOperationException("NetValue does not exist!");
            }

            while (!cachedNetValues.ContainsKey(NetValueName))
                await Task.Delay(10);

            return (T)cachedNetValues[NetValueName];
        }

        public async Task<Tuple<bool, T>> TryGetNetValue<T>(string NetValueName)
        {
            try
            {
                s.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.ValueRequest, NetValueName)).Bytes);
            }
            catch (SocketException)
            {
                return new Tuple<bool, T>(false, (T)new object());
            }

            while (!cachedNetValues.ContainsKey(NetValueName))
                await Task.Delay(10);

            return new Tuple<bool, T>(true, (T)cachedNetValues[NetValueName]);
        }

        //TODO: add getnetfunction and getnetvalue
    }
}