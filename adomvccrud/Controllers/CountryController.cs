using adomvccrud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace adomvccrud.Controllers
{
    public class CountryController : Controller
    {
        private IConfiguration Configuration;
        public CountryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public IActionResult CountryList()
        {
            /*FillCountryDDL();*/
            DataTable dt = new DataTable();
            string str = this.Configuration.GetConnectionString("MyConnection");
            SqlConnection conn1 = new SqlConnection(str);
            conn1.Open();
            SqlCommand objCmd = conn1.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_SELECT_COUNTRY";
            SqlDataReader objSDR = objCmd.ExecuteReader();
            if (objSDR.HasRows)
            {
                dt.Load(objSDR);
            }
            return View(dt);
        }

        //update

        public IActionResult Add(int? CountryId)
        {

            /*FillCountryDDL();*/

            if (CountryId != null)
            {
                SqlConnection objConn = new
               SqlConnection(this.Configuration.GetConnectionString("MyConnection"));
                objConn.Open();
                SqlCommand objCmd = objConn.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_Country_SelectByPK";
                objCmd.Parameters.Add("@CountryID",SqlDbType.Int).Value = CountryId;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = objCmd.ExecuteReader();
                
                dt.Load(objSDR);
                CountryModel cont = new CountryModel();
                foreach (DataRow dr in dt.Rows)
                {
                    
                    cont.CountryID = Convert.ToInt32(dr["CountryID"]);
                    cont.CountryName = dr["CountryName"].ToString();
                    cont.CountryCode = dr["CountryCode"].ToString();
                }
                return View("AddCountry", cont);
            }

            return View("AddCountry");
        }

        //insert

        [HttpPost]
        public  IActionResult Save(CountryModel Countrymodel)
        {

            SqlConnection objConn = new SqlConnection(this.Configuration.GetConnectionString("MyConnection"));
            objConn.Open();
            SqlCommand objCmd =  objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
       

            if (Countrymodel.CountryID == 0)
            {
                objCmd.CommandText = "PR_Country_Insert";
                
            }
            else
            {
                objCmd.CommandText = "PR_Country_UpdateByPK";
                objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value=Countrymodel.CountryID;
            }
            objCmd.Parameters.Add("@CountryName", SqlDbType.VarChar).Value = Countrymodel.CountryCode;
            objCmd.Parameters.Add("@CountryCode", SqlDbType.Int).Value = Countrymodel.CountryCode;

            objCmd.ExecuteNonQuery();
            objConn.Close();
            /*                if (Convert.ToBoolean(objCmd.ExecuteNonQuery()))
                            {
                                {
                                    if (Countrymodel.CountryID == null)
                                        TempData["Message"] = "Record Inserted Successfully";
                                    else
                                    {
                                        TempData["Message"] = "Record Updated Successfully";
                                        return RedirectToAction("AddCountry");
                                    }
                                }
                            }*/
            return RedirectToAction("CountryList");

            //Fill DropDown for Country
            /*public void FillCountryDDL()
            {

                string str = this.Configuration.GetConnectionString("MyConnection");
                List<LOC_CountryDropDownModel> loc_Country = new
               List<LOC_CountryDropDownModel>();
                SqlConnection objConn = new SqlConnection(str);
                objConn.Open();
                SqlCommand objCmd = objConn.CreateCommand();
                objCmd.CommandType = CommandType.StoredProcedure;
                objCmd.CommandText = "PR_Country_SelectDropDownList";
                SqlDataReader objSDR = objCmd.ExecuteReader();
                if (objSDR.HasRows)
                {
                    while (objSDR.Read())
                    {
                        LOC_CountryDropDownModel country = new LOC_CountryDropDownModel()
                        {
                            CountryID = Convert.ToInt32(objSDR["CountryID"]),
                            CountryName = objSDR["CountryName"].ToString()
                        };
                        loc_Country.Add(country);
                    }
                    objSDR.Close();
                }
                objConn.Close();
                ViewBag.CountryList = loc_Country;

            }*/
        }


        public IActionResult DeleteCountry(int CountryId)
        {
            SqlConnection objConn = new SqlConnection(this.Configuration.GetConnectionString("MyConnection"));
            objConn.Open();
            SqlCommand objCmd = objConn.CreateCommand();
            objCmd.CommandType = CommandType.StoredProcedure;
            objCmd.CommandText = "PR_Country_DeleteByPK";
            objCmd.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryId;
            
            if (Convert.ToBoolean( objCmd.ExecuteNonQuery()))
            {
                TempData["Message"] = " ";
            }
            objConn.Close();


            return RedirectToAction("CountryList");
        }
    }
}




