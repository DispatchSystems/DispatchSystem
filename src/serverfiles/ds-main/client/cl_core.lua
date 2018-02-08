--[[
    SERVER & CLIENT TRANSACTIONS
]]

-- Civ Transactions
function displayCivilian()
    enqueueEvent("req_civ", {getHandle()}, {getHandle(), 'civ_display'})
end
function displayVeh()
    enqueueEvent("req_veh", {getHandle()}, {getHandle(), 'veh_display'})
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
    enqueueEvent("civ_create", {getHandle(), name[1], name[2]})
    turnOnLastMenu()
end
function toggleWarrant()
    enqueueEvent("civ_toggle_warrant", {getHandle()})
end
function civCitations()
    exitAllMenus()
    local amount = tonumber(KeyboardInput("Citation Count", "", 3))
    if amount == nil then
        turnOnLastMenu()
        drawNotification("You must have a valid number")
        return
    end
    enqueueEvent("civ_set_citations", {getHandle(), amount})
    turnOnLastMenu()
end
function init911() 
    enqueueEvent("civ_911_init", {getHandle()})
end
function msg911()
    exitAllMenus()
    local msg = KeyboardInput("Text", "", 100)
    if msg == nil then
        turnOnLastMenu()
        drawNotification("Invalid message")
        return
    end
    enqueueEvent("civ_911_msg", {getHandle(), msg})
    turnOnLastMenu()
end
function end911()
    enqueueEvent("civ_911_end", {getHandle()})
end
function createCivVehicle()
    exitAllMenus()
    local plate = KeyboardInput("Plate", "", 8)
    if plate == nil then
        turnOnLastMenu()
        return
    end
    enqueueEvent("veh_create", {getHandle(), plate})
    turnOnLastMenu()
end
function toggleVehStolen()
    enqueueEvent("veh_toggle_stolen", {getHandle()})
end
function toggleVehRegi()
    enqueueEvent("veh_toggle_regi", {getHandle()})
end
function toggleVehInsured()
    enqueueEvent("veh_toggle_insurance", {getHandle()})
end

-- Leo Transactions
function createOfficer()
    exitAllMenus()
    local callsign = KeyboardInput("Officer Callsign", "", 9)
    if callsign == nil then 
        turnOnLastMenu()
        return 
    end
    enqueueEvent("leo_create", {getHandle(), callsign})
    turnOnLastMenu()
end
function displayStatus()
    enqueueEvent("leo_display_status", {getHandle()})
end
function changeStatus(type)
    if type == "onduty" then
        enqueueEvent("leo_on_duty", {getHandle()})
    elseif type == "offduty" then
        enqueueEvent("leo_off_duty", {getHandle()})
    elseif type == "busy" then
        enqueueEvent("leo_busy", {getHandle()})
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
    enqueueEvent("req_civ_by_name", {name[1], name[2]}, {getHandle(), 'leo_get_civ'})
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
    enqueueEvent("req_civ_by_name", {name[1], name[2]}, {getHandle(), 'leo_display_civ_notes'})
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
    enqueueEvent("req_civ_by_name", {name[1], name[2]}, {getHandle(), 'leo_display_civ_tickets'})
    turnOnLastMenu()
end
function leoPlate()
    exitAllMenus()
    local plate = KeyboardInput("Plate", "", 8)
    if plate == nil then
        turnOnLastMenu()
        return 
    end
    enqueueEvent("req_veh_by_plate", {plate}, {getHandle(), 'leo_get_civ_veh'})
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
    enqueueEvent("leo_add_civ_note", {getHandle(), name[1], name[2], note})
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
    enqueueEvent("leo_add_civ_ticket", {getHandle(), name[1], name[2], reason, amount})
    turnOnLastMenu()
end
function leoAddBolo()
    exitAllMenus()
    local reason = KeyboardInput("BOLO Reason", "", 250)
    if reason == nil then
        turnOnLastMenu()
        return
    end
    enqueueEvent("leo_bolo_add", {getHandle(), reason})
    turnOnLastMenu()
end
function leoViewBolos()
    enqueueEvent("req_bolos", {}, {getHandle(), 'leo_bolo_view'})
end
--[[                                 END OF TRANSACTIONS                                 ]]
