const bookings = [
    {
      "id": 0,
      "startTime": "2022-09-28T12:25:00.000Z",
      "endTime": "2022-09-28T15:15:00.000Z",
      "room": 0,
      "userId": 0,
      "createTime": "2022-09-28T11:39:19.065Z",
      "createdBy": 0
    },
    {
      "id": 1,
      "startTime": "2022-09-30T10:35:00.000Z",
      "endTime": "2022-09-28T13:25:00.000Z",
      "room": 0,
      "userId": 0,
      "createTime": "2022-09-28T11:39:19.065Z",
      "createdBy": 0
    }
];

function getBookings(room){
    return bookings;
}

module.exports = {
    getBookings
};