using System;
using Discord;

namespace TomatBot.Core.Framework.PunishmentFramework
{
    public abstract class Punishment
    {
        public abstract IUser AssociatedUser { get; }

        public abstract IGuild AssociatedGuild { get; }

        public abstract PunishmentType PunishmentType { get; }

        public abstract DateTime TimeWhenBanned { get; }

        public virtual DateTime? ExpirationTime => null;

        public virtual bool IsTemporary => false;

        public virtual void OnExpiration() { }
    }
}