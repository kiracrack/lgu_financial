﻿Imports MySql.Data.MySqlClient ' this is to import MySQL.NET
Imports System.Drawing
Imports System.IO
Imports System.Drawing.Printing
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraSplashScreen
Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Base

Public Class frmNewVoucher
    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean

        Return ProcessCmdKey
    End Function

    Private Sub frmRequisitionList_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

    Private Sub frmRequisitionList_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = ico
        ApplySystemTheme(ToolStrip1)
        ViewList()

    End Sub

    Private Sub txtSearchBar_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtSearchBar.KeyPress
        If e.KeyChar() = Chr(13) Then
            If txtSearchBar.Text = "" Then Exit Sub
            ViewList()
        End If

    End Sub

    Public Sub ViewList()
        LoadXgrid("SELECT periodcode,officeid,fundcode,payee, yeartrn, pid as 'Entry Code', if(cancelled,'CANCELLED',if(approved,'APPROVED',if(draft,'DRAFT',if(hold,'HOLD',if(forapproval,'FOR APPROVAL','-'))))) as Status, " _
                        + " requestno as 'Request No.', " _
                        + " (select description from tblrequisitiontype where code=a.requesttype) as 'Request Type' ," _
                        + " (select officename from tblcompoffice where officeid=a.officeid) as 'Requesting Office', " _
                        + " concat((select codename from tblfund where code=a.fundcode),'-',yeartrn) as 'Fund Period',  " _
                        + " date_format(postingdate,'%Y-%m-%d') as 'Posting Date', " _
                        + " (select fullname from tblaccounts where accountid=a.requestedby) as 'Requested By', " _
                        + " (select suppliername from tblsupplier where supplierid = a.payee) as 'Payee''s Name', " _
                        + " (select sum(amount) from tblrequisitionfund where pid=a.pid) as 'Total Amount', " _
                        + " Purpose, " _
                        + " Priority, " _
                        + " (select fullname from tblaccounts where accountid=a.trnby) as 'Posted By', " _
                        + " date_format(datetrn,'%Y-%m-%d') as 'Date Posted', " _
                        + " date_format(dateapproved,'%Y-%m-%d') as 'Date Approved' " _
                        + " FROM tblrequisition as a " _
                        + " where approved=1 and cancelled=0 and voucher=0 and requesttype in (select code from tblrequisitiontype where enablevoucher=1) and " _
                        + " (requestno Like '%" & rchar(txtSearchBar.Text) & "%' or " _
                        + " postingdate like '%" & rchar(txtSearchBar.Text) & "%' or " _
                        + " (select description from tblrequisitiontype where code=a.requesttype) like '%" & rchar(txtSearchBar.Text) & "%' or " _
                        + " Purpose like '%" & rchar(txtSearchBar.Text) & "%')  " _
                        + " order by requestno asc", "tblrequisition", Em, GridView1, Me)


        XgridColCurrency({"Total Amount"}, GridView1)
        XgridColAlign({"Status", "Entry Code", "Fund Period", "Posting Date", "Date Posted", "Draft", "ForApproval", "Approved", "Date Approved", "Cancelled", "Date Cancelled"}, GridView1, DevExpress.Utils.HorzAlignment.Center)
        XgridGeneralSummaryCurrency({"Total Amount"}, GridView1)

        DXgridColumnIndexing(Me.Name, GridView1)
        SaveFilterColumn(GridView1, Me.Text)
        XgridHideColumn({"periodcode", "officeid", "fundcode", "payee", "yeartrn"}, GridView1)
    End Sub


    Private Sub cmdView_Click(sender As Object, e As EventArgs) Handles cmdView.Click
        If XtraMessageBox.Show("Are sure you want to proceed for DV preparation for selected request?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            For I = 0 To GridView1.SelectedRowsCount - 1
                Dim voucherid As String = GetTransactionSeries("disbursement")
                com.CommandText = "insert into tbldisbursementvoucher set " _
                      + " voucherid='" & voucherid & "', " _
                      + " pid='" & GridView1.GetRowCellValue(GridView1.GetSelectedRows(I), "Entry Code").ToString & "', " _
                      + " fundcode='" & GridView1.GetRowCellValue(GridView1.GetSelectedRows(I), "fundcode").ToString & "', " _
                      + " periodcode='" & GridView1.GetRowCellValue(GridView1.GetSelectedRows(I), "periodcode").ToString & "', " _
                      + " yearcode='" & GridView1.GetRowCellValue(GridView1.GetSelectedRows(I), "yeartrn").ToString & "', " _
                      + " yeartrn='" & GridView1.GetRowCellValue(GridView1.GetSelectedRows(I), "yeartrn").ToString & "', " _
                      + " voucherdate=current_date, " _
                      + " supplierid='" & GridView1.GetRowCellValue(GridView1.GetSelectedRows(I), "payee").ToString & "', " _
                      + " officeid='" & GridView1.GetRowCellValue(GridView1.GetSelectedRows(I), "officeid").ToString & "', " _
                      + " amount='" & Val(CC(GridView1.GetRowCellValue(GridView1.GetSelectedRows(I), "Total Amount").ToString)) & "', " _
                      + " trnby='" & globaluserid & "', " _
                      + " datetrn=current_timestamp " : com.ExecuteNonQuery()
                com.CommandText = "UPDATE tblrequisition set voucher=1 where pid='" & GridView1.GetRowCellValue(GridView1.GetSelectedRows(I), "Entry Code").ToString & "'" : com.ExecuteNonQuery()
            Next
            ViewList()
            XtraMessageBox.Show("Voucher successfull created!", GlobalOrganizationName, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub gridview1_RowCellStyle(ByVal sender As Object, ByVal e As RowCellStyleEventArgs) Handles GridView1.RowCellStyle
        Dim View As GridView = sender
        Dim status As String = View.GetRowCellDisplayText(e.RowHandle, View.Columns("Status"))
        If status = "CANCELLED" Then
            e.Appearance.ForeColor = Color.Red
            e.Appearance.Font = New Font(gen_fontfamily, gen_FontSize, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, (CByte(204)))
        Else
            If e.Column.Name = "colStatus" Then
                If status = "FOR APPROVAL" Or status = "DRAFT" Then
                    e.Appearance.BackColor = Color.Orange
                    e.Appearance.BackColor2 = Color.Orange
                    e.Appearance.ForeColor = Color.Black

                ElseIf status = "HOLD" Then
                    e.Appearance.BackColor = Color.Red
                    e.Appearance.BackColor2 = Color.Red
                    e.Appearance.ForeColor = Color.White

                ElseIf status = "APPROVED" Then
                    e.Appearance.BackColor = Color.Green
                    e.Appearance.BackColor2 = Color.Green
                    e.Appearance.ForeColor = Color.White
                End If
            End If
        End If


        If e.Column.Name = "colPriority" Then
            Dim priority As String = View.GetRowCellDisplayText(e.RowHandle, View.Columns("Priority"))
            If priority = "Emergency" Then
                e.Appearance.BackColor = Color.Red
                e.Appearance.BackColor2 = Color.Red
                e.Appearance.ForeColor = Color.White
            End If
        End If
    End Sub

    Private Sub GridView1_DragObjectDrop(sender As Object, e As DevExpress.XtraGrid.Views.Base.DragObjectDropEventArgs) Handles GridView1.DragObjectDrop
        XgridColumnDropChanged(GridView1, Me.Name)
    End Sub

    Private Sub GridView1_ColumnWidthChanged(sender As Object, e As ColumnEventArgs) Handles GridView1.ColumnWidthChanged
        XgridColumnWidthChanged(GridView1, Me.Name)
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles cmdPrint.Click
        DXPrintDatagridview(Me.Text & "<br/><strong>" & Me.Text & "</strong>", "Requisition List", "Report as of from " & CDate(Now).ToString("MMMM, dd yyyy"), GridView1, Me)
    End Sub

    Private Sub cmdLocalData_Click(sender As Object, e As EventArgs) Handles cmdLocalData.Click
        ViewList()
    End Sub


    Private Sub CmdColumnSettings_Click(sender As Object, e As EventArgs) Handles cmdColumnSettings.Click
        Dim colname As String = ""
        For I = 0 To GridView1.Columns.Count - 1
            colname += GridView1.Columns(I).FieldName & ","
        Next
        frmColumnFilter.txtColumn.Text = colname.Remove(colname.Count - 1, 1)
        frmColumnFilter.GetFilterInfo(GridView1, Me.Text)
        frmColumnFilter.ShowDialog(Me)
    End Sub

    Private Sub Em_DoubleClick(sender As Object, e As EventArgs) Handles Em.DoubleClick
        cmdViewRequisitionInfo.PerformClick()
    End Sub

    Private Sub cmdViewRequisitionInfo_Click(sender As Object, e As EventArgs) Handles cmdViewRequisitionInfo.Click
        frmRequisitionInfo.mode.Text = ""
        frmRequisitionInfo.pid.Text = GridView1.GetFocusedRowCellValue("Entry Code").ToString
        frmRequisitionInfo.mode.Text = "edit"
        If frmRequisitionInfo.Visible = False Then
            frmRequisitionInfo.Show(Me)
        Else
            frmRequisitionInfo.WindowState = FormWindowState.Normal
        End If
    End Sub
End Class