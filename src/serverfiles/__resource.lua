ui_page 'nui/index.html'

files {
    'nui/index.css',
    'nui/index.html',
    'nui/menu.js'
}

server_script 'dispatchsystem.server.net.dll'
client_scripts {
    'callbacks.lua',
    'common.lua',
    'menu.lua',
    'transactions.lua'
}
file 'settings.ini'
file 'permissions.perms'