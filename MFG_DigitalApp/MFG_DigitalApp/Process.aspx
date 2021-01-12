<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.Master" CodeBehind="Process.aspx.cs" Inherits="MFG_DigitalApp.Process" %>

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
        .table>tbody>tr>td, 
        .table>tbody>tr>th, .table>tfoot>tr>td, 
        .table>tfoot>tr>th, .table>thead>tr>td, .table>thead>tr>th {
            vertical-align: middle;
              border-top: 1px solid #ddd;
              font-size: 12px;
        }
        .table>tbody>tr>td {
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

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
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
                                <asp:Label runat="server" ID="lblshift" Text=""></asp:Label></li>
                            <li class="breadcrumb-item">
                                <asp:Label runat="server" ID="lbldate" Text=""></asp:Label></li>
                        </ol>
                        <div class="row" runat="server" id="HeaderButtons">
                             <div class="col-md-1">
                                  <a class="level static" href="Process.aspx" tabindex="-1">Process</a>
                             </div>
                            <div class="col-md-4">
                                 <asp:RadioButtonList ID="drpStatus" runat="server" RepeatDirection="Horizontal" CssClass="rbtnlbl"
                                    OnSelectedIndexChanged="drpStatus_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Selected="True">Pending Records</asp:ListItem>
                                    <asp:ListItem>Saved Records</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            
                            <div class="col-md-7" align="Right">
                                <asp:Button runat="server" ID="btnBack" CssClass="btn btn-primary" Text="Back" OnClick="btnBack_Click" />
                                <asp:Button runat="server" ID="btnInProcessParameter" CssClass="btn btn-primary" Text="InProcess" OnClick="btnInProcessParameter_Click" />
                                <asp:Button runat="server" ID="btnWeight" CssClass="btn btn-primary" Text="Weight" OnClick="btnWeight_Click" />
                                <asp:Button runat="server" ID="btnSQC" CssClass="btn btn-primary" Text="SQC" OnClick="btnSQC_Click" />
                            </div>
                        </div>
                        <div class="box box-default" style="margin-top: 7px;">
                            <div class="box-body">
                                <div class="row" runat="server" id="ActionButton">
                                    <div class="col-md-12" align="Right">
                                        <asp:Button runat="server" ID="btnAddParameter" CssClass="btn btn-success" Text="Add Parameter" OnClick="btnAddParameter_Click" />
                                        <asp:Button runat="server" ID="btnSkipResult" CssClass="btn btn-success" Text="Skip Result" OnClick="btnSkipResult_Click" />
                                    </div>
                                </div>
                                <div class="row" runat="server" id="ExportButton">
                                    <div class="col-md-12" align="Right">
                                        <asp:Button runat="server" ID="btnExport" CssClass="btn btn-primary" Text="Export to Excel" OnClick="btnExport_Click" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12 table-responsive">
                                        <asp:GridView runat="server" ID="GrdProcessScheduleStatus" RowStyle-HorizontalAlign="Center" DataKeyNames="Id,ScheduleId" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Select"  runat="server" />
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
                                                <asp:TemplateField HeaderText ="Result">
                                                         <ItemTemplate>
                                                            <asp:TextBox ID="Result" Text='<%#Eval("Result")%>' runat="server" onkeypress="return isNumberKey(event)" onpaste="return false;" CssClass="form-control" MaxLength="3" ValidationGroup = "G1" />
                                                            <asp:CustomValidator ID="cfvResult" runat="server" ErrorMessage="Required" ForeColor="Red" ValidationGroup="G1" ControlToValidate="Result" ClientValidationFunction="checkValuesInRow" ValidateEmptyText="true"></asp:CustomValidator>
                                                         </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ExpectedResult" HeaderText="Expected Result" />
                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image_Status" ImageUrl='<%#Eval("Status")%>' runat="server" Height="15" Width="15" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                         <asp:GridView runat="server" ID="GrdProcessCompletedScheduleStatus" RowStyle-HorizontalAlign="Center"  AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" AsyncRendering = "false">
                                            <Columns>
                                                
                                                
                                                <asp:BoundField DataField="Station" HeaderText="Station" />
                                                <asp:BoundField DataField="Parameter" HeaderText="Parameter" />
                                                <asp:BoundField DataField="DueTime" HeaderText="Due Time" />
                                                <asp:BoundField DataField="Tool" HeaderText="Tool Required" />
                                                <asp:BoundField DataField="Sample" HeaderText="Sample" />
                                                <asp:BoundField DataField="ToleranceLCL" HeaderText="Tolerance LCL" />
                                                <asp:BoundField DataField="ToleranceUCL" HeaderText="Tolerance UCL" />
                                                <asp:BoundField DataField="UOM" HeaderText="UOM" />
                                                <asp:BoundField DataField="Result" HeaderText ="Result"/>
                                                <asp:BoundField DataField="ExpectedResult" HeaderText="Expected Result" />
                                                <asp:BoundField DataField="RecordedOn" HeaderText="Recorded On" />
                                                <asp:BoundField DataField="RecordedBy" HeaderText="Recorded By" />
                                                <asp:TemplateField HeaderText="Status" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image_Status" ImageUrl='<%#Eval("Status")%>' runat="server" Height="15" Width="15" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="row" runat="server" id="SubmitButton">
                                    <div class="col-md-12" align="Right">
                                        <asp:Button runat="server" ID="btnSaveResult" CssClass="btn btn-primary" Text="Save" OnClick="btnSaveResult_Click" ValidationGroup = "G1" />
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
                                                            <asp:DropDownList ID="drpParameter" runat="server" AutoPostBack="True" CssClass="form-control select2" Width=100% OnSelectedIndexChanged="OnParameterChange">
															</asp:DropDownList>
                                                        </div>
                                                    </div>
                                               
                                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                                        <label>Station</label>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                         <div class="form-group has-feedback">
                                                             <asp:DropDownList ID="drpStation" runat="server" CssClass="form-control select2" Width=100%>
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
                                                               <asp:DropDownList ID="drpSeverity" runat="server" CssClass="form-control select2" Width=100%>
																</asp:DropDownList>
                                                        </div>
                                                    </div>
													 <div class="col-md-2 col-sm-2 col-xs-2">
                                                        <label>No.of Sample</label>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
                                                             <asp:TextBox ID="txtNoOfSamples" runat="server" CssClass="form-control" MaxLength="3" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-2 col-sm-2 col-xs-2">
                                                        <label>UOM</label>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
															<asp:DropDownList ID="drpUOM" runat="server" CssClass="form-control select2" Width=100%>
															</asp:DropDownList>
                                                        </div>
                                                    </div>
													<div class="col-md-2 col-sm-2 col-xs-2">
                                                        <label>Repeat</label>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
															 <label>Hr</label>
																<asp:DropDownList ID="ddlHour" runat="server" CssClass="form-control select2" Width=30%>
																</asp:DropDownList>
															<label>Min</label>
																<asp:DropDownList ID="ddlMinute" runat="server" CssClass="form-control select2" Width=30%>
																</asp:DropDownList>
                                                        </div>
                                                    </div>
                                                 </div>
												 <div class="row" Id="QuantitativeGroup" runat="server">
													<div class="col-md-1 col-sm-1 col-xs-1">
                                                        <label>Tol.LCL</label>
                                                    </div>
                                                    <div class="col-md-3 col-sm-3 col-xs-3">
                                                        <div class="form-group has-feedback">
															<asp:TextBox ID="txtLCL" runat="server" CssClass="form-control" MaxLength="7" onkeypress="return isNumberKey(event)"></asp:TextBox>
														</div>
                                                    </div>
													<div class="col-md-1 col-sm-1 col-xs-1">
                                                        <label>Tol.UCL</label>
                                                    </div>
                                                    <div class="col-md-3 col-sm-3 col-xs-3">
                                                        <div class="form-group has-feedback">
															 <asp:TextBox ID="txtUCL" runat="server" CssClass="form-control" MaxLength="7" onkeypress="return isNumberKey(event)"></asp:TextBox>
															 <asp:RequiredFieldValidator ID="validatorUCL" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="txtUCL" ErrorMessage="Please enter UCL" ForeColor="Red"></asp:RequiredFieldValidator>
														</div>
                                                    </div>
													<div class="col-md-1 col-sm-1 col-xs-1">
                                                        <label>UOM</label>
                                                    </div>
                                                    <div class="col-md-3 col-sm-3 col-xs-3">
                                                        <div class="form-group has-feedback">
													        <asp:TextBox ID="txtQUOM" runat="server" CssClass="form-control" MaxLength="10" ></asp:TextBox>
														</div>
                                                    </div>
												 </div>
												 <div class="row" Id="QualitativeGroup" runat="server">
													<div class="col-md-4 col-sm-4 col-xs-4">
                                                        <label>Expected Result</label>
                                                    </div>
                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                        <div class="form-group has-feedback">
														<asp:TextBox ID="txtEResult" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
														</div>
                                                    </div>
												 </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button runat="server" ID="btnAddParam" CssClass="btn btn-primary" Text="Add Parameter" OnClick="btnAddParam_Click" UseSubmitBehavior="false" />
                                            <asp:Button runat="server" ID="btnOkAddParameter" CssClass="btn btn-default" Text="Close" OnClick="btnClearParam_Click" UseSubmitBehavior="false"/>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
