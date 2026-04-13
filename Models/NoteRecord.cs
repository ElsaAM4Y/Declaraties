using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Declaraties.Models
{
    public class NoteRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }

}
