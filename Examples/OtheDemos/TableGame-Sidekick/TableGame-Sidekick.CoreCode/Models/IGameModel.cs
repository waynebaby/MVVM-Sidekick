namespace TableGame_Sidekick.Models
{
    public interface IGameModel<TContext>
    {
        string Description { get; set; }
        TContext GameExecutingContext { get; set; }
        string Name { get; set; }
    }
}