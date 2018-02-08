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

-- obtaining language content
local lang = reqLang()

function errMsg(t, err, args, calArgs)
    local source = tonumber(args[1])

    if string.sub(t, 1, 3) == 'req' or calArgs[1] == 'silent' then
        return
    end
    if source == nil then
        return
    end

	-- Error messages
	if err == 'civ_not_exist' then
        dsMessage(source, lang.errors[err])
        CancelEvent()
	elseif err == 'civ_name_exist' then
        dsMessage(source, lang.errors[err])
        CancelEvent()
	elseif err == 'civ_911_exist' then
        dsCustomMessage(source, lang.global.headers["911"], lang.errors[err])
        CancelEvent()
	elseif err == 'civ_911_no_dispatchers' then
        dsCustomMessage(source, lang.global.headers["911"], lang.errors[err])
        CancelEvent()
	elseif err == 'civ_911_not_exist' then
        dsCustomMessage(source, lang.global.headers["911"], lang.errors[err])
        CancelEvent()
	elseif err == 'veh_not_exist' then
        dsMessage(source, lang.errors[err])
        CancelEvent()
	elseif err == 'veh_plate_exist' then
        dsMessage(source, lang.errors[err])
        CancelEvent()
	elseif err == 'leo_not_exist' then
        dsMessage(source, lang.errors[err])
        CancelEvent()
	elseif err == 'leo_status_prev' then
		local _status = status(args[2][2])
        dsMessage(source, string.format(lang.errors[err], _status))
        CancelEvent()
    end
end

function systemMsg(type, err, _args, calArgs)
    local source = _args[1]

    -- copying table so that event doesn't pick it up
    local args = deepcopy(_args)
    table.remove(args, 1)

    if calArgs[1] == 'silent' or err ~= 'none' then return end
	
    -- General messages
    if type == 'gen_reset' then
        dsMessage(source, lang.system[type])
    elseif type == 'gen_dump' then
        dsMessage(source, string.format(lang.system[type], args[2]))
        TriggerClientEvent('dispatchsystem:resetNUI', source)
        
    -- Events
	elseif type == 'on_leo_assignment_added' then
        dsCustomMessage(source, lang.global.headers["cad"], string.format(lang.system[type], args[1]))
    elseif type == 'on_leo_assignment_removed' then
        dsCustomMessage(source, lang.global.headers["cad"], lang.system[type])
    elseif type == 'on_leo_status_change' then
        local _status = status(args[1])
        dsCustomMessage(source, lang.global.headers["cad"], string.format(lang.system[type], _status))
    elseif type == 'on_leo_role_removed' then
        dsCustomMessage(source, lang.global.headers["cad"], lang.system[type])
    elseif type == 'on_civ_911_message' then
        dsCustomMessage(source, lang.global.headers["dispatch"], args[1])
    elseif type == 'on_civ_911_accepted' then
        dsCustomMessage(source, lang.global.headers["911"], lang.system[type])

    -- Civilian messages
    elseif type == 'civ_create' then
        dsMessage(source, string.format(lang.system[type], args[1][1], args[1][2]))
    elseif type == 'civ_toggle_warrant' then
        dsMessage(source, string.format(lang.system[type], boolean(args[1][3])))
    elseif type == 'civ_set_citations' then
        dsMessage(source, string.format(lang.system[type], tostring(args[1][4])))
    elseif type == 'civ_911_init' then
        dsCustomMessage(source, lang.global.headers["911"], lang.system[type])
    elseif type == 'civ_911_end' then
        dsCustomMessage(source, lang.global.headers["911"], lang.system[type])

    -- Vehicle messages
    elseif type == 'veh_create' then
        dsMessage(source, string.format(lang.system[type], args[1][2]))
    elseif type == 'veh_toggle_stolen' then
        dsMessage(source, string.format(lang.system[type], boolean(args[1][3])))
    elseif type == 'veh_toggle_regi' then
        dsMessage(source, string.format(lang.system[type], boolean(args[1][4])))
    elseif type == 'veh_toggle_insurance' then
        dsMessage(source, string.format(lang.system[type], boolean(args[1][5])))

    -- Leo messages
    elseif type == 'leo_create' then
        dsMessage(source, string.format(lang.system[type], args[1][1]))
    elseif type == 'leo_on_duty' or type == 'leo_off_duty' or type == 'leo_busy' then
        local _status = status(args[1][2])
        dsMessage(source, string.format(lang.system["leo_status_change"], _status))
    elseif type == 'leo_add_civ_note' then
        dsMessage(source, string.format(lang.system[type], args[3], args[1], args[2]))
    elseif type == 'leo_add_civ_ticket' then
        dsMessage(source, string.format(lang.system[type], args[2][3], round(args[3][1], 2), args[1], args[2]))
    elseif type == 'leo_bolo_add' then
        dsMessage(source, string.format(lang.system[type], args[1][2]))
    end
end

function customMsg(type, err, args, calArgs)
    local source = tonumber(calArgs[1])

    if err ~= 'none' or source == nil or calArgs[1] == 'silent' then
        return 
    end

    if calArgs[2] == 'civ_display' then
        dsCustomMessage(source, lang.global.headers['civ_info'], '')
        whiteMessage(source, string.format('^3%s: ^7%s %s', lang.global.types["name"], args[1], args[2]))
        whiteMessage(source, string.format('^3%s: ^7%s', lang.global.types["warrant"], boolean(args[3])))
        whiteMessage(source, string.format('^3%s: ^7%s', lang.global.types["citations"], tostring(args[4])))
    elseif calArgs[2] == 'veh_display' then
        dsCustomMessage(source, lang.global.headers['veh_info'], '')
        whiteMessage(source, string.format('^3%s: ^7%s', lang.global.types['plate'], args[2]))
        whiteMessage(source, string.format('^3%s: ^7%s %s', lang.global.types['owner'], args[1][1], args[1][2]))
        whiteMessage(source, string.format('^3%s: ^7%s', lang.global.types['stolen'], boolean(args[3])))
        whiteMessage(source, string.format('^3%s: ^7%s', lang.global.types['registration'], boolean(args[4])))
        whiteMessage(source, string.format('^3%s: ^7%s', lang.global.types['insurance'], boolean(args[5])))

    elseif calArgs[2] == 'leo_get_civ' then
        dsCustomMessage(source, lang.global.headers['civ_info'], '')
        whiteMessage(source, string.format('^3%s: ^7%s %s', lang.global.types["name"], args[1], args[2]))
        whiteMessage(source, string.format('^3%s: ^7%s', lang.global.types["warrant"], boolean(args[3])))
        whiteMessage(source, string.format('^3%s: ^7%s', lang.global.types["citations"], tostring(args[4])))
    elseif calArgs[2] == 'leo_get_civ_veh' then
        dsCustomMessage(source, lang.global.headers['veh_info'], '')
        whiteMessage(source, string.format('^3%s: ^7%s', lang.global.types['plate'], args[2]))
        whiteMessage(source, string.format('^3%s: ^7%s %s', lang.global.types['owner'], args[1][1], args[1][2]))
        whiteMessage(source, string.format('^3%s: ^7%s', lang.global.types['stolen'], boolean(args[3])))
        whiteMessage(source, string.format('^3%s: ^7%s', lang.global.types['registration'], boolean(args[4])))
        whiteMessage(source, string.format('^3%s: ^7%s', lang.global.types['insurance'], boolean(args[5])))
    elseif calArgs[2] == 'leo_display_civ_tickets' then
        dsCustomMessage(source, lang.global.types['tickets'], '')
        if #args[6] == 0 then
            whiteMessage(source, lang.global.types['none'])
        else
            for _, ticket in ipairs(args[6]) do
                whiteMessage(source, '$^8'..round(ticket[2], 2)..'^7: ^3'..ticket[1])
            end
        end
    elseif calArgs[2] == 'leo_display_civ_notes' then
        dsCustomMessage(source, lang.global.types['notes'], '')
        if #args[5] == 0 then
            whiteMessage(source, lang.global.types['none'])
        else
            for _, note in ipairs(args[5]) do
                whiteMessage(source, note)
            end
        end
    elseif calArgs[2] == 'leo_bolo_view' then
        dsCustomMessage(source, lang.global.types['bolos'], '')
        if #args == 0 then
            whiteMessage(source, lang.global.types['none'])
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
