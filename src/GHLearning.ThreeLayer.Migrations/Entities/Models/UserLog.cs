﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace GHLearning.ThreeLayer.Migrations.Entities;

public partial class UserLog
{
    /// <summary>
    /// 識別碼
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 使用者識別碼
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 事件
    /// </summary>
    public string Event { get; set; } = null!;

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// 日誌時間
    /// </summary>
    public DateTime CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}