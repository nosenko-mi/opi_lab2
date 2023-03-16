using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opi_lab2.web
{
    public interface ILinkScrapper
    {
        List<MoodleLink> GetNews(int amount);
    }
}
