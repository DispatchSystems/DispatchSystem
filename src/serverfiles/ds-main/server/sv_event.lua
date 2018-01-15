local civArr = {}
local vehArr = {}
local leoArr = {}
local ofcAssignment = {}

local lang = reqLang()
local none = lang.global.types.none

local function infoToClInfo()
	local civ = {
		civArr[1] or none,
		civArr[2] or none,
		civArr[3] ~= nil and boolean(civArr[3], true) or none,
		civArr[4] or none,
		vehArr[2] or none,
		vehArr[3] ~= nil and boolean(vehArr[3], true) or none,
		vehArr[4] ~= nil and boolean(vehArr[4], true) or none,
		vehArr[5] ~= nil and boolean(vehArr[5], true) or none
	}
	local leo = {
		leoArr[1] or none,
		leoArr[2] ~= nil and status(leoArr[2]) or none,
		ofcAssignment[1] or none
	}
	
	return civ, leo
end

AddEventHandler('dispatchsystem:event', function(type, err, args, calArgs)
	local source = calArgs[2]

	if type == 'req_civ' or type == 'req_civ_by_name' then
		civArr = args

		local civ, leo = infoToClInfo(civArr, vehArr, leoArr)
		TriggerClientEvent('dispatchsystem:pushbackData', source, civ, leo)
	elseif type == 'req_veh' or type == 'req_veh_by_plate' then
		vehArr = args
			
		local civ, leo = infoToClInfo(civArr, vehArr, leoArr)
		TriggerClientEvent('dispatchsystem:pushbackData', source, civ, leo)
	elseif type == 'req_leo' or type == 'req_leo_by_callsign' then
		leoArr = args
			
		local civ, leo = infoToClInfo(civArr, vehArr, leoArr)
		TriggerClientEvent('dispatchsystem:pushbackData', source, civ, leo)
	elseif type == 'req_leo_assignment' then
		if err == '' then
			ofcAssignment = args[2] or {}
		end

		local civ, leo = infoToClInfo(civArr, vehArr, leoArr)
		TriggerClientEvent('dispatchsystem:pushbackData', source, civ, leo)
	end
end)
