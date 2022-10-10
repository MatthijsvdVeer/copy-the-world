# Useful Queries

## Presentation example
SELECT * FROM DIGITALTWINS

### Where does this chair go?
SELECT rooms from DIGITALTWINS rooms
JOIN chairs RELATED rooms.contains
WHERE chairs.$dtId = 'chair-1'

### Empty chairs for a room
SELECT chairs FROM DIGITALTWINS rooms
JOIN chairs RELATED rooms.contains
WHERE chairs.occupied = false 
AND rooms.$dtId = 'room-8'

### All the chairs for a building
SELECT chairs FROM DIGITALTWINS buildings
JOIN rooms RELATED buildings.contains
JOIN chairs RELATED rooms.contains
WHERE buildings.$dtId = 'building-1'

### All the chairs for a building (variable hops)
SELECT chairs FROM DIGITALTWINS
MATCH (buildings)-[*1..3]->(chairs)
WHERE IS_OF_MODEL(chairs, 'dtmi:digitaltwins:ctw:Chair;1')
AND buildings.$dtId = 'building-1'

## Useful in demo
### All the rooms on the planet, times out after 60 seconds.
SELECT rooms FROM DIGITALTWINS
MATCH (planets)-[*5..6]-(rooms)
WHERE IS_OF_MODEL(rooms, 'dtmi:digitaltwins:rec_3_3:core:Room;1')
AND planets.$dtId = 'earth'

### All the available rooms on Earth.
SELECT rooms FROM DIGITALTWINS
MATCH (planets)<-[:isPartOf|locatedIn|locatedOn*4..6]-(rooms)
WHERE planets.$dtId = 'earth'
AND IS_OF_MODEL(rooms, 'dtmi:digitaltwins:rec_3_3:core:Room;1')
AND rooms.CO2.CO2Sensor < 800
AND rooms.occupancy.isOccupied = false

### Earth
SELECT T FROM DIGITALTWINS T WHERE T.$dtId = 'earth'

### Netherlands (for overlay of expanded Earth)
SELECT T FROM DIGITALTWINS T WHERE T.$dtId = 'netherlands'

### Search motion sensor by Map
SELECT sensor.$dtId FROM DIGITALTWINS
WHERE sensor.externalIds.deviceId = {0}

### City and Weather capability
SELECT city.$dtId
FROM DIGITALTWINS city
WHERE IS_OF_MODEL(city, 'dtmi:digitaltwins:ctw:City;1')

SELECT city.$dtId, weather.$dtId
FROM DIGITALTWINS city
JOIN weather RELATED weather.capabilityOf
WHERE city.$dtId IN['amsterdam', 'hilversum']

SELECT space.$dtId
FROM DIGITALTWINS sensors
JOIN space RELATED sensors.observes
WHERE sensors.$dtId = '{0}'"

SELECT city, weather FROM DIGITALTWINS city
JOIN weather RELATED weather.isCapabilityOf
