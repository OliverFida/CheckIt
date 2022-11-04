import base from './base';

async function getRooms(){
    return await base.apiRequest('rooms', "GET", null);
}

async function addRoom(number, name){
    return await base.apiRequest('rooms', "PUT", {
        number: number,
        name: name,
        active: true
    });
}

async function editRoom(id, number, name){
    return await base.apiRequest(`rooms/${id}`, "PATCH", {
        number: number,
        name: name,
        active: true
    });
}

async function setActive(id, state){
    if(state === true) return await base.apiRequest(`rooms/${id}/activate`, "PATCH", null);
    return await base.apiRequest(`rooms/${id}/deactivate`, "PATCH", null);
}

async function deleteRoom(id){
    return await base.apiRequest(`rooms/${id}`, "DELETE", null);
}

var exports = {
    getRooms,
    addRoom,
    deleteRoom,
    setActive,
    editRoom
};
export default exports;