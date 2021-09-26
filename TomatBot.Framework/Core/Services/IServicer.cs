#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Tomat.TomatBot.Core.Services
{
    public interface IServicer
    {
        IServiceCollection Services { get; }

        ServiceProvider ServiceProvider { get; }
    }
}