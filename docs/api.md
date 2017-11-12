# DispatchSystems API

Dispatchsystems API is sort of new. Because it is in the works there is no set API. While you can use the server.dll included as a reference and use the static methods inside, there really is the way of using Events.

## Events

These events will be in the context of C# and **NOT** lua.

```csharp
// Eventname: dispatchsystem:dsreset

void TriggerServerEvent("dispatchsystem:dsreset", String invokerHandle);
```
***
```csharp
// Eventname: dispatchsystem:911init

void TriggerServerEvent("dispatchsystem:911init", String invokerHandle);
```
***
```csharp
// Eventname: dispatchsystem:911msg

void TriggerServerEvent("dispatchsystem:911msg", String invokerHandle, String message);
```
***
```csharp
// Eventname: dispatchsystem:911end

void TriggerServerEvent("dispatchsystem:911end", String invokerHandle);
```
***
```csharp
// Eventname: dispatchsystem:initOfficer

void TriggerServerEvent("dispatchsystem:initOfficer", String invokerHandle, String callsign);
```
***
```csharp
// Eventname: dispatchsystem:onDuty

void TriggerServerEvent("dispatchsystem:onDuty", String invokerHandle);
```
***
```csharp
// Eventname: dispatchsystem:offDuty

void TriggerServerEvent("dispatchsystem:offDuty", String invokerHandle);
```
***
```csharp
// Eventname: dispatchsystem:busy

void TriggerServerEvent("dispatchsystem:busy", String invokerHandle);
```
***
```csharp
// Eventname: dispatchsystem:displayStatus

void TriggerServerEvent("dispatchsystem:displayStatus", String invokerHandle);
```
***
```csharp
// Eventname: dispatchsystem:ticketCiv

void TriggerServerEvent("dispatchsystem:ticketCiv", String invokerHandle, String first, String last, String ticket, Single amount);
```
***
```csharp
// Eventname: dispatchsystem:civTickets

void TriggerServerEvent("dispatchsystem:civTickets", String invokerHandle, String first, String last);
```
***
```csharp
// Eventname: dispatchsystem:getCivilian

void TriggerServerEvent("dispatchsystem:getCivilian", String invokerHandle, String firstName, String lastName);
```
***
```csharp
// Eventname: dispatchsystem:getCivilianVeh

void TriggerServerEvent("dispatchsystem:getCivilianVeh", String invokerHandle, String plate);
```
***
```csharp
// Eventname: dispatchsystem:addBolo

void TriggerServerEvent("dispatchsystem:addBolo", String invokerHandle, String reason);
```
***
```csharp
// Eventname: dispatchsystem:viewBolos

void TriggerServerEvent("dispatchsystem:viewBolos", String invokerHandle);
```
***
```csharp
// Eventname: dispatchsystem:addCivNote

void TriggerServerEvent("dispatchsystem:addCivNote", String invokerHandle, String firstName, String lastName, String note);
```
***
```csharp
// Eventname: dispatchsystem:displayCivNotes

void TriggerServerEvent("dispatchsystem:displayCivNotes", String invokerHandle, String firstName, String lastName);
```
***
```csharp
// Eventname: dispatchsystem:ticketCiv

void TriggerServerEvent("dispatchsystem:ticketCiv", String invokerHandle, String firstName, String lastName, String reason, Single amount);
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