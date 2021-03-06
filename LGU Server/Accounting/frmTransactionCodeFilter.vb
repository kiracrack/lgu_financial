﻿Imports DevExpress.XtraEditors
Imports MySql.Data.MySqlClient
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraReports.UI
Imports DevExpress.Skins
Imports DevExpress.XtraSplashScreen

Public Class frmTransactionCodeFilter
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        If keyData = (Keys.Escape) Then
            Me.Close()

        End If
        Return ProcessCmdKey
    End Function

    Private Sub frmTransactionCodeFilter_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        rst.Close()
    End Sub

    Private Sub frmGLAccountsFilter_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SkinManager.EnableMdiFormSkins() : SetIcon(Me)
        LoadCategory()
        ShowUnfilteredProducts()
        ShowfilteredProducts()

    End Sub

    Public Sub LoadCategory()
        LoadXgridLookupSearch("select accesscode,description 'Select Permission' from tblclientaccess order by description asc", "tblclientaccess", txtPermission, gvpermission, Me)
        gvpermission.Columns("accesscode").Visible = False
    End Sub

    Private Sub txtprocat_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPermission.EditValueChanged
        On Error Resume Next
        Dim iCurrentRow As Integer = CInt(txtPermission.Properties.View.FocusedRowHandle.ToString)
        permissioncode.Text = txtPermission.Properties.View.GetFocusedRowCellValue("accesscode").ToString()
        ShowUnfilteredProducts()
        ShowfilteredProducts()
    End Sub

    Public Sub ShowUnfilteredProducts()
        Dim productquery As String = ""
        com.CommandText = "select itemcode from tblglaccountfilter where permissioncode='" & permissioncode.Text & "'" : rst = com.ExecuteReader
        While rst.Read
            productquery = productquery + "'" & rst("itemcode").ToString & "',"
        End While
        rst.Close()
        If productquery.Length > 0 Then
            productquery = productquery.Remove(productquery.Length - 1, 1)
            productquery = "where itemcode not in (" & productquery & ")"
        End If
        If permissioncode.Text <> "" Then
            LoadXgrid("select itemcode, itemname as 'Item Name' from tblglitem " & productquery & " ", "tblglitem", Em_unfiltered, GridView1, Me)
            GridView1.Columns("itemcode").Visible = False
            XgridColMemo({"Item Name"}, GridView1)
            XgridColWidth({"Item Name"}, GridView1, Em_unfiltered.Width)
        End If
    End Sub

    Public Sub ShowfilteredProducts()
        If permissioncode.Text <> "" Then
            LoadXgrid("select itemcode, (select itemname from tblglitem where itemcode= tblglaccountfilter.itemcode) as 'Item Name' from tblglaccountfilter where permissioncode='" & permissioncode.Text & "' order by itemcode asc", "tblglaccountfilter", Em_filtered, GridView2, Me)
            GridView2.Columns("itemcode").Visible = False
            XgridColMemo({"Item Name"}, GridView2)
            XgridColWidth({"Item Name"}, GridView2, Em_filtered.Width)
        End If
    End Sub

    Private Sub BarLargeButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarLargeButtonItem2.ItemClick
        Me.Close()
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles cmdMoveRight.Click
        For I = 0 To GridView1.SelectedRowsCount - 1
            com.CommandText = "insert into tblglaccountfilter set permissioncode='" & permissioncode.Text & "', itemcode='" & GridView1.GetRowCellValue(GridView1.GetSelectedRows(I), "itemcode").ToString & "'" : com.ExecuteNonQuery()
        Next
        ShowUnfilteredProducts()
        ShowfilteredProducts()
    End Sub

    Private Sub SimpleButton2_Click(sender As Object, e As EventArgs) Handles cmdMoveLeft.Click
        For I = 0 To GridView2.SelectedRowsCount - 1
            com.CommandText = "delete from tblglaccountfilter where itemcode='" & GridView2.GetRowCellValue(GridView2.GetSelectedRows(I), "itemcode").ToString & "' and permissioncode='" & permissioncode.Text & "'" : com.ExecuteNonQuery()
        Next
        ShowUnfilteredProducts()
        ShowfilteredProducts()
    End Sub


End Class

