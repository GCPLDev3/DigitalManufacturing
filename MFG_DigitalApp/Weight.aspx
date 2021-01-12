<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="Weight.aspx.cs" Inherits="MFG_DigitalApp.Weight" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .btn {
            font-weight: bold !important;
            font-size: 12px !important;
        }

        .Grd tbody tr th {
            vertical-align: central !important;
        }

        text-align: center !important;


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

        .lblResult {
            border: none;
            background: transparent;
            text-align: center;
            position: relative;
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

        function checkValuesInRow(sender, element) {
            var isValid = true;
            if ($("#" + sender.controltovalidate).closest('tr').find('input[type="checkbox"]').prop("checked")) {
                if (element.Value) {
                    isValid = false;
                }
            }
            element.IsValid = isValid;
        }
        function btnOkSampleInput_Click() {
            var arr = []; $("#myModalSampleInput input[type=decimal]").each(function (index, element) {
                if ($(element).val() == "") {
                    $(element).val("0")
                }
                arr.push($(element).val());
            });
            var tot = 0;
            arr.forEach(function (v) {
                tot = parseFloat(tot) + parseFloat(v);
            })
            if ($("[id*=drpStatus] input:checked").val() == "Saved Records") {
                var grid = document.getElementById('MainContent_GrdWeightCompletedScheduleStatus');
                var index = document.getElementById("MainContent_myhiddenField").value;
                var txtbox = document.getElementById(grid.id + '_lblResult_' + index);
                var resValue = tot / arr.length;
                resValue = resValue.toFixed(3);
                txtbox.value = parseFloat(resValue);
                var hidInput = document.getElementById(grid.id + '_HiddenField1_' + index);
                hidInput.value = JSON.stringify(arr);
            }
            else if ($("[id*=drpStatus] input:checked").val() == "Pending Records") {
                var grid = document.getElementById('MainContent_GrdWeightScheduleStatus');
                var index = document.getElementById("MainContent_myhiddenField").value;
                var txtbox = document.getElementById(grid.id + '_lblResult_' + index);
                var resValue = tot / arr.length;
                resValue = resValue.toFixed(3);
                txtbox.value = parseFloat(resValue);
                var hidInput = document.getElementById(grid.id + '_HiddenField1_' + index);
                hidInput.value = JSON.stringify(arr);
            }
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
                                <asp:Label runat="server" ID="lblshift" Text="" Style="display: none;"></asp:Label>
                                <asp:Label runat="server" ID="lblshiftnew" Text=""></asp:Label>

                            </li>
                            <li class="breadcrumb-item">
                                <asp:Label runat="server" ID="lbldate" Text=""></asp:Label></li>
                        </ol>

                        <div class="row" runat="server">
                            <div class="col-md-2">
                                <a class="level static" href="Weight.aspx" tabindex="-1">Weight</a>
                                <asp:Label ID="txtGrammage" class="level static" runat="server">Grammage</asp:Label>
                            </div>
                            <div class="col-md-4" align="Left">
                                <asp:RadioButtonList ID="drpStatus" runat="server" RepeatDirection="Horizontal" CssClass="rbtnlbl"
                                    OnSelectedIndexChanged="drpStatus_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Selected="True" style="margin: 7px;">Pending Records</asp:ListItem>
                                    <asp:ListItem style="margin: 7px;">Saved Records</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>

                            <div class="col-md-6" align="Right">
                                <asp:Button runat="server" ID="btnBack" CssClass="btn btn-primary" Text="Back" OnClick="btnBack_Click" />
                                <asp:Button runat="server" ID="Refresh" CssClass="btn btn-primary" Text="Refresh" OnClick="Refresh_Click" />
                                <asp:Button runat="server" ID="btnProcessParameter" CssClass="btn btn-primary" Text="Process" OnClick="btnProcessParameter_Click" />
                                <asp:Button runat="server" ID="btnInProcess" CssClass="btn btn-primary" Text="In-Process" OnClick="btnInProcess_Click" />
                                <asp:Button runat="server" ID="btnSQC" CssClass="btn btn-primary" Text="SQC" OnClick="btnSQC_Click" />
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
                                            <asp:Button runat="server" ID="BtnShow" CssClass="btn btn-success" Style="margin-top: 26px" Text="Show" OnClick="BtnShow_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-3" align="Right" style="margin-left: -49px; margin-top: 27px;">
                                        <asp:Button runat="server" ID="btnExport" CssClass="btn btn-primary" Text="Export to Excel" OnClick="ExportToExcel" />
                                    </div>
                                </div>
                                <div class="row" style="margin-top: 10px;">
                                    <div class="col-md-12 table-responsive">
                                        <asp:GridView runat="server" ID="GrdWeightScheduleStatus" RowStyle-HorizontalAlign="Center" DataKeyNames="Id,ScheduleId" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Select" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="DueTime" HeaderText="Due Time" />
                                                <asp:BoundField DataField="Station" HeaderText="Station" />
                                                <asp:BoundField DataField="Parameter" HeaderText="Parameter" />
                                                <asp:BoundField DataField="Tool" HeaderText="Tool Required" />
                                                <asp:BoundField DataField="Sample" HeaderText="Sample" />
                                                <asp:BoundField DataField="ToleranceLCL" HeaderText="Tolerance LCL" />
                                                <asp:BoundField DataField="ToleranceUCL" HeaderText="Tolerance UCL" />
                                                <asp:BoundField DataField="UOM" HeaderText="UOM" />
                                                <asp:TemplateField HeaderText="Result">
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="lblResult" Text='<%#Eval("Result")%>' Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkAddSampleInput" Text="" CommandArgument='<%# Eval("Sample") %>' runat="server" OnClick="AddSampleInput"><img src="images/plus.png" id="lnkplus" runat="server"></img></asp:LinkButton>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="HiddenField1" runat="server"
                                                            Value='<%# Eval("SampleInputs") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ExpectedResult">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="ExpectedResult" Text='<%#Eval("ExpectedResult")%>' runat="server" CssClass="form-control" MaxLength="20" />
                                                        <%--<asp:CustomValidator ID="cfvExpectedResult" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="Result" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>--%>
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
                                        <asp:GridView runat="server" ID="GrdWeightCompletedScheduleStatus" RowStyle-HorizontalAlign="Center" DataKeyNames="Id,ScheduleId" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" AsyncRendering="false">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Select" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField DataField="Station" HeaderText="Station" />
                                                <asp:BoundField DataField="Parameter" HeaderText="Parameter" />
                                                <asp:BoundField DataField="DueTime" HeaderText="Due Time" />
                                                <asp:BoundField DataField="Tool" HeaderText="Tool Required" />
                                                <asp:BoundField DataField="Sample" HeaderText="Sample" />
                                                <asp:BoundField DataField="ToleranceLCL" HeaderText="Tolerance LCL" />
                                                <asp:BoundField DataField="ToleranceUCL" HeaderText="Tolerance UCL" />
                                                <asp:BoundField DataField="UOM" HeaderText="UOM" />
                                                <%--  <asp:BoundField DataField="Result" HeaderText="Result" />
                                                <asp:BoundField DataField="ExpectedResult" HeaderText="Expected Result" />--%>
                                                <asp:TemplateField HeaderText="Result">
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="lblResult" Text='<%#Eval("Result")%>' Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkAddSampleInput" Text="" CommandArgument='<%# Eval("Sample") %>' runat="server" OnClick="AddSampleInput"><img src="images/plus.png"></img></asp:LinkButton><%----%>
                                                                </td>
                                                            </tr>

                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Result" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResult1" runat="server" Text='<%#Eval("Result")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("SampleInputs") %>' Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="ExpectedResult">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="ExpectedResult" Text='<%#Eval("ExpectedResult")%>' runat="server" CssClass="form-control" MaxLength="20" ValidationGroup="G1" />
                                                        <%--<asp:CustomValidator ID="cfvExpectedResult" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="Result" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>--%>
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
                                        <asp:Button runat="server" ID="btnSaveResult1" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveResult_Click" ValidationGroup="G1" />
                                        <%--                                        <asp:Button runat="server" ID="btnPreviousResult" CssClass="btn btn-primary" Text="Previous Result" OnClick="btnPreviousResult_Click" />--%>
                                    </div>
                                </div>
                                <div class="row" runat="server" id="ResubmitButton">
                                    <div class="col-md-12" align="Right">
                                        <asp:Button runat="server" ID="btnEditResult" CssClass="btn btn-primary" Text="Save" OnClick="btnEditResult_Click" ValidationGroup="G1" />
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
                                                    <asp:TextBox ID="txtEResult" runat="server" AutoPostBack="True" CssClass="form-control" MaxLength="100" OnSelectedIndexChanged="OnFieldChange"></asp:TextBox>
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

                <!-- ModalPopupExtender Show Sample Value Reader -->
                <asp:Button ID="bSampleInput" runat="server" Style="display: none" />
                <asp:ModalPopupExtender ID="MPSampleInput" runat="server" TargetControlID="btnSampleInput"
                    PopupControlID="myModalSampleInput" CancelControlID="btnOkSampleInput" BackgroundCssClass="modalBackground">
                </asp:ModalPopupExtender>
                <div id="myModalSampleInput" style="width: 500px; align-content: center">
                    <div class="modal-dialog" style="width: auto; height: 500px; overflow-y: scroll">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4>
                                    <label>Sample Value Reader </label>
                                </h4>
                            </div>
                            <div class="modal-body">
                                <input type="hidden" id="myhiddenField" runat="server" value="" />
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder1"></asp:PlaceHolder>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="modal-footer">
                                <%--                                <asp:Button runat="server" ID="btnSampleInput"  class="btn btn-default" value="Close" OnClick="btnSampleInput_Click" OnClientClick="btnOkSampleInput_Click()" UseSubmitBehavior="true"/>--%>
                                <asp:Button runat="server" ID="btnSampleInput" class="btn btn-default" value="Close" Style="visibility: hidden" />
                                <input type="button" id="btnOkSampleInput" class="btn btn-default" value="Close" onclick="btnOkSampleInput_Click()" />
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
                <img src="images/loading.gif" style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
            </div>

        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
