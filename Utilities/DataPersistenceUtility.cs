using family_tree_builder.Data;
using family_tree_builder.Models;
using Microsoft.EntityFrameworkCore;

namespace family_tree_builder.Utilities;

public static class DatabasePersistenceUtility
{
    // Replace ALL nodes belonging to the current Google user
    public static async Task ReplaceAllPeopleNodesAsync(
        ApplicationDbContext db, 
        List<PersonNode> newPeople, 
        string? currentUserId)   
    {
        // If no one is logged in → do nothing
        if (string.IsNullOrEmpty(currentUserId))
            return;

        // Delete ONLY this user's nodes
        await db.PersonNodes
            .Where(p => p.UserId == currentUserId)
            .ExecuteDeleteAsync();

        // Reset auto-increment only if we deleted everything for this user
        await db.Database.ExecuteSqlRawAsync(
            "DELETE FROM sqlite_sequence WHERE name = 'PersonNodes'");

        // Save new data — tag every node with the owner
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

    // Get nodes for the current user (or demo nodes if not logged in)
    public static async Task<List<PersonNode>> GetAllPeopleNodesAsync(
        ApplicationDbContext db, 
        string? currentUserId)
    {
        IQueryable<PersonNode> query;

        if (string.IsNullOrEmpty(currentUserId))
        {
            // Guest → show only demo tree (UserId = null)
            query = db.PersonNodes.Where(p => p.UserId == null);
        }
        else
        {
            // Logged in → show only their private tree
            query = db.PersonNodes.Where(p => p.UserId == currentUserId);
        }

        return await query
            .AsNoTracking()
            .ToListAsync();
    }
}