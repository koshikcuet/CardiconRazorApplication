namespace Cardicon.wwwroot.Model
{
    public class xapi
    {
        public class registration
        {
            public long atn { get; set; }
            public string title { get; set; } = "";
            public string nam { get; set; } = "";
            public string degree { get; set; } = "";
            public string institute { get; set; } = "";
            public string designation { get; set; } = "";
            public string cat { get; set; } = "";
            public string city { get; set; } = "CHITTAGONG";
            public string country { get; set; } = "BANGLADESH";
            public string mobile { get; set; } = "";
            public string email { get; set; } = "";
            public DateTime dt { get; set; }
            public string bmdc { get; set; } = "";
            public string remark { get; set; } = "";
            public string slip { get; set; } = "";
            public string saveby { get; set; } = "";
            public int tk { get; set; } = 0;
            public string status { get; set; } = "Pending";
            public IFormFile? postedFile { get; set; }
        }
                
        public class abstracts
        {
            public long atn { get; set; }
            public string nam { get; set; } = "";
            public int reg_no { get; set; }
            public string mobile { get; set; } = "";
            public string email { get; set; } = "";
            public string abs_cat { get; set; } = "ABSTRACT";
            public string abs_head { get; set; } = "";
            public string abs_desc { get; set; } = "";
            public string abs_link { get; set; } = "";
            public string remark { get; set; } = "";
            public string status { get; set; } = "Pending";
            public string saveby { get; set; } = "";
            public DateTime dt { get; set; }
            public IFormFile? postedFile { get; set; }
        }

        public class responly
        {
            public string message { get; set; } = "";
        }
}
}
