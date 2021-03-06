﻿// <auto-generated />
using CustomerApi.Models.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CustomerApi.Migrations
{
    [DbContext(typeof(CustomerSQLiteDatabaseContext))]
    partial class CustomerSQLiteDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846");

            modelBuilder.Entity("CustomerApi.Models.SQLite.CustomerRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Age");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("CustomerApi.Models.SQLite.PhoneRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AreaCode");

                    b.Property<long>("CustomerId");

                    b.Property<int>("Number");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("PhoneRecord");
                });

            modelBuilder.Entity("CustomerApi.Models.SQLite.PhoneRecord", b =>
                {
                    b.HasOne("CustomerApi.Models.SQLite.CustomerRecord", "Customer")
                        .WithMany("Phones")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
