<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductionGraceLimitMaster.aspx.cs" Inherits="MFG_DigitalApp.ProductionGraceLimitMaster" MasterPageFile="~/MasterPage.Master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
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
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 2))
                return false;
            return true;
        }
    </script>
    <style>
        .rbtnlbl {
            margin: 5px 0px;
        }

            .rbtnlbl label {
                padding-left: 5px;
                padding-right: 10px;
            }

        .modalBackground {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 1040;
            background-color: #000;
            opacity: 0.8;
        }

        #myModalAlertStartAddParam {
            width: 600px;
            top: 0 !important;
        }

        .btn {
            font-weight: bold !important;
            font-size: 12px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box box-default">
        <div class="box-body">
            <div class="row" style="margin-bottom: 10px; margin-top: 10px;">
                <div class="col-md-12">
                    <div class="col-md-3">
                        <asp:DropDownList ID="ddlPlant" runat="server" CssClass="form-control select2"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvPlant" runat="server" ControlToValidate="ddlPlant" ErrorMessage="Please select the plant" Display="Dynamic" ValidationGroup="validate" InitialValue="Select Plant"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-3" style="display: none;">
                        <asp:DropDownList ID="ddlLine" runat="server" CssClass="form-control select2"></asp:DropDownList>
                    </div>
                    <div class="col-md-3">
                        <asp:TextBox ID="txtPercentage" runat="server" Placeholder="Percentage" CssClass="form-control" onkeypress="return isNumeric(event,this)"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvpercentage" runat="server" Display="Dynamic" ControlToValidate="txtPercentage" ErrorMessage="Please enter percentage" ValidationGroup="validate"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-3">
                        <asp:Button ID="txtSubmit" runat="server" Text="Submit" CssClass="btn btn-success" OnClick="txtSubmit_Click" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="txtClear" runat="server" Text="Clear" CssClass="btn btn-primary" OnClick="txtClear_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="box box-default">
        <div class="box-body">
            <div class="row" style="margin-bottom: 10px; margin-top: 10px;">
                <div class="col-md-12">
                    <center>
                    <asp:GridView ID="grdPlantDetails" runat="server" AutoGenerateColumns="false" Width="90%" CssClass="table table-striped table-bordered table-hover" AutoGenerateEditButton="false"
                        OnRowEditing="grdPlantDetails_RowEditing" OnRowDeleting="grdPlantDetails_RowDeleting" OnRowCommand="grdPlantDetails_RowCommand" DataKeyNames="PlantCode">
                        <Columns>
                            <asp:BoundField DataField="PlantCode" HeaderText="Plant Code" ItemStyle-Width="28%"/>
                            <asp:BoundField DataField="PlantName" HeaderText="Plant Name" ItemStyle-Width="28%" />
                            <asp:BoundField DataField="Percentage" HeaderText="Percentage" ItemStyle-Width="28%" />
                                                  <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <center>
                                            <asp:ImageButton ID="getInvoiceByID" CommandArgument="VendorId" CommandName ="Edit" runat="server" ImageUrl="~/images/edit.png" Height="22px" OnClick="getInvoiceByID_Click"/>&nbsp;&nbsp;
                                            <asp:ImageButton ID="btnDeleteVendor" CommandArgument="VendorId" CommandName ="Delete" runat="server" ImageUrl="~/images/rejectok.jpg" Height="22px" OnClick="ImageButton1_Click" OnClientClick="return confirm('Are you sure you want to delete Vendor?');"/>
                                                </center>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                    </Columns>
                        <emptydatatemplate>
                            <center>No Plant data available</center>
                        </emptydatatemplate>
                    </asp:GridView>
                        </center>
                </div>
            </div>
        </div>
</asp:Content>
