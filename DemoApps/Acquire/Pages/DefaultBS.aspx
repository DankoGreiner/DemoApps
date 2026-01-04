<%@ Page Title="" Language="C#" MasterPageFile="~/Acquire/ACQMaster.Master" AutoEventWireup="true" CodeBehind="DefaultBS.aspx.cs" Inherits="DemoApps.Acquire.Pages.DefaultBS" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="updMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            ACQUIRE game helper

 <hr />
            <asp:GridView ID="gvGameList" runat="server" Visible="false"></asp:GridView>
            <hr />

            <div class="container-fluid">
    <div>
     <div class="row">
         <div class="col">
             <b>Hotel</b>
         </div>
         <div class="col"></div>
         <div class="col"></div>
         <div class="col">
             <b>Stock price</b>
         </div>
         <div class="col">
             <b>Holders</b>
         </div>
     </div>

     <div class="row">
         <div class="col">
             <img src="Images/Sackson.png" height="50px" />
         </div>
         <div class="col">
             <asp:Button ID="btnStartSackson" runat="server" Text="Start" OnClick="btnStartSackson_Click" />
             <asp:TextBox ID="txtSacksonCount" runat="server" Enabled="false" Text="0"></asp:TextBox>
             <asp:Button ID="btnAddSackson" runat="server" Text="+" OnClick="btnAddSackson_Click" />
             <asp:Button ID="btnResetSackson" runat="server" Text="Reset" OnClick="btnResetSackson_Click" />
         </div>
         <div class="col"></div>
         <div class="col">
             <asp:Label ID="lblSacksonPrice" runat="server"></asp:Label>
         </div>
         <div class="col">
             <asp:Label ID="lblSacksonHolderValues" runat="server"></asp:Label>
         </div>
     </div>

     <div class="row">
         <div class="col">
             <img src="Images/Tower.png" height="50px" />
         </div>
         <div class="col">
             <asp:Button ID="btnStartTower" runat="server" Text="Start" OnClick="btnStartTower_Click" />
             <asp:TextBox ID="txtTowerCount" runat="server" Enabled="false" Text="0"></asp:TextBox>
             <asp:Button ID="btnAddTower" runat="server" Text="+" OnClick="btnAddTower_Click" />
             <asp:Button ID="btnResetTower" runat="server" Text="Reset" OnClick="btnResetTower_Click" />
         </div>
         <div class="col"></div>
         <div class="col">
             <asp:Label ID="lblTowerPrice" runat="server"></asp:Label>
         </div>
         <div class="col">
             <asp:Label ID="lblTowerHolderValues" runat="server"></asp:Label>
         </div>
     </div>
     <div class="row">
         <div class="col">
             <hr />
         </div>
     </div>
     <div class="row">
         <div class="col">
             <img src="Images/American.png" height="50px" />
         </div>
         <div class="col">
             <asp:Button ID="btnStartAmerican" runat="server" Text="Start" OnClick="btnStartAmerican_Click" />
             <asp:TextBox ID="txtAmericanCount" runat="server" Enabled="false" Text="0"></asp:TextBox>
             <asp:Button ID="btnAddAmerican" runat="server" Text="+" OnClick="btnAddAmerican_Click" />
             <asp:Button ID="btnResetAmerican" runat="server" Text="Reset" OnClick="btnResetAmerican_Click" />
         </div>
         <div class="col"></div>
         <div class="col">
             <asp:Label ID="lblAmericanPrice" runat="server"></asp:Label>
         </div>
         <div class="col">
             <asp:Label ID="lblAmericanHolderValues" runat="server"></asp:Label>
         </div>
     </div>

     <div class="row">
         <div class="col">
             <img src="Images/Festival.png" height="50px" />
         </div>
         <div class="col">
             <asp:Button ID="btnStartFestival" runat="server" Text="Start" OnClick="btnStartFestival_Click" />
             <asp:TextBox ID="txtFestivalCount" runat="server" Enabled="false" Text="0"></asp:TextBox>
             <asp:Button ID="btnAddFestival" runat="server" Text="+" OnClick="btnAddFestival_Click" />
             <asp:Button ID="btnResetFestival" runat="server" Text="Reset" OnClick="btnResetFestival_Click" />
         </div>
         <div class="col"></div>
         <div class="col">
             <asp:Label ID="lblFestivalPrice" runat="server"></asp:Label>
         </div>
         <div class="col">
             <asp:Label ID="lblFestivalHolderValues" runat="server"></asp:Label>
         </div>
     </div>

     <div class="row">
         <div class="col">
             <img src="Images/Worldwide.png" height="50px" />
         </div>
         <div class="col">
             <asp:Button ID="btnStartWorldwide" runat="server" Text="Start" OnClick="btnStartWorldwide_Click" />
             <asp:TextBox ID="txtWorldwideCount" runat="server" Enabled="false" Text="0"></asp:TextBox>
             <asp:Button ID="btnAddWorldwide" runat="server" Text="+" OnClick="btnAddWorldwide_Click" />
             <asp:Button ID="btnResetWorldwide" runat="server" Text="Reset" OnClick="btnResetWorldwide_Click" />
         </div>
         <div class="col"></div>
         <div class="col">
             <asp:Label ID="lblWorldwidePrice" runat="server"></asp:Label>
         </div>
         <div class="col">
             <asp:Label ID="lbllWorldwideHolderValues" runat="server"></asp:Label>
         </div>
     </div>

     <div class="row">
         <div class="col">
             <hr />
         </div>
     </div>

     <div class="row">
         <div class="col">
             <img src="Images/Continental.png" height="50px" />
         </div>
         <div class="col">
             <asp:Button ID="btnStartContinental" runat="server" Text="Start" OnClick="btnStartContinental_Click" />
             <asp:TextBox ID="txtContinentalCount" runat="server" Enabled="false" Text="0"></asp:TextBox>
             <asp:Button ID="btnAddContinental" runat="server" Text="+" OnClick="btnAddContinental_Click" />
             <asp:Button ID="btnResetContinental" runat="server" Text="Reset" OnClick="btnResetContinental_Click" />
         </div>
         <div class="col"></div>
         <div class="col">
             <asp:Label ID="lblContinentalPrice" runat="server"></asp:Label>
         </div>
         <div class="col">
             <asp:Label ID="lblContinentalHolderValues" runat="server"></asp:Label>
         </div>
     </div>

     <div class="row">
         <div class="col">
             <img src="Images/Imperial.png" height="50px" />
         </div>
         <div class="col">
             <asp:Button ID="btnStartImperial" runat="server" Text="Start" OnClick="btnStartImperial_Click" />
             <asp:TextBox ID="txtImperialCount" runat="server" Enabled="false" Text="0"></asp:TextBox>
             <asp:Button ID="btnAddImperial" runat="server" Text="+" OnClick="btnAddImperial_Click" />
             <asp:Button ID="btnResetImperial" runat="server" Text="Reset" OnClick="btnResetImperial_Click" />
         </div>
         <div class="col"></div>
         <div class="col">
             <asp:Label ID="lblImperialPrice" runat="server"></asp:Label>
         </div>
         <div class="col">
             <asp:Label ID="lblImperialHolderValues" runat="server"></asp:Label>
         </div>
     </div>



 </div>

        </ContentTemplate>
    </asp:UpdatePanel>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBbodyEnd" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphCustomPageScript" runat="server">
</asp:Content>
