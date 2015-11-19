using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdd.Services
{
    public interface IGameRoundService
    {

        Task<bool> ProcessRoundAsync(string roomId);
    }
}
