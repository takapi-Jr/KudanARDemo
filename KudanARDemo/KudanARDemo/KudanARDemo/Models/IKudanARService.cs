using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KudanARDemo.Models
{
    public interface IKudanARService
    {
        Task Init();
        Task StartMarkerARActivityAsync();
        Task StartMarkerlessARActivityAsync();
        Task StartMarkerlessWallActivityAsync();
    }
}
