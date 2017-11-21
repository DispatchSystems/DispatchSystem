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
--[[                                 END OF COMMON                                 ]]

--[[
    SERVER & CLIENT TRANSACTIONS
]]

-- Civ Transactions
function displayCivilian()
    Citizen.CreateThread(function()
        TriggerServerEvent("dispatchsystem:displayCiv", getHandle())
    end)
end
function displayVeh()
    Citizen.CreateThread(function()
        TriggerServerEvent("dispatchsystem:displayVeh", getHandle())
    end)
end
function createCivilian()
    Citizen.CreateThread(function()
        exitAllMenus()
        local nameNotSplit = KeyboardInput("Name", "", 20)
        if nameNotSplit == nil then
            turnOnLastMenu()
            return
        end
        local name = stringsplit(nameNotSplit, ' ')
        if tablelength(name) < 2 then
            drawNotification("You must have a first name and a last name")
            turnOnLastMenu()
            return
        end
        TriggerServerEvent("dispatchsystem:setName", getHandle(), name[1], name[2])
        turnOnLastMenu()
    end)
end
function toggleWarrant()
    Citizen.CreateThread(function()
        TriggerServerEvent("dispatchsystem:toggleWarrant", getHandle())
    end)
end
function civCitations()
    Citizen.CreateThread(function()
        exitAllMenus()
        local amount = tonumber(KeyboardInput("Citation Count", "", 3))
        if amount == nil then
            turnOnLastMenu()
            drawNotification("You must have a valid number")
            return
        end
        TriggerServerEvent("dispatchsystem:setCitations", getHandle(), amount)
        turnOnLastMenu()
    end)
end
function init911() 
    Citizen.CreateThread(function()
        TriggerServerEvent("dispatchsystem:911init", getHandle())
    end)
end
function msg911()
    Citizen.CreateThread(function()
        exitAllMenus()
        local msg = KeyboardInput("Text", "", 100)
        if msg == nil then
            turnOnLastMenu()
            drawNotification("Invalid message")
            return
        end
        TriggerServerEvent("dispatchsystem:911msg", getHandle(), msg)
        turnOnLastMenu()
    end)
end
function end911()
    Citizen.CreateThread(function()
        TriggerServerEvent("dispatchsystem:911end", getHandle())
    end)
end
function createCivVehicle()
    Citizen.CreateThread(function()
        exitAllMenus()
        local plate = KeyboardInput("Plate", "", 8)
        if plate == nil then
            turnOnLastMenu()
            return
        end
        TriggerServerEvent("dispatchsystem:setVehicle", getHandle(), plate)
        turnOnLastMenu()
    end)
end
function toggleVehStolen()
    Citizen.CreateThread(function()
        TriggerServerEvent("dispatchsystem:toggleVehStolen", getHandle())
    end)
end
function toggleVehRegi()
    Citizen.CreateThread(function()
        TriggerServerEvent("dispatchsystem:toggleVehRegi", getHandle())
    end)
end
function toggleVehInsured()
    Citizen.CreateThread(function()
        TriggerServerEvent("dispatchsystem:toggleVehInsured", getHandle())
    end)
end

-- Leo Transactions
function createOfficer()
    Citizen.CreateThread(function()
        exitAllMenus()
        local callsign = KeyboardInput("Officer Callsign", "", 9)
        if callsign == nil then 
            turnOnLastMenu()
            return 
        end
        TriggerServerEvent("dispatchsystem:initOfficer", getHandle(), callsign)
        turnOnLastMenu()
    end)
end
function displayStatus()
    Citizen.CreateThread(function()
        TriggerServerEvent("dispatchsystem:displayStatus", getHandle())
    end)
end
function changeStatus(type)
    Citizen.CreateThread(function()
        if type == "onduty" then
            TriggerServerEvent("dispatchsystem:onDuty", getHandle())
        elseif type == "offduty" then
            TriggerServerEvent("dispatchsystem:offDuty", getHandle())
        elseif type == "busy" then
            TriggerServerEvent("dispatchsystem:busy", getHandle())
        end
    end)
end
function leoNcic()
    Citizen.CreateThread(function()
        exitAllMenus()
        local nameNotSplit = KeyboardInput("Name", "", 50)
        if nameNotSplit == nil then
            turnOnLastMenu()
            drawNotification("Invalid name")
            return
        end
        local name = stringsplit(nameNotSplit, " ")
        TriggerServerEvent("dispatchsystem:getCivilian", getHandle(), name[1], name[2])
        turnOnLastMenu()
    end)
end
function leoNcicNotes()
    Citizen.CreateThread(function()
        exitAllMenus()
        local nameNotSplit = KeyboardInput("Name", "", 50)
        if nameNotSplit == nil then
            turnOnLastMenu()
            drawNotification("Invalid name")
            return
        end
        local name = stringsplit(nameNotSplit, " ")
        TriggerServerEvent("dispatchsystem:displayCivNotes", getHandle(), name[1], name[2])
        turnOnLastMenu()
    end)
end
function leoNcicTickets()
    exitAllMenus()
    local nameNotSplit = KeyboardInput("Name", "", 50)
    if nameNotSplit == nil then
        turnOnLastMenu()
        drawNotification("Invalid name")
        return
    end
    local name = stringsplit(nameNotSplit, " ")
    TriggerServerEvent("dispatchsystem:civTickets", getHandle(), name[1], name[2])
    turnOnLastMenu()
end
function leoPlate()
    Citizen.CreateThread(function()
        exitAllMenus()
        local plate = KeyboardInput("Plate", "", 8)
        if plate == nil then
            turnOnLastMenu()
            return 
        end
        TriggerServerEvent("dispatchsystem:getCivilianVeh", getHandle(), plate)
        turnOnLastMenu()
    end)
end
function leoAddNote()
    Citizen.CreateThread(function()
        exitAllMenus()
        local nameNotSplit = KeyboardInput("Name", "", 50)
        if nameNotSplit == nil then
            turnOnLastMenu()
            drawNotification("Invalid name")
            return
        end
        local name = stringsplit(nameNotSplit, " ")
        local note = KeyboardInput("Note Text", "", 150)
        if note == nil then
            turnOnLastMenu()
            return
        end
        TriggerServerEvent("dispatchsystem:addCivNote", getHandle(), name[1], name[2], note)
        turnOnLastMenu()
    end)
end
function leoAddTicket()
    Citizen.CreateThread(function()
        exitAllMenus()
        local nameNotSplit = KeyboardInput("Name", "", 50)
        if nameNotSplit == nil then
            turnOnLastMenu()
            drawNotification("Invalid name")
            return
        end
        local name = stringsplit(nameNotSplit, " ")
        local amount = tonumber(KeyboardInput("Amount", "", 7))
        if amount == nil then
            turnOnLastMenu()
            drawNotification("You must have a valid number")
            return
        end
        if amount > 9999.99 then
            turnOnLastMenu()
            drawNotification("Your amount must be below 9999.99")
            return
        end
        local reason = KeyboardInput("Reason", "", 150)
        if reason == nil then
            turnOnLastMenu()
            return
        end
        TriggerServerEvent("dispatchsystem:ticketCiv", getHandle(), name[1], name[2], reason, amount)
        turnOnLastMenu()
    end)
end
function leoAddBolo()
    Citizen.CreateThread(function()
        exitAllMenus()
        local reason = KeyboardInput("BOLO Reason", "", 250)
        if reason == nil then
            turnOnLastMenu()
            return
        end
        TriggerServerEvent("dispatchsystem:addBolo", getHandle(), reason)
        turnOnLastMenu()
    end)
end
function leoViewBolos()
    Citizen.CreateThread(function()
        TriggerServerEvent("dispatchsystem:viewBolos", getHandle())
    end)
end
--[[                                 END OF TRANSACTIONS                                 ]]

--[[
    NUI & NUI Callbacks
]]
RegisterNetEvent("dispatchsystem:toggleLeoNUI")
RegisterNetEvent("dispatchsystem:toggleCivNUI")

local menu = nil
function turnOnCivMenu()
	menu = "civ"
	SetNuiFocus(true, true)
	SendNUIMessage({showcivmenu = true})
end
function turnOnLeoMenu()
	menu = "leo"
	SetNuiFocus(true, true)
	SendNUIMessage({showleomenu = true})
end
function turnOnLastMenu()
    menu = "unknown"
    SetNuiFocus(true, true)
    SendNUIMessage({openlastmenu = true})
end
function exitAllMenus()
	menu = nil
	SetNuiFocus(false)
	SendNUIMessage({hidemenus = true})
end
function resetMenu()
    if menu == "civ" then
        SendNUIMessage({hidemenus = true})
        SendNUIMessage({showcivmenu = true})
    elseif menu == "leo" then
        SendNUIMessage({hidemenus = true})
        SendNUIMessage({showleomenu = true})
    end
end
function safeExit()
    SetNuiFocus(false)
    menu = nil
end

-- Adding event handler at the end to use all of the above functions
AddEventHandler("dispatchsystem:toggleCivNUI", function()
	if menu == nil then
		turnOnCivMenu()
	else
		exitAllMenus()
	end
end)
AddEventHandler("dispatchsystem:toggleLeoNUI", function()
	if menu == nil then
		turnOnLeoMenu()
	else
		exitAllMenus()
	end
end)

Citizen.CreateThread(function()
	Citizen.Wait(500)
	
	-- Disabling NUI focus so that people can move in-game again
	SetNuiFocus(false, false)
end)

-- Adding callbacks for NUI button presses

--[[ADDING CIV CALLBACKS]]
RegisterNUICallback("civ", function(data, cb)
    if data[1] == nil then
        return
    elseif data[1] == "newname" then
        createCivilian()
    elseif data[1] == "warrant" then
        toggleWarrant()
    elseif data[1] == "citations" then
        civCitations()
    elseif data[1] == "911init" then
        init911()
    elseif data[1] == "911msg" then
        msg911()
    elseif data[1] == "911end" then
        end911()
    elseif data[1] == "newveh" then
        createCivVehicle()
    elseif data[1] == "vehstolen" then
        toggleVehStolen()
    elseif data[1] == "vehregi" then
        toggleVehRegi()
    elseif data[1] == "vehinsurance" then
        toggleVehInsured()
    elseif data[1] == "civdisplay" then
        displayCivilian()
    elseif data[1] == "vehdisplay" then
        displayVeh()
    end

	if cb then cb("OK") end
end)
--[[ADDING LEO CALLBACKS]]
RegisterNUICallback("leo", function(data, cb)
    if data[1] == nil then
        return
    elseif data[1] == "create" then
        createOfficer()
    elseif data[1] == "displayduty" then
        displayStatus()
    elseif data[1] == "onduty" or data[1] == "offduty" or data[1] == "busy" then
        changeStatus(data[1])
    elseif data[1] == "ncic" then
        leoNcic()
    elseif data[1] == "note" then
        if data[2] == "add" then
            leoAddNote()
        elseif data[2] == "view" then
            leoNcicNotes()
        end
    elseif data[1] == "ticket" then
        if data[2] == "add" then
            leoAddTicket()
        elseif data[2] == "view" then
            leoNcicTickets()
        end
    elseif data[1] == "plate" then
        leoPlate()
    elseif data[1] == "bolo" then
        if data[2] == "add" then
            leoAddBolo()
        elseif data[2] == "view" then
            leoViewBolos()
        end
    end

    if cb then cb("OK") end
end)
--[[ADDING COMMON CALLBACKS]]
RegisterNUICallback("common", function(data, cb)
    if data[1] == nil then
        return
    elseif data[1] == "exit" then
        safeExit()
    elseif data[1] == "dsreset" then
        TriggerServerEvent("dispatchsystem:dsreset", getHandle())
    end

    if cb then cb("OK") end
end)
--[[                                 END OF NUI                                 ]]

SendNUIMessage({setname = true, metadata = GetCurrentResourceName()}) -- Telling JS of the resource name
sendMessage("DispatchSystem", {0,0,0}, "DispatchSystem.Client by BlockBa5her loaded")
