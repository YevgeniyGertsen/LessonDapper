using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Team
    {       
        public int TeamId { get; set; }    
        public string Name { get; set; }
        public DateTime? FoundingDate { get; set; }
        public int? SportId { get; set; }
        public virtual IEnumerable<Player> Player { get; set; }
        public Sport Sport { get; set; }
    }
}