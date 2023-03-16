using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace opi_lab2.web
{
    public struct MoodleLink : ILink
    {
        public string url;
        public string header;
        public string annotation;

        public MoodleLink(string url, string header, string annotation)
        {
            this.url = url;
            this.header = header;
            this.annotation = annotation;
        }

        public MainWindow MainWindow
        {
            get => default;
            set
            {
            }
        }

        public override string ToString()
        {
            return $"URL: {url}\nHeader: {header}\nAnndotation: {annotation}\n";
        }
    }
}
