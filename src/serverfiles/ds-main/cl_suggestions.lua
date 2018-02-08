-- Help Function 
Citizen.CreateThread(function()
	  TriggerEvent('chat:addSuggestion', '/dsciv' , 'Open the civilian dashboard.')
	  TriggerEvent('chat:addSuggestion', '/dsleo' , 'Open the police dashboard.')
	  TriggerEvent('chat:addSuggestion', '/dsdmp' , 'Build dump file.')
end)