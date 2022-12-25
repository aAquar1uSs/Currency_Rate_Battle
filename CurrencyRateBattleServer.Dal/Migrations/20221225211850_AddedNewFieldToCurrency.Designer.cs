﻿// <auto-generated />
using System;
using CurrencyRateBattleServer.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CurrencyRateBattleServer.Dal.Migrations
{
    [DbContext(typeof(CurrencyRateBattleContext))]
    [Migration("20221225211850_AddedNewFieldToCurrency")]
    partial class AddedNewFieldToCurrency
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CurrencyRateBattleServer.Dal.Entities.AccountDal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Dal.Entities.AccountHistoryDal", b =>
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

            modelBuilder.Entity("CurrencyRateBattleServer.Dal.Entities.CurrencyDal", b =>
                {
                    b.Property<string>("CurrencyName")
                        .HasColumnType("varchar(3)");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("varchar(3)");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(128)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("CurrencyName");

                    b.ToTable("Currency", (string)null);

                    b.HasData(
                        new
                        {
                            CurrencyName = "USD",
                            CurrencyCode = "$",
                            Description = "US Dollar",
                            Rate = 0m,
                            UpdateDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            CurrencyName = "EUR",
                            CurrencyCode = "$",
                            Description = "Euro",
                            Rate = 0m,
                            UpdateDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            CurrencyName = "PLN",
                            CurrencyCode = "zł",
                            Description = "Polish Zlotych",
                            Rate = 0m,
                            UpdateDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            CurrencyName = "GBP",
                            CurrencyCode = "£",
                            Description = "British Pound",
                            Rate = 0m,
                            UpdateDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            CurrencyName = "CHF",
                            CurrencyCode = "Fr",
                            Description = "Swiss Franc",
                            Rate = 0m,
                            UpdateDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Dal.Entities.CurrencyStateDal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("CurrencyExchangeRate")
                        .HasColumnType("numeric");

                    b.Property<string>("CurrencyName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("RoomId", "CurrencyCode")
                        .IsUnique();

                    b.ToTable("CurrencyState", (string)null);
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Dal.Entities.RateDal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("CurrencyName")
                        .IsRequired()
                        .HasColumnType("varchar(3)");

                    b.Property<bool>("IsClosed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsWon")
                        .HasColumnType("boolean");

                    b.Property<decimal?>("Payout")
                        .HasColumnType("numeric");

                    b.Property<decimal>("RateCurrencyExchange")
                        .HasColumnType("numeric");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SetDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("SettleDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CurrencyName");

                    b.HasIndex("RoomId");

                    b.ToTable("Rate", (string)null);
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Dal.Entities.RoomDal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsClosed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.HasKey("Id");

                    b.ToTable("Room", (string)null);
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Dal.Entities.UserDal", b =>
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

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Dal.Entities.AccountDal", b =>
                {
                    b.HasOne("CurrencyRateBattleServer.Dal.Entities.UserDal", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Dal.Entities.AccountHistoryDal", b =>
                {
                    b.HasOne("CurrencyRateBattleServer.Dal.Entities.AccountDal", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CurrencyRateBattleServer.Dal.Entities.RoomDal", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId");

                    b.Navigation("Account");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Dal.Entities.CurrencyStateDal", b =>
                {
                    b.HasOne("CurrencyRateBattleServer.Dal.Entities.RoomDal", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("CurrencyRateBattleServer.Dal.Entities.RateDal", b =>
                {
                    b.HasOne("CurrencyRateBattleServer.Dal.Entities.AccountDal", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CurrencyRateBattleServer.Dal.Entities.CurrencyDal", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CurrencyRateBattleServer.Dal.Entities.RoomDal", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Currency");

                    b.Navigation("Room");
                });
#pragma warning restore 612, 618
        }
    }
}
