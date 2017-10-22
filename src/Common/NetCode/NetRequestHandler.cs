using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using DispatchSystem.Common.DataHolders;

namespace DispatchSystem.Common.NetCode
{
    public class NetRequestHandler
    {
        private readonly Socket S;
        private readonly Dictionary<string, NetRequestResult> CachedNetEvents;
        private readonly Dictionary<string, Tuple<NetRequestResult, object>> CachedNetFunctions;
        private readonly Dictionary<string, Tuple<bool, object>> CachedNetValues;
        private bool Sending;
        private bool Receiving;
        private bool Closed;

        public Dictionary<string, NetEvent> Events { get; set; }
        public Dictionary<string, NetFunction> Functions { get; set; }
        public Dictionary<string, object> Values { get; set; }
        public Dictionary<string, Func<Task<object>>> Properties { get; set; }
        public TextWriter Console;
        public StreamWriter Log;

        public string IP { get; }
        public int Port { get; }

        public NetRequestHandler(Socket socket)
        {
            string[] vals = socket.RemoteEndPoint.ToString().Split(':');
            IP = vals[0];
            Port = int.Parse(vals[1]);

            Events = new Dictionary<string, NetEvent>();
            Functions = new Dictionary<string, NetFunction>();
            Values = new Dictionary<string, object>();

            CachedNetValues = new Dictionary<string, Tuple<bool, object>>();
            CachedNetFunctions = new Dictionary<string, Tuple<NetRequestResult, object>>();
            CachedNetEvents = new Dictionary<string, NetRequestResult>();

            Sending = false;
            Receiving = false;

            S = socket;
        }

        public async Task Receive()
        {
            byte[] buffer = new byte[5000];
            int end;
            try
            {
                end = S.Receive(buffer);
            }
            catch (SocketException)
            {
                return;
            }
            buffer = buffer.Take(end).ToArray();

            if (buffer.Length == 0)
                return;

            NetRequest netRequest;

            try
            {
                netRequest = new StorableValue<NetRequest>(buffer).Value;
            }
            catch (SerializationException)
            {
                S.Shutdown(SocketShutdown.Both);
                S.Close();

                Closed = true;
                return;
            }

            if (netRequest.Data.Value.Length == 0)
            {
                await Receive();

                return;
            }

            switch (netRequest.Metadata)
            {
#pragma warning disable 4014
                case NetRequestMetadata.InvocationRequest:
                    try
                    {
                        await HandleInvocationRequest(netRequest);
                    }
                    catch (Exception e)
                    {
                        StorableValue<NetRequest> returnNetRequest = new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.InvocationReturn, netRequest.Data.Value[0], NetRequestResult.Incompleted));
                        S.Send(returnNetRequest.Bytes);

                        Console.WriteLine($"\n[{DateTime.Now}] [ERROR] An exception was caught when executing NetEvent \"{netRequest.Data.Value[0]}\":\n{e}\n");
                    }
                    break;

                case NetRequestMetadata.InvocationReturn:
                    await HandleInvocationReturn(netRequest);
                    break;

                case NetRequestMetadata.ValueRequest:
                    await HandleValueRequest(netRequest);
                    break;

                case NetRequestMetadata.ValueReturn:
                    await HandleValueReturn(netRequest);
                    break;

                case NetRequestMetadata.FunctionRequest:
                    try
                    {
                        await HandleFunctionRequest(netRequest);
                    }
                    catch (Exception e)
                    {
                        StorableValue<NetRequest> returnNetRequest = new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.FunctionReturn, netRequest.Data.Value[0], NetRequestResult.Incompleted));
                        S.Send(returnNetRequest.Bytes);

                        Console.WriteLine($"\n[{DateTime.Now}] [ERROR] An exception was caught when executing NetFunction \"{netRequest.Data.Value[0]}\":\n{e}\n");
                        Log.WriteLine();
                    }
                    break;

                case NetRequestMetadata.FunctionReturn:
                    await HandleFunctionReturn(netRequest);
                    break;
#pragma warning restore 4014
            }
        }

        #region handlers

        private async Task<bool> HandleInvocationRequest(NetRequest request)
        {
            if (request.Metadata != NetRequestMetadata.InvocationRequest)
                return false;

            string netEvent = (string)request.Data.Value[0];

            if (!Events.ContainsKey(netEvent))
            {
                try
                {
                    S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.InvocationReturn, netEvent, NetRequestResult.Incompleted)).Bytes);
                }
                catch
                {
                    return false;
                }
            }

            object[] parameters = (object[])request.Data.Value[1];

            await Events[netEvent].Invoke(this, parameters);

            try
            {
                S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.InvocationReturn, netEvent, NetRequestResult.Completed)).Bytes);
            }
            catch (SocketException)
            {
                return false;
            }

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleInvocationReturn(NetRequest request)
        {
            if (request.Metadata != NetRequestMetadata.InvocationReturn)
                return false;

            string valueName = (string)request.Data.Value[0];
            NetRequestResult result = (NetRequestResult)request.Data.Value[1];

            CachedNetEvents.Add(valueName, result);

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleValueRequest(NetRequest request)
        {
            if (request.Metadata != NetRequestMetadata.ValueRequest)
                return false;

            string valueName = (string)request.Data.Value[0];

            if (!Values.ContainsKey(valueName))
            {
                try
                {
                    S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.ValueReturn, valueName,
                        false)).Bytes);
                }
                catch (SocketException)
                {
                    return false;
                }

                return true;
            }

            try
            {
                S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.ValueReturn, valueName, true,
                    Values[valueName])).Bytes);
            }
            catch (SocketException)
            {
                return false;
            }

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleValueReturn(NetRequest request)
        {
            if (request.Metadata != NetRequestMetadata.ValueReturn)
                return false;

            string valueName = (string)request.Data.Value[0];
            bool result = (bool)request.Data.Value[1];

            if (!result)
            {
                CachedNetValues.Add(valueName, new Tuple<bool, object>(false, null));
                return true;
            }

            CachedNetValues.Add(valueName, new Tuple<bool, object>(true, request.Data.Value[2]));

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleFunctionRequest(NetRequest request)
        {
            if (request.Metadata != NetRequestMetadata.FunctionRequest)
                return false;

            string functionName = null;
            object[] parameters = null;
            try
            {
                functionName = (string)request.Data.Value[0];
                parameters = (object[])request.Data.Value[1];
            }
            catch (Exception e)
            {
                Log.WriteLine(e.ToString());
            }

            if (!Functions.ContainsKey(functionName))
            {
                try
                {
                    S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.FunctionReturn, functionName, NetRequestResult.Invalid)).Bytes);
                }
                catch (SocketException)
                {
                    return false;
                }

                return true;
            }

            try
            {
                S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.FunctionReturn, functionName, NetRequestResult.Completed, await Functions[functionName].Invoke(this, parameters))).Bytes);
            }
            catch (SocketException)
            {
                return false;
            }

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleFunctionReturn(NetRequest request)
        {
            if (request.Metadata != NetRequestMetadata.FunctionReturn)
                return false;

            string functionName = (string)request.Data.Value[0];
            NetRequestResult result = (NetRequestResult)request.Data.Value[1];

            if (result != NetRequestResult.Completed)
            {
                CachedNetFunctions.Add(functionName, new Tuple<NetRequestResult, object>(result, null));
                return true;
            }

            CachedNetFunctions.Add(functionName, new Tuple<NetRequestResult, object>(result, request.Data.Value[2]));

            await Task.FromResult(0);
            return true;
        }

        #endregion

        public async Task TriggerNetEvent(string netEventName, params object[] parameters)
        {
            while (Sending)
                await Task.Delay(10);

            Sending = true;
            S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.InvocationRequest, netEventName, parameters)).Bytes);
            Sending = false;

            while (Receiving)
                await Task.Delay(10);

            Receiving = true;
            await Receive();
            Receiving = false;

            while (!CachedNetEvents.ContainsKey(netEventName) && !Closed)
                await Task.Delay(10);

            switch (CachedNetEvents[netEventName])
            {
                case NetRequestResult.Invalid:
                    throw new InvalidOperationException("NetEvent does not exist!");
                case NetRequestResult.Incompleted:
                    throw new InvalidOperationException("NetEvent was not completed!");
            }

            CachedNetFunctions.Remove(netEventName);
        }

        public async Task<NetRequestResult> TryTriggerNetEvent(string netEventName, params object[] parameters)
        {
            while (Sending)
                await Task.Delay(10);

            Sending = true;
            S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.InvocationRequest, netEventName, parameters)).Bytes);
            Sending = false;

            while (Receiving)
                await Task.Delay(10);

            Receiving = true;
            await Receive();
            Receiving = false;

            while (!CachedNetEvents.ContainsKey(netEventName) && !Closed)
                await Task.Delay(10);

            if (Closed)
                return NetRequestResult.Disconnected;

            NetRequestResult result = CachedNetEvents[netEventName];
            CachedNetFunctions.Remove(netEventName);

            return result;
        }

        public async Task<T> GetNetValue<T>(string netValueName)
        {
            while (Sending)
                await Task.Delay(10);

            Sending = true;
            S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.ValueRequest, netValueName)).Bytes);
            Sending = false;

            while (Receiving)
                await Task.Delay(10);

            Receiving = true;
            await Receive();
            Receiving = false;

            while (!CachedNetValues.ContainsKey(netValueName) && !Closed)
                await Task.Delay(10);

            Tuple<bool, T> tuple = new Tuple<bool, T>(CachedNetValues[netValueName].Item1, (T)CachedNetValues[netValueName].Item2);
            CachedNetFunctions.Remove(netValueName);

            if (!tuple.Item1)
                throw new InvalidOperationException("NetValue does not exist!");

            return tuple.Item2;
        }

        public async Task<Tuple<bool, T>> TryGetNetValue<T>(string netValueName)
        {
            while (Sending)
                await Task.Delay(10);

            Sending = true;
            S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.ValueRequest, netValueName)).Bytes);
            Sending = false;

            while (Receiving)
                await Task.Delay(10);

            Receiving = true;
            await Receive();
            Receiving = false;

            while (!CachedNetValues.ContainsKey(netValueName) && !Closed)
                await Task.Delay(10);

            Tuple<bool, T> tuple = CachedNetValues[netValueName].Item2 == null ? new Tuple<bool, T>(CachedNetValues[netValueName].Item1, default(T)) : new Tuple<bool, T>(CachedNetValues[netValueName].Item1, (T)CachedNetValues[netValueName].Item2);
            CachedNetValues.Remove(netValueName);
            return tuple;
        }

        public async Task<T> TriggerNetFunction<T>(string netFunctionName, params object[] parameters)
        {
            while (Sending)
                await Task.Delay(10);

            Sending = true;
            S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.FunctionRequest, netFunctionName, parameters)).Bytes);
            Sending = false;

            while (Receiving)
                await Task.Delay(10);

            Receiving = true;
            await Receive();
            Receiving = false;

            while (!CachedNetFunctions.ContainsKey(netFunctionName) && !Closed)
                await Task.Delay(10);

            Tuple<NetRequestResult, T> tuple = new Tuple<NetRequestResult, T>(CachedNetFunctions[netFunctionName].Item1, (T)CachedNetValues[netFunctionName].Item2);
            CachedNetFunctions.Remove(netFunctionName);

            switch (tuple.Item1)
            {
                case NetRequestResult.Invalid:
                    throw new InvalidOperationException("NetFunction does not exist!");
                case NetRequestResult.Incompleted:
                    throw new InvalidOperationException("NetFunction was not completed!");
            }

            return tuple.Item2;
        }

        public async Task<Tuple<NetRequestResult, T>> TryTriggerNetFunction<T>(string netFunctionName, params object[] parameters)
        {
            while (Sending)
                await Task.Delay(10);

            Sending = true;
            S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.FunctionRequest, netFunctionName, parameters)).Bytes);
            Sending = false;

            while (Receiving)
                await Task.Delay(10);

            Receiving = true;
            await Receive();
            Receiving = false;

            while (!CachedNetFunctions.ContainsKey(netFunctionName) && !Closed)
                await Task.Delay(10);

            if (Closed)
                return new Tuple<NetRequestResult, T>(NetRequestResult.Disconnected, default(T));

            Tuple<NetRequestResult, T> tuple = CachedNetFunctions[netFunctionName].Item2 == null ? new Tuple<NetRequestResult, T>(CachedNetFunctions[netFunctionName].Item1, default(T)) : new Tuple<NetRequestResult, T>(CachedNetFunctions[netFunctionName].Item1, (T)CachedNetFunctions[netFunctionName].Item2);
            CachedNetFunctions.Remove(netFunctionName);
            return tuple;
        }
    }
}