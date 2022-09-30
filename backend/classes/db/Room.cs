using System;
using System.Collections.Generic;

namespace awl_raumreservierung
{
    public partial class Room
    {
        public long Id { get; set; }
        public string? Number { get; set; }
        public string? Name { get; set; }
        public bool Active {get; set; }
      public Room() { }
      public Room(string? number, string? name, bool active)
      {
         Number = number;
         Name = name;
         Active = active;
      }
   }
}
