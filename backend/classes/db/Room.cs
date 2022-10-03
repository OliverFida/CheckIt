using System;
using System.Collections.Generic;

namespace awl_raumreservierung
{
   /// <summary>
   /// DB-Raum
   /// </summary>
    public partial class Room
    {
      /// <summary>
      /// Raum-ID
      /// </summary>
      /// <value></value>
        public long Id { get; set; }

        /// <summary>
        /// Raumnummer
        /// </summary>
        /// <value></value>
        public string? Number { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        /// <value></value>
        public string? Name { get; set; }

        /// <summary>
        /// Aktivstatus
        /// </summary>
        /// <value></value>
        public bool Active {get; set; }

        /// <summary>
        /// 
        /// </summary>
      public Room() { }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="number"></param>
      /// <param name="name"></param>
      /// <param name="active"></param>
      public Room(string? number, string? name, bool active)
      {
         Number = number;
         Name = name;
         Active = active;
      }
   }
}
