// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyInvest.Persistence;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MyInvest.Migrations
{
    [DbContext(typeof(MyInvestDbContext))]
    [Migration("20220518201645_AddClientAddress")]
    partial class AddClientAddress
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MyInvest.Persistence.Accounts.InvestmentAccountEntity", b =>
                {
                    b.Property<Guid>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("account_id");

                    b.Property<string>("AccountStatus")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric")
                        .HasColumnName("balance");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid")
                        .HasColumnName("client_id");

                    b.HasKey("AccountId");

                    b.HasIndex("ClientId");

                    b.HasIndex("ClientId", "AccountType")
                        .IsUnique();

                    b.ToTable("investment_accounts");
                });

            modelBuilder.Entity("MyInvest.Persistence.Clients.ClientEntity", b =>
                {
                    b.Property<Guid>("ClientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("client_id");

                    b.Property<bool>("AddressIsVerified")
                        .HasColumnType("boolean")
                        .HasColumnName("address_is_verified");

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("address_line_1");

                    b.Property<string>("AddressLine2")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("address_line_2");

                    b.Property<string>("AddressPostcode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("address_postcode");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("ClientId");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("clients");
                });
#pragma warning restore 612, 618
        }
    }
}
