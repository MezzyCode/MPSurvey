﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Model.Models
{
    public partial class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Answer2> Answer2s { get; set; } = null!;
        public virtual DbSet<HelperTable> HelperTables { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<CrudLog> CrudLogs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=DB_MPSurvey;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Alamat).IsUnicode(false);
                entity.Property(e => e.C1)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.C10)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.C2)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.C3A)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.C3B)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.C4)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.C5)
                    .IsUnicode(false);
                entity.Property(e => e.C6)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.C7)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.C8)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.C9)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ClientID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(300)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedTime).HasColumnType("datetime");
                entity.Property(e => e.Kecamatan)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Kelurahan)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(300)
                    .IsUnicode(false);
                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");
                entity.Property(e => e.Nama)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Nama_kk)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Rt)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.Rw)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                entity.Property(e => e.TimeStatus)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<Answer2>(entity =>
            {
                entity.ToTable("Answer2");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Alamat).IsUnicode(false);

                entity.Property(e => e.C1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.C2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.C3)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.C4)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClientID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Kecamatan)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Kelurahan)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Kota)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.Nama)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NIK)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.NoTelp)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rt)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Rw)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Simpul)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStatus)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<CrudLog>(entity =>
            {
                entity.ToTable("CrudLog");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClientID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Data).IsUnicode(false);

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.ProcessName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks).IsUnicode(false);

                entity.Property(e => e.TableName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TimeStatus)
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<HelperTable>(entity =>
            {
                entity.ToTable("HelperTable");

                entity.Property(e => e.ID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.ClientID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedBy).IsUnicode(false);
                entity.Property(e => e.CreatedTime).HasColumnType("datetime");
                entity.Property(e => e.Description)
                    .IsUnicode(false);
                entity.Property(e => e.Description2)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");
                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.TimeStatus)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                entity.Property(e => e.Value)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                //entity.Property(e => e.Id)
                //    .HasMaxLength(50)
                //    .IsUnicode(false)
                //    .HasColumnName("ID");
                entity.Property(e => e.ClientID)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.CreatedBy).IsUnicode(false);
                entity.Property(e => e.CreatedTime).HasColumnType("datetime");
                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.LastModifiedBy).IsUnicode(false);
                entity.Property(e => e.LastModifiedTime).HasColumnType("datetime");
                entity.Property(e => e.Name).IsUnicode(false);
                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .IsUnicode(false);
                entity.Property(e => e.TimeStatus)
                    .IsRowVersion()
                    .IsConcurrencyToken();
                //entity.Property(e => e.Username)
                //    .HasMaxLength(50)
                //    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}