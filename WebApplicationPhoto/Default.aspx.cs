using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;

using WebApplicationPhoto;

namespace WebApplicationPhoto
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                BindDataPhotos();
                if (DetailsView1.Rows.Count == 0)
                {
                    DetailsView1.ChangeMode(DetailsViewMode.Insert);
                }

                BindDataPersons();
                if (DetailsView2.Rows.Count == 0)
                {
                    DetailsView2.ChangeMode(DetailsViewMode.Insert);
                }
            }

        }

        private void BindDataPhotos()
        {
            List<Photo> photos = PhotoHelper.GetAll();
            DetailsView1.DataSource = photos;
            DetailsView1.DataBind();
        }

        private void BindDataPersons()
        {
            List<Person> persons = PersonHelper.GetAll();
            DetailsView2.DataSource = persons;
            DetailsView2.DataBind();
        }


        protected void DetailsView1_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            DetailsView1.PageIndex = e.NewPageIndex;
            BindDataPhotos();
        }

        protected void DetailsView2_PageIndexChanging(object sender, DetailsViewPageEventArgs e)
        {
            DetailsView2.PageIndex = e.NewPageIndex;
            BindDataPersons();
        }
        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            DetailsView1.ChangeMode(e.NewMode);
            BindDataPhotos();
        }

        protected void DetailsView2_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            DetailsView2.ChangeMode(e.NewMode);
            BindDataPhotos();
        }

        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            Photo p = new Photo();

            TextBox t1 = ((TextBox)DetailsView1.Rows[1].Cells[1].Controls[0]);
            TextBox t2 = ((TextBox)DetailsView1.Rows[2].Cells[1].Controls[0]);
            FileUpload fu = ((FileUpload)DetailsView1.Rows[3].Cells[1].Controls[1]);

            p.Title = t1.Text;
            p.Description = t2.Text;

            Stream imgdatastream = fu.PostedFile.InputStream;
            int imgdatalen = fu.PostedFile.ContentLength;
            byte[] imgdata = new byte[imgdatalen];
            int n = imgdatastream.Read(imgdata, 0, imgdatalen);

            p.PhotoData = imgdata;

            PhotoHelper.Insert(p);
            BindDataPhotos();
        }

        protected void DetailsView2_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            Person p = new Person();

            TextBox t1 = ((TextBox)DetailsView2.Rows[1].Cells[1].Controls[0]);
            TextBox t2 = ((TextBox)DetailsView2.Rows[2].Cells[1].Controls[0]);
            TextBox t3 = ((TextBox)DetailsView2.Rows[3].Cells[1].Controls[0]);
            FileUpload fu = ((FileUpload)DetailsView2.Rows[4].Cells[1].Controls[1]);

            p.NationalInsuranceNumber = t1.Text;
            p.FirstName = t2.Text;
            p.LastName = t3.Text;

            Stream imgdatastream = fu.PostedFile.InputStream;
            int imgdatalen = fu.PostedFile.ContentLength;
            byte[] imgdata = new byte[imgdatalen];
            int n = imgdatastream.Read(imgdata, 0, imgdatalen);

            p.Photo = imgdata;

            PersonHelper.Insert(p);
            BindDataPersons();
        }


        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            Photo p = new Photo();

            TextBox t0 = ((TextBox)DetailsView1.Rows[0].Cells[1].Controls[0]);
            TextBox t1 = ((TextBox)DetailsView1.Rows[1].Cells[1].Controls[0]);
            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert t1:" + t1 + "');", true   );
            TextBox t2 = ((TextBox)DetailsView1.Rows[2].Cells[1].Controls[0]);
            FileUpload fu = ((FileUpload)DetailsView1.Rows[3].Cells[1].Controls[1]);

            p.PhotoID = Convert.ToInt32(t0.Text);
            p.Title = t1.Text;
            p.Description = t2.Text;

            Stream imgdatastream = fu.PostedFile.InputStream;
            int imgdatalen = fu.PostedFile.ContentLength;
            byte[] imgdata = new byte[imgdatalen];
            int n = imgdatastream.Read(imgdata, 0, imgdatalen);

            p.PhotoData = imgdata;

            PhotoHelper.Update(p);
            BindDataPhotos();
        }

        protected void DetailsView2_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            Person p = new Person();

            TextBox t0 = ((TextBox)DetailsView2.Rows[0].Cells[1].Controls[0]);
            TextBox t1 = ((TextBox)DetailsView2.Rows[1].Cells[1].Controls[0]);
            TextBox t2 = ((TextBox)DetailsView2.Rows[2].Cells[1].Controls[0]);
            TextBox t3 = ((TextBox)DetailsView2.Rows[3].Cells[1].Controls[0]);
            FileUpload fu = ((FileUpload)DetailsView2.Rows[4].Cells[1].Controls[1]);

            p.PersonID = Convert.ToInt32(t0.Text);
            p.NationalInsuranceNumber = t1.Text;
            p.FirstName = t2.Text;
            p.LastName = t3.Text;

            Stream imgdatastream = fu.PostedFile.InputStream;
            int imgdatalen = fu.PostedFile.ContentLength;
            byte[] imgdata = new byte[imgdatalen];
            int n = imgdatastream.Read(imgdata, 0, imgdatalen);

            p.Photo = imgdata;

            PersonHelper.Update(p);
            BindDataPersons();
        }

        protected void DetailsView1_ItemDeleting(object sender, DetailsViewDeleteEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.RowIndex);
           
            int photoid = rowIndex + 1;

            PhotoHelper.Delete(photoid);
            BindDataPhotos();
        }

        protected void DetailsView2_ItemDeleting(object sender, DetailsViewDeleteEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.RowIndex);

            int personid = rowIndex + 1;
            
            PersonHelper.Delete(personid);
            BindDataPersons();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {
                PatientEncryption encryptedPatient = new PatientEncryption();
                
                string FirstName = tbFirstName.Text;
                string LastName = tbLastName.Text;
                string BirthYear = ddlBirthYear.SelectedValue;
                string BirthMonth = ddlBirthMonth.SelectedValue;
                string BirthDay = ddlBirthDay.SelectedValue;
                encryptedPatient.insert(FirstName, LastName, BirthYear, BirthMonth, BirthDay);
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message.ToString();
            }

        }

        protected void btnFetch_Click(object sender, EventArgs e)
        {
            PatientEncryption encryptedPatient = new PatientEncryption();

            try
            {
                encryptedPatient.fetch();
                tbFirstName.Text = encryptedPatient.FirstName;
                tbLastName.Text = encryptedPatient.LastName;
                lblBirthDate.Text = encryptedPatient.BirthDate;
                
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message.ToString();
            }

        }

    }
}