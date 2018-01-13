function deepcopy(orig)
    local orig_type = type(orig)
    local copy
    if orig_type == 'table' then
        copy = {}
        for orig_key, orig_value in next, orig, nil do
            copy[deepcopy(orig_key)] = deepcopy(orig_value)
        end
        setmetatable(copy, deepcopy(getmetatable(orig)))
    else -- number, string, boolean, etc
        copy = orig
    end
    return copy
end

function errMsg(t, err, args, calArgs)
    local source = tonumber(args[1])

    if string.sub(t, 1, 3) == 'req' then
        return
    end

    if source == nil then
        whiteMessage(-1, tostring(args[1]) or 'nil')
        whiteMessage(-1, err)
        return
    end

	-- Error messages
	if err == 'civ_not_exist' then
        dsMessage(source, 'Civilian not existing in the system')
        CancelEvent()
	elseif err == 'civ_name_exist' then
        dsMessage(source, 'It seems as if that name already exists in the system')
        CancelEvent()
	elseif err == 'civ_911_exist' then
        dsCustomMessage(source, '911', 'A 911 call already exists under your name')
        CancelEvent()
	elseif err == 'civ_911_no_dispatchers' then
        dsCustomMessage(source, '911', 'There are no dispatchers online to answer your 911 call')
        CancelEvent()
	elseif err == 'civ_911_not_exist' then
        dsCustomMessage(source, '911', 'We don\'t have a 911 call for you')
        CancelEvent()
	elseif err == 'veh_not_exist' then
        dsMessage(source, 'Vehicle not existing in the system')
        CancelEvent()
	elseif err == 'veh_plate_exist' then
        dsMessage(source, 'It seems as if that plate already exists in the system')
        CancelEvent()
	elseif err == 'leo_not_exist' then
        dsMessage(source, 'Officer not existing in the system')
        CancelEvent()
	elseif err == 'leo_status_prev' then
		local _status = status(args[2])
        dsMessage(source, 'Your status is already '.._status)
        CancelEvent()
    end
end

function systemMsg(type, err, _args, calArgs)
    local source = _args[1]

    -- copying table so that event doesn't pick it up
    local args = deepcopy(_args)
    table.remove(args, 1)

    if calArgs[1] == 'silent' then return end
	
    -- General messages
    if type == 'gen_reset' then
        dsMessage(source, 'Profiles reset successfully')
    elseif type == 'gen_dump' then
        dsMessage(source, 'DispatchSystem has been dumped by '..args[2]..'. Everything has been reset and deleted.')
        TriggerClientEvent('dispatchsystem:resetNUI', source)
        
    -- Events
	elseif type == 'on_leo_assignment_added' then
        dsCustomMessage(source, 'DispatchCAD', 'New Assignment: '..args[1])
    elseif type == 'on_leo_assignment_added' then
        dsCustomMessage(source, 'DispatchCAD', 'Assignment removed by Dispatcher')
    elseif type == 'on_leo_status_change' then
        local _status = status(args[1])
        dsCustomMessage(source, 'DispatchCAD', 'New status applied by Dispatcher: '.._status)
    elseif type == 'on_leo_role_removed' then
        dsCustomMessage(source, 'DispatchCAD', 'You were removed from officer by a Dispatcher')
    elseif type == 'on_civ_911_message' then
        dsCustomMessage(source, 'Dispatcher', args[1])
    elseif type == 'on_civ_911_accepted' then
        dsCustomMessage(source, '911', 'Your 911 call has been accepted')

    -- Civilian messages
    elseif type == 'civ_create' then
        dsMessage(source, 'New Civilian created: '..args[1][1]..' '..args[1][2])
    elseif type == 'civ_toggle_warrant' then
        dsMessage(source, 'New warrant status applied: '..boolean(args[1][3]))
    elseif type == 'civ_set_citations' then
        dsMessage(source, 'New citation count applied: '..tostring(args[1][4]))
    elseif type == 'civ_911_init' then
        dsCustomMessage(source, '911', 'Please wait for a dispatcher to answer your call')
    elseif type == 'civ_911_end' then
        dsCustomMessage(source, '911', 'Your 911 call has been ended')

    -- Vehicle messages
    elseif type == 'veh_create' then
        dsMessage(source, 'New Vehicle created: '..args[1][2])
    elseif type == 'veh_toggle_stolen' then
        dsMessage(source, 'New stolen status applied: '..boolean(args[1][3]))
    elseif type == 'veh_toggle_regi' then
        dsMessage(source, 'New registration status applied: '..boolean(args[1][4]))
    elseif type == 'veh_toggle_insurance' then
        dsMessage(source, 'New insurance status applied: '..boolean(args[1][5]))

    -- Leo messages
    elseif type == 'leo_create' then
        dsMessage(source, 'New Officer created: '..args[1][1])
        dsMessage(source, 'Current Status: '.._status)
    elseif type == 'leo_on_duty' or type == 'leo_off_duty' or type == 'leo_busy' then
        local _status = status(args[1][2])
        dsMessage(source, 'New officer status applied: '.._status)
    elseif type == 'leo_add_civ_note' then
        dsMessage(source, 'Note of "'..args[3]..'" added to '..args[1]..' '..args[2])
    elseif type == 'leo_add_civ_ticket' then
        dsMessage(source, 'Ticket of "'..args[3][2]..'" for $'..round(args[3][1], 2)..' to the Civilian '..args[1]..' '..args[2])
    elseif type == 'leo_bolo_add' then
        dsMessage(source, 'A new bolo for "'..args[1][2]..'" added')
    end
end

function customMsg(type, err, args, calArgs)
    local source = tonumber(calArgs[1])

    if err ~= 'none' or source == nil then
        return 
    end

    if calArgs[2] == 'civ_display' then
        dsCustomMessage(source, 'Civilian Information', '')
        whiteMessage(source, '^3Name: ^7'..args[1]..' '..args[2])
        whiteMessage(source, '^3Warrant: ^7'..boolean(args[3]))
        whiteMessage(source, '^3Citations: ^7'..tostring(args[4]))

    elseif calArgs[2] == 'veh_display' then
        dsCustomMessage(source, 'Vehicle Information', '')
        whiteMessage(source, '^3Plate: ^7'..args[2])
        whiteMessage(source, '^3Owner: ^7'..args[1][1]..' '..args[1][2])
        whiteMessage(source, '^3Stolen: ^7'..boolean(args[3]))
        whiteMessage(source, '^3Registered: ^7'..boolean(args[4]))
        whiteMessage(source, '^3Insured: ^7'..boolean(args[5]))

    elseif calArgs[2] == 'leo_get_civ' then
        dsCustomMessage(source, 'Civilian Information', '')
        whiteMessage(source, '^3Name: ^7'..args[1]..' '..args[2])
        whiteMessage(source, '^3Warrant: ^7'..boolean(args[3]))
        whiteMessage(source, '^3Citations: ^7'..tostring(args[4]))
    elseif calArgs[2] == 'leo_get_civ_veh' then
        dsCustomMessage(source, 'Vehicle Information', '')
        whiteMessage(source, '^3Plate: ^7'..args[2])
        whiteMessage(source, '^3Owner: ^7'..args[1][1]..' '..args[1][2])
        whiteMessage(source, '^3Stolen: ^7'..boolean(args[3]))
        whiteMessage(source, '^3Registered: ^7'..boolean(args[4]))
        whiteMessage(source, '^3Insured: ^7'..boolean(args[5]))
    elseif calArgs[2] == 'leo_display_civ_tickets' then
        dsCustomMessage(source, 'Tickets', '')
        if #args[6] == 0 then
            whiteMessage(source, 'None')
        else
            for _, ticket in ipairs(args[6]) do
                whiteMessage(source, '$^8'..round(ticket[2], 2)..'^7: ^3'..ticket[1])
            end
        end
    elseif calArgs[2] == 'leo_display_civ_notes' then
        dsCustomMessage(source, 'Notes', '')
        if #args[5] == 0 then
            whiteMessage(source, 'None')
        else
            for _, note in ipairs(args[5]) do
                whiteMessage(source, note)
            end
        end
    elseif calArgs[2] == 'leo_bolo_view' then
        dsCustomMessage(source, 'BOLOs', '')
        if #args == 0 then
            whiteMessage(source, 'None')
        else
            for _, bolo in ipairs(args) do
                whiteMessage(source, '^8'..bolo[1]..': ^7'..bolo[2])
            end
        end
    end
end

AddEventHandler('dispatchsystem:event', errMsg)
AddEventHandler('dispatchsystem:event', customMsg)
AddEventHandler('dispatchsystem:event', systemMsg)
