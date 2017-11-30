# Setting up the Server

## Toggling the server
Basically, just open up the `settings.ini` that comes with the server, and change the option under the `[server]` tag that says `enable` to `0`

## Setting up the server
Don't change any settings in the INI, unless you are having problems. All of the settings in there should work with your average joe

## Setting up the client
In the `settings.ini`, change the the IP of the server to the IP of your server, and keep the PORT the same unless you changed it on the server's side

## Toggling the database
In the `settings.ini` of the server, change the `database` option to 0.

## Ports to have open

If you are running this off a basic home internet, then you will need to open the PORT `33333`, and that is 5 threes just to confirm. If you are running this off a VPS or hosting website then you shouldn't have to open any ports because they are already open for you.
