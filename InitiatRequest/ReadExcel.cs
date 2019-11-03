using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InitiatRequest
{
    public partial class ReadExcel : CCWin.Skin_Mac
    {
        public ReadExcel(Dictionary<string, string> dor)
        {
            InitializeComponent();

            foreach (var item in dor)
            {
                SkinListBoxItem sitem = new SkinListBoxItem();
                sitem.Text = item.Key;
                ExcelList.Items.Add(sitem);
            }
            ExcelList.SelectedValueChanged += (a, b) =>
            {
                SkinListBox box = (SkinListBox)a;
                actionName.Text = dor[box.Text];
            };
        }


    }
}
