using System;
using System.Collections.Generic;

namespace awl_raumreservierung
{
    public partial class Booking
    {
        public long Id { get; set; }
        public long? StartTime { get; set; }
        public long? EndTime { get; set; }
        public long Room { get; set; }
        public long UserId { get; set; }
        public long? CreateTime { get; set; }
        public long? CreatedBy { get; set; }
    }
}
