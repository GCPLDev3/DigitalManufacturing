<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="QI_EditSchedule.aspx.cs" Inherits="MFG_DigitalApp.QI_EditSchedule" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .topmarg {
            margin-top: 25px;
        }

        .btn {
            font-weight: bold !important;
            font-size: 12px !important;
        }
    </style>
    <script>
        $(function () {
            $(".timepicker").timepicker({
                minuteStep: 15
            });
        })
        function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : evt.keyCode;
                 if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="content-wrapper" style="min-height: 500px;">
                <div class="container">
                    <section class="content">
                        <div class="box box-default" style="margin-top: 7px;">
                            <div class="box-body">
                                <div class="row form-group">
                                    <div class="col-md-2 text-right control-label">
                                        Schedule Number <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtScheduleNumber" runat="server" CssClass="form-control" Width=100% Enabled="false"></asp:TextBox>
                                    </div>
                                     <div class="col-md-2 text-right control-label">
                                        Brand <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="drpBrand" runat="server" CssClass="form-control select2" AutoPostBack="True" Width=100% OnSelectedIndexChanged="LoadVariant">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="Brand" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpBrand" ErrorMessage="Please select brand" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-2 text-right control-label">
                                        Plant <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-md-3">
                                       <asp:DropDownList ID="drpPlant" runat="server" CssClass="form-control select2" AutoPostBack="True" Width=100% OnSelectedIndexChanged="LoadStation">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="RequiredFieldPlanrValidator2" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpPlant" ErrorMessage="Please select plant" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                     <div class="col-md-2 text-right control-label">
                                        Variant <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="drpVariant" runat="server" CssClass="form-control select2" Width=100%>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="RequiredFieldVariantValidator" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpVariant" ErrorMessage="Please select variant" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="row form-group">
                                    <div class="col-md-2 text-right control-label">
                                        Schedule Description <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtScheduleDesc" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="ScheduleDescription" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="txtScheduleDesc" ErrorMessage="Please enter schedule description" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2 text-right control-label">
                                        Grammage <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-md-3">
                                       <asp:DropDownList ID="drpGrammage" runat="server" CssClass="form-control select2" Width=100%>
                                            </asp:DropDownList>
                                        <asp:RequiredFieldValidator InitialValue="0" ID="Grammage" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpGrammage" ErrorMessage="Please select Grammage" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row form-group">
                                    <div class="col-md-2 text-right control-label">
                                        Schedule Type <span class="text-danger">*</span>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="drpScheduleType" runat="server" CssClass="form-control select2" Width=100%>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="0" ID="ScheduleType" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpScheduleType" ErrorMessage="Please select schedule type" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2 text-right control-label">
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-success" Text="Save Schedule" ValidationGroup="g1" OnClick="btnSaveSchedule_Click" />
                                        <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-danger" Text="Back" OnClick="btnCancel_Click" />
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="box box-default">
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:GridView runat="server" ID="GrdAddedParameterList" AutoGenerateColumns="false" DataKeyNames ="Id" EmptyDataText="No Records Found!!"
                                            Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true" OnRowDeleting="GrdAddedParameterList_RowDeleting" OnRowDataBound="GrdAddedParameterList_RowDataBound">
                                           
                                            <Columns>
                                                <asp:BoundField DataField="Repeat" HeaderText="Repeat" />
                                                <asp:BoundField DataField="Station" HeaderText="At Station" />
                                                <asp:TemplateField HeaderText="Parameter">
                                                    <ItemTemplate>
                                                        <asp:LinkButton Text='<%# Eval("Parameter") %>' runat="server" CommandArgument="<%# Container.DataItemIndex %>" ID="lnkParameterName" OnClick="lnkParameterName_Click" Font-Bold="true" ForeColor="#3333ff" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Tool" HeaderText="Tool Required"/>
                                                <asp:BoundField DataField="ToleranceLCL" HeaderText="Tolerance LCL" ItemStyle-HorizontalAlign="Right" />
                                                 <asp:BoundField DataField="ToleranceUCL" HeaderText="Tolerance UCL" ItemStyle-HorizontalAlign="Right" />
                                                 <asp:BoundField DataField="UOM" HeaderText="UOM" ItemStyle-HorizontalAlign="Right" />
                                                 <asp:BoundField DataField="Result" HeaderText="Expected Result (Qualitative)"  />
                                                 <asp:BoundField DataField="Sample" HeaderText="Sample" ItemStyle-HorizontalAlign="Right" />
                                                 <asp:BoundField DataField="Severity" HeaderText="Severity"/>
                                                 <asp:CommandField ShowDeleteButton="True" HeaderText="Action" ControlStyle-CssClass="btn btn-info" ButtonType="Button" />
                                            </Columns>
                                            <PagerStyle CssClass="GridPager" />
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="box box-default" style="margin-top: 7px;">
                             <div class="box-body">
                                 <div class="row">
                                     <div class="col-md-12" style="text-align:center">
                                         <h4><strong>Edit Parameter</strong> </h4>
                                     </div>
                                 </div>
                                 <br />
                                 <div class="row form-group">
                                     <div class="col-md-1 text-right control-label">
                                         Parameter <span class="text-danger">*</span>
                                     </div>
                                     <div class="col-md-3">
                                          <asp:HiddenField ID="hdnSParameterId" runat="server" />
                                         <asp:DropDownList ID="drpParameter" runat="server" CssClass="form-control select2" Width=100% AutoPostBack="true" OnSelectedIndexChanged="onParameterSelectChange">
                                         </asp:DropDownList>
                                         <asp:RequiredFieldValidator InitialValue="0" ID="Parameter" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="drpParameter" ErrorMessage="Please select parameter" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </div>
                                     <div class="col-md-1 text-right control-label">
                                         Station  <span class="text-danger">*</span>
                                     </div>
                                     <div class="col-md-3">
                                         <asp:DropDownList ID="drpStation" runat="server" CssClass="form-control select2" Width=100%>
                                         </asp:DropDownList>
                                         <asp:RequiredFieldValidator InitialValue="0" ID="Station" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="drpStation" ErrorMessage="Please select station" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </div>
                                      <div class="col-md-1 text-right control-label">
                                         Shift  <span class="text-danger">*</span>
                                     </div>
                                     <div class="col-md-3">
                                             <asp:DropDownList ID="drpShiftHeader" runat="server" CssClass="form-control select2" Width="100%">
                                             </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvShift" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="drpShiftHeader" ErrorMessage="Please select shift" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </div>
                                 </div>
                                 <div class="row form-group">
                                     <div class="col-md-1 text-right control-label">
                                         Severity  <span class="text-danger">*</span>
                                     </div>
                                     <div class="col-md-3">
                                         <asp:DropDownList ID="drpSeverity" runat="server" CssClass="form-control select2" Width=100%>
                                         </asp:DropDownList>
                                         <asp:RequiredFieldValidator InitialValue="0" ID="Severity" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="drpSeverity" ErrorMessage="Please select severity" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </div>
                                     <div class="col-md-1 text-right control-label">
                                         No.ofSample <span class="text-danger">*</span>
                                     </div>
                                     <div class="col-md-3">
                                      <asp:TextBox Id="txtNoofSamples" runat="server" CssClass="form-control" MaxLength="3" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="NoOfSamples" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="txtNoofSamples" ErrorMessage="Please enter No.of Sample" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </div>
                                     <div class="col-md-1 text-right control-label">
                                         UOM<span class="text-danger">*</span>
                                     </div>
                                     <div class="col-md-3">
                                         <asp:DropDownList ID="drpUOM" runat="server" CssClass="form-control select2" Width=100%>
                                             </asp:DropDownList>
                                             <asp:RequiredFieldValidator InitialValue="0" ID="UOM" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="drpUOM" ErrorMessage="Please select UOM" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </div>
                                   
                                 </div>
                                 <div class="row form-group">
                                      <div class="col-md-1 text-right control-label">
                                         Repeat<span class="text-danger">*</span>
                                     </div>
                                     <div class="col-md-3">
                                         <label>Hr</label>
                                            <asp:DropDownList ID="ddlHour" runat="server" CssClass="form-control select2" Width=30%>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="" ID="ReqHour" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="ddlHour" ErrorMessage="Please select hour" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <label>Min</label>
                                            <asp:DropDownList ID="ddlMinute" runat="server" CssClass="form-control select2" Width=30%>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator InitialValue="" ID="ReqMinute" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="ddlMinute" ErrorMessage="Please select minute" ForeColor="Red"></asp:RequiredFieldValidator>
                                        
                                     </div>
                                 </div>
                               
                                 <div class="row form-group" runat="server" id="QualitativeGroup">
                                     <div class="col-md-1 text-right control-label">
                                         Tol.LCL <span class="text-danger">*</span>
                                     </div>
                                     <div class="col-md-3">
                                      <asp:TextBox ID="txtLCL" runat="server" CssClass="form-control" MaxLength="7" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                      <asp:RequiredFieldValidator ID="validatorLCL" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="txtLCL" ErrorMessage="Please enter LCL" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </div>
                                     <div class="col-md-1 text-right control-label">
                                         Tol.UCL<span class="text-danger">*</span>
                                     </div>
                                     <div class="col-md-3">
                                         <asp:TextBox ID="txtUCL" runat="server" CssClass="form-control" MaxLength="7" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                         <asp:RequiredFieldValidator ID="validatorUCL" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="txtUCL" ErrorMessage="Please enter UCL" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </div>
                                    <div class="col-md-1 text-right control-label">
                                         UOM<span class="text-danger">*</span>
                                     </div>
                                     <div class="col-md-3">
                                       <asp:TextBox ID="txtQUOM" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                         <asp:RequiredFieldValidator ID="validatorQUOM" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="txtQUOM" ErrorMessage="Please enter UOM" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </div>
                                 </div>
                           
                                 <div class="row form-group" runat="server" id="QuantitativeGroup">
                                     <div class="col-md-2 text-right control-label">
                                         Expected Result <span class="text-danger">*</span>
                                     </div>
                                     <div class="col-md-4">
                                     <asp:TextBox ID="txtEResult" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="validatorEResult" Display="Dynamic" ValidationGroup="g2" runat="server" ControlToValidate="txtEResult" ErrorMessage="Please enter excepted result" ForeColor="Red"></asp:RequiredFieldValidator>
                                     </div>
                                      
                                 </div>
                                 <div class="row form-group" align="right">
                                     <div class="col-md-12">
                                      <asp:Button runat="server" ID="UpdateParam" CssClass="btn btn-success" Text="Save Parameter" ValidationGroup="g2" OnClick="btnUpdateParam_Click" />
                                      <asp:Button runat="server" ID="AddParam" CssClass="btn btn-success" Text="Add Parameter" ValidationGroup="g2" OnClick="btnAddParam_Click" />
                                      <asp:Button runat="server" ID="Clear" CssClass="btn btn-danger" Text="Clear" OnClick="btnClearParam_Click" />
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
