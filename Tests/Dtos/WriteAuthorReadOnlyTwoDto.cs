using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Tests.Dtos
{
    public class WriteAuthorReadOnlyTwoDto
    {
        public int AuthorId { get; set; }

        public string Name { get; set; }

        [ReadOnly(true)]
        public string Email { get; set; }
    }
}
