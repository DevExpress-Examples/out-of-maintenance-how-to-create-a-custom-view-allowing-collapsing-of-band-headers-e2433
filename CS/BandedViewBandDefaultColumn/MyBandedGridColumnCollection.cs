using DevExpress.XtraGrid.Views.BandedGrid;

namespace BandedViewBandDefaultColumn
{
	class MyBandedGridColumnCollection : BandedGridColumnCollection
	{
		public MyBandedGridColumnCollection(DevExpress.XtraGrid.Views.Base.ColumnView view)
			: base(view)
		{
		}

		protected override DevExpress.XtraGrid.Columns.GridColumn CreateColumn()
		{
			return new MyBandedGridColumn();
		}
	}
}
