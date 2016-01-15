﻿
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports DBManager
Imports ITSBase
Imports ITSBase.Accessory
Imports TraceCtrlLib.PanelExtend
Imports UIControlLib
Imports ZhiFa.Base.MessageControl

Public Class FrmTraceQuery
    Private _entityInfoTable As DataTable
    Private _tracePanelManage As TracePanelManage
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _entityInfoTable = New DataTable()
    End Sub
    Private Sub FrmLocationQuery_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.pnlTraceContainer.Width = Me.Width * 0.8
        If (Not TraceFoundation.LoadLayout(TEXT_LBS_LAYOUT_DEMO, Me.pnlTraceContainer)) Then
            BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", MSG_LBS_LOAD_LAYOUT_FIAL, 10)
        Else
            _tracePanelManage = New TracePanelManage(Me.pnlTraceContainer)
        End If
        If Not LoadCardEntityInfo() Then
            BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", MSG_LBS_LOAD_ENTITY_FIAL, 10)
        End If
        InitialCategory()
    End Sub

    Private Function LoadCardEntityInfo() As Boolean
        Try
            Dim traceDb As New DbTraceLayout
            traceDb.QueryCardInfo(_entityInfoTable)
            Return Not _entityInfoTable.IsNullOrEmpty()
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub InitialCategory()
        If Not _entityInfoTable.IsNullOrEmpty() Then
            Dim dv As DataView = _entityInfoTable.DefaultView
            Dim categoryColl As DataTable = dv.ToTable(True, New String() {DBConstDef.ID_INFO_CATEGORY})
            For Each category As Integer In From categoryRow As DataRow In categoryColl.Rows Select Judgement.JudgeDBNullValue(categoryRow.Item(DBConstDef.ID_INFO_CATEGORY), ENUM_DATA_TYPE.DATA_TYPE_INTEGER)
                Me.cmbCategory.Items.Add(New With {.Key = category, .Value = [Enum].GetName(GetType(TRACE_ID_CATEGORY), category)})
            Next
            Me.cmbCategory.Items.Add(New With {.Key = 0, .Value = TEXT_ALL})
            Me.cmbCategory.DisplayMember = "Value"
            Me.cmbCategory.ValueMember = "Key"
            Me.cmbCategory.Sorted = True
            Me.cmbCategory.SelectedIndex = Me.cmbCategory.Items.Count - 1
        End If
    End Sub

    Private Sub BindEntityTable()
        If _entityInfoTable.IsNullOrEmpty() Then Return
        Me.dgv.DataSource = GenerateFilterTable()
        Me.dgv.ClearSelection()
    End Sub

    Private Function GenerateFilterTable() As DataTable
        Dim category As Integer = Me.cmbCategory.SelectedItem.Key
        Dim text As String = Me.tbNameFilter.Text
        Dim filter As String = String.Empty
        If category > 0 Then
            filter += IIf(String.IsNullOrEmpty(filter), "", " and ") + String.Format("{0}={1}", ID_INFO_CATEGORY, category)
        End If
        If Not String.IsNullOrEmpty(text) Then
            filter += IIf(String.IsNullOrEmpty(filter), "", " and ") + String.Format("{0} like '%{1}%'", ID_INFO_CODE, text)
        End If
        Dim selectRows As DataRow() = _entityInfoTable.Select(filter)
        Dim filterTable As New DataTable
        filterTable.Columns.Add(TEXT_ID_INFO_NAME, GetType(String))
        filterTable.Columns.Add(TEXT_ID_INFO_CODE, GetType(String))
        'filterTable.Columns.Add(TEXT_ID_INFO_CATEGORY, GetType(String))
        For Each selRow As DataRow In selectRows
            Dim newRow As DataRow = filterTable.NewRow()
            newRow.Item(TEXT_ID_INFO_NAME) = Judgement.JudgeDBNullValue(selRow.Item(DBConstDef.ID_INFO_NAME), ENUM_DATA_TYPE.DATA_TYPE_STRING)
            newRow.Item(TEXT_ID_INFO_CODE) = Judgement.JudgeDBNullValue(selRow.Item(DBConstDef.ID_INFO_CODE), ENUM_DATA_TYPE.DATA_TYPE_STRING)
            'newRow.Item(TEXT_ID_INFO_CATEGORY) = Judgement.JudgeDBNullValue(selRow.Item(DBConstDef.ID_INFO_CATEGORY), ENUM_DATA_TYPE.DATA_TYPE_STRING)
            filterTable.Rows.Add(newRow)
        Next
        Return filterTable
    End Function

    Private Sub cmbCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCategory.SelectedIndexChanged
        BindEntityTable()
    End Sub

    Private Sub tbNameFilter_TextChanged(sender As Object, e As EventArgs) Handles tbNameFilter.TextChanged
        BindEntityTable()
    End Sub

    Private Sub btnPlay_Click(sender As Object, e As EventArgs) Handles btnPlay.Click
        If dgv.SelectedRows.Count <> 1 Then
            BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Information, "", MSG_ERROR_DGV_SEL_ONE, 10)
            Return
        Else
            Dim traceDb As New DbTraceLayout, traceTable As New DataTable
            Dim code As String = dgv.SelectedRows(0).Cells(TEXT_ID_INFO_CODE).Value.ToString()
            Dim dateRangeStart As DateTime = dtpStart.Value.Date
            Dim dateRangeEnd As DateTime = dtpEnd.Value.Date.AddDays(1).AddSeconds(-1)
            If DBMEDITS_RESULT.SUCCESS <> traceDb.QueryTraceInfo(traceTable, code, dateRangeStart, dateRangeEnd) OrElse traceTable.IsNullOrEmpty() Then
                BaseMessageBox.ShowCustomerMessage(MessageBoxIcon.Error, "", String.Format(MSG_LBS_LOAD_CARD_LOCATION_TIME_RANGE_FIAL, dateRangeStart, dateRangeEnd, code), 10)
                Return
            Else
                playProgress.Value = 0
                _tracePanelManage.ClearTrace()
                _tracePanelManage.ClearMonitor()
                If traceTable.Rows.Count = 1 Then
                    playProgress.Maximum = 1
                    Dim panelName As String = Judgement.JudgeDBNullValue(traceTable.Rows(0).Item(TEXT_TRACE_LOCATION_NAME), ENUM_DATA_TYPE.DATA_TYPE_STRING)
                    Dim arriveTime As String = Judgement.JudgeDBNullValue(traceTable.Rows(0).Item(TEXT_TRAIL_RECORD_ARRIVE_TIME), ENUM_DATA_TYPE.DATA_TYPE_STRING)
                    Dim cardName As String = Judgement.JudgeDBNullValue(traceTable.Rows(0).Item(TEXT_TRACE_CARD_NAME), ENUM_DATA_TYPE.DATA_TYPE_STRING)
                    Dim traceHint As TraceMonitorPoint = New TraceMonitorPoint() With {.Name = cardName, .Arrive = arriveTime, .Color = Color.Red}
                    _tracePanelManage.AddMonitor(String.Format("{0}___{1}", pnlTraceContainer.Name, panelName), traceHint)
                    playProgress.Value += 1
                Else
                    For i As Integer = 0 To traceTable.Rows.Count - 2
                        playProgress.Maximum = traceTable.Rows.Count - 1
                        Dim panelPre As String = Judgement.JudgeDBNullValue(traceTable.Rows(i).Item(TEXT_TRACE_LOCATION_NAME), ENUM_DATA_TYPE.DATA_TYPE_STRING)
                        Dim panelNext As String = Judgement.JudgeDBNullValue(traceTable.Rows(i + 1).Item(TEXT_TRACE_LOCATION_NAME), ENUM_DATA_TYPE.DATA_TYPE_STRING)
                        _tracePanelManage.TraceFromTo(code, String.Format("{0}___{1}", pnlTraceContainer.Name, panelPre), String.Format("{0}___{1}", pnlTraceContainer.Name, panelNext))
                        playProgress.Value += 1
                        Threading.Thread.Sleep(1000)
                    Next
                End If
            End If
        End If
    End Sub

    Private Sub FrmLocationQuery_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Me.pnlTraceContainer.Width = Me.Width * 3 / 4
        BindEntityTable()
    End Sub
End Class
