Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.XtraGrid.Views.BandedGrid

Namespace BandedViewBandDefaultColumn
	Friend Class MyBandedGridColumn
		Inherits BandedGridColumn
		Private defaultBandColumn_Renamed As Boolean

		Public Sub New()
			MyBase.New()
		End Sub

		Public Property DefaultBandColumn() As Boolean
			Get
				Return defaultBandColumn_Renamed
			End Get
			Set(ByVal value As Boolean)
				defaultBandColumn_Renamed = value
			End Set
		End Property
	End Class
End Namespace
