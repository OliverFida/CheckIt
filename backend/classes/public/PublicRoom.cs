namespace awl_raumreservierung
{
    // TK - Ja ich weiß die klasse is redundant aber sollten
    // vielleicht irgendwann mal sachen dazukommen ist das praktisch
    public class PublicRoom
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public string? Name { get; set; }
        public PublicRoom(Room room)
        {
            if(room != null)
            {
                this.Id = room.Id.ToInt();
                this.Number = room.Number;
                this.Name = room.Name;
            }
        }
    }
}
