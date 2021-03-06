// <auto-generated />
using Data.BalancerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Data.BalancerService.Migrations
{
    [DbContext(typeof(MigrationContext))]
    partial class MigrationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Data.BalancerService.Objects.Route", b =>
                {
                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ObjectName")
                        .HasColumnType("text");

                    b.Property<string>("ResourceConnection")
                        .HasColumnType("text");

                    b.HasKey("Number");

                    b.ToTable("Route");
                });
#pragma warning restore 612, 618
        }
    }
}
