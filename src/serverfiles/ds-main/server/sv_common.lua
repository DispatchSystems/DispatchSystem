-- Common functions used across server
function dsMessage(p, msg)
    TriggerClientEvent('chatMessage', p, 'DispatchSystem', {0,0,0}, msg)
end
function dsMessageAll(msg)
    TriggerClientEvent('chatMessage', -1, 'DispatchSystem', {0,0,0}, msg)
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
function boolean(bool, upper)
    if type(bool) == 'boolean' then
        upper = upper ~= nil and upper or false
        local t = 'true'
        local f = 'false'
        if upper then
            t = 'True'
            f = 'False'
        end
        return bool and t or f
    end
    return bool
end
function status(enum)
    return enum == 0 and 'On Duty' or enum == 1 and 'Off Duty' or enum == 2 and 'Busy' or 'Gay'
end

dsMessageAll('DispatchSystem.Server by BlockBa5her loaded.')
