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
    
    <asp:HiddenField runat="server" ID="hdnfldVariableprod" Value="False" />
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
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="col-md-2">
                                <asp:Label runat="server" ID="lblWRP1" Font-Bold="true"></asp:Label></div>
                                <div class="col-md-6"></div>
                                <div class="col-md-4"><asp:Label runat="server" ID="lblMsgWRP1" Font-Bold="true" ForeColor="Red"></asp:Label></div>
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
<%--                                        <asp:TemplateField HeaderText="Qlty Rejection">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtQltyRejection" Text='<%# Eval("QltyRejection") %>' onkeypress="return isNumberKey(event)" Width="50px" CssClass="numberalign"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList runat="server" ID="drpReasons"></asp:DropDownList>
                                                <asp:HiddenField runat="server" ID="hdnReasonCode" Value='<%# Eval("ReasonCode") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qlty Rejection" ItemStyle-Width="13%">
                                            <%--<asp:TemplateField HeaderText="Qlty Rej" ItemStyle-Width="5%">--%>
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtQltyRejection" Text='<%# Eval("QltyRejection") %>' onkeypress="return isNumberKey(event)" Width="50px" CssClass="numberalign"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Process Rejection" ItemStyle-Width="13%">
                                            <%--<asp:TemplateField HeaderText="Proc Rej" ItemStyle-Width="5%">--%>
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtProcessRejection" Text='<%# Eval("ProcessRejection") %>' onkeypress="return isNumberKey(event)" Width="50px" CssClass="numberalign"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="row" runat="server" id="DivHeaderEntry2">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="col-md-2"><asp:Label runat="server" ID="lblWRP2" Font-Bold="true"></asp:Label></div>
                                <div class="col-md-6"></div>
                                <div class="col-md-4"><asp:Label runat="server" ID="lblMsgWRP2" Font-Bold="true" ForeColor="Red"></asp:Label></div>
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
                                        <%--<asp:TemplateField HeaderText="Qlty Rejection">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtQltyRejection" Text='<%# Eval("QltyRejection") %>' onkeypress="return isNumberKey(event)" Width="50px" CssClass="numberalign"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList runat="server" ID="drpReasons"></asp:DropDownList>
                                                <asp:HiddenField runat="server" ID="hdnReasonCode" Value='<%# Eval("ReasonCode") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qlty Rejection" ItemStyle-Width="13%">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtQltyRejection" Text='<%# Eval("QltyRejection") %>' onkeypress="return isNumberKey(event)" Width="50px" CssClass="numberalign"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Process Rejection" ItemStyle-Width="13%">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtProcessRejection" Text='<%# Eval("ProcessRejection") %>' onkeypress="return isNumberKey(event)" Width="50px" CssClass="numberalign"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="row" runat="server" id="DivHeaderEntry3">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="col-md-2"><asp:Label runat="server" ID="lblWRP3" Font-Bold="true"></asp:Label></div>
                                <div class="col-md-6"></div>
                                <div class="col-md-4"><asp:Label runat="server" ID="lblMsgWRP3" Font-Bold="true" ForeColor="Red"></asp:Label></div>
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
                                        <%--<asp:TemplateField HeaderText="Qlty Rejection">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtQltyRejection" Text='<%# Eval("QltyRejection") %>' onkeypress="return isNumberKey(event)" Width="50px" CssClass="numberalign"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Reason">
                                            <ItemTemplate>
                                                <asp:DropDownList runat="server" ID="drpReasons"></asp:DropDownList>
                                                <asp:HiddenField runat="server" ID="hdnReasonCode" Value='<%# Eval("ReasonCode") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qlty Rejection" ItemStyle-Width="13%">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtQltyRejection" Text='<%# Eval("QltyRejection") %>' onkeypress="return isNumberKey(event)" Width="50px" CssClass="numberalign"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Process Rejection" ItemStyle-Width="13%">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtProcessRejection" Text='<%# Eval("ProcessRejection") %>' onkeypress="return isNumberKey(event)" Width="50px" CssClass="numberalign"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class="row">
                           <div class="col-md-2 col-sm-2 col-xs-2">
                                <asp:Label runat="server" ID="Label1" Font-Bold="true" Text="WIP"></asp:Label>
                            </div>
                            <div class="col-md-2 col-sm-2 col-xs-2">
                                <asp:TextBox runat="server" ID="txtWIP" MaxLength="10" Width="80px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div><div class="col-md-1"></div>
                            <div class="col-md-2 col-sm-2 col-xs-2">
                                <asp:Label runat="server" ID="Label2" Font-Bold="true" Text="Off Loadings Nos."></asp:Label>
                            </div>
                            <div class="col-md-2 col-sm-2 col-xs-2">
                                <asp:TextBox runat="server" ID="txtOffLoadingsNos" MaxLength="10" Width="80px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div><div class="col-md-1"></div>
                            <div class="col-md-2 col-sm-2 col-xs-2" style="display:none">
                                <asp:Label runat="server" ID="Label3" Font-Bold="true" Text="Quality Rejection"></asp:Label>
                            </div>
                            <div class="col-md-1 col-sm-1 col-xs-1" style="display:none">
                                <asp:TextBox runat="server" ID="txtQualityRejection" MaxLength="10" Width="80px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-sm-2 col-xs-2" style="display:none">
                                <asp:Label runat="server" ID="lblProcessRejection" Font-Bold="true" Text="Process Rejection"></asp:Label>
                            </div>
                            <div class="col-md-1 col-sm-1 col-xs-1" style="display:none">
                                <asp:TextBox runat="server" ID="txtProcRejection" MaxLength="10" Width="80px" onkeypress="return isNumberKey(event)"></asp:TextBox>
                            </div>
                            <div class="col-md-1 col-sm-1 col-xs-1" style="text-align: right;">
                                <asp:Button runat="server" ID="btnSaveProdEntry" CssClass="btn btn-success" Text="Save Entry" OnClick="btnSaveProdEntry_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
    <script type="text/javascript">
        getCurrentDate();
        

        function getCurrentDate() {
            debugger;
            now = new Date();
            var timezone = now.getTimezoneOffset() * 60000;
            var utc = now.getTime() + (now.getTimezoneOffset() * 60000);
            indotime = new Date(utc + (3600000 * 7));

            var now = new Date();
            var CurrentTime = new Date(indotime);
            var date = "" + CurrentTime.getDate(); if (date.length == 1) { date = "0" + date; }
            var hours = "" + CurrentTime.getHours(); if (hours.length == 1) { hours = "0" + hours; }
            var Minutes = "" + CurrentTime.getMinutes(); if (Minutes.length == 1) { Minutes = "0" + Minutes; }
            var second = "" + CurrentTime.getSeconds(); if (second.length == 1) { second = "0" + second; }
            var Month = "" + (CurrentTime.getMonth() + 1); if (Month.length == 1) { Month = "0" + Month; }
            var Year = "" + CurrentTime.getFullYear();
            var curDate = date + "-" + Month + "-" + Year + "  " + hours + ":" + Minutes + ":" + second;
            document.getElementById('<%= hdnfldVariableprod.ClientID %>').value = curDate;
            setTimeout(getCurrentDate, 1000);
        }
    </script>
</asp:Content>
