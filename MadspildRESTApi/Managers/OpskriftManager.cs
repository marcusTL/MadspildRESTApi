using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ModelLib;


namespace MadspildRESTApi.Managers
{
    public class OpskriftManager
    {
        private string DBstring = "Server=tcp:3rd-semester-server.database.windows.net,1433;Initial Catalog=madspildDB;Persist Security Info=False;User ID=admin1;Password=DataBPwd1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private string GET_ALL;
        private string GET_OTHER_TABLE;
        private string GET_ONE_NAME;
        private string GET_ONE;
        private string INSERT;
        private string DELETE;
        private string LINQ;

        public OpskriftManager()
        {

            string tableName = "opskrifter";
            string secondtable = "ingredienser";

            GET_ALL = $"SELECT * FROM {tableName}";
            GET_OTHER_TABLE = $"select * from {secondtable} where ingredienserId = @Id";
            GET_ONE = $"select * from {tableName} where opskrifterId = @Id";

            //test sql query, to see if i could combine sql querys for the search sektion.
            GET_ONE_NAME = $"select * from {tableName} where navn = @Navn";

            //Finished, the sql adds to both the tables, becaouse the ing* columns allow null
            //the web app dosent need to take into account the every column of ingredients.
            INSERT = $"Insert into {tableName}(navn, fremgangsmåde,tid, billede)values('@Navn','@fremgangsmåde',@tid,'@billede')" +
                     $"INSERT INTO {secondtable}(ingredienserId, ing1, ing2, ing3, ing4, ing5,ing6,ing7, ing8, ing9, ing10)" +
                     $"VALUES ((SELECT opskrifterId FROM {secondtable} WHERE opskrifterId = )," +
                     $"'@Ingredienser[0]', '@Ingredienser[1]', '@Ingredienser[2]', '@Ingredienser[3]','@Ingredienser[4]', " +
                     $"'@Ingredienser[5]', '@Ingredienser[6]', '@Ingredienser[7]', @Ingredienser[8]', '@Ingredienser[9]');";

            DELETE = $"delete from {tableName} where opskrifterIdid = @Id" +
                     $"delete from {secondtable} where ingredienserId = @Id";
        }
        
        private Opskrift ReadNextElement(SqlDataReader reader)
        {
            Opskrift opskrift = new Opskrift();

            opskrift.Id = reader.GetInt32(0);
            opskrift.Navn = reader.GetString(1);
            opskrift.Fremgangsmåde = reader.GetString(2);
            opskrift.Tid = reader.GetInt32(3);
            opskrift.Billede = reader.GetString(4);
            return opskrift;
        }

        private List<string> ReadOtherTable(int id)
        {
            List<string> ingredienser = new List<string>();
            int column = new int();
            using (SqlConnection conn = new SqlConnection(DBstring))
            using (SqlCommand cmd = new SqlCommand(GET_OTHER_TABLE, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bool check = false;
                    while (check == false)
                    {
                        column++;
                        if (!reader.IsDBNull(column))
                        {
                            ingredienser.Add(reader.GetString(column));
                        }
                        else
                        {
                            check = true;
                        }
                    }
                }
                reader.Close();
                return ingredienser;
            }
        }

        public List<Opskrift> GetAll()
        {
            List<Opskrift> list = new List<Opskrift>();
            using (SqlConnection conn = new SqlConnection(DBstring))
            using (SqlCommand cmd = new SqlCommand(GET_ALL, conn))
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Opskrift ops = ReadNextElement(reader);
                    ops.Ingredienser = ReadOtherTable(ops.Id);
                    list.Add(ops);
                }
                reader.Close();
            }

            return list;
        }

        public Opskrift GetOne(int id)
        {
            Opskrift ops = null;
            using (SqlConnection conn = new SqlConnection(DBstring))
            using (SqlCommand cmd = new SqlCommand(GET_ONE, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ops = ReadNextElement(reader);
                    ops.Ingredienser = ReadOtherTable(ops.Id);
                }
                reader.Close();
            }

            return ops;
        }
        //Method to test out the possiblity of combining SQL commands for search.
        public Opskrift GetOne(string name)
        {
            Opskrift ops = null;
            using (SqlConnection conn = new SqlConnection(DBstring))
            using (SqlCommand cmd = new SqlCommand(GET_ONE_NAME + GET_ONE, conn))
            {
                cmd.Parameters.AddWithValue("@Navn", name);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ops = ReadNextElement(reader);
                    ops.Ingredienser = ReadOtherTable(ops.Id);
                }
                reader.Close();
            }

            return ops;
        }
        public bool AddOpskrift(Opskrift opskrift)
        {
            using (SqlConnection conn = new SqlConnection(DBstring))
            using (SqlCommand cmd = new SqlCommand(INSERT, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", opskrift.Id);
                cmd.Parameters.AddWithValue("@Navn", opskrift.Navn);
                cmd.Parameters.AddWithValue("@Fremgangsmåde", opskrift.Fremgangsmåde);
                cmd.Parameters.AddWithValue("@Tid", opskrift.Tid);
                cmd.Parameters.AddWithValue("@Billede", opskrift.Billede);
                cmd.Parameters.AddWithValue("@Ingredienser", opskrift.Ingredienser);

                int noOfRowsAffected = cmd.ExecuteNonQuery();
                bool ok = noOfRowsAffected == 1;

                conn.Close();
                return ok;
            }
        }

        public bool DeleteOpskrift(int id)
        {
            using (SqlConnection conn = new SqlConnection(DBstring))
            using (SqlCommand cmd = new SqlCommand(DELETE, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@Id", id);
                int noOfRowsAffected = cmd.ExecuteNonQuery();
                bool ok = noOfRowsAffected == 1;

                conn.Close();

                return ok;
            }


        }

        //public void TestGetAll()
        //{
        //    using (var con = new SqlConnection(DBstring))
        //    {
        //        using (var cmd = new SqlCommand(GET_ALL, con))
        //        {
        //            int rowCount = 0;
        //            con.Open();
        //            using (IDataReader rdr = cmd.ExecuteReader())
        //            {
        //                while (rdr.Read())
        //                {
        //                    String object1 = String.Format("Object 1 in Row {0}: '{1}'", ++rowCount, rdr[0]);
        //                }
        //                if (rdr.NextResult())
        //                {
        //                    rowCount = 0;
        //                    while (rdr.Read())
        //                    {
        //                        String object1 = String.Format("Object 1 in Row {0}: '{1}'", ++rowCount, rdr[0]);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //}
    }


}
