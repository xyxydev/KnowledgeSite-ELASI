using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class TrainingViewModel
    {
        public int Id { get; set; }
        public string TrainingName { get; set; }
        public string TrainingDesc { get; set; }
        public string TrainingAuthor { get; set; }

        public int CategoryId { get; set; }
    }
}
