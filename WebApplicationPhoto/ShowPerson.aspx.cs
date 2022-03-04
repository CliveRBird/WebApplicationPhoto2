using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplicationPhoto
{
    public partial class ShowPerson : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            int personid = Convert.ToInt32(Request.QueryString["personid"]);
            Person p = PersonHelper.GetByID(personid);

            Response.Clear();
            Response.ContentType = "image/pjpeg";
            Response.BinaryWrite(p.Photo);
            Response.End();

        }
    }
}