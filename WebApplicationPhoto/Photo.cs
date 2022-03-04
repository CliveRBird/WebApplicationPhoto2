using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace WebApplicationPhoto
{
    public class Photo
    {
        public int PhotoID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] PhotoData { get; set; }

    }

}