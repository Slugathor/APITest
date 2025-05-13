Watch this short video for a quick demo that shows off the features. Turn on subtitles for explanations! (no sound)

https://www.youtube.com/watch?v=hxOUdGAeYL0


The backend is created with XAMPP 3.3.0
	-Windows
	-Apache
	-MySQL
	-PHP

HOW TO ?

-1. Unzip the project archive

0. Open the SERVER_RES folder

1. Import the database using the provided dump

2. Put the testapi folder into the XAMPP htdocs folder
	the baseurl in Unity has been set to http://localhost/testapi/
	so it should work.
	IF it does not, the baseurl definition is in Application.cs on line 51

3. Run the Unity project and press start
	I wanted to build this, but this way it is easy to follow along in the Debug Log




Try these test cases:


1. Create an account
	This account will be regular user and doesn't have any characters

2. Change password
	Try to enter passwords that don't match

3. Log out

4. Log in using new password

5. Log out again to test another account

6. Log in as admin
	username: admin
	password: notadmin

7. View other accounts' characters

8. Delete a character by typing its exact name and pressing the delete button
	The view should automatically refresh and no longer show the deleted character.


You may also want to observe these changes on the database

There are only 2 Unity C# scripts, SessionManager.cs and the massive Application.cs.
The software architecture is terrible on the Unity side, but this is mostly due to bad planning and the project simply growing and growing as I went. 
"Oh, and then I need to add a button for this..." "oh, now I need a method for closing this window.." etc.