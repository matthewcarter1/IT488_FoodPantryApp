using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp1;

namespace IT488_FoodPantryApp
{
    public partial class Form3 : Form
    {
        private int _itemId;
        private List<Tracking> _trackingData;

        public Form3(int itemId)
        {
            InitializeComponent();

            _itemId = itemId;
            _trackingData = GetTrackingData(_itemId);

            // Get the unit for the item
            string unit;
            using (var dbContext = new FoodPantryDbContext())
            {
                var item = dbContext.Items.FirstOrDefault(i => i.ItemID == itemId);
                unit = item.Unit;
            }

            // Configure chart properties
            chart1.DataSource = _trackingData;
            chart1.Series["Quantity"].XValueMember = "TrackingDate";
            chart1.Series["Quantity"].YValueMembers = "Quantity";
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "MM/dd/yyyy";
            chart1.ChartAreas[0].AxisY.Title = $"Quantity ({unit})";
        }

        private List<Tracking> GetTrackingData(int itemId)
        {
            using (var dbContext = new FoodPantryDbContext())
            {
                return dbContext.Tracking
                    .Where(t => t.ItemID == itemId)
                    .OrderBy(t => t.TrackingDate)
                    .ToList();
            }
        }

    }
}