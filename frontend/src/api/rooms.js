const apiRequest = require('./base').apiRequest;

async function getRooms(){
    return await apiRequest('rooms', "GET", null);
}

async function addRoom(number, name){
    return await apiRequest('rooms', "PUT", {
        number: number,
        name: name,
        active: true
    });
}

async function editRoom(id, number, name){
    return await apiRequest(`rooms/${id}`, "PATCH", {
        number: number,
        name: name,
        active: true
    });
}

async function setActive(id, state){
    if(state === true) return await apiRequest(`rooms/${id}/activate`, "PATCH", null);
    return await apiRequest(`rooms/${id}/deactivate`, "PATCH", null);
}

async function deleteRoom(id){
    return await apiRequest(`rooms/${id}`, "DELETE", null);
}

module.exports = {
    getRooms,
    addRoom,
    deleteRoom,
    setActive,
    editRoom
};