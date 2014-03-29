using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.IO;
using System.Collections.Generic;
//using Mono.Data.Sqlite;
//using System.Data;
using System.Data.SqlClient;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
 public class DatabaseConnection
 {
     List<HashAndID> database;
     
     public DatabaseConnection()
     {
   database = new List<HashAndID>();
     }
 }

 public class HashAndID
 {
  string hash{ get; set;}
  int id{ get; set; }
  public HashAndID(string hash, int id)
  {
   this.hash = hash;
   this.id = id;
  }
 }
}