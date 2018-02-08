-- this can all be ignored but it's also explained.
-- this is performance boosters and other options

config = {
    client = { -- client sided changes
        refresh = 3000, -- time in milliseconds that it refreshes the Civilian (higher is better performance)
    },
    server = { -- server sided changes
        commands = { -- commands for dispatch system
            dsciv = "/dsciv",
            dsleo = "/dsleo",
            dsdmp = "/dsdmp"
        }
    }
}
