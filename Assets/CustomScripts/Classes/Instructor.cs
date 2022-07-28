using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Instructor
{

    public String email { get; set; }
    public String firstName { get; set; }
    public String lastName { get; set; }
    public String username { get; set; }
    public String password { get; set; }
    public String uid  { get; set; }
    public List<String> learnersUIDs { get; set; }

    public Instructor(String uid, String email, String username, String password, String firstName, String lastName, List<String> learnersUIDs)
    {
        this.email = email;
        this.username = username;
        this.password = password;
        this.uid = uid;
        this.learnersUIDs = learnersUIDs;
        this.firstName = firstName;
        this.lastName = lastName;
    }



}
