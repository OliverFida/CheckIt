import base from './base';

async function getBookings(roomId, startDate, endDate){
  var response = await base.apiRequest(`rooms/${roomId}/bookings`, "POST", {
    startDate: startDate,
    endDate: endDate
  });
  return response.data;
}

async function book(roomId, startTime, endTime, note, studentCount, username){
  await base.apiRequest("bookings", "PUT", {
    roomID: roomId,
    startTime: startTime,
    endTime: endTime,
    note: note,
    username: username,
    studentCount: studentCount
  });
}

async function editBooking(id, endTime, note, studentCount){
  await base.apiRequest(`bookings/${id}`, "PATCH", {
    "endTime": endTime,
    "note": note,
    "studentCount": studentCount
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