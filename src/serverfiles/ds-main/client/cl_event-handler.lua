local eventQueue = {}

function enqueueEvent(type, args, calArgs)
    if type == nil then error("No type specified") end
    if args == nil then args = {} end
    if calArgs == nil then calArgs = {} end
    table.insert(eventQueue, {type, args, calArgs})
end

Citizen.CreateThread(function()
    Citizen.Wait(500) -- wating for config to load in
    local profileRefresh = config.client.profileSync
    local boundRefresh = config.client.refresh

    -- creating a default private function for posting the server
    local function refreshCiv()
        enqueueEvent('req_civ', {getHandle()}, {'silent', tonumber(getHandle())})
        enqueueEvent('req_veh', {getHandle()}, {'silent', tonumber(getHandle())})
        enqueueEvent('req_leo', {getHandle()}, {'silent', tonumber(getHandle())})
        enqueueEvent('req_leo_assignment', {getHandle()}, {'silent', tonumber(getHandle())})
    end

    -- client refresh thread
    Citizen.CreateThread(function()
        while true do
            refreshCiv()
            Wait(profileRefresh)
        end
    end)

    -- server event thread
    Citizen.CreateThread(function()
        while true do
            Wait(boundRefresh)

            function run()
                if #eventQueue < 1 then
                    return
                end

                TriggerServerEvent('dispatchsystem:post-multi', eventQueue)
                eventQueue = {}
            end

            run()
        end
    end)
end)
