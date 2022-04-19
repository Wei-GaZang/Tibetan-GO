using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace Stone
{
    public partial class stone : PictureBox
    {
        public stone()
        {
            InitializeComponent();
            this.Width = 51;
            this.Height = 51;
            this.BackColor = Color.Transparent;
        }
        /// </summary>
        /// 当前棋子在数组里X坐标
        /// </summary>

        public int GridX { get; set; }

        /// <summary>
        /// 当前棋子在数组里Y坐标
        /// </summary>
        public int GridY { get; set; }
        public enum ChessType
        {
            Empty = 0,//空子
            Black = 1,//黑子
            White = 2,//白子
        }
        /// <summary>
        /// 当前棋子的类型
        /// </summary>
        public stone.ChessType type { get; set; }
        public int Lis;
        protected override void OnCreateControl()
        {
            Rectangle rec = new Rectangle(0, 0, 51, 51);
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(rec);
            // gp.AddEllipse(this.ClientRectangle);
            Region region = new Region(gp);
            this.Region = region;
            gp.Dispose();
            region.Dispose();
            base.OnCreateControl();
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Rectangle rec = new Rectangle(0, 0, 51, 51);
            var g = pe.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.DrawEllipse(Pens.White, rec);
        }
        
    }
}
