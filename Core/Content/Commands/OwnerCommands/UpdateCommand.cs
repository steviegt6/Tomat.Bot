using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using TomatBot.Core.Framework.CommandFramework;
using TomatBot.Core.Framework.DataStructures;

namespace TomatBot.Core.Content.Commands.OwnerCommands
{
    public class UpdateCommand : TomatCommand
    {
        //TODO: Change this Stevie, I don't know how your Framework works
        public override MethodInfo? AssociatedMethod { get; }
        public override HelpCommandData HelpData { get; }
        public override CommandType CType { get; }

        [Command()]
        public async Task UpdateCommandAsync()
        {
            
        }
    }
}