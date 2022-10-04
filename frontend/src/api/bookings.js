const apiRequest = require('./base').apiRequest;

async function getBookings(roomId){
  var response = await apiRequest(`rooms/${roomId}/bookings`, "GET", null);
  return response.data;
}

module.exports = {
    getBookings
};