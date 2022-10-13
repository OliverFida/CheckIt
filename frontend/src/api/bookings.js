import base from './base';

async function getBookings(roomId, startDate, endDate){
  var response = await base.apiRequest(`rooms/${roomId}/bookings`, "POST", {
    startDate: startDate,
    endDate: endDate
  });
  return response.data;
}

async function book(roomId, startTime, endTime, note, studentCount){
  await base.apiRequest("bookings", "PUT", {
    roomID: roomId,
    startTime: startTime,
    endTime: endTime,
    note: note,
    username: localStorage.getItem("loginUsername"),
    studentCount: studentCount
  });
}

async function editBooking(id, endTime, note){
  console.log(note);
  await base.apiRequest(`bookings/${id}`, "PATCH", {
    "endTime": endTime,
    "note": note
  });
}

async function del(bookingId){
  await base.apiRequest(`bookings/${bookingId}`, "DELETE", null);
}

var exports = {
    getBookings,
    book,
    editBooking,
    del
};
export default exports;