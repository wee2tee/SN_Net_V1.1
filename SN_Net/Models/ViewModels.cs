using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SN_Net.Models
{
    public class problemVM
    {
        public int id { get; set; }
        public string probcod { get; set; }
        public string probdesc { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string name { get; set; }

        public problem problem { get; set; }
    }

    public class istabVM
    {
        public const string TABTYP_HOWKNOWN = "03";
        public const string TABTYP_BUSITYP = "04";
        public const string TABTYP_PROBCOD = "05";
        public const string TABTYP_AREA = "06";
        public const string TABTYP_VEREXT = "07";

        public int id { get; set; }
        public string tabtyp { get; set; }
        public string typcod { get; set; }
        public string abbreviate_en { get; set; }
        public string abbreviate_th { get; set; }
        public string typdes_en { get; set; }
        public string typdes_th { get; set; }

        public istab istab { get; set; }
    }
}
