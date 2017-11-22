--[[
    MENU stuff
]]
RegisterNetEvent("dispatchsystem:toggleLeoNUI")
RegisterNetEvent("dispatchsystem:toggleCivNUI")
RegisterNetEvent("dispatchsystem:resetNUI")
RegisterNetEvent("dispatchsystem:pushbackData")

local menu = nil
function turnOnCivMenu()
	TriggerServerEvent("dispatchsystem:requestClientInfo", getHandle())

	menu = "civ"
	SetNuiFocus(true, true)
	SendNUIMessage({showcivmenu = true})
end
function turnOnLeoMenu()
	TriggerServerEvent("dispatchsystem:requestClientInfo", getHandle())

	menu = "leo"
	SetNuiFocus(true, true)
	SendNUIMessage({showleomenu = true})
end
function turnOnLastMenu()
    menu = "unknown"
    SetNuiFocus(true, true)
    SendNUIMessage({openlastmenu = true})
end
function exitAllMenus()
	menu = nil
	SetNuiFocus(false)
	SendNUIMessage({hidemenus = true})
end
function resetMenu()
    if menu == "civ" then
        SendNUIMessage({hidemenus = true})
        SendNUIMessage({showcivmenu = true})
    elseif menu == "leo" then
        SendNUIMessage({hidemenus = true})
        SendNUIMessage({showleomenu = true})
    end
end
function safeExit()
    SetNuiFocus(false)
    menu = nil
end

-- Adding event handler at the end to use all of the above functions
AddEventHandler("dispatchsystem:toggleCivNUI", function()
	if menu == nil then
		turnOnCivMenu()
	else
		exitAllMenus()
	end
end)
AddEventHandler("dispatchsystem:toggleLeoNUI", function()
	if menu == nil then
		turnOnLeoMenu()
	else
		exitAllMenus()
	end
end)
AddEventHandler("dispatchsystem:resetNUI", function()
    exitAllMenus()
end)
AddEventHandler("dispatchsystem:pushbackData", function(civData, ofcData)
	SendNUIMessage({pushback = true, data = {civData, ofcData}})
end)
--[[                                 END OF MENU STUFF                                 ]]
