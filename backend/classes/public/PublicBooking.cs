namespace awl_raumreservierung
{
    public class PublicBooking
    {
        public long Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StartTimeUTC
        {
            get { return StartTime.ToUtc(); }
        }
        public DateTime? EndTime { get; set; }
        public DateTime? EndTimeUTC
        {
            get { return EndTime.ToUtc(); }
        }
        public PublicRoom Room { get; set; }
        public PublicUser User { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? CreateTimeUTC
        {
            get { return CreateTime.ToUtc(); }
        }

        public PublicBooking(Booking booking)
        {
            this.Id = booking.Id;
            this.StartTime = booking.StartTime;
            this.EndTime = booking.EndTime;
            this.Room = Helpers.GetRoom(booking.Room).ToPublicRoom();
            this.User = Helpers.GetUser(booking.UserId.ToInt()).ToPublicUser();
            this.CreateTime = booking.CreateTime;
        }
    }
}
