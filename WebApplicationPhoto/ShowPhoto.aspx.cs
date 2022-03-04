using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplicationPhoto
{
    public partial class ShowPhoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int photoid = Convert.ToInt32(Request.QueryString["photoid"]);
            Photo p = PhotoHelper.GetByID(photoid);

            Response.Clear();
            Response.ContentType = "image/pjpeg";
            Response.BinaryWrite(p.PhotoData);
            Response.End();
        }
    }
}