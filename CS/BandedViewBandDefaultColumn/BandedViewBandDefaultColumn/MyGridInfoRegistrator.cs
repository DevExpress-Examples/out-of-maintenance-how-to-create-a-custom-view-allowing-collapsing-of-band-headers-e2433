using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Base;

namespace BandedViewBandDefaultColumn
{
	public class MyBandedGridInfoRegistrator : BandedGridInfoRegistrator
	{
		public override BaseView CreateView(GridControl grid)
		{
			return new MyBandedGridView(grid);
		}

		public override string ViewName
		{
			get
			{
				return "MyBandedGridView";
			}
		}
	}
}
