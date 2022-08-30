using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using doctor_management_system.date_access_layer;
using System.Text.RegularExpressions;

namespace doctor_management_system
{
    class Program
    {
        static DataSet ds = null;
        static void Main(string[] args)
        {
            verifyLogin();
        }
        static void verifyLogin()
        {
            string loginid, password;
            Console.Write("enter login id:");
            loginid = Console.ReadLine();
            Console.Write("enter password:");
            password = Console.ReadLine();
            dataaccesslayer dal = new dataaccesslayer();
            ds = dal.verifyusercred(loginid, password);
            if (ds.Tables[0].Columns.Count== 3)
            {
                displaymenu(ds);
            }
            else
            {
                string message = ds.Tables[0].Rows[0][0].ToString();
                Console.WriteLine(message);
                verifyLogin();
                Console.ReadLine();
            }
        }
        public static void displaymenu(DataSet ds)
        {
            string userName;
            int rollId;
            int userId;
            userName = ds.Tables[0].Rows[0]["username"].ToString();
            rollId = Convert.ToInt32(ds.Tables[0].Rows[0]["role_id"].ToString());
            userId = Convert.ToInt32(ds.Tables[0].Rows[0]["uid"].ToString());
            Console.WriteLine("===============================================================");
            Console.WriteLine($"Welcome {userName} to the hospital ");
            Console.WriteLine("===============================================================");
            if (rollId == 1 || rollId == 2)
            {
                Console.WriteLine("===============================================================");
                Console.WriteLine("Hospital doctor and Htaff Management");
                Console.WriteLine("===============================================================");
                Console.WriteLine("1. Doctors");
                Console.WriteLine("2. Rooms");
                Console.WriteLine("3. Add patient");
                Console.WriteLine("4. Asssign room and Assign dcotor");
                Console.WriteLine("5. Generate bill");
                Console.WriteLine("6. Payment bill");
                Console.WriteLine("7. Discharge patient");
                Console.WriteLine("8. Manage reports");
                Console.WriteLine("9. Exit");
                Console.WriteLine("enter choice 1/2/3/4/5/6/7/8/9: ");
                Console.WriteLine("===============================================================");
                int ch;
                if (int.TryParse(Console.ReadLine(), out ch))
                {
                    switch (ch)
                    {
                        case 1:
                            Console.WriteLine("===============================================================");
                            Console.WriteLine("1. Add Doctor");
                            Console.WriteLine("2. Edit Doctor");
                            Console.WriteLine("3. Delete Doctor");
                            Console.WriteLine("enter choice 1/2/3/4: ");
                            Console.WriteLine("===============================================================");
                            int ch1 = int.Parse(Console.ReadLine());
                            switch (ch1)
                            {
                                case 1:
                                    createDoctor();
                                    break;
                                case 2:
                                    editdoctor();
                                    break;
                                case 3:
                                    deletedoctor();
                                    break;
                            }

                            break;
                        case 2:
                            Console.WriteLine("===============================================================");
                            Console.WriteLine("1. Add room");
                            Console.WriteLine("2. Edit room");
                            Console.WriteLine("3. Delete room");
                            Console.WriteLine("enter choice 1/2/3/4: ");
                            Console.WriteLine("===============================================================");
                            int ch2 = int.Parse(Console.ReadLine());
                            switch (ch2)
                            {
                                case 1:
                                    addroom();
                                    break;
                                case 2:
                                    editroom();
                                    break;
                                case 3:
                                    deleteroom();
                                    break;
                            }
                            break;
                        case 3:
                            addpatient();
                            break;
                        case 4:
                            Console.WriteLine("1. Assign room");
                            Console.WriteLine("2. Assign doctor");

                            Console.WriteLine("enter choice 1/2: ");
                            int ch3 = int.Parse(Console.ReadLine());
                            switch (ch3)
                            {
                                case 1:
                                    assignroom();
                                    assigndoctor();
                                    break;
                                case 2:
                                    assigndoctor();
                                    break;

                            }
                            break;
                        case 5:
                            generatebill();
                            break;
                        case 6:
                            paymentbill();
                            break;
                        case 7:
                            discharge();
                            break;
                        case 8:
                            Console.WriteLine("1. Admitted patient report by from date and to date");
                            Console.WriteLine("2. Discharge patient report by from date and to date ");
                            Console.WriteLine("3. Patient Bill report");
                            Console.WriteLine("4. Patient payment report ");
                            Console.WriteLine("5. Patient Room type wise report ");
                            Console.WriteLine("6. Patients under doctor ");

                            Console.WriteLine("enter choice 1/2/3/4/5/6 : ");
                            int ch4 = int.Parse(Console.ReadLine());
                            switch (ch4)
                            {
                                case 1:
                                    admitreport();
                                    break;
                                case 2:
                                    dischargereport();
                                    break;
                                case 3:
                                    patientbillreport();
                                    break;
                                case 4:
                                    patientpaymentreport();
                                    break;
                                case 5:
                                    roomwisereport();
                                    break;
                                case 6:
                                    docotrpatient();
                                    break;

                            }
                            break;
                        case 9:
                            verifyLogin();
                            break;
                        default:
                            Console.WriteLine("invalid choice");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("invalid entry");
                }
            }
            else
            {
                Console.WriteLine("patient management");
                Console.WriteLine("1. Myprofile");
                Console.WriteLine("2. My patients list");
                Console.WriteLine("3. My collections");
                Console.WriteLine("4. Change password");
                Console.WriteLine("5. exit");
                Console.WriteLine("enter choice 1/2/3/4/5: ");
                Console.WriteLine("===============================================================");
                int ch ;
                if (int.TryParse(Console.ReadLine(), out ch))
                {
                    switch (ch)
                    {
                        case 1:
                            myprofile(userId);
                            break;
                        case 2:
                            mypatientlist(userId);
                            break;
                        case 3:
                            mycollections(userId);
                            break;
                        case 4:
                            Console.WriteLine("enter new password");
                            string password = Console.ReadLine();
                            changepasword(userId, password);
                            break;
                        case 5:
                            verifyLogin();
                            break;
                        default:
                            Console.WriteLine("invalid choice");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("invalid entry");
                }

            }

            Console.WriteLine("==============================================================================");

  
            displaymenu(ds);
            Console.ReadLine();
        }
        public static void createDoctor()
        {
            string Name, mobile, speciality, loginid,
                password, userName;
            speciality = "";
            float fee;
            Console.Write("enter doctor name:");
            Name = Console.ReadLine();
            Console.Write("enter doctor mobile number:");
            mobile = Console.ReadLine();
            Console.WriteLine("===============================================================");
            Console.WriteLine("enter doctor speciality:");
            string choice = "enter choice ";
            dataaccesslayer dal1 = new dataaccesslayer();
            DataSet ds1 = dal1.showAllSpeciality();
            l1:
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ds1.Tables[0].Rows[i]["speciality_name"].ToString()}");
                choice += i + 1 + "/";
            }
            Console.WriteLine(choice.Substring(0, choice.Length - 1) + ":");
            Console.WriteLine("===============================================================");
            int ch2 = int.Parse(Console.ReadLine());
            switch(ch2)
            {
                case 1:
                    speciality = ds1.Tables[0].Rows[0]["speciality_name"].ToString();
                    break;
                case 2:
                    speciality = ds1.Tables[0].Rows[1]["speciality_name"].ToString();
                    break;
                case 3:
                    speciality = ds1.Tables[0].Rows[2]["speciality_name"].ToString();
                    break;
                case 4:
                    speciality = ds1.Tables[0].Rows[3]["speciality_name"].ToString();
                    break;
                case 5:
                    speciality = ds1.Tables[0].Rows[4]["speciality_name"].ToString();
                    break;
                default:
                    Console.WriteLine("===============================================================");
                    Console.WriteLine("enter valid number");
                    Console.WriteLine("===============================================================");
                    goto l1;
                    break;
            }
            Console.Write("enter doctor fee:");
            if (float.TryParse(Console.ReadLine(),out fee))
            {
                Console.Write("enter doctor username:");
                userName = Console.ReadLine();
                Console.Write("enter docotr login id:");
                loginid = Console.ReadLine();
                if (loginid.Length >= 6 && loginid.Length <= 15)
                {
                    l2:
                    Console.Write("enter doctor password:");
                    password = Console.ReadLine();
                    string patternText = @"^(?=.*\d)(?=.*[a-zA-Z]){2,}(?=.*[A-Z])(?=.*[!*@#$%^&+=])[A-Za-z\d!*@#$%^&+=]{6,10}$"; 
                    Regex reg = new Regex(patternText);
                    if (reg.IsMatch(password))
                    {
                        dataaccesslayer dal = new dataaccesslayer();
                        int result = dal.CreateDoctor(Name, mobile, speciality, fee, loginid, password, userName);
                        if (result > 0)
                        {
                            Console.WriteLine("======================================");
                            Console.WriteLine("doctor created successfully!=");
                            Console.WriteLine("======================================");
                        }
                        else
                            Console.WriteLine("Something went wrong while creating doctor!");
                    }
                    else
                    {
                        Console.WriteLine("======================================");
                        Console.WriteLine("enter valid password");
                        Console.WriteLine("======================================");
                        goto l2;
                    }
                }
                else
                {
                    Console.WriteLine("======================================");
                    Console.WriteLine("enter valid login id");
                    Console.WriteLine("======================================");
                }
            }
            else
            {
                Console.WriteLine("invalid amount ");
            }
           
        }
        public static void editdoctor()
        {

            string name="", mobile="";
            Console.WriteLine("select from below list:");
            string choice = "enter choice";
            int[] arr = new int[ds.Tables[3].Rows.Count];
            for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
            {
                Console.WriteLine($"{ds.Tables[3].Rows[i]["did"].ToString()}\t{ds.Tables[3].Rows[i]["doctorcode"].ToString()}  {ds.Tables[3].Rows[i]["doctorname"].ToString()}  {ds.Tables[3].Rows[i]["speciality"].ToString()}");
                choice += i + 1 + "/";
                arr[i] = int.Parse( ds.Tables[3].Rows[i]["did"].ToString());
            }
            Console.WriteLine(choice.Substring(0, choice.Length - 1) + ":");
            int id = int.Parse(Console.ReadLine());
            if (arr.Contains(id))
            {
                int did = int.Parse(ds.Tables[3].Rows[id - 1]["did"].ToString());
                Console.WriteLine("enter the choicce to change either of them");
                Console.WriteLine("1. name ");
                Console.WriteLine("2. mobile ");
                Console.WriteLine("3. both");
                int ch = int.Parse(Console.ReadLine());
                if (ch == 1)
                {
                    Console.WriteLine("enter the name to update");
                    name = Console.ReadLine();
                }
                else if (ch == 2)
                {
                    Console.WriteLine("enter the mobile number to update");
                    mobile = Console.ReadLine();
                }
                else if (ch == 3)
                {
                    Console.WriteLine("enter the name to update");
                    name = Console.ReadLine();
                    Console.WriteLine("enter the mobile number to update");
                    mobile = Console.ReadLine();
                }
                dataaccesslayer dal = new dataaccesslayer();
                int result = dal.editDoctor(did, name, mobile);
                if (result > 0)
                    Console.WriteLine("doctor edited successfully!=");
                else
                    Console.WriteLine("Something went wrong while editing doctor");
            }
            Console.WriteLine("invalid doctor id");

        }
        public static void deletedoctor()
        {

            
            Console.WriteLine("enter doctor id:");
            string choice = "enter choice";
            int[] arr = new int[ds.Tables[3].Rows.Count];
            for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
            {
                Console.WriteLine($"{ds.Tables[3].Rows[i]["did"].ToString()}\t{ds.Tables[3].Rows[i]["doctorcode"].ToString()}\t{ds.Tables[3].Rows[i]["doctorname"].ToString()}\t{ds.Tables[3].Rows[i]["speciality"].ToString()}");
                choice += i + 1 + "/";
                arr[i] = int.Parse(ds.Tables[3].Rows[i]["did"].ToString());
            }
            Console.WriteLine(choice.Substring(0, choice.Length - 1) + ":");
            int id = int.Parse(Console.ReadLine());
            if (arr.Contains(id))
            {
                int did = int.Parse(ds.Tables[3].Rows[id - 1]["did"].ToString());
                dataaccesslayer dal = new dataaccesslayer();
                int result = dal.deleteDoctor(did);
                if (result > 0)
                    Console.WriteLine("doctor deleted successfully!=");
                else
                    Console.WriteLine("Something went wrong while deleting doctor");
            }
            else
            {
                Console.WriteLine("invalid doctor id");
            }

        }
        public static void addroom()
        {
            string Name;
            float price;
            l1:
            Console.WriteLine("enter room id:");
            string choice = "enter choice ";
            int[] arr = new int[ds.Tables[1].Rows.Count];
            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ds.Tables[1].Rows[i]["roomtype"].ToString()}");
                choice += i + 1 + "/";
                arr[i] = i+1;
            }
            Console.WriteLine(choice.Substring(0, choice.Length - 1) + ":");
            int id = int.Parse(Console.ReadLine());
            if (arr.Contains(id))
            {
                Name = ds.Tables[1].Rows[id - 1]["roomtype"].ToString();
                Console.Write("enter room price :");
                price = float.Parse(Console.ReadLine());
                dataaccesslayer dal = new dataaccesslayer();
                int result = dal.addRoom(Name, price);
                if (result > 0)
                {
                    Console.WriteLine("===================================");
                    Console.WriteLine("room addedd successfully!=");
                }
                else
                    Console.WriteLine("Something went wrong while adding room");
            }
            else
            {
                Console.WriteLine("===================================");
                Console.WriteLine("enter valid room no");
                Console.WriteLine("===================================");
                goto l1;
            }
        }
        public static void editroom()
        {

            l1:
            Console.WriteLine("enter room rid:");
            string choice = "enter choice";
            int[] arr = new int[ds.Tables[4].Rows.Count];
            for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ds.Tables[4].Rows[i]["rid"].ToString()}  {ds.Tables[4].Rows[i]["roomtype"].ToString()}  {ds.Tables[4].Rows[i]["roomcode"].ToString()}  {ds.Tables[4].Rows[i]["price"].ToString()}");
                choice += i + 1 + "/";
                arr[i] = i + 1;
            }
            Console.WriteLine(choice.Substring(0, choice.Length - 1) + ":");
            int id = int.Parse(Console.ReadLine());
            if (arr.Contains(id))
            {
                Console.WriteLine("enter the price u want to update");
                float price = float.Parse(Console.ReadLine());
                int rid = int.Parse(ds.Tables[4].Rows[id - 1]["rid"].ToString());
                dataaccesslayer dal = new dataaccesslayer();
                int result = dal.editRoom(rid, price);
                if (result > 0)
                {
                    Console.WriteLine("===================================");
                    Console.WriteLine("room cost changed successfully!=");
                }
                else
                    Console.WriteLine("Something went wrong while changing cost");
            }
            else
            {
                Console.WriteLine("===================================");
                Console.WriteLine("enter valid room no");
                Console.WriteLine("===================================");
                goto l1;
            }
        }
        public static void deleteroom()
        {
            l1:
            Console.WriteLine("enter room rid:");
            string choice = "enter choice";
            int[] arr = new int[ds.Tables[4].Rows.Count];
            for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ds.Tables[4].Rows[i]["rid"].ToString()}  {ds.Tables[4].Rows[i]["roomtype"].ToString()}  {ds.Tables[4].Rows[i]["roomcode"].ToString()}  {ds.Tables[4].Rows[i]["price"].ToString()}");
                choice += i + 1 + "/";
                arr[i] = i + 1; ;
            }
            Console.WriteLine(choice.Substring(0, choice.Length - 1) + ":");
            int id = int.Parse(Console.ReadLine());
            if (arr.Contains(id))
            {
                int rid = int.Parse(ds.Tables[4].Rows[id - 1]["rid"].ToString());
                dataaccesslayer dal = new dataaccesslayer();
                int result = dal.deleteRoom(rid);
                if (result > 0)
                    Console.WriteLine("room deleted successfully!=");
                else
                    Console.WriteLine("Something went wrong while deleting room");
            }
            else
            {
                Console.WriteLine("===================================");
                Console.WriteLine("enter valid room no");
                Console.WriteLine("===================================");
                goto l1;
            }
        }
        public static void addpatient()
        {
            string name, gender,address,city,reference,disease;
            int age;
            long mobile;
            int pincode;
            Console.Write("enter patient name: ");
            name = Console.ReadLine();
            Console.Write("enter gender: ");
            gender = Console.ReadLine();
            Console.Write("enter age: ");
            age = int.Parse(Console.ReadLine());
            Console.Write("enter address: ");
            address = Console.ReadLine();
            Console.Write("enter city: ");
            city = Console.ReadLine();
            Console.Write("enter pincode: ");
            if (int.TryParse(Console.ReadLine(), out pincode))
            {
                string pincode1 = Convert.ToString(pincode);
                string mobile1;
                Console.Write("enter mobile number: ");
                if (long.TryParse(Console.ReadLine(), out mobile))
                {
                    mobile1 = Convert.ToString(mobile);
                    Console.Write("enter reference doctor: ");
                    reference = Console.ReadLine();
                    Console.Write("enter disease: ");
                    disease = Console.ReadLine();
                    dataaccesslayer dal = new dataaccesslayer();
                    int result = dal.addPatient(name, gender, age, address, city, pincode1, mobile1, reference, disease);
                    if (result > 0)
                        Console.WriteLine("patient added successfully!=");
                    else
                        Console.WriteLine("Something went wrong while adding room");
                }
                else
                {
                    Console.WriteLine("===================================");
                    Console.WriteLine("invalid mobile number");
                }
            }
            else
            {
                Console.WriteLine("invalid pincode");
            }
        }
        public static void assignroom()
        {
            Console.WriteLine("enter patient id");
            int pid = int.Parse(Console.ReadLine());
            l1:
            Console.WriteLine("enter room id:");
            string choice = "enter choice";
            int[] arr = new int[ds.Tables[2].Rows.Count];
            Console.WriteLine("sno\troomtype\troomcode\t");
            for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ds.Tables[2].Rows[i]["roomtype"].ToString()} \t {ds.Tables[2].Rows[i]["roomcode"].ToString()}");
                choice += i + 1 + "/";
                arr[i] = i + 1;
            }
            Console.WriteLine(choice.Substring(0, choice.Length - 1) + ":");
            int id = int.Parse(Console.ReadLine());
            if (arr.Contains(id))
            {
                string roomtype = ds.Tables[2].Rows[id - 1]["roomtype"].ToString();
                int rid = int.Parse(ds.Tables[2].Rows[id - 1]["rid"].ToString());
                dataaccesslayer dal = new dataaccesslayer();
                int result = dal.assignRoom(pid, roomtype, rid);
                if (result > 0)
                    Console.WriteLine("room assigned successfully!=");
                else
                    Console.WriteLine("Something went wrong while assigning  room or there is no patient with such id");
            }
            else
            {
                Console.WriteLine("===================================");
                Console.WriteLine("enter valid room no");
                Console.WriteLine("===================================");
                goto l1;
            }

        }
        public static void assigndoctor()
        {
            l1:
            Console.WriteLine("===================================");
            Console.WriteLine("assigning doctor");
            Console.WriteLine("===================================");
            int[] arr = new int[ds.Tables[3].Rows.Count];
            Console.WriteLine("enter patient id");
            int pid = int.Parse(Console.ReadLine());
            if (ds.Tables[3].Rows.Count != 0)
            {
                Console.WriteLine("enter doctor id:");
                string choice = "enter choice";
                Console.WriteLine("sno\tdoctorcode\tname\tspeciality");
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {ds.Tables[3].Rows[i]["doctorcode"].ToString()}  {ds.Tables[3].Rows[i]["doctorname"].ToString()}  {ds.Tables[3].Rows[i]["speciality"].ToString()}");
                    choice += i + 1 + "/";
                    arr[i] = i + 1;
                }
                Console.WriteLine(choice.Substring(0, choice.Length - 1) + ":");
                int id = int.Parse(Console.ReadLine());
                if (arr.Contains(id))
                {
                    string doctorname = ds.Tables[3].Rows[id - 1]["doctorname"].ToString();
                    dataaccesslayer dal = new dataaccesslayer();
                    int result = dal.assignDoctor(pid, doctorname);
                    if (result > 0)
                        Console.WriteLine("doctor assigned successfully!=");
                    else
                        Console.WriteLine("Something went wrong while assigning doctor");
                }
                else
                {
                    Console.WriteLine("===================================");
                    Console.WriteLine("enter valid doctor id");
                    Console.WriteLine("===================================");
                    goto l1;
                }
            }
            else
            {
                Console.WriteLine("===================================");
                Console.WriteLine("no doctors are available");
            }

        }
        public static void generatebill()
        {
            Console.WriteLine("enter patient id");
            int pid = int.Parse(Console.ReadLine());
            Console.WriteLine("enter medicine bill");
            float medicinebill;
            if (float.TryParse(Console.ReadLine(), out medicinebill))
            {
                Console.WriteLine("enter paid bill");
                float paidbill;
                if (float.TryParse(Console.ReadLine(), out paidbill))
                {
                    dataaccesslayer dal = new dataaccesslayer();
                    int result = dal.generateBill(pid, paidbill, medicinebill);
                    if (result > 0)
                        Console.WriteLine("bill generated  successfully!=");
                    else
                        Console.WriteLine("Something went wrong while generating bill or invalid patient id or patient bill already generated");
                }
                else
                {
                    Console.WriteLine("enter valid amount");
                }
            }
            else
            {
                Console.WriteLine("enter valid amount");
            }

        }
        public static void paymentbill()
        {
            string check="", bank="";
            string paytype="";
            Console.WriteLine("enter patient id");
            int pid = int.Parse(Console.ReadLine());
           
            Console.WriteLine("enter amount paid by patient");
            float amount = float.Parse(Console.ReadLine());
            Console.WriteLine("enter payment mode");
            Console.WriteLine("1. cash");
            Console.WriteLine("2. check");
            int  mode = int.Parse( Console.ReadLine());
            if (mode == 2)
            {
                paytype = "check";
                Console.WriteLine("enter check no");
                 check = Console.ReadLine();
                Console.WriteLine("enter bank name");
                bank = Console.ReadLine();
            }
            else if (mode == 1)
            {
                paytype = "cash";
                check = "";
                bank = "";
            }
            dataaccesslayer dal = new dataaccesslayer(); 
            int result = dal.paymentBill(pid,amount,paytype,check,bank);
            if (result > 0)
                Console.WriteLine("bill generated  successfully!=");
            else
                Console.WriteLine("Something went wrong while generating bill or patient bill already generated or patient id invalid");

        }
        public static void discharge()
        {
            Console.WriteLine("enter patient id");
            int pid = int.Parse(Console.ReadLine());
            dataaccesslayer dal = new dataaccesslayer();
            DataSet dsd = dal.askdischarge(pid);
            if (dsd.Tables[0].Rows.Count > 0)
            {
                Console.WriteLine($"The patient name is {dsd.Tables[1].Rows[0]["name"]} and pending amount is {dsd.Tables[0].Rows[0]["remainingbill"]}");
                Console.WriteLine("do u want to discharge ???");
                Console.Write("enter choice 1. yes/2. no : ");
                int choice = int.Parse(Console.ReadLine());
                if (choice == 1)
                {
                    int result = dal.discharge(pid);
                    if (result > 0)
                    {
                        Console.WriteLine("patient successfully discharged!=");
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong while discharging the patient");
                    }
                }
                else
                {
                    Console.WriteLine("asmin deicided not to discharge patient");
                }
            }
            else
            {
                Console.WriteLine("invalid patient id or bill not yet generated ");
            }
        }
        public static void admitreport()
        {
            Console.WriteLine("enter from date");
            string fromdate = Console.ReadLine();
            Console.WriteLine("enter to date");
            string todate = Console.ReadLine();

            dataaccesslayer dal = new dataaccesslayer();
            DataSet ds = dal.admitreport(fromdate,todate);
            if (ds.Tables[0].Rows.Count != 0)
            {
                Console.WriteLine("===============================================================");
                Console.WriteLine("no\tname\tage\tdoctorname\tedate\trid");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {ds.Tables[0].Rows[i]["name"].ToString()}\t{ds.Tables[0].Rows[i]["age"].ToString()}\t{ds.Tables[0].Rows[i]["doctor_name"].ToString()}\t{ds.Tables[0].Rows[i]["edate"].ToString()}\t{ds.Tables[0].Rows[i]["rid"].ToString()}");

                }
            }
            else
            {
                Console.WriteLine("no records found");
            }
        }
        public static void dischargereport()
        {
            Console.WriteLine("enter from date");
            string fromdate = Console.ReadLine();
            Console.WriteLine("enter to date");
            string todate = Console.ReadLine();
            dataaccesslayer dal = new dataaccesslayer();
            DataSet ds = dal.dischargereport(fromdate, todate);
            if (ds.Tables[0].Rows.Count != 0)
            {
                Console.WriteLine("===============================================================");
                Console.WriteLine("no\tname\tage\tdoctorname\tedate\trid");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {ds.Tables[0].Rows[i]["name"].ToString()}\t{ds.Tables[0].Rows[i]["age"].ToString()}\t{ds.Tables[0].Rows[i]["doctor_name"].ToString()}\t{ds.Tables[0].Rows[i]["edate"].ToString()}\t{ds.Tables[0].Rows[i]["rid"].ToString()}");

                }
            }
            else
            {
                Console.WriteLine("no records found");
            }

        }
        public static void patientbillreport()
        {
            Console.WriteLine("enter the patient id");
            int pid = int.Parse(Console.ReadLine());
            
            Console.WriteLine("===============================================================");
            
            dataaccesslayer dal = new dataaccesslayer();
            DataSet ds = dal.patientbill(pid);
            try
            {
                int[] arr = new int[ds.Tables[0].Rows.Count];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    arr[i] = int.Parse(ds.Tables[0].Rows[i]["pid"].ToString());
                }
                if (arr.Contains(pid))
                {
                    Console.WriteLine("no\tpid\tname\tpatientstatus\tadmitted_date\tpaid_bill\tremaining_bill");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}\t{ds.Tables[0].Rows[i]["name"].ToString()}\t{ds.Tables[0].Rows[i]["patietntstatus"].ToString()}\t{ds.Tables[0].Rows[i]["admitted_date"].ToString()}\t{ds.Tables[0].Rows[i]["paidbill"].ToString()}\t{ds.Tables[0].Rows[i]["remainingbill"].ToString()}");

                    }
                }
                else
                {
                    Console.WriteLine("invalid pid");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("invalid patient id");
            }

        }
        public static void patientpaymentreport()
        {
            Console.WriteLine("enter the patient id");
            int pid = int.Parse(Console.ReadLine());

            dataaccesslayer dal = new dataaccesslayer();
            DataSet ds = dal.patientpayment(pid);
            try
            {
                int[] arr = new int[ds.Tables[0].Rows.Count];

                Console.WriteLine("sno\tpid\tname\tpatientstatus\tadmitteddate\tbid\tpaymenttype");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Console.WriteLine($"{i + 1}\t{ds.Tables[0].Rows[i]["pid"].ToString()}\t{ds.Tables[0].Rows[i]["name"].ToString()}\t{ds.Tables[0].Rows[i]["patietntstatus"].ToString()}\t{ds.Tables[0].Rows[i]["admitted_date"].ToString()}\t{ds.Tables[0].Rows[i]["bid"].ToString()}\t{ds.Tables[0].Rows[i]["payment_type"].ToString()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("invalid patient id");
            }

        }
        public static void roomwisereport()
        {
            
            Console.WriteLine("enter room id:");
            string choice = "enter choice";
            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ds.Tables[1].Rows[i]["roomtype"].ToString()}");
                choice += i + 1 + "/";
            }
            Console.WriteLine(choice.Substring(0, choice.Length - 1) + ":");
            int id = int.Parse(Console.ReadLine());
            string roomtype = ds.Tables[1].Rows[id - 1]["roomtype"].ToString();
            dataaccesslayer dal1 = new dataaccesslayer();
            DataSet ds1 = dal1.roomwisereport(roomtype);
            Console.WriteLine("s.no\troomtype\troomcode\tpatientname");
            if (ds1.Tables[0].Rows.Count != 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    Console.WriteLine($"{i + 1}\t{ds1.Tables[0].Rows[i]["room_type"].ToString()}\t\t{ds1.Tables[0].Rows[i]["roomcode"].ToString()}\t\t{ds1.Tables[0].Rows[i]["patient_name"].ToString()}");

                }
            }
            else
            {
                Console.WriteLine("===============================================================");
                Console.WriteLine("no patients are asigned in the room");
            }

        }
        public static void docotrpatient()
        {
            Console.WriteLine("enter doctor id:");
            string choice = "enter choice";
            for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ds.Tables[5].Rows[i]["doctorcode"].ToString()}  {ds.Tables[5].Rows[i]["doctorname"].ToString()}  {ds.Tables[5].Rows[i]["speciality"].ToString()}");
                choice += i + 1 + "/";
            }
            Console.WriteLine(choice.Substring(0, choice.Length - 1) + ":");
            int id = int.Parse(Console.ReadLine());
            string doctorname = ds.Tables[5].Rows[id - 1]["doctorname"].ToString();
            dataaccesslayer dal1 = new dataaccesslayer();
            DataSet ds1 = dal1.doctorpatient(doctorname);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {ds1.Tables[0].Rows[i]["doctorname"].ToString()}\t{ds1.Tables[0].Rows[i]["patient_name"].ToString()}");
            }
        }
        public static void myprofile(int userid)
        {

            Console.WriteLine("===============================================================");
            dataaccesslayer dal = new dataaccesslayer();
            DataSet ds = dal.myprofile(userid);
            Console.WriteLine("no\tdoctorname\tmobile\t\tspeciality\tconsultationfee");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}.\t{ds.Tables[0].Rows[i]["doctorname"].ToString()}\t\t{ds.Tables[0].Rows[i]["mobile"].ToString()}\t{ds.Tables[0].Rows[i]["speciality"].ToString()}\t\t{ds.Tables[0].Rows[i]["consultation_fee"].ToString()}");

            }
        }
        public static void mypatientlist(int userid)
        {
            Console.WriteLine("===============================================================");
            dataaccesslayer dal = new dataaccesslayer();
            DataSet ds = dal.mypatientlist(userid);
            Console.WriteLine("no\tdoctorname\tpatientname");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}.\t{ds.Tables[0].Rows[i]["doctorname"].ToString()}\t\t{ds.Tables[0].Rows[i]["patient_name"].ToString()}");

            }
        }
        public static void mycollections(int userid)
        {
            Console.WriteLine("===============================================================");
            dataaccesslayer dal = new dataaccesslayer();
            DataSet ds = dal.mycollections(userid);
            Console.WriteLine("no\tdocotorname\tcollectionamount");
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Console.WriteLine($"{i + 1}.\t{ds.Tables[0].Rows[i]["doctor_name"].ToString()}\t\t{ds.Tables[0].Rows[i]["totalcollection"].ToString()}");

            }
        }

        public static void changepasword(int userid,string password)
        {
            dataaccesslayer dal = new dataaccesslayer();
            int result = dal.changepassword(userid,password);
            if (result > 0)
                Console.WriteLine("password updated successfully!=");
            else
                Console.WriteLine("Something went wrong while updating password");
        }

    }
}
