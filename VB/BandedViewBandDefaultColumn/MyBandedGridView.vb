Imports Microsoft.VisualBasic
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.Utils
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Drawing
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Drawing
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports DevExpress.XtraGrid.Views.BandedGrid.ViewInfo

Namespace BandedViewBandDefaultColumn
	Public Class MyBandedGridView
		Inherits BandedGridView
		Public Sub New(ByVal ownerGrid As GridControl)
			MyBase.New(ownerGrid)
		End Sub
		Public Sub New()
			MyBase.New()
			AddHandler CustomDrawBandHeader, AddressOf OnCustomDrawBandHeader
			AddHandler MouseMove, AddressOf OnMouseMove
			AddHandler MouseDown, AddressOf OnMouseDown
			AddHandler MouseUp, AddressOf OnMouseUp
		End Sub

		Private Function CalcButtonBounds(ByVal headerBounds As Rectangle) As Rectangle
			Dim buttonBounds As Rectangle = headerBounds
			buttonBounds.Inflate(-1, -1)
			buttonBounds.Width = 21

			Return buttonBounds
		End Function

		Private Function CalcCaptionRectWithButton(ByVal e As BandHeaderCustomDrawEventArgs, ByVal addButtonWidth As Boolean) As Rectangle
			Dim captionRect As Rectangle = e.Info.CaptionRect
			If addButtonWidth Then
				captionRect.X += CalcButtonBounds(e.Bounds).Width
			Else
				captionRect.X -= CalcButtonBounds(e.Bounds).Width
			End If

			Return captionRect
		End Function

		Private Function FindButtonInnerElement(ByVal bandInfo As GridBandInfoArgs) As EditorButtonObjectInfoArgs
			Dim retValue As EditorButtonObjectInfoArgs = Nothing
			For i As Integer = 0 To bandInfo.InnerElements.Count - 1
				If TypeOf bandInfo.InnerElements(i).ElementInfo Is EditorButtonObjectInfoArgs Then
					retValue = CType(bandInfo.InnerElements(i).ElementInfo, EditorButtonObjectInfoArgs)
					Exit For
				End If
			Next i

			Return retValue
		End Function

		Private Function GetBandColumnsDefaultOnlyVisibility(ByVal band As GridBand) As Boolean
			For i As Integer = 0 To band.Columns.Count - 1
				If Not(CType(band.Columns(i), MyBandedGridColumn)).DefaultBandColumn AndAlso (band.Columns(i)).Visible Then
					Return False
				End If
			Next i

			Return True
		End Function

		Private Sub OnCustomDrawBandHeader(ByVal sender As Object, ByVal e As BandHeaderCustomDrawEventArgs)
			Dim buttonElementInfo As DrawElementInfo = Nothing
			For i As Integer = 0 To e.Info.InnerElements.Count - 1
				If TypeOf e.Info.InnerElements(i).ElementInfo Is EditorButtonObjectInfoArgs Then
					buttonElementInfo = e.Info.InnerElements(i)
					Exit For
				End If
			Next i

			If buttonElementInfo Is Nothing Then
				Dim buttonKind As ButtonPredefines = ButtonPredefines.Minus
				If GetBandColumnsDefaultOnlyVisibility(e.Band) Then
					buttonKind = ButtonPredefines.Plus
				End If

				Dim button As New EditorButton(buttonKind)
				Dim buttonPainter As EditorButtonPainter = EditorButtonHelper.GetPainter(BorderStyles.Default)
				Dim buttonInfoArgs As New EditorButtonObjectInfoArgs(e.Cache, button, e.Info.Appearance)

				buttonElementInfo = New DrawElementInfo(buttonPainter, buttonInfoArgs)
				buttonElementInfo.ElementInfo.Bounds = CalcButtonBounds(e.Bounds)
				buttonElementInfo.ElementInfo.State = ObjectState.Normal

				e.Info.InnerElements.Add(buttonElementInfo)
				e.Info.CaptionRect = CalcCaptionRectWithButton(e, True)
			End If

			e.Painter.DrawObject(e.Info)
			e.Handled = True
		End Sub

		Private Sub OnMouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
			If Not(TypeOf sender Is MyBandedGridView) Then
				Return
			End If

			Dim view As MyBandedGridView = CType(sender, MyBandedGridView)
			Dim hitInfo As BandedGridHitInfo = view.CalcHitInfo(e.X, e.Y)

			If hitInfo.HitTest = BandedGridHitTest.Band Then
				Dim bandInfo As GridBandInfoArgs = view.ViewInfo.BandsInfo(hitInfo.Band)
				Dim bandButtonInfo As EditorButtonObjectInfoArgs = FindButtonInnerElement(bandInfo)
				If bandButtonInfo IsNot Nothing Then
					If CalcButtonBounds(bandInfo.Bounds).Contains(e.X, e.Y) Then
						bandButtonInfo.State = ObjectState.Hot
						view.InvalidateBandHeader(hitInfo.Band)

						CType(e, DXMouseEventArgs).Handled = True
					Else
						bandButtonInfo.State = ObjectState.Normal
						view.InvalidateBandHeader(hitInfo.Band)
					End If
				End If
			Else
				For i As Integer = 0 To view.ViewInfo.BandsInfo.Count - 1
					Dim bandButtonInfo As EditorButtonObjectInfoArgs = FindButtonInnerElement(view.ViewInfo.BandsInfo(i))
					If bandButtonInfo IsNot Nothing Then
						bandButtonInfo.State = ObjectState.Normal
					End If
				Next i
			End If
		End Sub

		Private Sub OnMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
			If e.Button <> MouseButtons.Left OrElse Not(TypeOf sender Is MyBandedGridView) Then
				Return
			End If

			Dim view As MyBandedGridView = CType(sender, MyBandedGridView)
			Dim hitInfo As BandedGridHitInfo = view.CalcHitInfo(e.X, e.Y)

			If hitInfo.HitTest = BandedGridHitTest.Band Then
				Dim bandInfo As GridBandInfoArgs = view.ViewInfo.BandsInfo(hitInfo.Band)
				If CalcButtonBounds(bandInfo.Bounds).Contains(e.X, e.Y) Then
					Dim bandButtonInfo As EditorButtonObjectInfoArgs = FindButtonInnerElement(bandInfo)
					If bandButtonInfo IsNot Nothing Then
						bandButtonInfo.State = ObjectState.Pressed
						view.InvalidateBandHeader(hitInfo.Band)
					End If

					CType(e, DXMouseEventArgs).Handled = True
				Else
					view.InvalidateBandHeader(hitInfo.Band)
				End If
			End If
		End Sub

		Private Sub OnMouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
			If e.Button <> MouseButtons.Left OrElse Not(TypeOf sender Is MyBandedGridView) Then
				Return
			End If

			Dim view As MyBandedGridView = CType(sender, MyBandedGridView)
			Dim hitInfo As BandedGridHitInfo = view.CalcHitInfo(e.X, e.Y)

			If hitInfo.HitTest = BandedGridHitTest.Band Then
				Dim bandInfo As GridBandInfoArgs = view.ViewInfo.BandsInfo(hitInfo.Band)
				If CalcButtonBounds(bandInfo.Bounds).Contains(e.X, e.Y) Then
					Dim bandButtonInfo As EditorButtonObjectInfoArgs = FindButtonInnerElement(bandInfo)
					If bandButtonInfo IsNot Nothing Then
						For i As Integer = 0 To hitInfo.Band.Columns.Count - 1
							If Not(CType(hitInfo.Band.Columns(i), MyBandedGridColumn)).DefaultBandColumn Then
								hitInfo.Band.Columns(i).Visible = Not hitInfo.Band.Columns(i).Visible
							End If
						Next i
					End If

					CType(e, DXMouseEventArgs).Handled = True
				End If
			End If
		End Sub

		Protected Overrides Function CreateColumnCollection() As GridColumnCollection
			Return New MyBandedGridColumnCollection(Me)
		End Function

		Protected Overrides ReadOnly Property ViewName() As String
			Get
				Return "MyBandedGridView"
			End Get
		End Property

		Public Shadows ReadOnly Property ViewInfo() As BandedGridViewInfo
			Get
				Return TryCast(MyBase.ViewInfo, BandedGridViewInfo)
			End Get
		End Property
	End Class
End Namespace