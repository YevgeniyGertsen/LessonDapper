using System;

namespace ConsoleApp1
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string FirstName { get; set; }    
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? TeamId { get; set; }

        //public virtual Team Team { get; set; }
    }
}