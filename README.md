# Dispatch Systems

> Dispatch Systems is a CAD/MDT system for ingame FiveM use, this is not a permenant solution, but it works for free. This is an open source free project courtesy of BlockBa5her (the coder). It is free for anyone to use, as long as they do not re-distribute the software under their own name. It does not store data in CouchDB and MySQL so EssentialMode is not needed. It stores all of the player's information in the RAM of the computer, so the next restart of the server clears all of the names that it has stored.

## Uses

* For a community just starting up and not having the money to pay for a CAD/MDT system that costs money
* Easy use with long lasting terms
* A great author that will always keep it updated
* C# with open source code
* Availability to everyone, not just people who pay
* Open for suggestions and always looking for more to add-on too
* Will always stay non-SQL/CouchDB based for easy use

## Commands

### ---Civilian Commands---

* /newname {first} {last} - Set yourself as a new PED name in the system (clears all other information including wanted and vehicle)
* /warrant - Toggles a warrant on your current PED
* /citations {num} - Set the amount of citations that your PED has gotten in the past

### ---Vehicle Commands---

* /newveh {plate} - Set's a plate as a new plate in the system
* /stolen - Toggles a stolen status of the vehicle
* /registered - Toggles the registration status of the vehicle
* /insured - Toggles the insurance status of the vehicle

### ---Police Commands---

* /2729 {first} {last} - Check the name of a person in the system
* /28 {plate} - Check the plate of a vehicle in the system

## In the works

1. Arrest ability - `/arrest {first} {last}` arrests a ped and show it in the system

2. BOLO - `/bolo {first} {last}` `/bolo {plate}` Either one will show a bolo tag on the type if searched up in the system

3. Ticket - `/ticket {first} {last}` Tickets the player then adds 1 to the citation count

4. Notes `/note {playerid} {notes}` Notes a comment on the player's profile