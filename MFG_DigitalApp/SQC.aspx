<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SQC.aspx.cs" enableEventValidation ="false" Inherits="MFG_DigitalApp.SQC" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .rbtnlbl {
            margin: 5px 0px;
        }

            .rbtnlbl label {
                padding-left: 5px;
                padding-right: 10px;
            }

        .btn {
            font-weight: bold !important;
            font-size: 12px !important;
        }

        .Grd tbody tr th {
            vertical-align: central !important;
            text-align: center !important;
        }

        .lblTotal {
            border: none;
            background: transparent;
            text-align: center;
            position: relative;
            top: -7px;
        }

        .txtGrd {
            /*position: relative;
                top: 11px;*/
        }

        .table > tbody > tr > td,
        .table > tbody > tr > th, .table > tfoot > tr > td,
        .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th {
            vertical-align: middle;
            border-top: 1px solid #ddd;
            font-size: 12px;
        }

        .table > tbody > tr > td {
            padding: 3px;
        }

        .level {
            height: 30px !important;
            padding: 10px;
            background-color: #BA075F !important;
            color: #ffffff !important;
            font-weight: bold !important;
            margin-left: 10px;
        }
    </style>
    <script>
        $(document).ready(function () {
            $("[id*=lblTotal]").attr("readonly", true);
        });
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31
                && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
        function isNumeric(evt, obj) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            var value = obj.value;
            var number = obj.value.split('.');
            var dotcontains = value.indexOf(".") != -1;
            var caratPos = value.length;//getSelectionStart(obj);
            var dotPos = obj.value.indexOf(".");
            if (dotcontains)
                if (charCode == 46) return false;
            if (charCode == 46) return true;
            if (charCode == 45) return true;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 2))
                return false;
            return true;
        }
        $(document).on("keyup mouseup", "[id*=NCriticalD]", function () {
            if (!jQuery.trim($(this).val()) == '') {
                if (!isNaN(parseFloat($(this).val()))) {
                    var row = $(this).closest("tr");
                    var majorDef = 0; var minorDef = 0;
                    if (!isNaN(parseFloat($("[id*=NMajorD]", row).val()))) {
                        majorDef = parseFloat($("[id*=NMajorD]", row).val());
                    }
                    if (!isNaN(parseFloat($("[id*=NMinorD]", row).val()))) {
                        minorDef = parseFloat($("[id*=NMinorD]", row).val());
                    }
                    $("[id*=lblTotal]", row).val(minorDef + majorDef + parseFloat($(this).val()));
                }
            } else {
                $(this).val('');
            }
        });

        $(document).on("keyup mouseup", "[id*=NMajorD]", function () {
            if (!jQuery.trim($(this).val()) == '') {
                if (!isNaN(parseFloat($(this).val()))) {
                    var row = $(this).closest("tr");
                    var criticalDef = 0; var minorDef = 0;
                    if (!isNaN(parseFloat($("[id*=NCriticalD]", row).val()))) {
                        criticalDef = parseFloat($("[id*=NCriticalD]", row).val());
                    }
                    if (!isNaN(parseFloat($("[id*=NMinorD]", row).val()))) {
                        minorDef = parseFloat($("[id*=NMinorD]", row).val());
                    }
                    $("[id*=lblTotal]", row).val(minorDef + criticalDef + parseFloat($(this).val()));
                }
            } else {
                $(this).val('');
            }
        });

        $(document).on("keyup mouseup", "[id*=NMinorD]", function () {
            if (!jQuery.trim($(this).val()) == '') {
                if (!isNaN(parseFloat($(this).val()))) {
                    var row = $(this).closest("tr");
                    var majorDef = 0; var criticalDef = 0;
                    if (!isNaN(parseFloat($("[id*=NMajorD]", row).val()))) {
                        majorDef = parseFloat($("[id*=NMajorD]", row).val());
                    }
                    if (!isNaN(parseFloat($("[id*=NCriticalD]", row).val()))) {
                        criticalDef = parseFloat($("[id*=NCriticalD]", row).val());
                    }
                    $("[id*=lblTotal]", row).val(criticalDef + majorDef + parseFloat($(this).val()));
                }
            } else {
                $(this).val('');
            }
        });

        function checkValuesInRow(sender, element) {
            var isValid = true;
            if ($("#" + sender.controltovalidate).closest('tr').find('input[type="checkbox"]').prop("checked")) {
                if (element.Value) {
                    isValid = false;
                }
            }
            element.IsValid = isValid;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="content-wrapper" style="min-height: 500px;">
                <div class="container">
                    <section class="content">
                        <ol class="breadcrumb">
                            <li class="breadcrumb-item">
                                <asp:Label runat="server" ID="lblplantcode" Text=""></asp:Label>
                            </li>
                            <li class="breadcrumb-item">
                                <asp:Label runat="server" ID="lblline" Text=""></asp:Label></li>
                             <li class="breadcrumb-item">
                                <asp:Label runat="server" ID="lblshift" Text="" style="display: none;"></asp:Label>
                                <asp:Label runat="server" ID="lblshiftnew" Text=""></asp:Label>

                            </li>
                            <li class="breadcrumb-item">
                                <asp:Label runat="server" ID="lbldate" Text=""></asp:Label></li>
                        </ol>

                        <div class="row" runat="server" id="HeaderButtons">
                            <div class="col-md-2">
                                <a class="level static" href="SQC.aspx" tabindex="-1">SQC</a>
                                    <asp:label ID="txtGrammage" class="level static" runat="server" >Grammage</asp:label>
                            </div>
                            <div class="col-md-4" align="Left">
                                <asp:RadioButtonList ID="drpStatus" runat="server" RepeatDirection="Horizontal" CssClass="rbtnlbl"
                                    OnSelectedIndexChanged="drpStatus_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Selected="True">Pending Records</asp:ListItem>
                                    <asp:ListItem>Saved Records</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>

                            <div class="col-md-6" align="Right">
                                <asp:Button runat="server" ID="btnBack" CssClass="btn btn-primary" Text="Back" OnClick="btnBack_Click" />
                            <asp:Button runat="server" ID="Refresh" CssClass="btn btn-primary" Text="Refresh" OnClick="Refresh_Click" />
                                <asp:Button runat="server" ID="btnProcessParameter" CssClass="btn btn-primary" Text="Process" OnClick="btnProcessParameter_Click" />
                                <asp:Button runat="server" ID="btnWeight" CssClass="btn btn-primary" Text="Weight" OnClick="btnWeight_Click" />
                                <asp:Button runat="server" ID="btnInProcessParameter" CssClass="btn btn-primary" Text="In-Process" OnClick="btnInProcessParameter_Click" />
                            </div>
                        </div>
                        <div class="box box-default" style="margin-top: 7px;">
                            <div class="box-body">
                                <div class="row" runat="server" id="ActionButton">
                                    <div class="col-md-12" align="Right">
                                        <asp:Button runat="server" ID="btnAddParameter" CssClass="btn btn-success" Text="Add Parameter" OnClick="btnAddParameter_Click" />
                                        <asp:Button runat="server" ID="btnSkipResult" CssClass="btn btn-success" Text="Skip Result" OnClick="btnSkipResult_Click" />
                                        <asp:Button runat="server" ID="btnSaveResult" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveResult_Click" ValidationGroup="G1" />
                                    </div>
                                </div>
                                <div class="row" runat="server" id="ExportButton">
                                     <div class="col-md-9">
                                 <div class="col-md-2 col-sm-2 col-xs-2">
                                            <label>From Date</label>
                                             <asp:TextBox ID="txtScheduleFromDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                                                TargetControlID="txtScheduleFromDate">
                                                            </asp:CalendarExtender>
                                        </div>
                                        <div class="col-md-2 col-sm-2 col-xs-2">
                                            <label>To Date</label>
                                             <asp:TextBox ID="txtScheduleToDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="yyyy-MM-dd" Enabled="True"
                                                                TargetControlID="txtScheduleToDate">
                                                            </asp:CalendarExtender>
                                        </div>
                                        <div class="col-md-2 col-sm-2 col-xs-2">
                                            <label>
                                            </label>
                                             <asp:Button runat="server" ID="BtnShow" CssClass="btn btn-success"  style="margin-top:26px" Text="Show" OnClick="BtnShow_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-3" align="Right" style="margin-left: -49px;margin-top: 27px;">
                                        <asp:Button runat="server" ID="btnExport" CssClass="btn btn-primary" Text="Export to Excel" OnClick="ExportToExcel" />
                                    </div>
                                </div>
                                <div class="row" style="margin-top: 10px;">
                                    <div class="col-md-12 table-responsive">
                                        <asp:GridView runat="server" RowStyle-HorizontalAlign="Center" DataKeyNames="Id,ScheduleId" ID="GrdSQCScheduleStatus" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover Grd" ShowHeaderWhenEmpty="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Select" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DueTime" HeaderText="Due Time" />
                                                <asp:BoundField DataField="Station" HeaderText="Station" />
                                                <asp:BoundField DataField="Parameter" HeaderText="Parameter" />
                                                <asp:BoundField DataField="Tool" HeaderText="Tool Required" />
                                                <asp:BoundField DataField="Sample" HeaderText="Sample" />
                                                <asp:TemplateField HeaderText="Pallet No">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="PalletNo" Text='<%#Eval("Pallet_No")%>' runat="server" onkeypress="return isNumberKey(event)"  CssClass="form-control txtGrd" MaxLength="4" ValidationGroup="G1"/>
                                                        <%-- <asp:CustomValidator ID="cfvPalletNo" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="PalletNo" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No.of Critical Def">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="NCriticalD" Text='<%#Convert.ToString(Eval("No_Of_Critical_Def"))==""?0:Eval("No_Of_Critical_Def")%>' runat="server" AutoPostBack="true" OnTextChanged="NCriticalD_TextChanged2"    onkeypress="return isNumberKey(event)"  CssClass="form-control txtGrd" MaxLength="4" ValidationGroup="G1" />
                                                        <%--                                                            <asp:CustomValidator ID="cfvNCriticalD" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="NCriticalD" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No.of Major Def">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="NMajorD" Text='<%#Convert.ToString(Eval("No_Of_Major_Def"))==""?0:Eval("No_Of_Major_Def")%>' runat="server" AutoPostBack="true" OnTextChanged="NMajorD_TextChanged1" onkeypress="return isNumberKey(event)"  CssClass="form-control txtGrd" MaxLength="4" ValidationGroup="G1" />
                                                        <%--                                                            <asp:CustomValidator ID="cfvNMajorD" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="NMajorD" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No.Of Minor Def">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="NMinorD" Text='<%#Convert.ToString(Eval("No_Of_Minor_Def"))==""?0:Eval("No_Of_Minor_Def")%>' runat="server" AutoPostBack="true" OnTextChanged="NMinorD_TextChanged1" onkeypress="return isNumberKey(event)"  CssClass="form-control txtGrd" MaxLength="4" ValidationGroup="G1" />
                                                        <%--                                                 <asp:CustomValidator ID="cfvNMinorD" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="NMinorD" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblTotal" Text='<%#Eval("Total")%>' ReadOnly="true"  runat="server" CssClass="form-control txtGrd" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remark">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Remarks" Text='<%#Eval("Remarks")%>' runat="server"  CssClass="form-control txtGrd" MaxLength="100" ValidationGroup="G1" />
                                                        <%--                                                            <asp:CustomValidator ID="cfvRemarks" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="Remarks" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image_Status" ImageUrl='<%#Eval("Status")%>' runat="server" Height="15" Width="15" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                        <asp:GridView runat="server" ID="GrdSQCCompletedScheduleStatus" RowStyle-HorizontalAlign="Center" DataKeyNames="Id,ScheduleId" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" AsyncRendering="false">
                                            <Columns>
                                                 <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Select" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Station" HeaderText="Station" />
                                                <asp:BoundField DataField="Parameter" HeaderText="Parameter" />
                                                <asp:BoundField DataField="Tool" HeaderText="Tool Required" />
                                                <asp:BoundField DataField="DueTime" HeaderText="Due Time" />
                                                <asp:BoundField DataField="Sample" HeaderText="Sample" />
                                               <%-- <asp:BoundField DataField="PalletNo" HeaderText="Pallet No" />
                                                <asp:BoundField DataField="NCriticalD" HeaderText="No_Of_Critical_Def" />
                                                <asp:BoundField DataField="NMajorD" HeaderText="No_Of_Major_Def" />
                                                <asp:BoundField DataField="NMinorD" HeaderText="No_Of_Minor_Def" />
                                                <asp:BoundField DataField="Total" HeaderText="Total" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" />--%>
                                                 <asp:TemplateField HeaderText="Pallet No">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="PalletNo" Text='<%#Eval("PalletNo")%>' runat="server" onkeypress="return isNumberKey(event)"  CssClass="form-control txtGrd" MaxLength="4" ValidationGroup="G1" />
                                                        <%-- <asp:CustomValidator ID="cfvPalletNo" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="PalletNo" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No.of Critical Def">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="NCriticalD" Text='<%#Eval("NCriticalD")%>' runat="server"  AutoPostBack="true" OnTextChanged="NCriticalD_TextChanged" onkeypress="return isNumberKey(event)" CssClass="form-control txtGrd" MaxLength="4" ValidationGroup="G1" />
                                                        <%--                                                            <asp:CustomValidator ID="cfvNCriticalD" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="NCriticalD" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No.of Major Def">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="NMajorD" Text='<%#Eval("NMajorD")%>' runat="server"  AutoPostBack="true" OnTextChanged="NMajorD_TextChanged" onkeypress="return isNumberKey(event)"  CssClass="form-control txtGrd" MaxLength="4" ValidationGroup="G1" />
                                                        <%--                                                            <asp:CustomValidator ID="cfvNMajorD" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="NMajorD" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No.Of Minor Def">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="NMinorD" Text='<%#Eval("NMinorD")%>' AutoPostBack="true"  OnTextChanged="NMinorD_TextChanged" runat="server" onkeypress="return isNumberKey(event)"  CssClass="form-control txtGrd" MaxLength="4" ValidationGroup="G1" />
                                                        <%--                                                 <asp:CustomValidator ID="cfvNMinorD" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="NMinorD" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Total">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblTotal" Text='<%#Eval("Total")%>' ReadOnly="true" runat="server" CssClass="form-control txtGrd" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Remark">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="Remarks" Text='<%#Eval("Remarks")%>' runat="server"  CssClass="form-control txtGrd" MaxLength="100" ValidationGroup="G1" />
                                                        <%--                                                            <asp:CustomValidator ID="cfvRemarks" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="Remarks" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="RecordedOn" HeaderText="Recorded On" />
                                                <asp:BoundField DataField="RecordedBy" HeaderText="Recorded By" />
                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image_Status" ImageUrl='<%#Eval("Status")%>' runat="server" Height="15" Width="15" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Convert.ToString(Eval("Status1"))=="True"?"Tested":"Skipped" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row" runat="server" id="SubmitButton">
                                    <div class="col-md-12" align="Right">
                                        <asp:Button runat="server" ID="btnSaveResult1" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveResult_Click" ValidationGroup = "G1" />
<%--                                        <asp:Button runat="server" ID="btnPreviousResult" CssClass="btn btn-primary" Text="Previous Result" OnClick="btnPreviousResult_Click" />--%>
                                    </div>
                                </div>
                                
 <div class="row" runat="server" id="ResubmitButton">
                                    <div class="col-md-12" align="Right">
                                        <asp:Button runat="server" ID="BtnEditButton" CssClass="btn btn-primary" Text="Save" OnClick="BtnEditButton_Click" ValidationGroup="G1" />
                                        <%--                                        <asp:Button runat="server" ID="btnPreviousResult" CssClass="btn btn-primary" Text="Previous Result" OnClick="btnPreviousResult_Click" />--%>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:ModalPopupExtender ID="MPAddParameter" runat="server" TargetControlID="btnAddParam"
                            PopupControlID="myModalAlertAddParameter" CancelControlID="btnOkAddParameter" BackgroundCssClass="modalBackground">
                        </asp:ModalPopupExtender>
                        <div id="myModalAlertAddParameter">
                            <div class="modal-dialog" style="width: auto;">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h4>
                                            <label>Add Parameter</label></h4>
                                        <asp:Label runat="server" ID="lblStartMessage" ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="modal-body">
                                        <div style="width: 100%;">
                                            <div class="row">
                                                <div class="col-md-2 col-sm-2 col-xs-2">
                                                    <label>Parameter</label>
                                                </div>
                                                <div class="col-md-4 col-sm-4 col-xs-4">
                                                    <div class="form-group has-feedback">
                                                        <asp:DropDownList ID="drpParameter" runat="server" AutoPostBack="True" CssClass="form-control select2" Width="100%" OnSelectedIndexChanged="OnParameterChange">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="col-md-2 col-sm-2 col-xs-2">
                                                    <label>Station</label>
                                                </div>
                                                <div class="col-md-4 col-sm-4 col-xs-4">
                                                    <div class="form-group has-feedback">
                                                        <asp:DropDownList ID="drpStation" runat="server" AutoPostBack="True" CssClass="form-control select2" Width="100%" OnSelectedIndexChanged="OnFieldChange">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-2 col-sm-2 col-xs-2">
                                                    <label>Severity</label>
                                                </div>
                                                <div class="col-md-4 col-sm-4 col-xs-4">
                                                    <div class="form-group has-feedback">
                                                        <asp:DropDownList ID="drpSeverity" runat="server" AutoPostBack="True" CssClass="form-control select2" Width="100%" OnSelectedIndexChanged="OnFieldChange">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="col-md-2 col-sm-2 col-xs-2">
                                                    <label>No.of Sample</label>
                                                </div>
                                                <div class="col-md-4 col-sm-4 col-xs-4">
                                                    <div class="form-group has-feedback">
                                                        <asp:TextBox ID="txtNoOfSamples" runat="server" AutoPostBack="True" CssClass="form-control" MaxLength="3" onkeypress="return isNumberKey(event)" OnTextChanged="OnFieldChange"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-2 col-sm-2 col-xs-2">
                                                    <label>UOM</label>
                                                </div>
                                                <div class="col-md-4 col-sm-4 col-xs-4">
                                                    <div class="form-group has-feedback">
                                                        <asp:DropDownList ID="drpUOM" runat="server" AutoPostBack="True" CssClass="form-control select2" Width="100%" OnSelectedIndexChanged="OnFieldChange">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <%--<div class="col-md-2 col-sm-2 col-xs-2">
                                                        <label>Repeat</label>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
															 <label>Hr</label>
																<asp:DropDownList ID="ddlHour" runat="server" AutoPostBack="True" CssClass="form-control select2" Width=30% OnSelectedIndexChanged="OnFieldChange">
																</asp:DropDownList>
															<label>Min</label>
																<asp:DropDownList ID="ddlMinute" runat="server" AutoPostBack="True" CssClass="form-control select2" Width=30% OnSelectedIndexChanged="OnFieldChange">
																</asp:DropDownList>
                                                        </div>--%>
                                               
                                            </div>
                                        </div>
                                        <div class="row" id="QuantitativeGroup" runat="server">
                                            <div class="col-md-2 col-sm-2 col-xs-3">
                                                <label>Tol.LCL</label>
                                            </div>
                                           <div class="col-md-4 col-sm-4 col-xs-4">
                                                <div class="form-group has-feedback">
                                                    <asp:TextBox ID="txtLCL" runat="server" AutoPostBack="True" CssClass="form-control" MaxLength="7" onkeypress="return isNumeric(event,this)" OnTextChanged="OnFieldChange"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2 col-sm-2 col-xs-2">
                                                <label>Tol.UCL</label>
                                            </div>
                                             <div class="col-md-4 col-sm-4 col-xs-5">
                                                <div class="form-group has-feedback">
                                                    <asp:TextBox ID="txtUCL" runat="server" AutoPostBack="True" CssClass="form-control" MaxLength="7" onkeypress="return isNumeric(event,this)" OnTextChanged="OnFieldChange"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="validatorUCL" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="txtUCL" ErrorMessage="Please enter UCL" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                             <div class="col-md-2 col-sm-2 col-xs-2">
                                                <label>Q.UOM</label>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <div class="form-group has-feedback">
                                                    <asp:DropDownList ID="drpQUOM" runat="server" AutoPostBack="True" CssClass="form-control select2" Width="100%" OnSelectedIndexChanged="OnFieldChange">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row" id="QualitativeGroup" runat="server">
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <label>Expected Result</label>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-xs-4">
                                                <div class="form-group has-feedback">
                                                    <asp:TextBox ID="txtEResult" runat="server" AutoPostBack="True" CssClass="form-control" MaxLength="100" OnTextChanged="OnFieldChange"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                
                                <div class="modal-footer">
                                    <asp:Button runat="server" ID="btnAddParam" CssClass="btn btn-primary" Text="Add Parameter" OnClick="btnAddParam_Click" UseSubmitBehavior="false" />
                                    <asp:Button runat="server" ID="btnOkAddParameter" CssClass="btn btn-default" Text="Close" OnClick="btnClearParam_Click" UseSubmitBehavior="false" />
                                </div>
                                    </div>
                            </div>
                        </div>
                </div>
                </section>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
     <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1"
        runat="server">
        <ProgressTemplate>
              <div class="loading">
                 <img src="images/loading.gif" style="padding: 10px;position:fixed;top:45%;left:50%;"/>
            </div>
                       
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
