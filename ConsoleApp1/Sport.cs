using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class Sport
    {
        public int SportId { get; set; }
        public string Name { get; set; }
        public ICollection<Team> Teams { get; set; }
    }
}