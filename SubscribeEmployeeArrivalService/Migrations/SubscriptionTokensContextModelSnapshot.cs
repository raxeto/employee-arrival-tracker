﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SubscribeEmployeeArrivalService.Data;

#nullable disable

namespace SubscribeEmployeeArrivalService.Migrations
{
    [DbContext(typeof(SubscriptionTokensContext))]
    partial class SubscriptionTokensContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SubscribeEmployeeArrivalService.Models.Token", b =>
                {
                    b.Property<string>("TokenValue")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2");

                    b.HasKey("TokenValue");

                    b.ToTable("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}