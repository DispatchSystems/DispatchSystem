AddEventHandler('chatMessage', function(source, n, msg)
    local split = stringsplit(msg, ' ');
    
    if split[1] == '/dsciv' then
        TriggerClientEvent('dispatchsystem:toggleCivNUI', source)
        CancelEvent()
    elseif split[1] == '/dsleo' then
        TriggerClientEvent('dispatchsystem:toggleLeoNUI', source)
        CancelEvent()
    elseif split[1] == '/dsdmp' then
        TriggerEvent('dispatchsystem:post', "gen_dump", {tostring(source)})
        CancelEvent()
    end
end)

function stringsplit(inputstr, sep)
    if sep == nil then
        sep = "%s"
    end
    local t={} ; i=1
    for str in string.gmatch(inputstr, "([^"..sep.."]+)") do
        t[i] = str
        i = i + 1
    end
    return t
end
