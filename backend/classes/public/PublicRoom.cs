namespace awl_raumreservierung
{
	/// <summary>
	/// Öffentliche Daten eines Raums
	/// </summary>
	public class PublicRoom
	{
		/// <summary>
		/// ID
		/// </summary>
		/// <value></value>
		public int Id { get; set; }

		/// <summary>
		/// Raumnummer
		/// </summary>
		/// <value></value>
		public string? Number { get; set; }

		/// <summary>
		/// Raumname
		/// </summary>
		/// <value></value>
		public string? Name { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="room"></param>
		public PublicRoom(Room? room)
		{
			if (room != null)
			{
				this.Id = room.Id.ToInt();
				this.Number = room.Number;
				this.Name = room.Name;
			}
		}
	}
}
