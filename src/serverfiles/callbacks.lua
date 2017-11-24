--[[
    NUI
]]

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

    TriggerServerEvent("dispatchsystem:requestClientInfo", getHandle())

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

    TriggerServerEvent("dispatchsystem:requestClientInfo", getHandle())

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

    TriggerServerEvent("dispatchsystem:requestClientInfo", getHandle())

    if cb then cb("OK") end
end)
--[[                                 END OF NUI                                 ]]
