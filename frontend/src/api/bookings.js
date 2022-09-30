const bookings2 = [
    {
      "id": 0,
      "startTime": "2022-09-30T07:05:00.000Z",
      "startTimeUTC": "2022-09-30T05:05:00.000Z",
      "endTime": "2022-09-30T07:55:00.000Z",
      "endTimeUTC": "2022-09-30T05:55:00.000Z",
      "room": {
        "id": 2,
        "number": "402",
        "name": "Olis rechtes Nasenloch"
      },
      "user": {
        "username": "dageorg",
        "firstName": "Georg",
        "lastname": "Friedrich",
        "lastLogon": "2022-09-30T05:40:10.124Z",
        "lastLogonUTC": "2022-09-30T05:40:10.124Z",
        "lastchange": "2022-09-30T05:40:10.124Z",
        "lastchangeUTC": "2022-09-30T05:40:10.124Z",
        "role": 0,
        "active": true
      },
      "createTime": "2022-09-30T05:40:10.124Z",
      "createTimeUTC": "2022-09-30T05:40:10.124Z"
    },
    {
      "id": 1,
      "startTime": "2022-10-03T12:35:00.000Z",
      "startTimeUTC": "2022-10-03T10:35:00.000Z",
      "endTime": "2022-09-30T13:25:00.000Z",
      "endTimeUTC": "2022-09-30T11:25:00.000Z",
      "room": {
        "id": 2,
        "number": "402",
        "name": "Olis rechtes Nasenloch"
      },
      "user": {
        "username": "damichi",
        "firstName": "Michael",
        "lastname": "Platzer",
        "lastLogon": "2022-09-30T05:40:10.124Z",
        "lastLogonUTC": "2022-09-30T05:40:10.124Z",
        "lastchange": "2022-09-30T05:40:10.124Z",
        "lastchangeUTC": "2022-09-30T05:40:10.124Z",
        "role": 0,
        "active": true
      },
      "createTime": "2022-09-30T05:40:10.124Z",
      "createTimeUTC": "2022-09-30T05:40:10.124Z"
    },
    {
      "id": 2,
      "startTime": "2022-10-06T15:15:00.000Z",
      "startTimeUTC": "2022-10-06T13:15:00.000Z",
      "endTime": "2022-09-30T13:25:00.000Z",
      "endTimeUTC": "2022-09-30T11:25:00.000Z",
      "room": {
        "id": 2,
        "number": "402",
        "name": "Olis rechtes Nasenloch"
      },
      "user": {
        "username": "dageorg",
        "firstName": "Georg",
        "lastname": "Friedrich",
        "lastLogon": "2022-09-30T05:40:10.124Z",
        "lastLogonUTC": "2022-09-30T05:40:10.124Z",
        "lastchange": "2022-09-30T05:40:10.124Z",
        "lastchangeUTC": "2022-09-30T05:40:10.124Z",
        "role": 0,
        "active": true
      },
      "createTime": "2022-09-30T05:40:10.124Z",
      "createTimeUTC": "2022-09-30T05:40:10.124Z"
    },
    {
      "id": 3,
      "startTime": "2022-10-12T15:15:00.000Z",
      "startTimeUTC": "2022-10-12T13:15:00.000Z",
      "endTime": "2022-09-30T13:25:00.000Z",
      "endTimeUTC": "2022-09-30T11:25:00.000Z",
      "room": {
        "id": 2,
        "number": "402",
        "name": "Olis rechtes Nasenloch"
      },
      "user": {
        "username": "dageorg",
        "firstName": "Georg",
        "lastname": "Friedrich",
        "lastLogon": "2022-09-30T05:40:10.124Z",
        "lastLogonUTC": "2022-09-30T05:40:10.124Z",
        "lastchange": "2022-09-30T05:40:10.124Z",
        "lastchangeUTC": "2022-09-30T05:40:10.124Z",
        "role": 0,
        "active": true
      },
      "createTime": "2022-09-30T05:40:10.124Z",
      "createTimeUTC": "2022-09-30T05:40:10.124Z"
    }
];

const bookings0 = [
  {
    "id": 4,
    "startTime": "2022-10-12T15:15:00.000Z",
    "startTimeUTC": "2022-10-12T13:15:00.000Z",
    "endTime": "2022-09-30T13:25:00.000Z",
    "endTimeUTC": "2022-09-30T11:25:00.000Z",
    "room": {
      "id": 0,
      "number": "307",
      "name": "307"
    },
    "user": {
      "username": "dageorg",
      "firstName": "Georg",
      "lastname": "Friedrich",
      "lastLogon": "2022-09-30T05:40:10.124Z",
      "lastLogonUTC": "2022-09-30T05:40:10.124Z",
      "lastchange": "2022-09-30T05:40:10.124Z",
      "lastchangeUTC": "2022-09-30T05:40:10.124Z",
      "role": 0,
      "active": true
    },
    "createTime": "2022-09-30T05:40:10.124Z",
    "createTimeUTC": "2022-09-30T05:40:10.124Z"
  }
];

function getBookings(roomId){
  if(roomId === 0) return bookings0;
  if(roomId === 2) return bookings2;
  return [];
}

module.exports = {
    getBookings
};