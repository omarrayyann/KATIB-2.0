using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Static class that that handles most file-fetching and saving, as well as holding and transfering temporary memory between scenes
/// </summary>
public static class Data
{

	public static List<Collectable> collectables = new List<Collectable>();

	/// <summary>
	/// Dictionary linking each scene to a string stack that holds the panel order
	/// </summary>
	public static Dictionary<string, Stack<string>> lastActiveSceneMenus = new Dictionary<string, Stack<string>>() { { "LearnersTab", new Stack<string>() }, { "Classes", new Stack<string>() }, { "Settings", new Stack<string>() } };
	/// <summary>
	/// List of loaded instructors
	/// </summary>
	public static List<Instructor> instructors = new List<Instructor>();
	/// <summary>
	/// List of loaded Learners
	/// </summary>
	public static List<Learner> learners = new List<Learner>();
	/// <summary>
	/// List of loaded classes
	/// </summary>
	public static List<Class> classes = new List<Class>();
	/// <summary>
	/// List of loaded classes
	/// </summary>
	public static List<Task> tasks = new List<Task>();


	/// <summary>
	/// Current instructor being dealt with
	/// </summary>
	public static Instructor currentInstructor = null;
	/// <summary>
	/// Current learner being dealt with
	/// </summary>
	public static Learner currentLearner = null;
	/// <summary>
	/// Current class being dealt with
	/// </summary>
	public static Class currentClass = null;
	/// <summary>
	/// Current class being dealt with
	/// </summary>
	public static Task currentTask = null;

	
	

  	public static string lettersType = "";
    public static string lettersLanguage = "English";
	public static Boolean resetLetters = false;
	public static List<Letter> selectedLetters = new List<Letter>();

}
		



