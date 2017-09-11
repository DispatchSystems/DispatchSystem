# DispatchSystems API

Dispatchsystems API is sort of new. Because it is in the works there is no set API. While you can use the server.dll included as a reference and use the static methods inside, there really is the way of using Events.

## Events

These events will be in the context of C# and **NOT** lua.

```csharp
// Eventname: dispatchsystem:runPlayerInformation

void TriggerServerEvent("dispatchsystem:getCivilian", String invokerHandle, String firstName, String lastName);
```
***
```csharp
// Eventname: dispatchsystem:setName

void TriggerServerEvent("dispatchsystem:setName", String playerHandle, String firstName, String lastName);
```
***
```csharp
// Eventname: dispatchsystem:toggleWarrant

void TriggerServerEvent("dispatchsystem:toggleWarrant", String playerHandle);
```
***
```csharp
// Eventname: dispatchsystem:setCitations

void TriggerServerEvent("dispatchsystem:setCitations", String playerHandle, Int32 citationCount);
```