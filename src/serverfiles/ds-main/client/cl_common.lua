--[[
	COMMON APPS
]]
function drawNotification(text)
    SetNotificationTextEntry("STRING")
    AddTextComponentString(text)
    DrawNotification(false, false)
end
function KeyboardInput(TextEntry, ExampleText, MaxStringLenght) -- ps thanks @Flatracer
    -- TextEntry		-->	The Text above the typing field in the black square
    -- ExampleText		-->	An Example Text, what it should say in the typing field
    -- MaxStringLenght	-->	Maximum String Lenght
    
    AddTextEntry('FMMC_KEY_TIP1', TextEntry .. ':') --Sets the Text above the typing field in the black square
    DisplayOnscreenKeyboard(1, "FMMC_KEY_TIP1", "", ExampleText, "", "", "", MaxStringLenght) --Actually calls the Keyboard Input
    blockinput = true --Blocks new input while typing if **blockinput** is used
    
    while UpdateOnscreenKeyboard() ~= 1 and UpdateOnscreenKeyboard() ~= 2 do --While typing is not aborted and not finished, this loop waits
        Citizen.Wait(0)
    end
            
    if UpdateOnscreenKeyboard() ~= 2 then
        local result = GetOnscreenKeyboardResult() --Gets the result of the typing
        Citizen.Wait(500) --Little Time Delay, so the Keyboard won't open again if you press enter to finish the typing
        blockinput = false --This unblocks new Input when typing is done
        return result --Returns the result
    else
        blockinput = false --This unblocks new Input when typing is done
        return nil --Returns nil if the typing got aborted
    end
end
function sendMessage(title, rgb, text)
	TriggerEvent("chatMessage", title, rgb, text)
end
function stringsplit(inputstr, sep)
    if sep == nil then
        sep = "%s"
    end
    local t={} ; i=1
    for str in string.gmatch(inputstr, "([^"..sep.."]+)") do
        t[i] = str
        i = i + 1
    end
    return t
end
function getHandle()
    return tostring(GetPlayerServerId(PlayerId()))
end
function tablelength(T)
    local count = 0
        for _ in pairs(T) do count = count + 1 end
    return count
end
function terminateMenu()
    Citizen.CreateThread(function()
        Citizen.Wait(500)
        turnOnCivMenu = nil
        turnOnLeoMenu = nil
        turnOnLastMenu = nil
        exitAllMenus = nil
        resetMenu = nil
        safeExit = nil
    end)
end
--[[                                 END OF COMMON                                 ]]

--[[
	INIT ITEMS
]]
Citizen.CreateThread(function()
    Citizen.Wait(500)
    local resource = GetCurrentResourceName()
    if (resource ~= string.lower(resource)) then
        terminateMenu()

        while true do
            if DoesEntityExist(PlayerPedId()) then
                drawNotification("DispatchSystem:~n~~r~PLEASE CHANGE RESOURCE NAME TO ALL LOWER")
            end
            Wait(10)
        end
        return
    end
    SendNUIMessage({setname = true, metadata = GetCurrentResourceName()}) -- Telling JS of the resource name
    sendMessage("DispatchSystem", {0,0,0}, "DispatchSystem.Client by BlockBa5her loaded")    
end)
--[[                                 END OF INIT                                 ]]