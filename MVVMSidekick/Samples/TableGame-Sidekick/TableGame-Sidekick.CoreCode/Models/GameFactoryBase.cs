using System;
using System.Collections.Generic;
using System.Text;

namespace TableGame_Sidekick.Models
{
    public abstract class GameFactoryBase<TContext>
    {
        Action<GameBuilder<TContext>> _buildAction;
        protected abstract void CofigureGameFactory(Action<GameBuilder<TContext>> buildAction);

        

    }
}
