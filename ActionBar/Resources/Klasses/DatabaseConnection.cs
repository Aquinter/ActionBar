using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
//using Mono.Data.Sqlite;
//using System.Data;

namespace IngHackaton //Moet maar aangepast worden voor het project
{
 //Database deamon

 public class DatabaseConnection
 {
     List<HashAndID> database;
     
     public DatabaseConnection()
     {
        database = new List<HashAndID>();
     }
  public void openConnection()
  {
   Console.WriteLine("Opening connection to database");
  }
  public void closeConnection()
  {
   Console.WriteLine("Closing connection to database");
  }
  public void addHashAndID(HashAndID record)
  {
   database.Add(record);
  }
  public string getHashByID(int idNumber)
  {
//   //string result = database.First();
//   var item = (from item in database where item.id == idNumber select item).First();
//   return item;
   foreach (HashAndID combination in database)
   {
    if (combination.id == idNumber)
    {
     return combination.hash;
    }
   }
   return "";
  }
  public void populateDatabase()
  {
   database.Add(new HashAndID("QkFxErs6hsNvTJDDXI0apmCn2hvPaqFuMbWArNn+VE2p5yjUgQJEj4ftjfUZTC/AUxHj+PIHetotMjaSicaONTczA4ZEH9o3j3N0cQd9M1Y0mVTI1TtTirSNlS931JcC6scIK0d698i26cHJ6rpw9V11tclL8Ieql2pINSoU8ozLBm0wh2CF5PK/p7qYUOJdflp3Ngyd5bnYBCYkGlSk0/Ava2nX9rAz8ufMBZFbVrYfXBpttP4dTfqnfYgZcFB9X7w5Y6SGFI9k5EB5qiXT2K4xQnE09x2e7lKLRq6uVVm+5N7/xMHFMHBNhP+tJ2ABjwkHzKAD2rDtWgtb71ZjXbXBKz8YmTWtbHqryI3/hgyZkyVsk20xUaYsJF4z6f1d20epDSZgIsQiwfPxqArrtw==", 123456789));
  }
 }

 public class HashAndID
 {
  public string hash{ get; set;}
  public int id{ get; set; }
  public HashAndID(string hash, int id)
  {
   this.hash = hash;
   this.id = id;
  }
 }
}