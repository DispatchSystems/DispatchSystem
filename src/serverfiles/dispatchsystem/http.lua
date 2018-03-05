local current = {}

AddEventHandler('__cfx_internal:httpResponse', function(token, status, body, headers)
    if current[tostring(token)] then
        local cb = current[tostring(token)]
        current[tostring(token)] = nil
        cb({status, body, headers})
    end
end)

function httpRequest(args)
    local t = {
        url = args[1],
        method = args[2],
        data = args[3],
        headers = args[4]
    }

    local d = json.encode(t)
    local token = PerformHttpRequestInternal(d, d:len())

    current[tostring(token)] = args[5]
end
