using Store.CoreLayer.Entirty.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.ISepecfication
{
    public class BrandAndCategoryParams
    {
        public Sorting? sort { get; set; } 
        public int PageIndex { get; set; } = 1;

        private int pageSize = 5;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 10 ? 10 : value; }
        }
    }
}
