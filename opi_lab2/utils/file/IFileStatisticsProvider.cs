using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opi_lab2.utils.file
{
    interface IFileStatisticsProvider
    {
        FileStatistics FullStatistics();
    }
}
