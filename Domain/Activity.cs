using System.Collections.Generic;

namespace Domain
{
    // Entity Framework needs all properties to be public and have getters and setters
    // Files like this can be referred to as "models" or "entities"
    // An entity is something we will store in the database
    // The class name relates to a table
    // Each property relates to a column in the table
    public class Activity
    {
        // GUID = Globally Unique Identifier
        public Guid Id { get; set; }  // Must be Id Entity Framework will recognize primary key of the database
        
        public string Title { get; set; }

        public DateTime Date { get; set; }
        
        public string Description { get; set; }
        
        public string Category { get; set; }
        
        public string City { get; set; }
        
        public string Venue { get; set; } 

        public bool IsCancelled { get; set; } 

        public ICollection<ActivityAttendee> Attendees { get; set; } = new List<ActivityAttendee>();

        //public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}

// Object Relational Mapper. The one I will use is Entity Framework. It provides an abstraction away from db
// ORM's remove the need to write queries ourselves.
// For development will use SQL Lite. When it comes ot production, switching over to SQL Server will be easier