using awl_raumreservierung.core;

namespace awl_raumreservierung {
	/// <summary>
	/// Öffentliche Daten eines Raums
	/// </summary>
	public class PublicRoom {
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
		/// Raumstatus
		/// </summary>
		/// <value></value>
		public bool Active { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="room"></param>
		public PublicRoom(Room? room) {
			if(room != null) {
				Id = room.Id.ToInt();
				Number = room.Number;
				Name = room.Name;
				Active = room.Active;
			}
		}
	}
}
