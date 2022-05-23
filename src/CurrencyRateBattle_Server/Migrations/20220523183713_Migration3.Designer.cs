﻿// <auto-generated />
using System;
using CurrencyRateBattleServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CurrencyRateBattleServer.Migrations
{
    [DbContext(typeof(CurrencyRateBattleContext))]
    [Migration("20220523183713_Migration3")]
    partial class Migration3
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CurrencyRateBattleServer.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Models.AccountHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsCredit")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("RoomId");

                    b.ToTable("AccountHistory", (string)null);
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Models.Currency", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CurrencyName")
                        .IsRequired()
                        .HasColumnType("varchar(3)");

                    b.Property<string>("CurrencySymbol")
                        .IsRequired()
                        .HasColumnType("varchar(3)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("varchar(128)");

                    b.HasKey("Id");

                    b.ToTable("Currency", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("3dca2505-c97a-4fe0-a586-bf9cdc7a83a8"),
                            CurrencyName = "USD",
                            CurrencySymbol = "$",
                            Description = "US Dollar"
                        },
                        new
                        {
                            Id = new Guid("c3bda91d-816d-4d7e-b3a3-1d828f548fa4"),
                            CurrencyName = "EUR",
                            CurrencySymbol = "$",
                            Description = "Euro"
                        },
                        new
                        {
                            Id = new Guid("0f49f4f4-62f2-4e9a-babc-f4f944b511ba"),
                            CurrencyName = "PLN",
                            CurrencySymbol = "zł",
                            Description = "Polish Zlotych"
                        },
                        new
                        {
                            Id = new Guid("729f0615-a1f3-4bc4-a917-abca615149ee"),
                            CurrencyName = "GBP",
                            CurrencySymbol = "£",
                            Description = "British Pound"
                        },
                        new
                        {
                            Id = new Guid("9638cc7d-48e5-44ef-8a6e-da9cc1c3afaa"),
                            CurrencyName = "CHF",
                            CurrencySymbol = "Fr",
                            Description = "Swiss Franc"
                        });
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Models.CurrencyState", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("CurrencyExchangeRate")
                        .HasColumnType("numeric");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("RoomId");

                    b.ToTable("CurrencyState", (string)null);
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Models.Rate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("CurrencyId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsClosed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsWon")
                        .HasColumnType("boolean");

                    b.Property<decimal?>("Payout")
                        .HasColumnType("numeric");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SetDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("SettleDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("RoomId");

                    b.ToTable("Rate", (string)null);
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Models.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsClosed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.HasKey("Id");

                    b.ToTable("Room", (string)null);
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Models.Account", b =>
                {
                    b.HasOne("CurrencyRateBattleServer.Models.User", "User")
                        .WithOne("Account")
                        .HasForeignKey("CurrencyRateBattleServer.Models.Account", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Models.AccountHistory", b =>
                {
                    b.HasOne("CurrencyRateBattleServer.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CurrencyRateBattleServer.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId");

                    b.Navigation("Account");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Models.CurrencyState", b =>
                {
                    b.HasOne("CurrencyRateBattleServer.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CurrencyRateBattleServer.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Models.Rate", b =>
                {
                    b.HasOne("CurrencyRateBattleServer.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CurrencyRateBattleServer.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CurrencyRateBattleServer.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Currency");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Models.User", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
