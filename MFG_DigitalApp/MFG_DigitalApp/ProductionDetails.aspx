<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ProductionDetails.aspx.cs" Inherits="MFG_DigitalApp.ProductionDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>

    <style>
        .btn {
            font-weight: bold !important;
            font-size: 12px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                <div class="row" style="margin-top: 7px;">
                    <div class="col-md-12 col-sm-12 col-xs-12">
                        <div class="col-md-5 col-sm-5 col-xs-5">
                            <asp:Button runat="server" ID="btnstoppages" CssClass="btn btn-primary" Text="Stoppages" OnClick="btnstoppages_Click" />
                            <asp:Button runat="server" ID="btnRunDetails" CssClass="btn btn-primary" Text="Run Details" OnClick="btnRunDetails_Click" />
                            <asp:Button runat="server" ID="btnParameter" CssClass="btn btn-primary" Text="Parameter" OnClick="btnParameter_Click" />
                        </div>
                        <div class="col-md-4 col-sm-4 col-xs-4" style="padding-left: 0px; margin-left: -15px;">
                            <asp:DropDownList ID="drpShift" runat="server" AutoPostBack="true" CssClass="form-control select2" OnSelectedIndexChanged="drpShift_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3 col-sm-3 col-xs-3" style="text-align: right;">
                            <asp:Button runat="server" ID="btnSaveProdEntry1" CssClass="btn btn-success" Text="Save Entry" OnClick="btnSaveProdEntry_Click" />
                        </div>
                    </div>
                </div>
                <div class="box box-default" style="margin-top: 7px;">
                    <div class="box-body">
                        <div class="row" runat="server" id="DivHeaderEntry1">
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:Label runat="server" ID="lblWRP1" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div class="row" runat="server" id="DivGridEntry1">
                            <div class="col-md-12">
                                <asp:GridView runat="server" ID="GrdProdEntry1" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                    Width="100%" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="GrdProdEntry1_RowDataBound" ShowFooter="true">
                                    <Columns>
                                        <asp:BoundField DataField="WRPLine" HeaderText="WRP" Visible="false" />
                                        <asp:BoundField DataField="Time" HeaderText="Time" />
                                        <asp:BoundField DataField="PONumber" HeaderText="PO" />
                                        <asp:BoundField DataField="MaterialDescrip" HeaderText="Material Descrip" />
                                        <asp:BoundField DataField="SensorQty" HeaderText="Sen.Qty" ItemStyle-HorizontalAlign="Right" />
                                        <asp:TemplateField HeaderText="Act.Qty">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtActualQty" Text='<%# Eval("ActualQty") %>' onkeypress="return isNumberKey(event)" Width="50px" CssClass="numberalign"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList runat="server" ID="drpReasons"></asp:DropDownList>
                                                <asp:HiddenField runat="server" ID="hdnReasonCode" Value='<%# Eval("ReasonCode") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="row" runat="server" id="DivHeaderEntry2">
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:Label runat="server" ID="lblWRP2" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div class="row" runat="server" id="DivGridEntry2">
                            <div class="col-md-12">
                                <asp:GridView runat="server" ID="GrdProdEntry2" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                    Width="100%" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="GrdProdEntry1_RowDataBound" ShowFooter="true">
                                    <Columns>
                                        <asp:BoundField DataField="WRPLine" HeaderText="WRP" Visible="false" />
                                        <asp:BoundField DataField="Time" HeaderText="Time" />
                                        <asp:BoundField DataField="PONumber" HeaderText="PO" />
                                        <asp:BoundField DataField="MaterialDescrip" HeaderText="Material Descrip" />
                                        <asp:BoundField DataField="SensorQty" HeaderText="Sen.Qty" ItemStyle-HorizontalAlign="Right" />
                                        <asp:TemplateField HeaderText="Act.Qty">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtActualQty" Text='<%# Eval("ActualQty") %>' onkeypress="return isNumberKey(event)" Width="50px" CssClass="numberalign"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList runat="server" ID="drpReasons"></asp:DropDownList>
                                                <asp:HiddenField runat="server" ID="hdnReasonCode" Value='<%# Eval("ReasonCode") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="row" runat="server" id="DivHeaderEntry3">
                            <div class="col-md-8 col-sm-8 col-xs-8">
                                <asp:Label runat="server" ID="lblWRP3" Font-Bold="true"></asp:Label>
                            </div>
                        </div>
                        <div class="row" runat="server" id="DivGridEntry3">
                            <div class="col-md-12">
                                <asp:GridView runat="server" ID="GrdProdEntry3" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                    Width="100%" CssClass="table table-striped table-bordered table-hover" OnRowDataBound="GrdProdEntry1_RowDataBound" ShowFooter="true">
                                    <Columns>
                                        <asp:BoundField DataField="WRPLine" HeaderText="WRP" Visible="false" />
                                        <asp:BoundField DataField="Time" HeaderText="Time" />
                                        <asp:BoundField DataField="PONumber" HeaderText="PO" />
                                        <asp:BoundField DataField="MaterialDescrip" HeaderText="Material Descrip" />
                                        <asp:BoundField DataField="SensorQty" HeaderText="Sen.Qty" ItemStyle-HorizontalAlign="Right" />
                                        <asp:TemplateField HeaderText="Act.Qty">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtActualQty" Text='<%# Eval("ActualQty") %>' onkeypress="return isNumberKey(event)" Width="50px" CssClass="numberalign"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList runat="server" ID="drpReasons"></asp:DropDownList>
                                                <asp:HiddenField runat="server" ID="hdnReasonCode" Value='<%# Eval("ReasonCode") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="row"> 
                            <div class="col-sm-1">
                                <asp:Label runat="server" ID="Label1" Font-Bold="true" Text="WIP"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox runat="server" ID="txtWIP" MaxLength="10" Width="80px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:Label runat="server" ID="Label2" Font-Bold="true" Text="Off Loadings Nos."></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox runat="server" ID="txtOffLoadingsNos" MaxLength="10" Width="80px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:Label runat="server" ID="Label3" Font-Bold="true" Text="Quality Rejection"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox runat="server" ID="txtQualityRejection" MaxLength="10" Width="80px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:Label runat="server" ID="Label4" Font-Bold="true" Text="Process Rejection"></asp:Label>
                            </div>
                            <div class="col-sm-1">
                                <asp:TextBox runat="server" ID="txtProcessRejection" MaxLength="10" Width="80px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div>
                            <div class="col-md-1" style="text-align: right;">
                                <asp:Button runat="server" ID="btnSaveProdEntry" CssClass="btn btn-success" Text="Save Entry" OnClick="btnSaveProdEntry_Click" />
                            </div>
                            <div class="col-sm-1" style="text-align: right;">
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
