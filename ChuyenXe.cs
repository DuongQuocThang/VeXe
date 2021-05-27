using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeXeServer
{
    public class ChuyenXe
    {
        public string TenChuyenXe { get; set; }
        public List<LoaiVe> loaiVeList { get; set; }

        public ChuyenXe()
        {
            loaiVeList = new List<LoaiVe>();
        }
    }
}
