﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace GHLearning.ThreeLayer.Repositories.Entities;

/// <summary>
/// 使用者資訊
/// </summary>
public partial class UserInfo
{
    /// <summary>
    /// 使用者主表識別碼
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// 暱稱
    /// </summary>
    public string NickName { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}