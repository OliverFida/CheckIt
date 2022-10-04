const apiRequest = require('./base').apiRequest;

async function getBookings(roomId){
  var response = await apiRequest(`booking/room/${roomId}`, "GET", null);
  return response.data;
}

module.exports = {
    getBookings
};