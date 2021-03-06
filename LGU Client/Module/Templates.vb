﻿Imports DevExpress.XtraEditors

Module Templates

    Public Function PrintDisbursementVoucher(ByVal voucherid As String, ByVal print As Boolean, ByVal form As Form) As String
        'CreateHTMLReportTemplate("ResidentProfile.html")
        Dim TableRow As String = ""
        Dim Template As String = Application.StartupPath.ToString & "\Templates\A-24_DV.html"
        Dim SaveLocation As String = Application.StartupPath.ToString & "\Printing\A-24_DV_" & voucherid & ".html"

        If Not System.IO.Directory.Exists(Application.StartupPath.ToString & "\Printing") Then
            System.IO.Directory.CreateDirectory(Application.StartupPath.ToString & "\Printing")
        End If

        If System.IO.File.Exists(SaveLocation) = True Then
            System.IO.File.Delete(SaveLocation)
        End If

        My.Computer.FileSystem.CopyFile(Template, SaveLocation)

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[municipality]", GlobalOrganizationName), False)

        Dim pid As String = "" : Dim supplierid As String = "" : Dim officeid As String = "" : Dim sb As Boolean = False : Dim requesttype As String = ""
        com.CommandText = "select *,date_format(datetrn,'%M %d, %Y') as trndate, " _
            + " (select sb from tblcompoffice where officeid=a.officeid) as sb,  " _
            + " (select requesttype from tblrequisition where pid=a.pid) as requesttype, " _
            + " (select centercode from tblcompoffice where officeid=a.officeid) as centercode  " _
            + " from tbldisbursementvoucher as a where voucherid='" & voucherid & "'" : rst = com.ExecuteReader
        While rst.Read
            pid = rst("pid").ToString : supplierid = rst("supplierid").ToString : officeid = rst("officeid").ToString : sb = CBool(rst("sb").ToString) : requesttype = rst("requesttype").ToString
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[voucherno]", rst("voucherno").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[voucherdate]", rst("voucherdate").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[checkno]", rst("checkno").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[bankname]", rst("checkbank").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[checkdate]", rst("checkdate").ToString), False)
        End While
        rst.Close()

        ' office
        Dim officerid As String = ""
        com.CommandText = "select centercode, officerid, (select fullname from  tblaccounts where accountid=a.officerid) as officehead from tblcompoffice as a where officeid='" & officeid & "'" : rst = com.ExecuteReader
        While rst.Read
            officerid = rst("officerid").ToString
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[responsibilty]", rst("centercode").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[officehead]", UCase(rst("officehead").ToString)), False)
        End While
        rst.Close()
        DigitalReportSigniture(SaveLocation, officerid, "officehead")


        'supplier
        com.CommandText = "select * from tblsupplier where supplierid='" & supplierid & "'" : rst = com.ExecuteReader
        While rst.Read
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[suppliername]", rst("suppliername").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[tin]", rst("tin").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[supplieraddress]", rst("completeaddress").ToString), False)
        End While
        rst.Close()

        'Voucher Item
        TableRow = "" : Dim itemRow As String = "" : Dim cnt As Integer = 0
        com.CommandText = "select purpose from tblrequisition where pid='" & pid & "'" : rst = com.ExecuteReader
        While rst.Read
            If cnt = 0 Then
                TableRow += "<tr> " _
                         + " <td colspan='3' style='padding:7px;'>" & rst("purpose").ToString & "</td> " _
                         + " <td  align='right' style='padding:7px;' rowspan='[total_row_span]'>[total]</td> " _
                   + " </tr> " & Chr(13)
            Else
                TableRow += "<tr> " _
                        + " <td colspan='3' style='padding:7px;'>" & rst("purpose").ToString & "</td> " _
                  + " </tr> " & Chr(13)
            End If
            cnt += 1
        End While
        rst.Close()


        If TableRow.Length > 0 Then
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[voucher_item]", TableRow), False)
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[voucher_item]", ""), False)
        End If

        'For I = 0 To 5 - cnt
        '    TableRow += "<tr> " _
        '                 + " <td colspan='3' style='padding:7px;'>&nbsp;</td> " _
        '                 + " <td  align='right' style='padding:7px;'>&nbsp;</td> " _
        '           + " </tr> " & Chr(13)
        'Next
        Dim total As Double = 0
        com.CommandText = "select ifnull(sum(credit),0) as total from tbljournalentryvoucher as a inner join tbljournalentryitem as b on a.jevno=b.jevno where a.dvid='" & voucherid & "' and itemcode in (select itemcode from tblglcashitem)" : rst = com.ExecuteReader
        While rst.Read
            total = rst("total").ToString
        End While
        rst.Close()

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[total]", FormatNumber(total, 2)), False)
        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[total_row_span]", cnt), False)

        Dim AcctRow = ""
        com.CommandText = "select b.*, (select itemname from tblglitem where itemcode=b.itemcode) as itemname from tbljournalentryvoucher as a inner join tbljournalentryitem as b on a.jevno=b.jevno where a.dvid='" & voucherid & "'" : rst = com.ExecuteReader
        While rst.Read
            AcctRow += " <tr> " _
                           + " <td>" & rst("itemname").ToString & "</td> " _
                           + " <td align='center'>" & rst("itemcode").ToString & "</td> " _
                           + " <td align='right'>" & If(Val(rst("debit").ToString) > 0, FormatNumber(rst("debit").ToString, 2), "") & "</td> " _
                           + " <td align='right'>" & If(Val(rst("credit").ToString) > 0, FormatNumber(rst("credit").ToString, 2), "") & "</td> " _
                        + "</tr> " & Chr(13)
        End While
        rst.Close()

        If AcctRow.Length > 0 Then
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[accounting_row]", AcctRow), False)
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[accounting_row]", ""), False)
        End If

        Dim documents As String = ""
        com.CommandText = "select * from (select description,if(required,'YES','OPTIONAL') as required, " _
                        + " (select if(count(*)>0,concat(cast(count(*) as CHAR), ' File(s)'),'None') from lgufiledir.tblattachmentlogs where refnumber='" & pid & "' and trntype='requisition' and docname=a.doccode) as files " _
                        + " from tblapprovingattachment As a inner join tbldocumenttype As b On a.doccode=b.code where trncode='" & requesttype & "') as x order by description asc" : rst = com.ExecuteReader
        While rst.Read
            documents += "<tr><td>" & rst("description").ToString & "</td> " _
                      + "<td align='center'>" & rst("required").ToString & "</td> " _
                      + "<td align='center'>" & rst("files").ToString & "</td></tr> "
        End While
        rst.Close()

        If documents.Length > 0 Then
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[supporting_documents]", documents), False)
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[supporting_documents]", ""), False)
        End If

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[accounting]", UCase(GlobalAccountantName)), False)
        DigitalReportSigniture(SaveLocation, GlobalAccountantID, "accounting")

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[treasurer]", UCase(GlobalTreasurerName)), False)
        DigitalReportSigniture(SaveLocation, GlobalTreasurerID, "treasurer")


        If sb Then
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[mayor]", UCase(GlobalViceMayorName)), False)
            DigitalReportSigniture(SaveLocation, GlobalViceMayorID, "mayor")
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[mayor]", UCase(GlobalMayorName)), False)
            DigitalReportSigniture(SaveLocation, GlobalMayorID, "mayor")
        End If

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[preparedby]", UCase(GlobalAccountantName)), False)
        DigitalReportSigniture(SaveLocation, GlobalAccountantID, "prepared")

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[watermark]", "© LGU Financial " & CDate(Now).ToString("yyyy") & " - Disbursement System v" & fversion & " (Printed On " & CDate(Now).ToString("MMMM dd, yyyy") & ") "), False)

        If print Then
            PrintViaInternetExplorer(SaveLocation.Replace("\", "/"), form)
        End If

        Return SaveLocation
    End Function

    Public Function PrintCAFOA(ByVal pid As String, ByVal print As Boolean, ByVal form As Form) As String
        'CreateHTMLReportTemplate("ResidentProfile.html")
        Dim TableRow As String = ""
        Dim Template As String = Application.StartupPath.ToString & "\Templates\A-28_CAFOA.html"
        Dim SaveLocation As String = Application.StartupPath.ToString & "\Printing\A-28_CAFOA_" & pid & ".html"

        If Not System.IO.Directory.Exists(Application.StartupPath.ToString & "\Printing") Then
            System.IO.Directory.CreateDirectory(Application.StartupPath.ToString & "\Printing")
        End If

        If System.IO.File.Exists(SaveLocation) = True Then
            System.IO.File.Delete(SaveLocation)
        End If

        My.Computer.FileSystem.CopyFile(Template, SaveLocation)

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[municipality]", GlobalOrganizationName), False)

        Dim officeid As String = "" : Dim periodcode As String = "" : Dim sb As Boolean = False : Dim requestorid As String = ""
        com.CommandText = "select *, date_format(postingdate,'%M %d, %Y') as daterequest, " _
            + " date_format(postingdate,'%m') as month_request, " _
            + " (select sb from tblcompoffice where officeid=a.officeid) as sb,  " _
            + " (select officerid from tblcompoffice where officeid=a.officeid) as id_requestor,  " _
            + " (select (select fullname from tblaccounts where accountid=tblcompoffice.officerid) from tblcompoffice where officeid=a.officeid) as name_requestor,  " _
            + " (select fullname from tblaccounts where accountid=a.requestedby) as payee,  " _
            + " ifnull((select sum(amount) from tblrequisitionfund where pid=a.pid),0) as total " _
            + " from tblrequisition as a where pid='" & pid & "'" : rst = com.ExecuteReader
        While rst.Read

            periodcode = rst("periodcode").ToString : officeid = rst("officeid").ToString : sb = CBool(rst("sb").ToString) : requestorid = rst("id_requestor").ToString
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[remarks]", rst("purpose").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[obligation_no]", rst("requestno").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[payee]", rst("payee").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[name_requestor]", rst("name_requestor").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[date_requestor]", rst("daterequest").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[total_amount]", FormatNumber(rst("total").ToString, 2)), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[approved_amount]", FormatNumber(rst("total").ToString, 2)), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[total_in_words]", ConvertCurrencyToEnglish(rst("total").ToString)), False)
        End While
        rst.Close()

        Dim budget As Boolean = False : Dim treasurer As Boolean = False : Dim accountant As Boolean = False
        com.CommandText = "select *,date_format(dateconfirm,'%m/%d/%y %h:%i %p') as date_approved from tblapprovalhistory as a where mainreference='" & pid & "' and status='Approved'" : rst = com.ExecuteReader
        While rst.Read
            If rst("confirmid").ToString = GlobalBudgetID Then
                budget = True
                My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[name_budget]", rst("confirmby").ToString), False)
                My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[date_budget]", rst("date_approved").ToString), False)
            End If

            If rst("confirmid").ToString = GlobalTreasurerID Then
                treasurer = True
                My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[name_treasurer]", rst("confirmby").ToString), False)
                My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[date_treasurer]", rst("date_approved").ToString), False)
            End If

            If rst("confirmid").ToString = GlobalAccountantID Then
                accountant = True
                My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[name_accountant]", rst("confirmby").ToString), False)
                My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[date_accountant]", rst("date_approved").ToString), False)
            End If
        End While
        rst.Close()

        DigitalReportSigniture(SaveLocation, requestorid, "requestor")


        If budget = True Then
            DigitalReportSigniture(SaveLocation, GlobalBudgetID, "budget")
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[name_budget]", "_________________"), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[date_budget]", "_________________"), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[hide_budget]", "hidden"), False)
        End If

        If treasurer = True Then
            DigitalReportSigniture(SaveLocation, GlobalTreasurerID, "treasurer")
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[name_treasurer]", "_________________"), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[date_treasurer]", "_________________"), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[hide_treasurer]", "hidden"), False)
        End If

        If accountant = True Then
            DigitalReportSigniture(SaveLocation, GlobalAccountantID, "accountant")
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[name_accountant]", "_________________"), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[date_accountant]", "_________________"), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[hide_accountant]", "hidden"), False)
        End If

        Dim ItemRow = "" : Dim acctint As Integer = 0
        com.CommandText = "select *, SUBSTRING_INDEX(periodcode,'-',1) as fund, (select centercode from tblcompoffice where officeid=a.officeid) as centercode, " _
                + " (select shortname from tblcompoffice where officeid=a.officeid) as office, " _
                + " (select expenditurecode from tblexpendituretagging where glitemcode=a.itemcode) as expenditure from tblrequisitionfund as a where pid='" & pid & "' " : rst = com.ExecuteReader
        While rst.Read
            ItemRow += " <tr> " _
                           + " <td align='center' style='font-size: 12px; white-space: nowrap;'>" & rst("centercode").ToString & "-" & rst("office").ToString & "</td> " _
                           + " <td style='font-size: 12px'>" & rst("fund").ToString & " (" & rst("expenditure").ToString & ")" & "</td> " _
                           + " <td align='center' style='font-size: 12px; white-space: nowrap;'>" & rst("itemcode").ToString & "</td> " _
                           + " <td align='right' style='font-size: 12px; white-space: nowrap;'>" & FormatNumber(rst("amount").ToString, 2) & "</td> " _
                        + "</tr> " & Chr(13)
            acctint += 1
        End While
        rst.Close()

        For I = 0 To 4 - acctint
            ItemRow += " <tr> " _
                           + " <td align='center' style='font-size: 0.6vw; white-space: nowrap;'>&nbsp;</td> " _
                           + " <td style='font-size: 0.6vw;'>&nbsp;</td> " _
                           + " <td align='center' style='font-size: 0.6vw; white-space: nowrap;'>&nbsp;</td> " _
                           + " <td align='right' style='font-size: 0.6vw; white-space: nowrap;'>&nbsp;</td> " _
                        + "</tr> " & Chr(13)
        Next
        If ItemRow.Length > 0 Then
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[accounting_row]", ItemRow), False)
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[accounting_row]", ""), False)
        End If



        Dim AcctRow = "" : Dim acct As Integer = 0
        com.CommandText = "SELECT b.postingdate, (select itemname from tblglitem where itemcode = a.itemcode) as itemname, a.requestno,a.prevbalance,a.amount,a.newbalance, " _
                                    + " b.paid as cleared FROM `tblrequisitionfund` as a inner join tblrequisition as b on a.pid = b.pid where a.pid='" & pid & "' order by a.itemcode, b.postingdate asc; " : rst = com.ExecuteReader
        While rst.Read
            Dim cleared As Boolean = CBool(rst("cleared"))
            AcctRow += " <tr> " _
                           + " <td class='item_list' align='center'>" & rst("postingdate").ToString & "</td> " _
                           + " <td class='item_list' align='center'>" & rst("itemname").ToString & "</td> " _
                           + " <td class='item_list' align='right'>" & If(cleared, FormatNumber(Val(rst("amount").ToString), 2), "") & "</td> " _
                           + " <td class='item_list' align='right'>" & If(Not cleared, FormatNumber(Val(rst("amount").ToString), 2), "") & "</td> " _
                           + " <td class='item_list' align='right'>" & FormatNumber(Val(rst("newbalance").ToString), 2) & "</td> " _
                        + "</tr> " & Chr(13)
            acct += 1
        End While
        rst.Close()

        'For I = 0 To 4 - acct
        '    AcctRow += " <tr> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                + "</tr> " & Chr(13)
        'Next

        If AcctRow.Length > 0 Then
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[item_transaction]", AcctRow), False)
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[item_transaction]", ""), False)
        End If

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[accounting]", UCase(GlobalAccountantName)), False)
        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[treasurer]", UCase(GlobalTreasurerName)), False)

        If sb Then
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[mayor]", UCase(GlobalViceMayorName)), False)
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[mayor]", UCase(GlobalMayorName)), False)
        End If

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[preparedby]", UCase(globalfullname)), False)
        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[watermark]", "© LGU Financial " & CDate(Now).ToString("yyyy") & " - Disbursement System v" & fversion & " (Printed On " & CDate(Now).ToString("MMMM dd, yyyy") & ") "), False)
        If print Then
            PrintViaInternetExplorer(SaveLocation.Replace("\", "/"), form)
        End If
        Return SaveLocation
    End Function

    Public Function PrintFURS(ByVal pid As String, ByVal print As Boolean, ByVal form As Form) As String
        'CreateHTMLReportTemplate("ResidentProfile.html")
        Dim TableRow As String = ""
        Dim Template As String = Application.StartupPath.ToString & "\Templates\A-29_FURS.html"
        Dim SaveLocation As String = Application.StartupPath.ToString & "\Printing\A-29_FURS_" & pid & ".html"

        If Not System.IO.Directory.Exists(Application.StartupPath.ToString & "\Printing") Then
            System.IO.Directory.CreateDirectory(Application.StartupPath.ToString & "\Printing")
        End If

        If System.IO.File.Exists(SaveLocation) = True Then
            System.IO.File.Delete(SaveLocation)
        End If

        My.Computer.FileSystem.CopyFile(Template, SaveLocation)

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[municipality]", GlobalOrganizationName), False)

        Dim periodcode As String = "" : Dim officeid As String = "" : Dim sb As Boolean = False : Dim requestorid As String = "" : Dim purpose As String = ""
        com.CommandText = "select *, date_format(postingdate,'%M %d, %Y') as daterequest,  date_format(postingdate,'%m') as month_request, " _
                            + " (select officename from tblcompoffice where officeid=a.officeid) as office, " _
                            + " (select officerid from tblcompoffice where officeid=a.officeid) as officerid,  " _
                            + " (select (select fullname from tblaccounts where accountid=tblcompoffice.officerid) from tblcompoffice where officeid=a.officeid) as name_requestor,  " _
                            + " (select (select designation from tblaccounts where accountid=tblcompoffice.officerid) from tblcompoffice where officeid=a.officeid) as position_requestor,  " _
                            + " (select sb from tblcompoffice where officeid=a.officeid) as sb,  " _
                            + " (select fullname from tblaccounts where accountid=a.requestedby) as payee,  " _
                            + " ifnull((select sum(amount) from tblrequisitionfund where pid=a.pid),0) as total " _
                            + " from tblrequisition as a where pid='" & pid & "'" : rst = com.ExecuteReader
        While rst.Read
            periodcode = rst("periodcode").ToString : officeid = rst("officeid").ToString : sb = CBool(rst("sb").ToString) : requestorid = rst("officerid").ToString : purpose = rst("purpose").ToString
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[fursno]", rst("requestno").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[name_requestor]", rst("name_requestor").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[position_requestor]", rst("position_requestor").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[office]", rst("office").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[date_requestor]", rst("daterequest").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[date]", rst("daterequest").ToString), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[fund]", rst("fundcode").ToString), False)
        End While
        rst.Close()
        DigitalReportSigniture(SaveLocation, requestorid, "requestor")

        Dim budget As Boolean = False : Dim treasurer As Boolean = False : Dim accountant As Boolean = False
        com.CommandText = "select *,date_format(dateconfirm,'%m/%d/%y %h:%i %p') as date_approved, (select designation from tblaccounts where accountid=a.confirmid) as position from tblapprovalhistory as a where mainreference='" & pid & "' and status='Approved'" : rst = com.ExecuteReader
        While rst.Read
            If rst("confirmid").ToString = GlobalAccountantID Then
                accountant = True
                My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[name_accountant]", rst("confirmby").ToString), False)
                My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[date_accountant]", rst("date_approved").ToString), False)
                My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[position_accountant]", rst("position").ToString), False)
            End If
        End While
        rst.Close()

        If accountant = True Then
            DigitalReportSigniture(SaveLocation, GlobalAccountantID, "accountant")
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[name_accountant]", "_________________"), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[date_accountant]", "_________________"), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[position_accountant]", "_________________"), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[hide_accountant]", "hidden"), False)
        End If

        Dim itemrow = "" : Dim item As Integer = 0 : Dim total As Double = 0
        Dim totalsourcecount As Integer = countqry("tblrequisitionfund", "pid='" & pid & "' ")
        com.CommandText = "select *, (select itemname from tblglitem where itemcode=a.itemcode) as itemname from tblrequisitionfund as a where pid='" & pid & "' " : rst = com.ExecuteReader
        While rst.Read
            itemrow += " <tr> " _
                           + " <td class='item_list' align='center'>" & rst("itemname").ToString & "</td> " _
                           + If(totalsourcecount > 1, If(item = 0, "  <td class='item_list' rowspan='" & totalsourcecount & "'>" & purpose & "</td>", ""), "<td class='item_list'>" & purpose & "</td>") _
                           + " <td class='item_list' align='center'>" & rst("itemcode").ToString & "</td> " _
                           + " <td class='item_list' align='right'>" & FormatNumber(rst("amount").ToString, 2) & "</td> " _
                        + "</tr> " & Chr(13)
            item += 1
            total += Val(rst("amount").ToString)
        End While
        rst.Close()

        'For I = 0 To 4 - item
        '    itemrow += " <tr> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                + "</tr> " & Chr(13)
        'Next
        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[total]", FormatNumber(total, 2)), False)
        If itemrow.Length > 0 Then
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[item_row]", itemrow), False)
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[item_row]", ""), False)
        End If


        Dim AcctRow = "" : Dim acct As Integer = 0
        com.CommandText = "SELECT b.postingdate,b.purpose,b.requestno,a.prevbalance,a.amount,a.newbalance, " _
                                    + " (select paid from tblrequisition where pid=a.pid) as cleared FROM `tblrequisitionfund` as a " _
                                    + " inner join tblrequisition as b on a.pid=b.pid " _
                                    + " where a.officeid='" & officeid & "' and a.periodcode='" & periodcode & "' order by a.itemcode, b.postingdate asc; " : rst = com.ExecuteReader
        While rst.Read
            Dim cleared As Boolean = CBool(rst("cleared"))
            AcctRow += " <tr> " _
                           + " <td class='item_list' align='center'>" & rst("postingdate").ToString & "</td> " _
                           + " <td class='item_list'>" & rst("purpose").ToString & "</td> " _
                           + " <td class='item_list' align='center'>" & rst("requestno").ToString & "</td> " _
                           + " <td class='item_list' align='right'>" & FormatNumber(Val(rst("prevbalance").ToString), 2) & "</td> " _
                           + " <td class='item_list' align='right'>" & FormatNumber(Val(rst("amount").ToString), 2) & "</td> " _
                           + " <td class='item_list' align='right'>" & If(cleared, FormatNumber(Val(rst("amount").ToString), 2), "") & "</td> " _
                           + " <td class='item_list' align='right'>" & FormatNumber(Val(rst("newbalance").ToString), 2) & "</td> " _
                           + " <td class='item_list' align='right'>" & If(cleared, "0.00", FormatNumber(Val(rst("amount").ToString), 2)) & "</td> " _
                        + "</tr> " & Chr(13)
            acct += 1
        End While
        rst.Close()

        'For I = 0 To 2 - acct
        '    AcctRow += " <tr> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                   + " <td class='item_list'>&nbsp;</td> " _
        '                + "</tr> " & Chr(13)
        'Next

        If AcctRow.Length > 0 Then
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[accounting_row]", AcctRow), False)
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[accounting_row]", ""), False)
        End If

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[accounting]", UCase(GlobalAccountantName)), False)
        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[treasurer]", UCase(GlobalTreasurerName)), False)

        If sb Then
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[mayor]", UCase(GlobalViceMayorName)), False)
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[mayor]", UCase(GlobalMayorName)), False)
        End If

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[preparedby]", UCase(globalfullname)), False)
        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[watermark]", "© LGU Financial " & CDate(Now).ToString("yyyy") & " - Disbursement System v" & fversion & " (Printed On " & CDate(Now).ToString("MMMM dd, yyyy") & ") "), False)

        If print Then
            PrintViaInternetExplorer(SaveLocation.Replace("\", "/"), form)
        End If

        Return SaveLocation
    End Function

    Public Sub PrintJournalVoucher(ByVal jevno As String, ByVal form As Form)
        'CreateHTMLReportTemplate("ResidentProfile.html")
        Dim TableRow As String = ""
        Dim Template As String = Application.StartupPath.ToString & "\Templates\A-23_JEV.html"
        Dim SaveLocation As String = Application.StartupPath.ToString & "\Printing\A-23_JEV_" & jevno & ".html"

        If Not System.IO.Directory.Exists(Application.StartupPath.ToString & "\Printing") Then
            System.IO.Directory.CreateDirectory(Application.StartupPath.ToString & "\Printing")
        End If

        If System.IO.File.Exists(SaveLocation) = True Then
            System.IO.File.Delete(SaveLocation)
        End If

        My.Computer.FileSystem.CopyFile(Template, SaveLocation)

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[municipality]", GlobalOrganizationName), False)

        Dim remarks As String = ""
        com.CommandText = "Select *,date_format(postingdate,'%M %d, %Y') as trndate from tbljournalentryvoucher where jevno='" & jevno & "'" : rst = com.ExecuteReader
        While rst.Read
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[jevno]", jevno), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[postingdate]", rst("trndate").ToString), False)
            remarks = rst("remarks").ToString
        End While
        rst.Close()

        'Voucher Item
        TableRow = "" : Dim totaldebit As Double = 0 : Dim totalcredit As Double = 0 : Dim cnt As Integer = 0
        com.CommandText = "select *, (select centercode from tblcompoffice where officeid=tbljournalentryitem.centercode) as office_center from tbljournalentryitem where jevno='" & jevno & "'" : rst = com.ExecuteReader
        While rst.Read
            TableRow += "<tr> " _
                           + " <td align='center' style='padding: 5px;'>" & rst("office_center").ToString & "</td> " _
                           + " <td style='padding: 5px;'>" & If(Val(rst("credit").ToString) > 0, "<div style='padding-left:25px'>" & rst("itemname").ToString & "</div>", rst("itemname").ToString) & "</td> " _
                           + " <td align='center' style='padding: 5px;'>" & rst("itemcode").ToString & "</td> " _
                           + " <td align='right' style='padding: 5px;'>" & If(Val(rst("debit").ToString) = 0, "", FormatNumber(rst("debit").ToString, 2)) & "</td> " _
                           + " <td align='right' style='padding: 5px;'>" & If(Val(rst("credit").ToString) = 0, "", FormatNumber(rst("credit").ToString, 2)) & "</td> " _
                     + " </tr> " & Chr(13)
            totaldebit += Val(rst("debit").ToString)
            totalcredit += Val(rst("credit").ToString)
            cnt += 1
        End While
        rst.Close()

        'For I = 0 To 7 - cnt
        '    TableRow += "<tr> " _
        '                 + " <td>&nbsp;</td> " _
        '                 + " <td>&nbsp;</td> " _
        '                 + " <td>&nbsp;</td> " _
        '                 + " <td>&nbsp;</td> " _
        '                 + " <td>&nbsp;</td> " _
        '           + " </tr> " & Chr(13)
        'Next
        If remarks.Length > 1 Then
            TableRow += "<tr> " _
                        + " <td>&nbsp;</td> " _
                        + " <td>" & remarks & "</td> " _
                        + " <td>&nbsp;</td> " _
                        + " <td>&nbsp;</td> " _
                        + " <td>&nbsp;</td> " _
                  + " </tr> " & Chr(13)
        End If

        If TableRow.Length > 0 Then
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[journal_item]", TableRow), False)
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[journal_item]", ""), False)
        End If

        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[totaldebit]", FormatNumber(totaldebit, 2)), False)
        My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[totalcredit]", FormatNumber(totalcredit, 2)), False)

        com.CommandText = "select * from tblaccounts where accountant=1" : rst = com.ExecuteReader
        While rst.Read
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[certified]", UCase(rst("fullname").ToString)), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[position]", rst("designation").ToString), False)
        End While
        rst.Close()

        PrintViaInternetExplorer(SaveLocation.Replace("\", "/"), form)
    End Sub

    Public Sub DigitalReportSigniture(ByVal SaveLocation As String, ByVal userid As String, ByVal code_command As String)
        If Not System.IO.Directory.Exists(Application.StartupPath.ToString & "\Printing\Signature") Then
            System.IO.Directory.CreateDirectory(Application.StartupPath.ToString & "\Printing\Signature")
        End If

        Dim pic As New PictureEdit
        Dim sig As Image = getAccountSignature(userid)
        If Not sig Is Nothing Then
            pic.Image = getAccountSignature(userid)
            pic.Image.Save(Application.StartupPath.ToString & "\Printing\Signature\" & userid & ".png")
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[sig_" & code_command & "]", Application.StartupPath.ToString & "\Printing\Signature\" & userid & ".png"), False)
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[hide_" & code_command & "]", ""), False)
        Else
            My.Computer.FileSystem.WriteAllText(SaveLocation, My.Computer.FileSystem.ReadAllText(SaveLocation).Replace("[hide_" & code_command & "]", "hidden"), False)
        End If
    End Sub
End Module
