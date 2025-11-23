using family_tree_builder.Data;
using family_tree_builder.Models;
using Microsoft.EntityFrameworkCore;

namespace family_tree_builder.Utilities;

public static class DatabasePersistenceUtility
{
  // Deletes everything and replaces it with the new full list of people.
  public static async Task ReplaceAllPeopleNodesAsync(ApplicationDbContext db, List<PersonNode> newPeople) // TODO: YOU WILL HAVE THE AUTH TOKEN in parameters
  {

    // TODO: auth check - replace data that's ONLY related to the authenticateed user

    await db.Database.ExecuteSqlRawAsync("DELETE FROM PersonNodes");
    await db.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name='PersonNodes'");

    if (newPeople?.Count > 0)
    {
      await db.PersonNodes.AddRangeAsync(newPeople);
      await db.SaveChangesAsync();
    }
  }

  public static async Task<List<PersonNode>> GetAllPeopleNodesAsync(ApplicationDbContext db)  // TODO: YOU WILL HAVE THE AUTH TOKEN in parameters
  {

    // TODO: auth check - get data that's ONLY related to the authenticateed user

    return await db.PersonNodes
      .AsNoTracking()
      .ToListAsync();
  }
}