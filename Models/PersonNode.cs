using System.Collections.Generic;

namespace family_tree_builder.Models
{
    public class PersonNode
    {
        public int Id { get; set; }
        // Father ID
        public int? Fid { get; set; }
        // Mother ID
        public int? Mid { get; set; }
        // Partner IDs
        public List<int>? Pids { get; set; }
        public required string Name { get; set; }

        // TODO: I may wanna add another field like authenticationEmail or authenticationToken (reasearch it)
        public string? UserId { get; set; }
    }
}