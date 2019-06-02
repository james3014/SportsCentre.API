﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SportsCentre.API.Data;

namespace SportsCentre.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190602191104_AttendeesAddedBookingModel")]
    partial class AttendeesAddedBookingModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("SportsCentre.API.Models.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Attendees");

                    b.Property<DateTime>("BookingDate");

                    b.Property<string>("BookingEmail");

                    b.Property<string>("BookingTime");

                    b.Property<string>("BookingType");

                    b.Property<int?>("ClassId");

                    b.Property<int?>("ClubId");

                    b.Property<string>("ContactNumber");

                    b.Property<int?>("CreatedById");

                    b.Property<string>("Facility");

                    b.Property<string>("Requirements");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("ClubId");

                    b.HasIndex("CreatedById");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("SportsCentre.API.Models.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AttendantId");

                    b.Property<DateTime>("ClassDate");

                    b.Property<string>("ClassName");

                    b.Property<string>("ClassTime");

                    b.Property<double>("Cost");

                    b.Property<string>("Facility");

                    b.Property<int>("MaxAttendees");

                    b.Property<int>("TotalAttendees");

                    b.HasKey("Id");

                    b.HasIndex("AttendantId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("SportsCentre.API.Models.Club", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Owner");

                    b.Property<int>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("SportsCentre.API.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Cost");

                    b.Property<string>("ItemName");

                    b.Property<int?>("OrderId");

                    b.Property<int>("StockLevel");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("SportsCentre.API.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("OrderDate");

                    b.Property<double>("Total");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("SportsCentre.API.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<int?>("ClubId");

                    b.Property<int?>("PaidById");

                    b.Property<DateTime>("PaymentDate");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ClubId");

                    b.HasIndex("PaidById");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("SportsCentre.API.Models.Staff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<DateTime>("HireDate");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("PostCode");

                    b.Property<string>("Role");

                    b.Property<string>("Surname");

                    b.Property<string>("Town");

                    b.HasKey("Id");

                    b.ToTable("Staff");
                });

            modelBuilder.Entity("SportsCentre.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<DateTime>("DateJoined");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<DateTime>("MembershipExpiry");

                    b.Property<string>("MembershipType");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("PostCode");

                    b.Property<string>("Surname");

                    b.Property<string>("Town");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SportsCentre.API.Models.Booking", b =>
                {
                    b.HasOne("SportsCentre.API.Models.Class")
                        .WithMany("Bookings")
                        .HasForeignKey("ClassId");

                    b.HasOne("SportsCentre.API.Models.Club")
                        .WithMany("Bookings")
                        .HasForeignKey("ClubId");

                    b.HasOne("SportsCentre.API.Models.User", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("SportsCentre.API.Models.Class", b =>
                {
                    b.HasOne("SportsCentre.API.Models.Staff", "Attendant")
                        .WithMany()
                        .HasForeignKey("AttendantId");
                });

            modelBuilder.Entity("SportsCentre.API.Models.Item", b =>
                {
                    b.HasOne("SportsCentre.API.Models.Order")
                        .WithMany("Items")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("SportsCentre.API.Models.Payment", b =>
                {
                    b.HasOne("SportsCentre.API.Models.Club")
                        .WithMany("Payments")
                        .HasForeignKey("ClubId");

                    b.HasOne("SportsCentre.API.Models.User", "PaidBy")
                        .WithMany()
                        .HasForeignKey("PaidById");
                });
#pragma warning restore 612, 618
        }
    }
}
