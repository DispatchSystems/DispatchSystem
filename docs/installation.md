# Installation guide

## Deciding options
Decide what options you would want to have on your server. The most important things to decide on are the database feature, and the server feature. They are both togglable in the server's `settings.ini` file. The server feature is what allows the client to connect to the server, and the database is where it stores all of your data on restart.

## Basic installation
To install all of the stuff on the server, you can make a drag and drop the resource folder `dispatchsystem` inside of the download folder inside of your `resources/` folder. From there, you can go inside of the resource folder and change all of the `ini` settings. Now all that's left to do is put the resource inside your `server.cfg`

## Client installation
To install the client is easy. All you have to do is make sure that the `dispatchsystem` resource has the `server` setting enabled. After that, just make sure that you have port `33333` open to use (If on web hoster or VPS it's already open). Now, you just have to configure your client's settings so that the IP matches with the IP that your FiveM server is run off of.

## Common issues
A lot of issues have been appearing with DispatchSystem lately. That is why I integrated the `/dsdmp` command to allow you to dump your server information to a file, and delete the rest of the info on the server. The `dispatchsystem.dmp` file that is exported in the dump process is located at the root of your FiveM directory.