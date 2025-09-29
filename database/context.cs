using Microsoft.EntityFrameworkCore;
namespace DataBase;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("DataSource=phones.db");
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Phone> Phones { get; set; }
    public DbSet<StringPropertyType> StringPropertyTypes { get; set; }
    public DbSet<LongPropertyType> LongPropertyTypes { get; set; }
    public DbSet<DoublePropertyType> DoublePropertyTypes { get; set; }
    public DbSet<BooleanPropertyType> BooleanPropertyTypes { get; set; }
    public DbSet<DatePropertyType> DatePropertyTypes { get; set; }
    
   
    public DbSet<SectionType> SectionTypes { get; set; }

    public required DbSet<PhoneReview> PhoneReviews { get; set; }

    public DbSet<Section> Sections { get; set; }

    public DbSet<StringProperty> StringProperties { get; set; }
    public DbSet<LongProperty> LongProperties { get; set; }
    public DbSet<DoubleProperty> DoubleProperties { get; set; }
    public DbSet<BooleanProperty> BooleanProperties { get; set; }
    public DbSet<DateProperty> DateProperties { get; set; }
    
  
} 