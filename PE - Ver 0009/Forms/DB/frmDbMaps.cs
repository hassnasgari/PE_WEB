using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;

namespace PE.Forms
{
    public partial class frmDbMaps : Form
    {
        public frmDbMaps()
        {
            InitializeComponent();
        }

        private void frmDbMaps_Load(object sender, EventArgs e)
        {
            ClsDiff.writeLogOfAction("ورود به صفحه نقشه دیتابیس");
            gMap.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMaps.Instance.Mode = GMap.NET.AccessMode.CacheOnly;
            GMap.NET.MapProviders.OpenStreetMapProvider.UserAgent = "IE";
            gMap.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            GMaps.Instance.OptimizeMapDb(null);
           // gMapControl1.CacheLocation = @"C:\Users\ASG\AppData\Local\GMap.NET\";
            gMap.Zoom = 14;
            gMap.Size = new Size(this.Width, this.Height);
            gMap.ShowCenter = true;

            GMaps.Instance.ImportFromGMDB(@"\\"+ MainForm.StrAccessNetIp + @"\P.E Data\PE\Data.gmdb");
            //GMaps.Instance.ImportFromGMDB(System.IO.Path.GetTempPath() + @"\Data.gmdb");
            //System.IO.Path.GetTempPath()
            try
            {
                gMap.Position = new GMap.NET.PointLatLng(double.Parse(MainForm.StrDbLat), double.Parse(MainForm.StrDbLong));
            }
            catch
            {
                gMap.Position = new GMap.NET.PointLatLng(double.Parse(MainForm.StrDbLat.Replace("/",".").ToString()), double.Parse(MainForm.StrDbLong.Replace("/", ".").ToString()));
            }
            
           
        }
    }
}
