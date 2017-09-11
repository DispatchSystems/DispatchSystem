-- Registering Events
RegisterNetEvent('dispatchsystem:requestLP')

-- Adding Event Handlers
AddEventHandler('dispatchsystem:requestLP', function()
  local playerPed = PlayerPedId()
  local playerVeh = GetVehiclePedIsIn(playerPed, false)
  local lp = "---------" -- Setting LP as nine characters because max license plate char is 8
  
  if (playerVeh ~= nil) then
    lp = GetVehicleLicensePlateText(playerVeh)
  end
  
  TriggerServerEvent('dispatchsystem:transferLP', lp)
end)
