ui_page 'nui/index.html'

files {
    'nui/index.css',
    'nui/index.html',
    'nui/menu.js'
}

server_scripts {
    'sv_commands.lua',
	'sv_event.lua',
    'sv_messages.lua',
	'sv_permissions.lua'
}
client_scripts {
    'cl_callbacks.lua',
    'cl_common.lua',
    'cl_menu.lua',
    'cl_transactions.lua'
}