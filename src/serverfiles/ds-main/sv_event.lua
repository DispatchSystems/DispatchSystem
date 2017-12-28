local function infoToClInfo(civArr, vehArr, leoArr)
	local civ = {
		civArr[1] or "None",
		civArr[2] or "None",
		civArr[3] or "None",
		civArr[4] or "None",
		vehArr[1] or "None",
		vehArr[2] or "None",
		vehArr[3] or "None",
		vehArr[4] or "None"
	}
	local leo = {
		leoArr[1] or "None",
		leoArr[2] or "None",
		leoArr[3] == 0 and "On Duty" or leoArr[3] == 1 and "Off Duty" or leoArr[3] == 2 and "Busy" or "None"
	}
	
	return civ, leo
end

AddEventHandler('dispatchsystem:event', function(source, type, err, args, calArgs)
	if type == 'gen_info' then
		CancelEvent()
			
		local civArr = args[1]
		local vehArr = args[2]
		local leoArr = args[3]
			
		local civ, leo = infoToClInfo(civArr, vehArr, leoArr)
			
		TriggerClientEvent('dispatchsystem:pushbackData', source, civ, leo)
	end
end)
