#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

namespace Tomat.TomatBot.Core.Services.Commands
{
    public readonly struct CommandData
    {
        public readonly string CommandName;
        public readonly string[] CommandAliases;
        public readonly string CommandParameters;
        public readonly string CommandDescription;

        public CommandData(string commandName, string[] commandAliases, string commandParameters,
            string commandDescription)
        {
            CommandName = commandName;
            CommandAliases = commandAliases;
            CommandParameters = commandParameters;
            CommandDescription = commandDescription;
        }

        public override string ToString() => CommandName;
    }
}