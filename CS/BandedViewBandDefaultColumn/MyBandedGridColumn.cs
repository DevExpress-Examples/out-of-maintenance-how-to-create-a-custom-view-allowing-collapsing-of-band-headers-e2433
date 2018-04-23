using DevExpress.XtraGrid.Views.BandedGrid;

namespace BandedViewBandDefaultColumn
{
	class MyBandedGridColumn : BandedGridColumn
	{
		private bool defaultBandColumn;

		public MyBandedGridColumn()
			: base()
		{
		}

		public bool DefaultBandColumn
		{
			get
			{
				return defaultBandColumn;
			}
			set
			{
				defaultBandColumn = value;
			}
		}
	}
}
