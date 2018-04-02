using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fractions;

namespace Photogasm
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public class ExifDet
        {
            public string _takendate { get; set; }
            public string _cameramodel { get; set; }
            public string _focal { get; set; }
            public string _isoSpeed { get; set; }
            public string _aperturevalue { get; set; }
            public string _shutterSpeed { get; set; }
        }

        public static SqlConnection Conn = new SqlConnection("Data Source=teambro.database.windows.net;Initial Catalog=PhotochArt_db;Integrated Security=False;User ID=n00ne;Password=123Qwe123;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        SqlCommand cmd;
        SqlDataReader dr;
        SqlDataAdapter ad;
        string imgUrl = "";
       //  List<exifPhoto> Exif_Items;
        List<ExifDet> Exif_Items;

      
        string takenDate;
        string cameraModel;
        string focalLength;
        string isoSpeed;
        string apertureValue;
        string shutterSpeed;

        private static Regex r = new Regex(":");

        protected void Page_Load(object sender, EventArgs e)
        {
            //Exif_Items = new List<exifPhoto>();
            Exif_Items = new List<ExifDet>();
            FileUpload1.Attributes["onchange"] = "UploadFile(this)";

        }
        private static Random random = new Random();
        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 20)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void CreatePrj(object sender, EventArgs e)
        {

            string url = Server.MapPath("~/Images/");
            string foldername = lblProjectName.Text;
            filepath1.Text = foldername;
            try
            {
                if (!Directory.Exists(url + foldername))
                {
                    Directory.CreateDirectory(url + foldername);
                    Errorlabel1.Text = "";

                    uploadCssID.Attributes.CssStyle.Add("transform", "scale(1)");
                    lblProjectName.Enabled = false;
                    btnProject.Visible = false;
                }
                else
                {
                    Errorlabel1.Text = "Project Name Exists or Null";
                }
            }
            catch { }


        }
        public void btn_Upload(object sender, EventArgs e)
        {
            string x = Server.MapPath(FileUpload1.FileName);
            string foldername = filepath1.Text;

            if (FileUpload1.HasFile)
            {
                try
                {
                    if ((FileUpload1.PostedFile.ContentType == "image/jpeg") ||
                        (FileUpload1.PostedFile.ContentType == "image/jpg") ||
                        (FileUpload1.PostedFile.ContentType == "image/png") ||
                        (FileUpload1.PostedFile.ContentType == "image/bmp") ||
                        (FileUpload1.PostedFile.ContentType == "image/gif"))
                    {
                        if (FileUpload1.PostedFile.ContentLength < 3000000) //3MB MAX
                        {
                            string filename = Path.GetFileName(FileUpload1.FileName);
                            string ext = System.IO.Path.GetExtension(FileUpload1.FileName);
                            string rndm = RandomString();
                            FileUpload1.SaveAs(Server.MapPath("~/Images/") + foldername + "/" + rndm + ext);
                            taken_Date(Server.MapPath("~/Images/") + foldername + "/" + rndm + ext);
                            camera_Model(Server.MapPath("~/Images/") + foldername + "/" + rndm + ext);
                            focal_Length(Server.MapPath("~/Images/") + foldername + "/" + rndm + ext);
                            iso_Speed(Server.MapPath("~/Images/") + foldername + "/" + rndm + ext);
                            aperture_value(Server.MapPath("~/Images/") + foldername + "/" + rndm + ext);
                            shutter_Speed(Server.MapPath("~/Images/") + foldername + "/" + rndm + ext);
                            string filePath = Server.MapPath("~/Images/") + foldername + "/" + rndm + ext;
                            Image1.Src = "~/Images/" + foldername + "/" + rndm + ext;

                            FileUpload1.Visible = false;
                            uploadCssID.Attributes.CssStyle.Add("transform", "scale(0)");
                            sImgId.Attributes.CssStyle.Add("transform", "scale(1)");
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('File must be max 3 MB')", true);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Only JPEG files accepted !')", true);
                }
                catch (Exception exc)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Something goes wrong !')", true);

                }
            }
        }
        public void taken_Date(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))

                using (System.Drawing.Image myImage = System.Drawing.Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
                    takenDate = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                }
                lbltaken_Date.Text = takenDate;
            }
            catch (Exception ex)
            {
                lbltaken_Date.Text = "null";

            }

        }
        public void camera_Model(string path)
        {

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (System.Drawing.Image myImage = System.Drawing.Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(272);
                    cameraModel = Encoding.UTF8.GetString(propItem.Value);
                }
                lblcamera_Model.Text = cameraModel;
            }
            catch { lblcamera_Model.Text = "Null"; }
        }
        public void focal_Length(string path)
        {

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (System.Drawing.Image myImage = System.Drawing.Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(37386);
                    //type 5
                    Fraction[] _resFraction = new Fraction[propItem.Len / (64 / 8)];
                    uint uNominator;
                    uint uDenominator;
                    for (int i = 0; i < _resFraction.Length; i++)
                    {
                        uNominator = BitConverter.ToUInt32(propItem.Value, i * (64 / 8));
                        uDenominator = BitConverter.ToUInt32(propItem.Value, i * (64 / 8) + (32 / 8));
                        _resFraction[i] = new Fraction(uNominator, uDenominator);
                    }
                    if (_resFraction.Length == 1)
                        lblfocal_Length.Text = _resFraction[0].ToString() + "mm";
                    else lblfocal_Length.Text = "null";
                }
            }
            catch { lblfocal_Length.Text = "Null"; }
        }
        public void iso_Speed(string path)
        {

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (System.Drawing.Image myImage = System.Drawing.Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(34855);
                    //type 5
                    ushort[] _resUshort = new ushort[propItem.Len / (16 / 8)];

                    for (int i = 0; i < _resUshort.Length; i++)
                    {
                        _resUshort[i] = BitConverter.ToUInt16(propItem.Value, i * (16 / 8));
                    }
                    if (_resUshort.Length == 1)
                        lbliso_Speed.Text = _resUshort[0].ToString();

                    else lbliso_Speed.Text = "null";
                }
            }
            catch { lbliso_Speed.Text = "Null"; }
        }
        public void aperture_value(string path)
        {

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (System.Drawing.Image myImage = System.Drawing.Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(33437);
                    //type 5
                    Fraction[] _resFraction = new Fraction[propItem.Len / (64 / 8)];
                    uint uNominator;
                    uint uDenominator;
                    for (int i = 0; i < _resFraction.Length; i++)
                    {
                        uNominator = BitConverter.ToUInt32(propItem.Value, i * (64 / 8));
                        uDenominator = BitConverter.ToUInt32(propItem.Value, i * (64 / 8) + (32 / 8));
                        _resFraction[i] = new Fraction(uNominator, uDenominator);
                    }

                    if (_resFraction.Length == 1)
                    {
                        string value = _resFraction[0].ToString();
                        string x, y;
                        decimal s = 0;
                        switch (value.Contains("/"))
                        {
                            case true:
                                int start = value.IndexOf("/");
                                x = value.Substring(0, start);
                                y = value.Substring(start + 1, value.Length - start - 1);
                                s = Convert.ToDecimal(x) / Convert.ToDecimal(y);
                                break;

                            case false:
                                s = Convert.ToDecimal(value);
                                break;
                        }


                        lblaperture_value.Text = "f" + s.ToString();
                    }
                    else lblaperture_value.Text = "null";
                }
            }
            catch { lblaperture_value.Text = "Null"; }
        }
        public void shutter_Speed(string path)
        {

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (System.Drawing.Image myImage = System.Drawing.Image.FromStream(fs, false, false))
                {

                    PropertyItem propItem = myImage.GetPropertyItem(33434);
                    Fraction[] _resFraction = new Fraction[propItem.Len / (64 / 8)];
                    int sNominator;
                    int sDenominator;
                    for (int i = 0; i < _resFraction.Length; i++)
                    {
                        sNominator = BitConverter.ToInt32(propItem.Value, i * (64 / 8));
                        sDenominator = BitConverter.ToInt32(propItem.Value, i * (64 / 8) + (32 / 8));
                        _resFraction[i] = new Fraction(sNominator, sDenominator);
                    }
                    if (_resFraction.Length == 1)
                        lblshutter_Speed.Text = _resFraction[0].ToString();
                    else lblshutter_Speed.Text = "null";

                }
            }
            catch { lblshutter_Speed.Text = "Null"; }

        }
        protected void rateImage(object sender, ImageClickEventArgs e)
        {
            var senderImage = sender as ImageButton;
            var ID = senderImage.ID;
            switch (ID)
            {
                case "s1":
                    s1.ImageUrl = "./Images/cstar.png";
                    s2.ImageUrl = "./Images/ucstar.png";
                    s3.ImageUrl = "./Images/ucstar.png";
                    s4.ImageUrl = "./Images/ucstar.png";
                    s5.ImageUrl = "./Images/ucstar.png";
                    lbl_rate.Text = "Rate:1";
                    break;
                case "s2":
                    s1.ImageUrl = "./Images/cstar.png";
                    s2.ImageUrl = "./Images/cstar.png";
                    s3.ImageUrl = "./Images/ucstar.png";
                    s4.ImageUrl = "./Images/ucstar.png";
                    s5.ImageUrl = "./Images/ucstar.png";
                    lbl_rate.Text = "Rate:2";
                    break;
                case "s3":
                    s1.ImageUrl = "./Images/cstar.png";
                    s2.ImageUrl = "./Images/cstar.png";
                    s3.ImageUrl = "./Images/cstar.png";
                    s4.ImageUrl = "./Images/ucstar.png";
                    s5.ImageUrl = "./Images/ucstar.png";
                    lbl_rate.Text = "Rate:3";
                    break;
                case "s4":
                    s1.ImageUrl = "./Images/cstar.png";
                    s2.ImageUrl = "./Images/cstar.png";
                    s3.ImageUrl = "./Images/cstar.png";
                    s4.ImageUrl = "./Images/cstar.png";
                    s5.ImageUrl = "./Images/ucstar.png";
                    lbl_rate.Text = "Rate:4";
                    break;
                case "s5":
                    s1.ImageUrl = "./Images/cstar.png";
                    s2.ImageUrl = "./Images/cstar.png";
                    s3.ImageUrl = "./Images/cstar.png";
                    s4.ImageUrl = "./Images/cstar.png";
                    s5.ImageUrl = "./Images/cstar.png";
                    lbl_rate.Text = "Rate:5";
                    break;
            }

        }

        public void infoImg_Click(object sender, ImageClickEventArgs e)
        {
            btnProject.Visible = true;
            exifCssID.Attributes.CssStyle.Add("transform", "translate(-50%, -50%) scale(1.5)");
            exifCssExitID.Attributes.CssStyle.Add("transform", "scale(1)");

        }
        public void taha()
        {
            //// Response.Redirect(Request.Url.AbsoluteUri);
            //Conn.Open();
            //SqlCommand cmd = new SqlCommand("INSERT INTO Photo(PID,U_ID,P_Path,Pr_ID) VALUES(@pid,@uid,@ppath,@prid) ", Conn);
            //cmd.Parameters.AddWithValue("@pid", "105");
            //cmd.Parameters.AddWithValue("@uid", "31");
            //cmd.Parameters.AddWithValue("@ppath", "http://teambro.azurewebsites.net/Images/" + foldername + "/" + rndm + ext);
            //cmd.Parameters.AddWithValue("@prid", "105");
            //cmd.ExecuteNonQuery();
            //Conn.Close();
            //Conn.Open();
            //cmd = new SqlCommand("INSERT INTO Category(CID,Name) VALUES(@cid,@nm)", Conn);
            //cmd.Parameters.AddWithValue("@cid", "105");
            //cmd.Parameters.AddWithValue("@nm", "test");
            //cmd.ExecuteNonQuery();
            //Conn.Close();
            //Conn.Open();
            //cmd = new SqlCommand("INSERT INTO Project(Pro_ID,Name,C_ID) VALUES(@proid,@nm,@cid)", Conn);
            //cmd.Parameters.AddWithValue("@proid", "105");
            //cmd.Parameters.AddWithValue("@nm", lblProjectName.Text);
            //cmd.Parameters.AddWithValue("@cid", "105");
            //cmd.ExecuteNonQuery();
            //Conn.Close();
        }
    }
}