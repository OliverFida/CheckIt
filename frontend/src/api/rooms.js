const apiRequest = require('./base').apiRequest;

const rooms = [
    {
        "id": 0,
        "number": "307",
        "name": "307"
    },
    {
        "id": 1,
        "number": "401",
        "name": "Tobias sei Mama"
    },
    {
        "id": 2,
        "number": "402",
        "name": "Oli sei rechts Nasenloch"
    },
];

async function getRooms(){
    return await apiRequest('room/get', "GET", null);
    return rooms;
}

module.exports = {
    getRooms
};