using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class
{
    public string classID { get; set; }
    public string className { get; set; }
    public System.DateTime dateCreated { get; set; }
    public string instructorID { get; set; }
    public List<string> learnerIDs { get; set; }

    public Class(string ID, string name, System.DateTime creation, string instrID, List<string> lrnrs)
    {
        classID = ID;
        className = name;
        dateCreated = creation;
        instructorID = instrID;
        learnerIDs = lrnrs;
    }


    public string DateCreated()
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;
        var ts = new System.TimeSpan(System.DateTime.Now.Ticks - dateCreated.Ticks);
        double delta = System.Math.Abs(ts.TotalSeconds);

        if (delta < 1 * MINUTE)
            return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

        if (delta < 2 * MINUTE)
            return "a minute ago";

        if (delta < 45 * MINUTE)
            return ts.Minutes + " minutes ago";

        if (delta < 90 * MINUTE)
            return "an hour ago";

        if (delta < 24 * HOUR)
            return ts.Hours + " hours ago";

        if (delta < 48 * HOUR)
            return "yesterday";

        if (delta < 30 * DAY)
            return ts.Days + " days ago";

        if (delta < 12 * MONTH)
        {
            int months = System.Convert.ToInt32(System.Math.Floor((double)ts.Days / 30));
            return months <= 1 ? "one month ago" : months + " months ago";
        }
        else
        {
            int years = System.Convert.ToInt32(System.Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
    }
}
