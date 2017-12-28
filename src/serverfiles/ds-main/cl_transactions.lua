--[[
    SERVER & CLIENT TRANSACTIONS
]]

-- Civ Transactions
function displayCivilian()
    TriggerServerEvent("dispatchsystem:post", "civ_display", {getHandle()})
end
function displayVeh()
    TriggerServerEvent("dispatchsystem:post", "veh_display", {getHandle()})
end
function createCivilian()
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
    TriggerServerEvent("dispatchsystem:post", "civ_create", {getHandle(), name[1], name[2]})
    turnOnLastMenu()
end
function toggleWarrant()
    TriggerServerEvent("dispatchsystem:post", "civ_toggle_warrant", {getHandle()})
end
function civCitations()
    exitAllMenus()
    local amount = tonumber(KeyboardInput("Citation Count", "", 3))
    if amount == nil then
        turnOnLastMenu()
        drawNotification("You must have a valid number")
        return
    end
    TriggerServerEvent("dispatchsystem:post", "civ_set_citations", {getHandle(), amount})
    turnOnLastMenu()
end
function init911() 
    TriggerServerEvent("dispatchsystem:post", "civ_911_init", {getHandle()})
end
function msg911()
    exitAllMenus()
    local msg = KeyboardInput("Text", "", 100)
    if msg == nil then
        turnOnLastMenu()
        drawNotification("Invalid message")
        return
    end
    TriggerServerEvent("dispatchsystem:post", "civ_911_msg", {getHandle(), msg})
    turnOnLastMenu()
end
function end911()
    TriggerServerEvent("dispatchsystem:post", "civ_911_end", {getHandle()})
end
function createCivVehicle()
    exitAllMenus()
    local plate = KeyboardInput("Plate", "", 8)
    if plate == nil then
        turnOnLastMenu()
        return
    end
    TriggerServerEvent("dispatchsystem:post", "veh_create", {getHandle(), plate})
    turnOnLastMenu()
end
function toggleVehStolen()
    TriggerServerEvent("dispatchsystem:post", "veh_toggle_stolen", {getHandle()})
end
function toggleVehRegi()
    TriggerServerEvent("dispatchsystem:post", "veh_toggle_regi", {getHandle()})
end
function toggleVehInsured()
    TriggerServerEvent("dispatchsystem:post", "veh_toggle_insurance", {getHandle()})
end

-- Leo Transactions
function createOfficer()
    exitAllMenus()
    local callsign = KeyboardInput("Officer Callsign", "", 9)
    if callsign == nil then 
        turnOnLastMenu()
        return 
    end
    TriggerServerEvent("dispatchsystem:post", "leo_create", {getHandle(), callsign})
    turnOnLastMenu()
end
function displayStatus()
    TriggerServerEvent("dispatchsystem:post", "leo_display_status", {getHandle()})
end
function changeStatus(type)
    if type == "onduty" then
        TriggerServerEvent("dispatchsystem:post", "leo_on_duty", {getHandle()})
    elseif type == "offduty" then
        TriggerServerEvent("dispatchsystem:post", "leo_off_duty", {getHandle()})
    elseif type == "busy" then
        TriggerServerEvent("dispatchsystem:post", "leo_busy", {getHandle()})
    end
end
function leoNcic()
    exitAllMenus()
    local nameNotSplit = KeyboardInput("Name", "", 50)
    if nameNotSplit == nil then
        turnOnLastMenu()
        drawNotification("Invalid name")
        return
    end
    local name = stringsplit(nameNotSplit, " ")
    TriggerServerEvent("dispatchsystem:post", "leo_get_civ", {getHandle(), name[1], name[2]})
    turnOnLastMenu()
end
function leoNcicNotes()
    exitAllMenus()
    local nameNotSplit = KeyboardInput("Name", "", 50)
    if nameNotSplit == nil then
        turnOnLastMenu()
        drawNotification("Invalid name")
        return
    end
    local name = stringsplit(nameNotSplit, " ")
    TriggerServerEvent("dispatchsystem:post", "leo_display_civ_notes", {getHandle(), name[1], name[2]})
    turnOnLastMenu()
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
    TriggerServerEvent("dispatchsystem:post", "leo_display_civ_tickets", {getHandle(), name[1], name[2]})
    turnOnLastMenu()
end
function leoPlate()
    exitAllMenus()
    local plate = KeyboardInput("Plate", "", 8)
    if plate == nil then
        turnOnLastMenu()
        return 
    end
    TriggerServerEvent("dispatchsystem:post", "leo_get_civ_veh", {getHandle(), plate})
    turnOnLastMenu()
end
function leoAddNote()
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
    TriggerServerEvent("dispatchsystem:post", "leo_add_civ_note", {getHandle(), name[1], name[2], note})
    turnOnLastMenu()
end
function leoAddTicket()
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
    TriggerServerEvent("dispatchsystem:post", "leo_add_civ_ticket", {getHandle(), name[1], name[2], reason, amount})
    turnOnLastMenu()
end
function leoAddBolo()
    exitAllMenus()
    local reason = KeyboardInput("BOLO Reason", "", 250)
    if reason == nil then
        turnOnLastMenu()
        return
    end
    TriggerServerEvent("dispatchsystem:post", "leo_bolo_add", {getHandle(), reason})
    turnOnLastMenu()
end
function leoViewBolos()
    TriggerServerEvent("dispatchsystem:viewBolos", "leo_bolo_view", {getHandle()})
end
--[[                                 END OF TRANSACTIONS                                 ]]
