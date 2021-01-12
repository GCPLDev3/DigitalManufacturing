<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ShiftDetails.aspx.cs" Inherits="MFG_DigitalApp.ShiftDetails" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-wrapper" style="min-height: 500px;">
        <div class="container">
            <section class="content">

                <div class="box box-default">
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <div class="form-group has-feedback">
                                    <label>Shift</label>
                                    <asp:DropDownList ID="drpShift" runat="server" AutoPostBack="true" CssClass="form-control select2" OnSelectedIndexChanged="drpShift_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator InitialValue="0" ID="Reqshift" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpShift" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                <div class="form-group has-feedback">
                                    <label>Plant</label>
                                    <asp:DropDownList ID="drpPlant" runat="server" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="drpPlant_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:RequiredFieldValidator InitialValue="0" ID="ReqStation" Display="Dynamic" ValidationGroup="g1" runat="server" ControlToValidate="drpPlant" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView runat="server" ID="GrdStoppageReason" AutoGenerateColumns="false" EmptyDataText="No Records Found!!"
                                    Width="100%" CssClass="table table-striped table-bordered table-hover" ShowHeaderWhenEmpty="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Line">
                                            <ItemTemplate>
                                                <asp:LinkButton Text='<%# Eval("Line") %>' runat="server" CommandArgument="<%# Container.DataItemIndex %>" ID="lnkLine" OnClick="lnkLine_Click" Font-Bold="true" ForeColor="#3333ff" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PONumber" HeaderText="Process Order" />
                                        <asp:BoundField DataField="MaterialDescrip" HeaderText="Material" />
                                        <asp:BoundField DataField="OrderQuantity" HeaderText="Order Quantity" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="ShiftProduction" HeaderText="Shift Production" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField DataField="TotalDeliveryQuantity" HeaderText="Total Delivery Quantity" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                    <PagerStyle CssClass="GridPager" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
