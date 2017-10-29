ui_page 'nui/index.html'

files {
    'nui/index.css',
    'nui/index.html',
    'nui/nui.js'
}

server_script 'dispatchsystem.server.net.dll'
client_scripts {
    'client.lua'
}
file 'settings.ini'
file 'permissions.perms'