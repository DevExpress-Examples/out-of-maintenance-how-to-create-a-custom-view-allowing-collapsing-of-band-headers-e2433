Imports System
Imports System.Data
Imports System.Windows.Forms

Namespace BandedViewBandDefaultColumn
    Partial Public Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
            myGridControl1.DataSource = CreateTable(30)

            CType(myBandedGridView1.Columns("Vendor"), MyBandedGridColumn).DefaultBandColumn = True
            CType(myBandedGridView1.Columns("Model"), MyBandedGridColumn).DefaultBandColumn = True
        End Sub

        Private Function CreateTable(ByVal RowCount As Integer) As DataTable
            Dim tbl As New DataTable()
            tbl.Columns.Add("ID", GetType(Integer))
            tbl.Columns.Add("Vendor", GetType(String))
            tbl.Columns.Add("Model", GetType(String))
            tbl.Columns.Add("ProductNumber", GetType(Integer))


            For i As Integer = 0 To RowCount - 1
                Dim rnd As Integer = (New Random(i)).Next(1, 4)
                tbl.Rows.Add(New Object() { i, String.Format("Vendor {0}", rnd), String.Format("Model {0}", i), i * (New Random()).Next(1, 10000) })
            Next i

            Return tbl
        End Function

    End Class
End Namespace
