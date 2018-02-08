function HasCivPermission(player)
	-- checking for the convar
	local convar = GetConvar('dsperm.civ', 'everyone')
	
	if convar == 'none' then
		return false
	elseif convar == 'everyone' then
		return true
	elseif conver == 'specific' then
		return IsPlayerAceAllowed(source, 'ds.civ')
	end
end

function HasLeoPermission(player)
	-- checking for the convar
	local convar = GetConvar('dsperm.leo', 'everyone')
	
	if convar == 'none' then
		return false
	elseif convar == 'everyone' then
		return true
	elseif conver == 'specific' then
		return IsPlayerAceAllowed(source, 'ds.leo')
	end
end

function GetDispatchPermissions()
	local type = GetConvar('dsperm.dispatch', 'everyone')
	local perms = {}
	
	local index = 0
	while true do
		local current = GetConvar('perm_dispatch'..index, 'invalid')
		if current == 'invalid' then
			break
		end
		table.insert(perms, current)
		
		index = index + 1
	end
	
	return type, perms
end

Citizen.CreateThread(function()
	Wait(1500)
	local type, perms = GetDispatchPermissions()
	if string.lower(type) ~= 'specific' then
		table.insert(perms, type)
	end
	TriggerEvent('dispatchsystem:post', 'set_dispatch_perms', perms, {})
end)
