local playerInfo = {}

local lang = reqLang()
local none = lang.global.types.none

local function infoToClInfo(source)
	local info = playerInfo[source] or {}
	local civArr = info.civArr or {}
	local vehArr = info.vehArr or {}
	local leoArr = info.leoArr or {}
	local ofcAssignment = info.assignment or {}

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
	local source = tostring(calArgs[2])

	if source == 'nil' then
		return
	end

	if playerInfo[source] == nil then
		playerInfo[source] = {}
	end

	if type == 'req_civ' or type == 'req_civ_by_name' then
		playerInfo[source].civArr = args
	elseif type == 'req_veh' or type == 'req_veh_by_plate' then
		playerInfo[source].vehArr = args
	elseif type == 'req_leo' or type == 'req_leo_by_callsign' then
		playerInfo[source].leoArr = args
	elseif type == 'req_leo_assignment' then
		if err == '' then
			playerInfo[source].assignment = args[2] or {}
		end
	end
end)

-- pushback data
Citizen.CreateThread(function()
	Wait(500) -- waiting for config to load

	-- thread to pushback client profile information
	Citizen.CreateThread(function()
		while true do
			function loop()
				for handle, val in pairs(playerInfo) do
					local civ, leo = infoToClInfo(handle)
					TriggerClientEvent('dispatchsystem:pushbackData', tonumber(handle), civ, leo)
				end
			end
			loop()
			Wait(2000)
		end
	end)
end)
