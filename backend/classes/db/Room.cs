using System;
using System.Collections.Generic;

namespace awl_raumreservierung
{
    public partial class Room
    {
        public long Id { get; set; }
        public string? Number { get; set; }
        public string? Name { get; set; }

      public Room(string? number, string? name)
      {
         Number = number;
         Name = name;
      }
   }
}
