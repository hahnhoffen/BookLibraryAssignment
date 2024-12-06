﻿// <auto-generated />
using System;
using BookLibrary.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookLibrary.Infrastructure.Migrations
{
    [DbContext(typeof(RealDataBase))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookLibrary.Domain.Entities.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = new Guid("ad789cfb-4bfc-48a6-87ae-0dcc9037dcb9"),
                            Name = "J.K. Rowling"
                        },
                        new
                        {
                            Id = new Guid("a362ed9e-46fd-40e8-b6cf-8a945993e25e"),
                            Name = "George R.R. Martin"
                        });
                });

            modelBuilder.Entity("BookLibrary.Domain.Entities.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = new Guid("e36660d0-156a-4d5b-ac1c-f05941966d48"),
                            AuthorId = new Guid("ad789cfb-4bfc-48a6-87ae-0dcc9037dcb9"),
                            Title = "Harry Potter",
                            Year = 1997
                        },
                        new
                        {
                            Id = new Guid("fe57c76f-a346-4556-aad9-635fd67081cc"),
                            AuthorId = new Guid("a362ed9e-46fd-40e8-b6cf-8a945993e25e"),
                            Title = "Game of Thrones",
                            Year = 1996
                        });
                });

            modelBuilder.Entity("BookLibrary.Domain.Entities.Book", b =>
                {
                    b.HasOne("BookLibrary.Domain.Entities.Author", null)
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookLibrary.Domain.Entities.Author", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
