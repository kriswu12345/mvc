using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class CreateUserInfoController : Controller
    {
        private string path = AppDomain.CurrentDomain.BaseDirectory + "bakupData2.csv";

        // GET: CreateUserInfo
        public ActionResult Index(CreateUserInfoModels vm)
        {

            if (vm.UserList != null)
            {
                DataTable dt = OpenCSV(path);
                vm.UserList = new List<CreateUserInfoModel>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    vm.UserList.Add(new CreateUserInfoModel
                    {
                        name = dt.Rows[i][0].ToString(),
                        age = dt.Rows[i][1].ToString(),
                        birthday = dt.Rows[i][2].ToString()
                    }); ;
                }
                return View(vm);
            }
            System.IO.File.Delete(path);
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            fs.Close();
            vm.UserList = new List<CreateUserInfoModel>();
            return View(vm);

        }
        public static bool SaveCSV(DataTable dt, string fullPath)
        {
            try
            {
                FileStream fs = new FileStream(fullPath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                fs.Close();
                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                    string data = "";

                    //寫出各行數據
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        data = "";
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string str = dt.Rows[i][j].ToString();
                            str = string.Format("\"{0}\"", str);
                            data += str;
                            if (j < dt.Columns.Count - 1)
                            {
                                data += ",";
                            }
                        }
                        sw.WriteLine(data);
                    }
                    sw.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static DataTable OpenCSV(string filePath)
        {
            DataTable dt = new DataTable();
            string strLine = "";
            string[] aryLine = null;
            dt.Columns.Add("name", typeof(string));
            dt.Columns.Add("old", typeof(string));
            dt.Columns.Add("birthday", typeof(string));

            using (FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        aryLine = strLine.Split(',');
                        if (aryLine.Length == 1)
                        {
                            break;
                        }
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < 3; j++)
                        {
                            dr[j] = aryLine[j].Replace("\"", "");
                        }
                        dt.Rows.Add(dr);
                    }
                    sr.Close();
                    fs.Close();
                }
            }
            return dt;
        }
        // GET: CreateUserInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CreateUserInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreateUserInfo/Create
        [HttpPost]
        public ActionResult Create(CreateUserInfoModels createUserInfo, FormCollection collection)
        {
            try
            {
                string row = collection["selectRow"];
                string action = collection["action"];
                if (createUserInfo.UserList == null)
                {
                    createUserInfo.UserList = new List<CreateUserInfoModel>();
                }
                if (action == "1")
                {
                    DataTable dt = OpenCSV(path);
                    for (int i = 0; i<dt.Rows.Count; i++)
                    {
                        if(i==Int32.Parse(row))
                        {
                            dt.Rows[i][0] = collection["name"];
                            dt.Rows[i][1] = collection["age"];
                            dt.Rows[i][2] = collection["birthday"];
                            break;
                        }
                    }
                    System.IO.File.Delete(path);
                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                    fs.Close();
                   
                    StreamWriter sw = new StreamWriter(path, true);
                    string data = "";

                    //寫出各行數據
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        data = "";
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            string str = dt.Rows[i][j].ToString();
                            str = string.Format("\"{0}\"", str);
                            data += str;
                            if (j < dt.Columns.Count - 1)
                            {
                                data += ",";
                            }
                        }
                        sw.WriteLine(data);
                    }
                    sw.Close();
                }
                else
                {
                    createUserInfo.UserList.Add(new CreateUserInfoModel
                    {
                        name = collection["name"],
                        age = collection["age"],
                        birthday = collection["birthday"]
                    }); ;
                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                    fs.Close();
                    StreamWriter sw = new StreamWriter(path, true);
                    for (int i = 0; i < createUserInfo.UserList.Count; i++)
                    {
                        string data = "";
                        string str = createUserInfo.UserList[i].name;
                        str = string.Format("\"{0}\"", str);
                        data += str;
                        data += ",";
                        str = createUserInfo.UserList[i].age;
                        str = string.Format("\"{0}\"", str);
                        data += str;
                        data += ",";
                        str = createUserInfo.UserList[i].birthday;
                        str = string.Format("\"{0}\"", str);
                        data += str;
                        data += ",";
                        sw.WriteLine(data);
                    }
                    sw.Close();
                }
               
               
                // TODO: Add insert logic here
                //return View(createUserInfo);
                return RedirectToAction("Index", "CreateUserInfo", createUserInfo);
            }
            catch
            {
                return View();
            }
        }

        // GET: CreateUserInfo/Edit/5
        public ActionResult Edit(string name, string age, string birthday, FormCollection collection)
        {
            return RedirectToAction("Index");
        }

        // POST: CreateUserInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(string name)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CreateUserInfo/Delete/5
        public ActionResult Delete(string name, string age, string birthday, FormCollection collection, CreateUserInfoModels createUserInfo)
        {
            if (createUserInfo.UserList == null)
            {
                createUserInfo.UserList = new List<CreateUserInfoModel>();
            }
            DataTable dt = OpenCSV(path);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][0].ToString().Equals(name) &&
                 dt.Rows[i][1].ToString().Equals(age) &&
                 dt.Rows[i][2].ToString().Equals(birthday))
                {
                    dt.Rows.RemoveAt(i);
                }
            }
            System.IO.File.Delete(path);
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            fs.Close();
            StreamWriter sw = new StreamWriter(path, true);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    str = string.Format("\"{0}\"", str);
                    data += str;
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }
            sw.Close();

            return RedirectToAction("Index", "CreateUserInfo", createUserInfo);
        }

        // POST: CreateUserInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
