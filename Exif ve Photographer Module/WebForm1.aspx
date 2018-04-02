<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Photogasm.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server" EnableViewState="true">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js"></script>
    <script type="text/javascript">
        function UploadFile(fileUpload) {
            if (fileUpload.value != '') {
                document.getElementById("<%=btnUpload.ClientID %>").click();
            }
        }
        function closeExif() {
            var classNameExit = document.getElementsByClassName("exifCssExit");
            var className = document.getElementsByClassName("exifCss");

            classNameExit[0].style.transition = "0.8s";
            className[0].style.transition = "0.6s";

            classNameExit[0].style.transform = "scale(0)";
            className[0].style.transform = "translate(-50%, -50%) scale(0)";

            className[0].style.opacity = "0";
            classNameExit[0].style.opacity = "0";
        }
    </script>

    <asp:UpdatePanel ID="panel1" runat="server">
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-md-3"></div>
                    <div class="col-md-6">
                        <div class="exifCssExit" runat="server" id="exifCssExitID" onclick="closeExif()"></div>
                        <h1 class="sBaslikR">Create Project</h1>
                        <asp:Label ID="prj_write" runat="server" Text="" Visible="false"></asp:Label>
                        <div class="input-group">
                            <span class="input-group-addon">Project Name</span>
                            <asp:TextBox ID="lblProjectName" CssClass="form-control" runat="server"> </asp:TextBox>
                        </div>
                        <br />
                        <asp:Button ID="btnProject" CssClass="btn btn-success btn-block" runat="server" Text="Next" OnClick="CreatePrj" />

                        <br />
                        <asp:Label ID="Errorlabel1" runat="server" Text=""></asp:Label>
                        <asp:Label ID="filepath1" runat="server" Text="" Visible="false"></asp:Label>

                        <asp:Label ID="IblStatus" runat="server" Text=""></asp:Label>

                        <div runat="server" class="uploadCss" id="uploadCssID">
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                            <div class="hidden">
                                <asp:Button ID="btnUpload" runat="server" Text="Next" OnClick="btn_Upload" CssClass="btn btn-success btn-block" />
                            </div>
                        </div>
                        <div runat="server" id="sImgId" class="sImgCss thumbnail center">
                            <div class="rateCss">
                                <asp:ImageButton ID="s1" runat="server" ImageUrl="~/Images/ucstar.png" OnClick="rateImage" />
                                <asp:ImageButton ID="s2" runat="server" ImageUrl="~/Images/ucstar.png" OnClick="rateImage" />
                                <asp:ImageButton ID="s3" runat="server" ImageUrl="~/Images/ucstar.png" OnClick="rateImage" />
                                <asp:ImageButton ID="s4" runat="server" ImageUrl="~/Images/ucstar.png" OnClick="rateImage" />
                                <asp:ImageButton ID="s5" runat="server" ImageUrl="~/Images/ucstar.png" OnClick="rateImage" />
                                <asp:Label ID="lbl_rate" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="projeImg">
                                <asp:ImageButton ID="ImageButton1" CssClass="sImgCssInfo" ImageUrl="~/Images/info.png" runat="server" OnClick="infoImg_Click" />
                                <img src="" id="Image1" runat="server" />
                                <br />
                                <div class="caption">
                                    <div class="input-group">
                                        <span class="input-group-addon">Description</span>
                                        <input id="msg" type="text" class="form-control" name="msg">
                                    </div>
                                </div>
                            </div>
                            <div class="exifCss" runat="server" id="exifCssID">
                                <table class="table table-condensed">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text="Taken Date"></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbltaken_Date"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text="Camera Model"></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" ID="lblcamera_Model"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text="Focal Length"></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" ID="lblfocal_Length"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text="ISO Speed"></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" ID="lbliso_Speed"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text="Aperture Value"></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" ID="lblaperture_value"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Text="Shutter Speed"></asp:Label></td>
                                        <td>
                                            <asp:Label runat="server" ID="lblshutter_Speed"></asp:Label></td>
                                    </tr>
                                    <tr>
                                    </tr>
                                </table>
                            </div>

                            <br />
                    
                                <div class="addCss">
                                    <img src="Images/add.png"/>
                                    <span class="addCssToolTip">Add Another Photo></span>
                                </div>
                                <asp:Button ID="Button1" runat="server" Text="Done !" CssClass="btn btn-primary" />
                           
                        </div>


                        
                    </div>
                    <div class="col-md-3"></div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
