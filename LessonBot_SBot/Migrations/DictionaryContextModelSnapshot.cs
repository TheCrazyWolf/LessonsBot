﻿// <auto-generated />
using LessonBot_SBot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LessonBot_SBot.Migrations
{
    [DbContext(typeof(DictionaryContext))]
    partial class DictionaryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("LessonBot_SBot.Model.Dictionary", b =>
                {
                    b.Property<long>("IdWord")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdWord");

                    b.ToTable("Dictionaries");
                });
#pragma warning restore 612, 618
        }
    }
}
