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

## Pictures

> Click to view bigger image

<a href="https://i.gyazo.com/8a17a134ee59189a2cfff4eb0357490d.png" target="_blank"><img src="https://i.gyazo.com/8a17a134ee59189a2cfff4eb0357490d.png" width="500" alt="Civilian"></img></a>
<a href="https://i.gyazo.com/84d5a31c36cf3de3524f1ce1836f2d56.png" target="_blank"><img src="https://i.gyazo.com/84d5a31c36cf3de3524f1ce1836f2d56.png" width="500" alt="Police"></img></a>
<a href="https://i.gyazo.com/4f157544ededf220b90ae23a99736ff8.png" target="_blank"><img src="https://i.gyazo.com/4f157544ededf220b90ae23a99736ff8.png" width="500" alt="BOLO"></img></a>
<a href="https://i.gyazo.com/78258256b01b67ce32d8bbe602b20866.png" target="_blank"><img src="https://i.gyazo.com/78258256b01b67ce32d8bbe602b20866.png" width="500" alt="Tickets and Notes"></img></a>
<a href="https://i.gyazo.com/e4b5ce6ca98c82fcc85739443037922e.png" target="_blank"><img src="https://i.gyazo.com/e4b5ce6ca98c82fcc85739443037922e.png" width="500" alt="Dispatch Main"></img></a>
<a href="https://i.gyazo.com/b9578ed701735082606ca0fe140317f2.png" target="_blank"><img src="https://i.gyazo.com/b9578ed701735082606ca0fe140317f2.png" width="500" alt="Dispatch BOLO"></img></a>

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
* /note {first} {last} {note} - Adds a note to a civilian
* /ticket {first} {last} {amount} {reason} - Adds a ticket to a civilian
* /notes {first} {last} - Displays all of the notes of a civilian
* /bolos - Displays all current BOLOs
* /bolo {desc} - Adds a new BOLO to the database
* /ticket {first} {last} {amount} {reason} - Tickets a Civilian
* /tickets {first} {last} - Displays all tickets for a Civilian

## In the works

1. Arrest ability - `/arrest {first} {last}` arrests a ped and show it in the system

2. Warrant Types - `/warrant {type}` have different types of bench warrants and also a toggle for outstanding

3. Database - A civilian database for storing civilians

4. Permissions - Permissions for Civilians, Cops, and Disptachers
