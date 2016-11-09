using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SMS : System.Web.UI.Page
{
    protected void btnLoadNums_Click(object sender, EventArgs e)
    {
        string numDump = txtDump.Text;
        List<string> nums = new List<string>();
        foreach (string num in numDump.Split(','))
        {
            if (num.Length >= 10)
            {
                nums.Add(num.Substring(num.Length - 10, 10));
            }
        }
        lstSMS.DataSource = nums;
        lstSMS.DataBind();
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        Dictionary<string, string> dictSMS = new Dictionary<string, string>(10);

        foreach (ListItem item in lstSMS.Items)
        {
            string num = item.Value;
            if (libSMSPbGovt.SMS.SendSMS(num, txtMessage.Text))
            {
                dictSMS.Add(num, "Sent");
            }
            else
            {
                dictSMS.Add(num, "Not Sent");
            }
        }
        string csv = string.Join(Environment.NewLine, dictSMS.Select(d => d.Key + "," + d.Value));
        string filename = Server.MapPath("smsrep.csv");
        System.IO.File.WriteAllText(filename, csv);
        Utils.DownloadFile(filename, true, "text/csv");
    }
}