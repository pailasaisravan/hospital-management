using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;



namespace doctor_management_system.date_access_layer
{
    
    class dataaccesslayer
    {
        public DataSet verifyusercred(string id, string password)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspverifylogin", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@password", password);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return ds;
        }
        public int CreateDoctor(string Name, string mobile, string speicality, float fee,string loginId, string password, string userName)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspcreatedoctor", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", Name);
            cmd.Parameters.AddWithValue("@mobile", mobile);
            cmd.Parameters.AddWithValue("@speciality", speicality);
            cmd.Parameters.AddWithValue("@fee", fee);
            cmd.Parameters.AddWithValue("@loginid", loginId);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@username", userName);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();

        }
        public int editDoctor(int did,string name,string mobile)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspeditdoctor", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@did", did);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@mobile", mobile);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();
        }
        public int deleteDoctor(int did)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspdeletedoctor", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@did", did);          
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();
        }
        public DataSet showAllSpeciality()
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspshowallspeciality", con);
            cmd.CommandType = CommandType.StoredProcedure;
             SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return ds;
        }
        public int addRoom(string type ,float price)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspaddroom", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@type", type);
            cmd.Parameters.AddWithValue("@price", price);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();
        }
        public int editRoom(int rid, float price)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspeditroom", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@rid", rid);
            cmd.Parameters.AddWithValue("@price", price);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();
        }
        public int deleteRoom(int rid)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspdeleteroom", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@rid", rid);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();
        }
        public int addPatient(string name,string gender,int age,string address,string city,string pincode,string mobile,
            string reference,string disease)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspaddpatient", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@age", age);
            cmd.Parameters.AddWithValue("@address", address);
            cmd.Parameters.AddWithValue("@city", city);
            cmd.Parameters.AddWithValue("@pincode", pincode);
            cmd.Parameters.AddWithValue("@mobile", mobile);
            cmd.Parameters.AddWithValue("@ref", reference);
            cmd.Parameters.AddWithValue("@disease", disease);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();
        }
        public int assignRoom(int pid, string roomtype,int rid)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("usproomassign", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pid",pid);
            cmd.Parameters.AddWithValue("@type", roomtype);
            cmd.Parameters.AddWithValue("@rid", rid);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();
        }
        public int assignDoctor(int pid, string name)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspdoctorassign", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pid", pid);
            cmd.Parameters.AddWithValue("@name", name);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();
        }
        public int generateBill(int pid,float paidbill,float medicinebill)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspgeneratebill", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pid", pid);
            cmd.Parameters.AddWithValue("@paidbill", paidbill);
            cmd.Parameters.AddWithValue("@medicinebill", medicinebill);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();
        }
        public int paymentBill(int pid,float amount,string paytype,string check,string bank)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("usppaymentbill", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pid", pid);
            
            cmd.Parameters.AddWithValue("@amount", amount);
            cmd.Parameters.AddWithValue("@paytype", paytype);
            cmd.Parameters.AddWithValue("@check", check);
            cmd.Parameters.AddWithValue("@bank", bank);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();
        }
        public int discharge(int pid)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspdischarge", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pid", pid);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();
        }
        public DataSet askdischarge(int pid)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("upsaskdischarge", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pid", pid);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return ds;
        }
        public DataSet admitreport(string fromdate,string todate)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspadmitreport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", fromdate);
            cmd.Parameters.AddWithValue("@todate", todate);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return ds;
        }
        public DataSet dischargereport(string fromdate, string todate)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspdischargereport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", fromdate);
            cmd.Parameters.AddWithValue("@todate", todate);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return ds;
        }
        public DataSet roomwisereport(string type)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("usproomwisereport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@type", type);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return ds;
        }
        public DataSet doctorpatient(string name)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspdoctorpatients", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", name);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return ds;
        }
        public DataSet patientbill(int pid)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("usppatientbill", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pid", pid);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return ds;
        }
        public DataSet patientpayment(int pid)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("usppatientpayment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pid", pid);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return ds;
        }
        public DataSet myprofile(int uid)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspmyprofile", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@uid", uid);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return ds;
        }
        public DataSet mypatientlist(int uid)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("usppatientlist", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@uid", uid);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return ds;
        }
        public DataSet mycollections(int uid)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspmycollections", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@uid", uid);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            con.Close();
            return ds;
        }

        public int changepassword(int uid,string password)
        {
            string constr = ConfigurationManager.ConnectionStrings["doctordb"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("uspchangepassword", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@uid", uid);
            cmd.Parameters.AddWithValue("@newpass", password);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            return result;
            con.Close();
        }
    }
}
