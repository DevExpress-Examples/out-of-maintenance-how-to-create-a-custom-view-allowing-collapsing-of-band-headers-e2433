Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Registrator
Imports DevExpress.XtraGrid.Views.Base

Namespace BandedViewBandDefaultColumn
	Public Class MyBandedGridInfoRegistrator
		Inherits BandedGridInfoRegistrator
		Public Overrides Function CreateView(ByVal grid As GridControl) As BaseView
			Return New MyBandedGridView(grid)
		End Function

		Public Overrides ReadOnly Property ViewName() As String
			Get
				Return "MyBandedGridView"
			End Get
		End Property
	End Class
End Namespace
