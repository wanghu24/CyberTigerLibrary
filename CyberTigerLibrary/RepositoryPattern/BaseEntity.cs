using System;
using System.Collections.Generic;
using System.Text;

namespace CyberTigerLibrary.RepositoryPattern
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public Guid Uid { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
