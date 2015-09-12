using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableGame_Sidekick.CoreCode.Services
{
    public interface IEntityFileService
    {
        Task SaveToFile<TEntity>(TEntity entity, string key);
        Task<TEntity> LoadToFile<TEntity>(string key);
    }
}
