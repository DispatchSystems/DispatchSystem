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
