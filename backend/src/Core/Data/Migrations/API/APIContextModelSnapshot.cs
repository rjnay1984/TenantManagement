﻿// <auto-generated />
using Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Core.Data.Migrations.API
{
    [DbContext(typeof(APIContext))]
    partial class APIContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Core.Entities.Building", b =>
                {
                    b.Property<int>("BuildingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .HasColumnType("text");

                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.HasKey("BuildingId");

                    b.ToTable("Buildings");
                });

            modelBuilder.Entity("Core.Entities.Unit", b =>
                {
                    b.Property<int>("UnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Bathrooms")
                        .HasColumnType("integer");

                    b.Property<int>("Bedrooms")
                        .HasColumnType("integer");

                    b.Property<int>("BuildingId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("UnitNumber")
                        .HasColumnType("text");

                    b.HasKey("UnitId");

                    b.HasIndex("BuildingId");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("Core.Entities.Unit", b =>
                {
                    b.HasOne("Core.Entities.Building", "Building")
                        .WithMany("Units")
                        .HasForeignKey("BuildingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Building");
                });

            modelBuilder.Entity("Core.Entities.Building", b =>
                {
                    b.Navigation("Units");
                });
#pragma warning restore 612, 618
        }
    }
}
