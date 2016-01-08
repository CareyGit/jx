﻿Public Module XMLConstDef
    Public Const XML_PATH_DBSETTING As String = "Configuration/DBSettings/add[@key='MedITS']"
    Public Const XML_ATT_SERVER As String = "DBServer"
    Public Const XML_ATT_USER As String = "DBUserName"
    Public Const XML_ATT_PWD As String = "DBUserPWD"
    Public Const XML_ATT_INSTANCE As String = "DBInstance"
    Public Const XML_ATT_TIMEOUT As String = "DBTimeout"
    Public Const XML_PATH_DBTYPE As String = "DBType"
#Region "Export Config File"
    Public Const XML_STR_XPATH_EXCELCONFIG As String = "/Configration/ExcelConfig"
    Public Const XML_STR_XPATH_DATATITLE As String = "/Configration/ExcelConfig/DataAreaSetting/dataItem[@name=""DataTitle""]"
    Public Const XML_STR_XPATH_DATAAREA As String = "/Configration/ExcelConfig/DataAreaSetting/dataItem[@name=""DataArea""]"
    Public Const XML_STR_XPATH_CHARTAREA As String = "/Configration/ExcelConfig/ChartSetting/chartItem[@name=""ChartArea""]"
    Public Const XML_STR_XPATH_PLOTAREA As String = "/Configration/ExcelConfig/ChartSetting/chartItem[@name=""PlotArea""]"
    Public Const XML_STR_XPATH_LEGEND As String = "/Configration/ExcelConfig/ChartSetting/chartItem[@name=""Legend""]"
    Public Const XML_STR_XPATH_COLUMN As String = "/Configration/ExcelConfig/ColumnSet/Column"
    Public Const XML_STR_XPATH_DRAWING As String = "/Configration/ExcelConfig/ChartSetting[@NeedDrawing=""true""]"
    Public Const XML_STR_XPATH_CHECKVALUE As String = "/Configration/ExcelConfig/ColumnSet/Column[@type=""check""]"
    Public Const XML_STR_XPATH_COLUMNSET_TYPE As String = "/Configration/ExcelConfig/ColumnSet[@type=""Auto""]"
    Public Const XML_STR_ATTRIBUTE_NAME As String = "name"
    Public Const XML_STR_ATTRIBUTE_TOP As String = "top"
    Public Const XML_STR_ATTRIBUTE_VALUE As String = "value"
    Public Const XML_STR_ATTRIBUTE_LEFT As String = "left"
    Public Const XML_STR_ATTRIBUTE_ALIGN_LEFT As String = "left"
    Public Const XML_STR_ATTRIBUTE_ALIGN_CENTER As String = "center"
    Public Const XML_STR_ATTRIBUTE_ALIGN_RIGHT As String = "right"
    Public Const XML_STR_ATTRIBUTE_WIDTH As String = "width"
    Public Const XML_STR_ATTRIBUTE_HEIGHT As String = "height"
    Public Const XML_STR_ATTRIBUTE_SIZE As String = "size"
    Public Const XML_STR_ATTRIBUTE_BOLD As String = "bold"
    Public Const XML_STR_ATTRIBUTE_ALIGN As String = "align"
    Public Const XML_STR_ATTRIBUTE_FONTNAME As String = "fontname"
    Public Const XML_STR_ATTRIBUTE_BACKCOLOR As String = "backcolor"
    Public Const XML_STR_ATTRIBUTE_TYPE As String = "type"
    Public Const XML_STR_ATTRIBUTE_FORMAT As String = "Format"
    Public Const XML_STR_ATTRIBUTE_TRUEVALUE As String = "truevalue"
    Public Const XML_STR_ATTRIBUTE_GRAPHTYPE As String = "GraphType"
    Public Const XML_STR_ATTRIBUTE_TITLEFONTNAME As String = "TitleFontName"
    Public Const XML_STR_ATTRIBUTE_TITLEFONTSIZE As String = "TitleFontSize"
    Public Const XML_STR_ATTRIBUTE_TITLEFONTBOLD As String = "TitleFontBold"
    Public Const XML_STR_ATTRIBUTE_XAXISFONTSIZE As String = "xAxisFontSize"
    Public Const XML_STR_ATTRIBUTE_YAXISFONTSIZE As String = "yAxisFontSize"
    Public Const XML_STR_ATTRIBUTE_NEEDDRAWING As String = "NeedDrawing"
    Public Const XML_STR_ATTRIBUTEVALUE_TRUE As String = "true"
    Public Const XML_STR_ATTRIBUTEVALUE_FONTNAME_SONGTI As String = "宋体"
    Public Const XML_STR_ATTRIBUTEVALUE_TITLEBACKCOLOR As String = "darkgray"
    Public Const XML_STR_ATTRIBUTEVALUE_PLOTBACKCOLOR As String = "white"
    Public Const XML_STR_ATTRIBUTEVALUE_DATABACKCOLOR As String = "blue"
    Public Const XML_STR_ATTRIBUTEVALUE_NOTNEEDDRAWING As String = "false"
    Public Const XML_STR_ATTRIBUTEVALUE_RETURN As String = ""
    Public Const XML_STR_COLUMNVALID_STATE As String = "1"
    Public Const XML_STR_COLUMNNOTVALID_STATE As String = "0"
    Public Const XML_STR_COLUMN_FORMAT As String = "c"
    Public Const XML_STR_ATTRIBUTEVALUE_SHEET As String = "sheet"
#End Region
End Module
