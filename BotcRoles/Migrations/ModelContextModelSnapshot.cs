﻿// <auto-generated />
using System;
using BotcRoles.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BotcRoles.Migrations
{
    [DbContext(typeof(ModelContext))]
    partial class ModelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("BotcRoles.Models.Edition", b =>
                {
                    b.Property<long>("EditionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EditionId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Editions");
                });

            modelBuilder.Entity("BotcRoles.Models.Game", b =>
                {
                    b.Property<long>("GameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DatePlayed")
                        .HasColumnType("TEXT");

                    b.Property<long>("EditionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Notes")
                        .HasColumnType("TEXT");

                    b.Property<long>("StoryTellerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("WinningAlignment")
                        .HasColumnType("INTEGER");

                    b.HasKey("GameId");

                    b.HasIndex("EditionId");

                    b.HasIndex("StoryTellerId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("BotcRoles.Models.Player", b =>
                {
                    b.Property<long>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Pseudo")
                        .HasColumnType("TEXT");

                    b.HasKey("PlayerId");

                    b.HasIndex("Name", "Pseudo")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("BotcRoles.Models.PlayerRoleGame", b =>
                {
                    b.Property<long>("PlayerRoleGameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FinalAlignment")
                        .HasColumnType("INTEGER");

                    b.Property<long>("GameId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("PlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("RoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PlayerRoleGameId");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("RoleId");

                    b.ToTable("PlayerRoleGames");
                });

            modelBuilder.Entity("BotcRoles.Models.Role", b =>
                {
                    b.Property<long>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CharacterType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DefaultAlignment")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("RoleId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BotcRoles.Models.RoleEdition", b =>
                {
                    b.Property<long>("RoleId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("EditionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("RoleId", "EditionId");

                    b.HasIndex("EditionId");

                    b.HasIndex("RoleId", "EditionId")
                        .IsUnique();

                    b.ToTable("RolesEdition");
                });

            modelBuilder.Entity("BotcRoles.Models.Game", b =>
                {
                    b.HasOne("BotcRoles.Models.Edition", "Edition")
                        .WithMany("Games")
                        .HasForeignKey("EditionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BotcRoles.Models.Player", "StoryTeller")
                        .WithMany("GamesStoryTelling")
                        .HasForeignKey("StoryTellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Edition");

                    b.Navigation("StoryTeller");
                });

            modelBuilder.Entity("BotcRoles.Models.PlayerRoleGame", b =>
                {
                    b.HasOne("BotcRoles.Models.Game", "Game")
                        .WithMany("PlayerRoleGames")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BotcRoles.Models.Player", "Player")
                        .WithMany("PlayerRoleGames")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BotcRoles.Models.Role", "Role")
                        .WithMany("PlayerRoleGames")
                        .HasForeignKey("RoleId");

                    b.Navigation("Game");

                    b.Navigation("Player");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BotcRoles.Models.RoleEdition", b =>
                {
                    b.HasOne("BotcRoles.Models.Edition", "Edition")
                        .WithMany("RolesEdition")
                        .HasForeignKey("EditionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BotcRoles.Models.Role", "Role")
                        .WithMany("RolesEdition")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Edition");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BotcRoles.Models.Edition", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("RolesEdition");
                });

            modelBuilder.Entity("BotcRoles.Models.Game", b =>
                {
                    b.Navigation("PlayerRoleGames");
                });

            modelBuilder.Entity("BotcRoles.Models.Player", b =>
                {
                    b.Navigation("GamesStoryTelling");

                    b.Navigation("PlayerRoleGames");
                });

            modelBuilder.Entity("BotcRoles.Models.Role", b =>
                {
                    b.Navigation("PlayerRoleGames");

                    b.Navigation("RolesEdition");
                });
#pragma warning restore 612, 618
        }
    }
}
