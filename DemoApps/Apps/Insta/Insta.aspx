<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="Insta.aspx.cs" Inherits="DemoApps.Apps.Insta.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

      <script type="text/javascript">
      //Code will go here

      function handlePaste(e) {
          for (var i = 0 ; i < e.clipboardData.items.length ; i++) {
              var item = e.clipboardData.items[i];
              console.log("Item: " + item.type);
              //alert(item.type);

              var blob = item.getAsFile();
              var reader = new FileReader();
              reader.onload = function (event) {
                  document.getElementById('lblBinaryString').value = event.target.result;
                  document.getElementById('lblBinaryString').innerHTML = event.target.result;
                  document.getElementById('hidBinaryString').value = event.target.result;
                  
              }; // data url!
              reader.readAsDataURL(blob);
          }

          //alert(document.getElementById('hidBinaryString').value);
          //document.getElementById('bntSave').click();
      }



      window.onload = function () {
          document.getElementById("pasteTargetAll").
              addEventListener("paste", handlePaste);
      };


      </script>

</head>
<body>
    <form id="form1" runat="server">
         <div id="pasteTargetAll">


     <asp:Label ID="lblBinaryString" runat="server" ClientIDMode="Static" Text="BinaryData"></asp:Label>
     <asp:HiddenField ID="hidBinaryString" runat="server" ClientIDMode="Static" />
     <br />
     <asp:Button ID="bntSave" runat="server" Text="Save" OnClick="bntSave_Click" ClientIDMode="Static" />
     <br />
     <br />
     <asp:TextBox ID="txtRename" runat="server"></asp:TextBox>
     <asp:Button ID="btnRename" runat="server" OnClick="btnRename_Click" Text="Rename" />

 </div>

        <hr />

        <div>
            <asp:TextBox ID="txtUrl" runat="server" Width="800px"></asp:TextBox>
            <asp:Button ID="btnInsta" runat="server" Text="PublishToInsta" OnClick="btnInsta_Click" />
            <br />
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
