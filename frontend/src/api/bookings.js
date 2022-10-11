const apiRequest = require('./base').apiRequest;

async function getBookings(roomId, startDate, endDate){
  var response = await apiRequest(`rooms/${roomId}/bookings`, "POST", {
    startDate: startDate,
    endDate: endDate
  });
  return response.data;
}

async function book(roomId, startTime, endTime, note){
  console.log(roomId, startTime, endTime, note);
  var returnValue;

  await apiRequest("bookings", "PUT", {
    roomID: roomId,
    startTime: startTime,
    endTime: endTime,
    note: note,
    username: localStorage.getItem("loginUsername")
  })
  .then(response => {
    console.log(response);
    returnValue = response.data;
  });

  return returnValue;
}

async function editBooking(id, endTime, note){
  await apiRequest(`bookings/${id}`, "PATCH", {
    "endTime": endTime,
    "note": note
  });
}

async function del(bookingId){
  await apiRequest(`bookings/${bookingId}`, "DELETE", null)
  .then(response => {
    console.log(response);
  });
}

module.exports = {
    getBookings,
    book,
    editBooking,
    del
};