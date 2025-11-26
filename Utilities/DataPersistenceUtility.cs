using family_tree_builder.Data;
using family_tree_builder.Models;
using Microsoft.EntityFrameworkCore;

namespace family_tree_builder.Utilities;

public static class DatabasePersistenceUtility
{
    
    public static async Task ReplaceAllPeopleNodesAsync(
        ApplicationDbContext db, 
        List<PersonNode> newPeople, 
        string? currentUserId)   
    {
 
        if (string.IsNullOrEmpty(currentUserId))
            return;

 
        await db.PersonNodes
            .Where(p => p.UserId == currentUserId)
            .ExecuteDeleteAsync();

 
        await db.Database.ExecuteSqlRawAsync(
            "DELETE FROM sqlite_sequence WHERE name = 'PersonNodes'");

      
        foreach (var person in newPeople)
        {
            person.UserId = currentUserId;
        }

        if (newPeople.Count > 0)
        {
            await db.PersonNodes.AddRangeAsync(newPeople);
            await db.SaveChangesAsync();
        }
    }


    public static async Task<List<PersonNode>> GetAllPeopleNodesAsync(
        ApplicationDbContext db, 
        string? currentUserId)
    {
        IQueryable<PersonNode> query;

        if (string.IsNullOrEmpty(currentUserId))
        {
      
            query = db.PersonNodes.Where(p => p.UserId == null);
        }
        else
        {
            
            query = db.PersonNodes.Where(p => p.UserId == currentUserId);
        }

        return await query
            .AsNoTracking()
            .ToListAsync();
    }
}