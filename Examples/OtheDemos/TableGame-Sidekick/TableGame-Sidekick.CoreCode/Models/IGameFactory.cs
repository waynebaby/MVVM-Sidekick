using System;
using System.Collections.Generic;
using System.Text;

namespace TableGame_Sidekick.Models
{
    public interface  IGameFactory	<TProduct>
    {
		TProduct Create();

    }
}
