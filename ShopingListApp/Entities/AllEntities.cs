using Microsoft.EntityFrameworkCore;

namespace ShopingListApp.Entities;


public class Dictionary
{
    public int ID { get; set; }
    public string Name { get; set; }
    public List<Definition> Definitions { get; set; }
}

public class Definition
{
    public int ID { get; set; }
    public string Word { get; set; }
    public string Meaning { get; set; }
    
    public int DictionaryId { get; set; }
    public Dictionary Dictionary { get; set; }
    
    public int LanguageId { get; set; }
    public Language Language { get; set; }
}

public static class DefinitionConfiguration
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Definition>()
            .HasOne(d => d.Language)
            .WithMany(l => l.Definitions)
            .HasForeignKey(sp => sp.LanguageId);

        modelBuilder.Entity<Definition>()
            .HasOne(d => d.Dictionary)
            .WithMany(l => l.Definitions)
            .HasForeignKey(sp => sp.DictionaryId);
    }
    
}

public class Language
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public List<Definition> Definitions { get; set; }
}

