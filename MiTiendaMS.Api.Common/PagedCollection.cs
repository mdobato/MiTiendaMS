using System.Linq;
using System.Collections.Generic;

namespace MiTiendaMS.Api.Common
{
    public class PagedCollection<T>
    {
        public bool HasItems
        {
            get
            {
                return Items != null && Items.Any();
            }
        }

        public IEnumerable<T> Items { get; set; }
        public int Total { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
    }
}
