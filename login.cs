using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming_Project {
    static class login {
        //FIELDS
        static public bool PlayersLoggedIn = false;
        static string[] usernames = new string[] { "player1", "player2", "player3" };
        static string[] passwords = new string[] { "pass1", "pass2", "pass3" };
        static public string[] displayNames = new string[2];

        //METHODS
        static public bool auth(string enteredUsername, string enteredPassword) {
            int usernamepos = Array.IndexOf(usernames, enteredUsername);
            if (usernamepos == -1) {
                return false;
            } else {
                if (enteredPassword == passwords[usernamepos]) {
                    return true;
                } else {
                    return false;
                }
            }
        }
    }
}
