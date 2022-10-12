import base from './base';

async function getBookings(roomId, startDate, endDate){
  var response = await base.apiRequest(`rooms/${roomId}/bookings`, "POST", {
    startDate: startDate,
    endDate: endDate
  });
  return response.data;
}

async function book(roomId, startTime, endTime, note){
  console.log(roomId, startTime, endTime, note);
  var returnValue;

  await base.apiRequest("bookings", "PUT", {
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
  await base.apiRequest(`bookings/${id}`, "PATCH", {
    "endTime": endTime,
    "note": note
  });
}

async function del(bookingId){
  await base.apiRequest(`bookings/${bookingId}`, "DELETE", null)
  .then(response => {
    console.log(response);
  });
}

var exports = {
    getBookings,
    book,
    editBooking,
    del
};
export default exports;