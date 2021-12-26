﻿// <auto-generated />
using Data.Balancer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Data.Balancer.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20211226194647_TestToRouteAdded")]
    partial class TestToRouteAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Data.Balancer.Objects.Route", b =>
                {
                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ObjectName")
                        .HasColumnType("text");

                    b.Property<string>("ResourceConnection")
                        .HasColumnType("text");

                    b.Property<string>("Test")
                        .HasColumnType("text");

                    b.HasKey("Number");

                    b.ToTable("Route");
                });
#pragma warning restore 612, 618
        }
    }
}
