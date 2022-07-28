using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Learner
{

    public String firstName { get; set; }
    public String lastName { get; set; }
    public String email { get; set; }
    public String username { get; set; }
    public String password { get; set; }
    public String uid { get; set; }
    public String instructorUID { get; set; }
    public List<string> tasks { get; set; }
    public DateTime lastActive { get; set; }

    public Learner(String uid, String firstName, String lastName, String email, String username, String password, DateTime lastActive, String instructorUID, List<string> tasks){
        this.firstName = firstName;
        this.lastName = lastName;
        this.username = username;
        this.email = email;
        this.password = password;
        this.uid = uid;
        this.lastActive = lastActive;
        this.instructorUID = instructorUID;
        this.tasks = tasks;
    }
    
    public String getName(){
        return firstName + " " + lastName;
    }

    public Learner getLearner(){
        return this;
    }

    public override string ToString()
    {
        return uid + ": " + username + "," + firstName + ", " + lastName + ", " + instructorUID;
    }


    public string LastActive()
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;
        var ts = new TimeSpan(DateTime.Now.Ticks - lastActive.Ticks);
        double delta = Math.Abs(ts.TotalSeconds);

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
            int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
            return months <= 1 ? "one month ago" : months + " months ago";
        }
        else
        {
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }
    }

}
