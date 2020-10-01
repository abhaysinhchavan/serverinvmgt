using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using serverinvmgt.Models;
using serverinvmgt.DBHelper;
using System.IO;

namespace serverinvmgt.operations
{
    public class clsItemsOps
    {
        SqlClient sqlClient = null;

        public List<clsItems> getItems()
        {
            List<clsItems> lstItems = new List<clsItems>();
            clsItems clsItems = new clsItems();
            sqlClient = new SqlClient();
            string sql = string.Empty;
            try
            {
                sql = "Select * from tblItems with(nolock)";
                DataTable dataTable = sqlClient.ExecuteQuery(sql, null);
                if (dataTable.Rows.Count > 0)
                {
                    lstItems = (from DataRow dr in dataTable.Rows
                                select new clsItems()
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    Name = Convert.ToString(dr["Name"]),
                                    Price = Convert.ToDouble(dr["Price"]),
                                    Desc = Convert.ToString(dr["Desc"])
                                }).ToList();
                }
            }
            catch (Exception ex) { 
            // Logs 
            }
            return lstItems;
        }

        public string addItem(clsItems objItem)
        {
            sqlClient = new SqlClient();
            string sql = string.Empty;
            string Return = string.Empty;
            try
            {
                sql = $"INSERT INTO tblItems (Name,Price, [Desc]) " +
                    $"values ('{objItem.Name}', {objItem.Price}, '{objItem.Desc}') " +
                    $"select Id AS Id from tblItems Where Id = @@IDENTITY "; 
                Return = sqlClient.ExecuteScalar(sql);
            }
            catch (Exception ex) { }
            return Return;
        }

        public clsItems getItembyId(long id)
        {
            List<clsItems> lstItems = new List<clsItems>();
            clsItems clsItems = new clsItems();
            sqlClient = new SqlClient();
            string sql = string.Empty;
            try
            {
                sql = $"Select * from tblItems with (nolock) where Id = {id}";
                DataTable dataTable = sqlClient.ExecuteQuery(sql, null);
                if (dataTable.Rows.Count > 0)
                {
                    lstItems = (from DataRow dr in dataTable.Rows
                                select new clsItems()
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    Name = Convert.ToString(dr["Name"]),
                                    Price = Convert.ToDouble(dr["Price"]),
                                    Desc = Convert.ToString(dr["Desc"])
                                }).ToList();
                    clsItems = lstItems.FirstOrDefault();
                    clsItems.File = Convert.ToBase64String(subGetImage(Convert.ToString(clsItems.Id)));  
                }
            }
            catch (Exception ex) { }
            return clsItems;
        }

        public bool editItembyId(clsItems objItem)
        {
            sqlClient = new SqlClient();
            string sql = string.Empty;
            bool Return = false;
            try
            {
                sql = $"Update tblItems with (rowlock) set Name = '{objItem.Name}'," +
                    $" Price = {objItem.Price}, [Desc] = '{objItem.Desc}' where Id = {objItem.Id}";
                Return = sqlClient.ExecuteNonQueryAsync(sql);
            }
            catch (Exception ex) { }
            return Return;
        }

        public bool deleteItembyId(long id)
        {
            sqlClient = new SqlClient();
            string sql = $"Delete from tblItems where Id = {id}";
            bool Return = sqlClient.ExecuteNonQueryAsync(sql);
            return Return;
        }

        public void subSaveImage(HttpPostedFile postedFile, string fileName)
        {
            string strImagesLocation = @"D:/tasks/images/";
            string strImgExt = ".jpeg";

            if (!Directory.Exists(strImagesLocation)) {
                Directory.CreateDirectory(strImagesLocation);
            }
            var filePath = Path.Combine($"{strImagesLocation.Trim()}{fileName}{strImgExt.Trim()}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                postedFile.InputStream.CopyTo(stream);
                stream.Close();
            }
        }

        public byte[] subGetImage(string itemId)
        {
            string strImagesLocation = @"D:/tasks/images/";
            string strImgExt = ".jpeg";
            string filePath = strImagesLocation + itemId + strImgExt;
            byte[] byteImage = null;

            if(File.Exists(filePath))
            {
                byteImage = File.ReadAllBytes(filePath).ToArray();
            }
            return byteImage;
        }
    }
}