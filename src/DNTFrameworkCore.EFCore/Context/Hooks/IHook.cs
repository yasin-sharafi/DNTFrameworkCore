﻿using System;
using DNTFrameworkCore.Dependency;
using Microsoft.EntityFrameworkCore;

namespace DNTFrameworkCore.EFCore.Context.Hooks
{
    /// <summary>
    /// A 'hook' usable for calling at certain point in an entities life cycle.
    /// </summary>
    public interface IHook : ITransientDependency
    {
        int Order { get; }

        /// <summary>
        /// Gets the entity state(s) to listen for.
        /// </summary>
        /// <remarks>The entity state being <see cref="FlagsAttribute"/>, it allows this hook to listen to multiple states.</remarks>
        EntityState HookState { get; }

        /// <summary>
        /// Executes the logic in the hook.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="uow">The current context</param>
        void Hook(object entity, HookEntityMetadata metadata, IUnitOfWork uow);
    }
}