const apiRequest = require('./base').apiRequest;

async function getRooms(){
    return await apiRequest('rooms', "GET", null);
}

module.exports = {
    getRooms
};