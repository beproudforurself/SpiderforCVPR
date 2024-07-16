using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderForCVPR.resource
{
    internal class MailFormat
    {
        public List<string>? TO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string>? CC { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? Subject { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? Body { get; set; }
    }
}
