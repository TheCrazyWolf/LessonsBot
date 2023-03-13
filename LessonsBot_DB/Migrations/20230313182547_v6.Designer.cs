﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LessonsBot_DB.Migrations
{
    [DbContext(typeof(DbProvider))]
    [Migration("20230313182547_v6")]
    partial class v6
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.3");

            modelBuilder.Entity("LessonsBot_DB.ModelService.ApiGroups", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("GroupsCache");
                });

            modelBuilder.Entity("LessonsBot_DB.ModelService.ApiTeacher", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("TEXT");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("TeacherCaches");
                });

            modelBuilder.Entity("LessonsBot_DB.ModelsDb.Bot", b =>
                {
                    b.Property<Guid>("IdBot")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<long?>("IdValueService")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimeOutResponce")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdBot");

                    b.ToTable("Bots");
                });

            modelBuilder.Entity("LessonsBot_DB.ModelsDb.Dicktionary", b =>
                {
                    b.Property<Guid>("IdDicktionary")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdDicktionary");

                    b.ToTable("Dicktionaries");
                });

            modelBuilder.Entity("LessonsBot_DB.ModelsDb.PeerProp", b =>
                {
                    b.Property<Guid>("IdPeerProp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("BotIdBot")
                        .HasColumnType("TEXT");

                    b.Property<long>("IdPeer")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastResult")
                        .HasColumnType("TEXT");

                    b.Property<int>("TypeLesson")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdPeerProp");

                    b.HasIndex("BotIdBot");

                    b.ToTable("PeerProps");
                });

            modelBuilder.Entity("LessonsBot_DB.ModelsDb.PeerProp", b =>
                {
                    b.HasOne("LessonsBot_DB.ModelsDb.Bot", null)
                        .WithMany("PeerProps")
                        .HasForeignKey("BotIdBot");
                });

            modelBuilder.Entity("LessonsBot_DB.ModelsDb.Bot", b =>
                {
                    b.Navigation("PeerProps");
                });
#pragma warning restore 612, 618
        }
    }
}
