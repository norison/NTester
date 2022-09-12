﻿using Microsoft.EntityFrameworkCore;
using NTester.DataAccess.Entities;

namespace NTester.DataAccess.Data;

/// <summary>
/// Database context of the NTester application.
/// </summary>
public interface INTesterDbContext
{
    /// <inheritdoc cref="DbSet{TEntity}"/>
    DbSet<TestEntity> Tests { get; set; }

    /// <inheritdoc cref="DbSet{QuestionEntity}"/>
    DbSet<QuestionEntity> Questions { get; set; }

    /// <inheritdoc cref="DbSet{AnswerEntity}"/>
    DbSet<AnswerEntity> Answers { get; set; }

    /// <inheritdoc cref="DbContext.SaveChangesAsync(System.Threading.CancellationToken)"/>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}