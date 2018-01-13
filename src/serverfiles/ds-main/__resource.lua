ui_page 'nui/index.html'

files {
    'nui/index.css',
    'nui/index.html',
    'nui/menu.js'
}

server_scripts {
    'server/sv_common.lua',
    'server/sv_commands.lua',
	'server/sv_event.lua',
    'server/sv_messages.lua',
	'server/sv_permissions.lua'
}
client_scripts {
    'client/cl_callbacks.lua',
    'client/cl_common.lua',
    'client/cl_menu.lua',
    'client/cl_transactions.lua'
}
