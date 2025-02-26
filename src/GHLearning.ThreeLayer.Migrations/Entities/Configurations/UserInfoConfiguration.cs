﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using GHLearning.ThreeLayer.Migrations.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

#nullable disable

namespace GHLearning.ThreeLayer.Migrations.Entities.Configurations
{
    public partial class UserInfoConfiguration : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> entity)
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity
                .ToTable("user_infos", tb => tb.HasComment("使用者資訊"))
                .UseCollation("utf8mb4_general_ci");

            entity.Property(e => e.UserId)
                .HasMaxLength(15)
                .HasComment("使用者主表識別碼")
                .HasColumnName("user_id");
            entity.Property(e => e.NickName)
                .HasMaxLength(63)
                .HasComment("暱稱")
                .HasColumnName("nick_name");

            entity.HasOne(d => d.User).WithOne(p => p.UserInfo)
                .HasForeignKey<UserInfo>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_infos_users");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<UserInfo> entity);
    }
}
