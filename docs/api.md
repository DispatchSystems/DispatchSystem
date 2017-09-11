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
// Eventname: dispatchsystem:getCivilianVeh

void TriggerServerEvent("dispatchsystem:getCivilianVeh", String invokerHandle, String plate);
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
***
```csharp
// Eventname: dispatchsystem:setVehicle

void TriggerServerEvent("dispatchsystem:setVehicle", String playerHandle, String plate);
```
***
```csharp
// Eventname: dispatchsystem:toggleVehStolen

void TriggerServerEvent("dispatchsystem:toggleVehStolen", String playerHandle);
```
***
```csharp
// Eventname: dispatchsystem:toggleVehRegi
// And yes, the end does end with "regi" and not "registration"

void TriggerServerEvent("dispatchsystem:toggleVehRegi", String playerHandle);
```
***
```csharp
// Eventname: dispatchsystem:toggleVehInsured

void TriggerServerEvent("dispatchsystem:toggleVehInsured", String playerHandle);
```