using System;
using System.Collections.Generic;

namespace awl_raumreservierung
{
    public partial class Booking
    {
        public long Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public long Room { get; set; }
        public long UserId { get; set; }
        public DateTime? CreateTime { get; set; }
        public long? CreatedBy { get; set; }

        public Booking() { }
        public Booking(DateTime? startTime, DateTime? endTime, long room, long userId, DateTime? createTime, long? createdBy)
        {
            StartTime = startTime;
            EndTime = endTime;
            Room = room;
            UserId = userId;
            CreateTime = createTime;
            CreatedBy = createdBy;
        }
    }
}
