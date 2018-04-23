Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.XtraGrid.Views.BandedGrid

Namespace BandedViewBandDefaultColumn
	Friend Class MyBandedGridColumnCollection
		Inherits BandedGridColumnCollection
		Public Sub New(ByVal view As DevExpress.XtraGrid.Views.Base.ColumnView)
			MyBase.New(view)
		End Sub

		Protected Overrides Function CreateColumn() As DevExpress.XtraGrid.Columns.GridColumn
			Return New MyBandedGridColumn()
		End Function
	End Class
End Namespace
