const apiRequest = require('./base').apiRequest;

async function getBookings(roomId){
  var response = await apiRequest(`rooms/${roomId}/bookings`, "GET", null);
  return response.data;
}

async function book(roomId, startTime, endTime, note){
  var returnValue;

  await apiRequest("bookings", "PUT", {
    roodID: roomId,
    startTime: startTime,
    endTime: endTime,
    note: note,
    username: localStorage.getItem("loginUsername")
  })
  .then(response => {
    returnValue = response.data;
  });

  return returnValue;
}

module.exports = {
    getBookings,
    book
};