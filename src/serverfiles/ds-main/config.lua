-- this can all be ignored but it's also explained.
-- this is performance boosters and other options

config = {
    client = { -- client sided changes
        -- time in milliseconds that it transmits all server-bound events (higher is better performance but more delay)
        refresh = 500,
        -- time in milliseconds to make requests to sync the civilian
        profileSync = 3000
    },
    server = { -- server sided changes
        pushbackSync = 2000, -- time in milliseconds that it transmits profile information from server to client
        commands = { -- commands for dispatch system
            dsciv = "/dsciv",
            dsleo = "/dsleo",
            dsdmp = "/dsdmp"
        }
    }
}
