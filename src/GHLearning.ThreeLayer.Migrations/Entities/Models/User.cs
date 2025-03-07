﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace GHLearning.ThreeLayer.Migrations.Entities;

/// <summary>
/// 使用者主表
/// </summary>
public partial class User
{
    /// <summary>
    /// 識別碼
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// 帳號名稱
    /// </summary>
    public string Account { get; set; } = null!;

    /// <summary>
    /// 密碼
    /// </summary>
    public string Password { get; set; } = null!;

    /// <summary>
    /// 創建時間
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 最後更新時間
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    public virtual UserInfo? UserInfo { get; set; }

    public virtual ICollection<UserLog> UserLogs { get; set; } = new List<UserLog>();

    public virtual UserStatus? UserStatus { get; set; }

    public virtual UserVipLevel? UserVipLevel { get; set; }
}