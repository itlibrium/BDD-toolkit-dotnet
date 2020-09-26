using System.Threading.Tasks;

namespace ITLIBRIUM.BddToolkit.Examples
{
    public interface CommandHandler<in TCommand, TResult>
        where TCommand : Command
    {
        Task<TResult> Handle(TCommand command);
    }
}