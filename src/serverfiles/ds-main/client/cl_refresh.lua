Citizen.CreateThread(function()
    Citizen.Wait(500) -- wating for config to load in
    local refreshTime = config.client.refresh

    -- creating a default private function for posting the server
    local function refreshCiv()
        TriggerServerEvent('dispatchsystem:post', 'req_civ', {getHandle()}, {'silent', tonumber(getHandle())})
        TriggerServerEvent('dispatchsystem:post', 'req_veh', {getHandle()}, {'silent', tonumber(getHandle())})
        TriggerServerEvent('dispatchsystem:post', 'req_leo', {getHandle()}, {'silent', tonumber(getHandle())})
        TriggerServerEvent('dispatchsystem:post', 'req_leo_assignment', {getHandle()}, {'silent', tonumber(getHandle())})
    end

    -- client refresh thread
    Citizen.CreateThread(function()
        while true do
            refreshCiv()
            Wait(refreshTime)
        end
    end)
end)
