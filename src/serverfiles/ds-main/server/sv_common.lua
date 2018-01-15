-- Common functions used across server
local lang = json.decode(LoadResourceFile(GetCurrentResourceName(), 'lang.json')).chat
function reqLang()
    return lang
end

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
        local t = lang.global.status["t_lower"]
        local f = lang.global.status["f_lower"]
        if upper then
            t = lang.global.status["t_upper"]
            f = lang.global.status["f_upper"]
        end
        return bool and t or f
    end
    return bool
end
function status(enum)
    return  enum == 0 and lang.global.status["on_duty"] or 
            enum == 1 and lang.global.status["off_duty"] or 
            enum == 2 and lang.global.status["busy"] or 
            lang.global.status["gay"]
end

dsMessageAll('DispatchSystem.Server by BlockBa5her loaded.')
