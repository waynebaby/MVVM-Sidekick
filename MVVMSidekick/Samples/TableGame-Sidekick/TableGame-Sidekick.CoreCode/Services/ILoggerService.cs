using System;
using System.Collections.Generic;
using System.Text;

namespace TableGame_Sidekick.Services
{
    public interface ILoggerService
    {
        void Log(string format, params string[] parameters);

    }
}
