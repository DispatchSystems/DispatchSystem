function dsMessage(p, msg)
    TriggerClientEvent('chatMessage', p, 'DispatchSystem', {0,0,0}, msg)
end
function dsCustomMessage(p, header, msg)
    TriggerClientEvent('chatMessage', p, header, {255,0,0}, msg)
end
function whiteMessage(p, msg)
    TriggerClientEvent('chatMessage', p, '', {0,0,0}, '^7'..msg)
end

function round(num, numDecimalPlaces)
    local mult = 10^(numDecimalPlaces or 0)
    return math.floor(num * mult + 0.5) / mult
end

function boolean(bool)
    return bool and 'true' or 'false'
end
function status(enum)
    return enum == 0 and 'On Duty' or enum == 1 and 'Off Duty' or enum == 2 and 'Busy' or 'Gay'
end

function onMsg(source, type, err, args, calArgs)
	if calArgs[1] == 'silent' then return end
	
    -- General messages
    if type == 'gen_reset' then
        dsMessage(source, 'Profiles reset successfully')
    elseif type == 'gen_dump' then
        dsMessage(source, 'DispatchSystem has been dumped by '..args[2]..'. Everything has been reset and deleted.')
        TriggerClientEvent('dispatchsystem:resetNUI', source)
        
    -- Events
    elseif type == 'on_load' then
        dsMessage(-1, 'DispatchSystem.Server by BlockBa5her loaded')
	elseif type == 'on_leo_assignment_added' then
        dsCustomMessage(source, 'DispatchCAD', 'New Assignment: '..args[1])
    elseif type == 'on_leo_assignment_added' then
        dsCustomMessage(source, 'DispatchCAD', 'Assignment removed by Dispatcher')
    elseif type == 'on_leo_status_change' then
        local _status = status(args[1])
        dsCustomMessage(source, 'DispatchCAD', 'New status applied by Dispatcher: '.._status)
    elseif type == 'on_leo_role_removed' then
        dsCustomMessage(source, 'DispatchCAD', 'You were removed from officer by a Dispatcher')
		
	-- Error messages
	elseif err == 'civ_not_exist' then
		dsMessage(source, 'Civilian not existing in the system')
	elseif err == 'civ_name_exist' then
		dsMessage(source, 'It seems as if that name already exists in the system')
	elseif err == 'civ_911_exist' then
		dsCustomMessage(source, '911', 'A 911 call already exists under your name')
	elseif err == 'civ_911_no_dispatchers' then
		dsCustomMessage(source, '911', 'There are no dispatchers online to answer your 911 call')
	elseif err == 'civ_911_not_exist' then
		dsCustomMessage(source, '911', 'We don\'t have a 911 call for you')
	elseif err == 'veh_not_exist' then
		dsMessage(source, 'Vehicle not existing in the system')
	elseif err == 'veh_plate_exist' then
		dsMessage(source, 'It seems as if that plate already exists in the system')
	elseif err == 'leo_not_exist' then
		dsMessage(source, 'Officer not existing in the system')
	elseif err == 'leo_status_prev' then
		local _status = status(args[1])
        dsMessage(source, 'Your status is already '.._status)

    -- Civilian messages
    elseif type == 'civ_create' then
        dsMessage(source, 'New Civilian created: '..args[1]..' '..args[2])
    elseif type == 'civ_display' then
        dsCustomMessage(source, 'Civilian Information', '')
        whiteMessage(source, '^3Name: ^7'..args[1]..' '..args[2])
        whiteMessage(source, '^3Warrant: ^7'..boolean(args[3]))
        whiteMessage(source, '^3Citations: ^7'..tostring(args[4]))
    elseif type == 'civ_toggle_warrant' then
        dsMessage(source, 'New warrant status applied: '..boolean(args[1]))
    elseif type == 'civ_set_citations' then
        dsMessage(source, 'New citation count applied: '..tostring(args[1]))        
    elseif type == 'civ_911_init' then
        dsCustomMessage(source, '911', 'Please wait for a dispatcher to answer your call')
    elseif type == 'civ_911_end' then
        dsCustomMessage(source, '911', 'Your 911 call has been ended')
    elseif type == 'civ_911_accepted' then
        dsCustomMessage(source, '911', 'Your 911 call has been accepted')
    elseif type == 'civ_911_msg' then
        dsCustomMessage(source, 'Dispatcher', args[1])

    -- Vehicle messages
    elseif type == 'veh_display' then
        dsCustomMessage(source, 'Vehicle Information', '')
        whiteMessage(source, '^3Plate: ^7'..args[1])
        whiteMessage(source, '^3Owner: ^7'..args[2]..' '..args[3])
        whiteMessage(source, '^3Registered: ^7'..boolean(args[4]))
        whiteMessage(source, '^3Insured: ^7'..boolean(args[5]))
    elseif type == 'veh_create' then
        dsMessage(source, 'New Vehicle created: '..args[1])
    elseif type == 'veh_toggle_stolen' then
        dsMessage(source, 'New stolen status applied: '..boolean(args[1]))
    elseif type == 'veh_toggle_regi' then
        dsMessage(source, 'New registration status applied: '..boolean(args[1]))
    elseif type == 'veh_toggle_insurance' then
        dsMessage(source, 'New insurance status applied: '..boolean(args[1]))

    -- Leo messages
    elseif type == 'leo_create' then
        dsMessage(source, 'New Officer created: '..args[1])
    elseif type == 'leo_display_status' then
        local _status = status(args[1])
        dsMessage(source, 'Current Status: '.._status)
    elseif type == 'leo_on_duty' or type == 'leo_off_duty' or type == 'leo_busy' then
        local _status = status(args[1])
        dsMessage(source, 'New officer status applied: '.._status)
    elseif type == 'leo_get_civ' then
        dsCustomMessage(source, 'Civilian Information', '')
        whiteMessage(source, '^3Name: ^7'..args[1]..' '..args[2])
        whiteMessage(source, '^3Warrant: ^7'..boolean(args[3]))
        whiteMessage(source, '^3Citations: ^7'..tostring(args[4]))
    elseif type == 'leo_get_civ_veh' then
        dsCustomMessage(source, 'Vehicle Information', '')
        whiteMessage(source, '^3Plate: ^7'..args[1])
        whiteMessage(source, '^3Owner: ^7'..args[2]..' '..args[3])
        whiteMessage(source, '^3Stolen: ^7'..boolean(args[4]))
        whiteMessage(source, '^3Registered: ^7'..boolean(args[5]))
        whiteMessage(source, '^3Insured: ^7'..boolean(args[6]))
    elseif type == 'leo_add_civ_note' then
        dsMessage(source, 'Note of "'..args[3]..'" added to '..args[1]..' '..args[2])
    elseif type == 'leo_add_civ_ticket' then
        dsMessage(source, 'Ticket of "'..args[3][2]..'" for $'..round(args[3][1], 2)..' to the Civilian '..args[1]..' '..args[2])
    elseif type == 'leo_display_civ_tickets' then
        dsCustomMessage(source, 'Tickets', '')
        if #args[3] == 0 then
            whiteMessage(source, 'None')
        else
            for _, ticket in ipairs(args[3]) do
                whiteMessage(source, '$^8'..round(ticket[1], 2)..'^7: ^3'..ticket[2])
            end
        end
    elseif type == 'leo_display_civ_notes' then
        dsCustomMessage(source, 'Notes', '')
        if #args[3] == 0 then
            whiteMessage(source, 'None')
        else
            for _, note in ipairs(args[3]) do
                whiteMessage(source, note)
            end
        end
    elseif type == 'leo_bolo_add' then
        dsMessage(source, 'A new bolo for "'..args[1]..'" added')
    elseif type == 'leo_bolo_view' then
        dsCustomMessage(source, 'BOLOs', '')
        if #args[1] == 0 then
            whiteMessage(source, 'None')
        else
            for _, bolo in ipairs(args[1]) do
                whiteMessage(source, bolo)
            end
        end
    end
		
	Citizen.Trace(source..' '..type..'\n')
end

AddEventHandler('dispatchsystem:event', onMsg)