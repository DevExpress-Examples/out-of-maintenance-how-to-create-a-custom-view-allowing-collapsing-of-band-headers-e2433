using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Base;

namespace BandedViewBandDefaultColumn
{
	public class MyGridControl : GridControl
	{
		public MyGridControl()
			: base()
		{
		}

		protected override BaseView CreateDefaultView()
		{
			return CreateView("MyBandedGridView");
		}
		
		protected override void RegisterAvailableViewsCore(InfoCollection collection)
		{
			base.RegisterAvailableViewsCore(collection);
			collection.Add(new MyBandedGridInfoRegistrator());
		}
	}

}
